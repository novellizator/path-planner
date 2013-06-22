#include <cstdio>
#include <iomanip>
#include <cstdlib>
#include <queue>
#include <iostream>
#include <algorithm> // reverse() 
#include <vector>

#include "Entry.h"
#include "globals.h"
#include "NSquare.h"
#include "BucketQueue.h"

using namespace std;


// extern variable defined here
int verticesScanned = 0;
int width, height;

vector<bool> map;
vector<int> NSquares[2]; // novella's squares (colorized map, int=color)


// check out all the neighbors && push them
// starting with the one up clockwise
const pair<int, int> neighbors[] = { // [y, x]
	make_pair(-1, 0),
	make_pair(-1, 1),
	make_pair(0, 1),
	make_pair(1, 1),
	make_pair(1, 0),
	make_pair(1, -1),
	make_pair(0, -1),
	make_pair(-1, -1),
};

const dist_t neighborDistances[] = {
	straight,
	tangent,
	straight,
	tangent,
	straight,
	tangent,
	straight,
	tangent
};



// ---- START OF DIJKSTRA
bool Dijkstra(xyLoc s, std::vector<Node_info_t>& outputMap)
{
	int from = linearize(s);

	outputMap.resize(width * height, Node_info_t());
    //priority_queue<Node_t, vector<Node_t>, Comp> Q;
	BucketQueue Q;
    
    Node_t fromV(from, 0.0);
    Q.push(fromV);
	outputMap[from].distance = 0.0;
	outputMap[from].precursorCoords = 0;

	const int neighborsCount = 8;

    while(!Q.empty()) 
    {
        // select vertex with the shortest distance
        Node_t vertex = Q.top();
        Q.pop();

		outputMap[vertex.coords].isClosed = true;
	
		// check all the neighbors
		for (int i = 0; i < neighborsCount; ++i)
		{

			xyLoc vc = linearToCoords(vertex.coords);

			xyLoc neighborC;
			neighborC.x = vc.x + neighbors[i].first;
			neighborC.y = vc.y + neighbors[i].second;
			
			int neighborCoords = linearize(neighborC);

			if (!ValidateLoc(neighborC) || outputMap[neighborCoords].isClosed == true)
			{
				continue;
			}

			
			// both adjacent cardinal directions must be unblocked
			int previous = (i + neighborsCount - 1) % neighborsCount;
			xyLoc previousNeighborC;
			previousNeighborC.x = vc.x + neighbors[previous].first;
			previousNeighborC.y = vc.y + neighbors[previous].second;
			//cout << "previousNeighbor x="<<previousNeighborC.x<<"y="<<previousNeighborC.y << endl;

			int next = (i + 1) % neighborsCount;
			xyLoc nextNeighborC;
			nextNeighborC.x = vc.x + neighbors[next].first;
			nextNeighborC.y = vc.y + neighbors[next].second;
			if (i % 2 == 1 && (!ValidateLoc(previousNeighborC) || !ValidateLoc(nextNeighborC)) )
			{
				continue;
			}


			
			// relax if necessary
			// s....u.v...t
			// if d(su)+ d(uv) < d(sv) 
			dist_t lengthThroughV = outputMap[vertex.coords].distance + neighborDistances[i];
			if (lengthThroughV < outputMap[neighborCoords].distance)
			{	
				Q.push(Node_t(neighborCoords, lengthThroughV));
				outputMap[neighborCoords].precursorCoords = vertex.coords;
				outputMap[neighborCoords].distance = vertex.distance + neighborDistances[i];
			}
		}
    }

	return true;
}


//-------- END OF DIJKSTRA


//-------- START OF A*
dist_t getGreatestLowerBound(int vertex, int goal, vector<Node_info_t>* landmarksOutputBitmap)
{
	dist_t lowerBound = 0.0;

	for (int i = 0; i!= TOTAL_LANDMARKS; ++i)
	{
		dist_t landmarkVertexDistance = landmarksOutputBitmap[i][vertex].distance;
		dist_t landmarkGoalDistance = landmarksOutputBitmap[i][goal].distance;

		dist_t delta;
		if (landmarkGoalDistance > landmarkVertexDistance)
		{
			delta = landmarkGoalDistance - landmarkVertexDistance;
		} 
		else
		{
			delta = landmarkVertexDistance - landmarkGoalDistance;
		}
		if (lowerBound < delta)
		{
			lowerBound = delta;
		}
	}

	return lowerBound;
}

class Comp
{
public:
    bool operator() (const Node_t& a, const Node_t& b)
    {
        return a.supposedDistance > b.supposedDistance; 
		//return a.distance > b.distance;
    }
};




// FIXME - store path as xyloc, not as a single int...
bool GetPath(void* data, xyLoc s, xyLoc g, std::vector<xyLoc> &path)
{
	vector<Node_info_t> * landmarksOutputBitmap = (vector<Node_info_t> *) data;
	int from = linearize(s);
	int to = linearize(g);

	if (inSameNSquare(s, g) > -1)
	{
		NSquarePath(path, s, g);
		return true;
	}


	vector<Node_info_t> outputMap(width * height, Node_info_t());
    priority_queue<Node_t, vector<Node_t>, Comp> Q;
	//BucketQueue Q;
    
    Node_t fromV(from, 0.0);
    Q.push(fromV);
	outputMap[from].distance = 0.0;
	outputMap[from].precursorCoords = 0;

	const int neighborsCount = 8;

	// check out all the neighbors && push them
	// starting with the one up clockwise

    while(!Q.empty()) 
    {
        // select vertex with the shortest distance
        Node_t vertex = Q.top();
        Q.pop();

		outputMap[vertex.coords].isClosed = true;
		if (vertex.coords == to)
		{
			break;
		}

		// check all the neighbors
		for (int i = 0; i < neighborsCount; ++i)
		{
			xyLoc vc = linearToCoords(vertex.coords);

			//cout << "vertex x="<<vc.x<<"y="<<vc.y << endl;
			xyLoc neighborC;
			neighborC.x = vc.x + neighbors[i].first;
			neighborC.y = vc.y + neighbors[i].second;
			
			int neighborCoords = linearize(neighborC);

			if (!ValidateLoc(neighborC) || outputMap[neighborCoords].isClosed == true)
			{
				continue;
			}

			
			// both adjacent cardinal directions must be unblocked
			int previous = (i + neighborsCount - 1) % neighborsCount;
			xyLoc previousNeighborC;
			previousNeighborC.x = vc.x + neighbors[previous].first;
			previousNeighborC.y = vc.y + neighbors[previous].second;
			//cout << "previousNeighbor x="<<previousNeighborC.x<<"y="<<previousNeighborC.y << endl;

			int next = (i + 1) % neighborsCount;
			xyLoc nextNeighborC;
			nextNeighborC.x = vc.x + neighbors[next].first;
			nextNeighborC.y = vc.y + neighbors[next].second;
			if (i % 2 == 1 && (!ValidateLoc(previousNeighborC) || !ValidateLoc(nextNeighborC)) )
			{
				continue;
			}


			// relax if necessary
			// s....u.v...t
			// if d(su)+ d(uv) < d(sv) 
			dist_t greatestLowerBound = getGreatestLowerBound(vertex.coords, to, landmarksOutputBitmap);
			dist_t lengthThroughV = outputMap[vertex.coords].distance + neighborDistances[i];
			if (lengthThroughV < outputMap[neighborCoords].distance)
			{	
				Q.push(Node_t(neighborCoords, lengthThroughV, lengthThroughV + greatestLowerBound)); 
				//Q.push(Node_t(neighborCoords, lengthThroughV, lengthThroughV)); 
				++verticesScanned;

				outputMap[neighborCoords].precursorCoords = vertex.coords;
				outputMap[neighborCoords].distance = vertex.distance + neighborDistances[i];
			}
		}
    }


    //cout << "qqvzidalenost " << outputMap[to].distance << endl;
	if (outputMap[to].isClosed == true)
	{
		//cout << "pocet skenovanych vrcholov " << verticesScanned << endl;
		//cout << "je to zavrety vrchol!" << endl;
		//vector<xyLoc> tmppath;
		int v = to;
		while (true)
		{
			//cout << "iterujem, som na " << v << endl;
			xyLoc loc = linearToCoords(v);
			path.push_back(loc);

			if (outputMap[v].precursorCoords != 0)
			{
				v = outputMap[v].precursorCoords;
			} else 
			{
				break;
			}

		}
		
		std::reverse(path.begin(), path.end());

	}

	return true;
}
//-------- END OF A*


void readVector(vector<int> * vectors, const char* filename)
{
	FILE * file = fopen(filename, "r");
	for (int i = 0; i != width*height; ++i)
	{
		int num;
		fscanf(file, "%d", &num);
		vectors[0].push_back(num); 
	}

	for (int i = 0; i != width*height; ++i)
	{
		int num;
		fscanf(file, "%d", &num);
		vectors[1].push_back(num); 
	}
	fclose(file);

}
void printVector(const vector<int> & v, const char* filename)
{
	FILE * file = fopen(filename, "a");
	for (int i = 0; i < v.size(); ++i)
	{
		fprintf(file, "%d ", v[i]);
	}
	fprintf(file, "\n");
	fclose(file);
}
void PreprocessMap(std::vector<bool> &bits, int w, int h, const char *filename)
{
	map = bits;
	width = w;
	height = h;

	printVector(Colorize(0), filename);
	printVector(Colorize(1), filename);
}


void *PrepareForSearch(std::vector<bool> &bits, int w, int h, const char *filename)
{
	map = bits;
	width = w;
	height = h;

	readVector(NSquares, filename);


	// I did some quick dijkstra in this function since it really takes VERY little time (just some milliseconds)
	// if this should be a reason for disqualification, I do apologize and I will reprogram in into the PreprocessMap...
	// this whole function calls dijkstra 6times on the map
	
	// pick TOTAL_LANDMARKS/2 random points

	int pos = 0;
	int landmarks[TOTAL_LANDMARKS];

	int randomVertex;
	for(int i = 0; i < TOTAL_LANDMARKS/2; ++i)
	{
		randomVertex = (rand() % (width * height));
		//cout << randomVertex << endl;

		while (!map[randomVertex])
		{
			randomVertex = (randomVertex+1) % (width*height);
		}
		landmarks[pos++] = randomVertex;
		// cout << "nas pokus " << p << endl;
	}

	// cout << "zbyvalo pokusov" << attempts << endl;

	vector<Node_info_t>* landmarkOutputBitmaps = new vector<Node_info_t>[TOTAL_LANDMARKS];
	for (int i=0; i != TOTAL_LANDMARKS; ++i)
	{
		Dijkstra(linearToCoords(landmarks[i]), landmarkOutputBitmaps[i]);

		if (i < TOTAL_LANDMARKS/2) 
		{
			// find most distant points
			int mostDistantVertex = 0;
			dist_t mostDistantDistance = 0;
			for (int tmp=0; tmp!= TOTAL_LANDMARKS; ++tmp)
				//cout << "size "<<landmarkOutputBitmaps[i].size() << endl;
			for (unsigned int j=0; j != landmarkOutputBitmaps[i].size(); ++j)
			{
				//cout <<"dist"<<landmarkOutputBitmaps[i][j].distance << 
				//	"j"<<j<<	endl;
				if (landmarkOutputBitmaps[i][j].distance > mostDistantDistance && landmarkOutputBitmaps[i][j].distance+1 < maxDistance)
				{
					mostDistantVertex = j;
					mostDistantDistance = landmarkOutputBitmaps[i][j].distance;
					//cout <<"MDD " << mostDistantDistance << endl;
				}
			}
			landmarks[TOTAL_LANDMARKS/2 + i] = mostDistantVertex;
		}
	}

	return landmarkOutputBitmaps;
}

const char *GetName() 
{
	return "NovellA*";
}




void LoadMap(const char *fname, std::vector<bool> &map, int &width, int &height)
{
	FILE *f;
	f = fopen(fname, "r");
	if (f)
    {
		fscanf(f, "type octile\nheight %d\nwidth %d\nmap\n", &height, &width);
		map.resize(height*width);
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				char c;
				do {
					fscanf(f, "%c", &c);
				} while (isspace(c));
				map[y*width+x] = (c == '.' || c == 'G' || c == 'S');
			}
		}
		fclose(f);
    }
}

template<class T>
void printMap(const T& m)
{
	for (int i=0; i != width*height; ++i)
	{
		cout << m[i];
		if (i %width == width-1 && i != 0)
		{
			cout << endl;
		}
	}
	cout << endl;
}

struct HelpMap {
	HelpMap(const char *m, int a, int b, int c, int d): map(m), xfrom(a), yfrom(b),xto(c), yto(d)
	{
	}
	const char * map;
	int xfrom;
	int yfrom;
	int xto;
	int yto;
};
HelpMap m("mapa.map", 1,1,7,1);
//HelpMap m("Aftershock.map", 163,428,170,427);
//HelpMap m("Aftershock.map", 1,130,9,113); //20.3136
//HelpMap m("Aftershock.map", 442,8,503,495); //726.247
//HelpMap m("Aftershock.map", 490, 264, 488, 260); //4.82843
int main()
{
	LoadMap(m.map, map, width, height);

	vector<Node_info_t> outputMap;
	PreprocessMap(map, width, height, "foofile");
	void * reference = PrepareForSearch(map, width, height, "foofile");

	vector<xyLoc> thePath;
	for(int i=0; i != 1; ++i)
	{
		xyLoc s, g;
		s.x = m.xfrom;
		s.y = m.yfrom;
		g.x = m.xto;
		g.y = m.yto;

		bool done = GetPath(reference, s, g, thePath);
	}

	cout << "velkost thePath (ideal length=726.247)" << thePath.size() << endl;
	for (int i=0; i != thePath.size(); ++i)
	{
		//cout << "x=" <<thePath[i].x<<" y="<<thePath[i].y << endl;
	}


	printMap(map);


	cout << "colorized graph" << endl;
	//vector<int> colorizedGraph = Colorize(1);
	printMap(NSquares[0]);

	cout << "colorized graph - 2 " << endl;
	//colorizedGraph = Colorize(0);
	printMap(NSquares[1]);
	

	system("pause");
    return 0;
}


