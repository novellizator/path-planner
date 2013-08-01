#include <cstdio>
#include <iomanip>
#include <cstdlib>
#include <queue>
#include <iostream>
#include <algorithm> // reverse() 
#include <vector>

#include "Entry.h"
#include "globals.h"
#include "colorize.h"
#include "BucketQueue.h"

using namespace std;


// extern variable defined here
int verticesScanned = 0;
int width, height;

vector<bool> map; 
vector<int> colorizedMap; // integer - signifies the color of a square
//Colorizator colorizator;


// neighbors 
// starting with the one upwards, going clockwise
const int neighborsCount = 8;
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



    while(!Q.empty()) 
    {
        // select vertex with the shortest distance
        Node_t vertex = Q.top();
        Q.pop();

		outputMap[vertex.coords].isClosed = true;
	
		// check all the neighbors
		for (int i = 0; i < neighborsCount; ++i)
		{

			xyLoc vc = coordinatize(vertex.coords);

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



bool GetPath(void* data, xyLoc s, xyLoc g, std::vector<xyLoc> &path)
{
	vector<Node_info_t> * landmarksOutputBitmap = (vector<Node_info_t> *) data;
	int from = linearize(s);
	int to = linearize(g);

	if (isSameColor(s, g))
	{
	    cout << "YEAH SAME SQUARE" << endl;
		setSameColorPath(path, s, g);
		return true;
	}


	vector<Node_info_t> outputMap(width * height, Node_info_t());
    priority_queue<Node_t, vector<Node_t>, Comp> Q;
	//BucketQueue Q;
    
    Node_t fromV(from, 0.0);
    Q.push(fromV);
	outputMap[from].distance = 0.0;
	outputMap[from].precursorCoords = 0;

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
			xyLoc vc = coordinatize(vertex.coords);

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


	// construct the path
	if (outputMap[to].isClosed == true) // makes sure I found the path
	{

		int v = to;
		while (true)
		{
			xyLoc loc = coordinatize(v);
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


void readVector(vector<int>& colorizedMap, const char* filename)
{
	FILE * file = fopen(filename, "r");
	for (int i = 0; i != width*height; ++i)
	{
		int num;
		fscanf(file, "%d", &num);
		colorizedMap.push_back(num); 
	}

	fclose(file);

}
void writeVector(const vector<int> & v, const char* filename)
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

	colorizedMap.resize(width * height, DEFAULT_COLOR);

	writeVector(colorizedMap, filename);
}


int getRandomValidVertex()
{
	int randomVertex = rand() % (width * height);

	while (!map[randomVertex])
	{
		randomVertex = (randomVertex+1) % (width * height);
	}
	return randomVertex;
}

vector<Node_info_t>* pickLandmarks()
{
	int landmarks[TOTAL_LANDMARKS];

	for(int i = 0; i < TOTAL_LANDMARKS/2; ++i)
	{
		landmarks[i] = getRandomValidVertex();
	}

	vector<Node_info_t>* landmarkOutputBitmaps = new vector<Node_info_t>[TOTAL_LANDMARKS];
	for (int i=0; i != TOTAL_LANDMARKS; ++i)
	{
		Dijkstra(coordinatize(landmarks[i]), landmarkOutputBitmaps[i]);

		if (i < TOTAL_LANDMARKS/2) 
		{
			// find most distant points
			int mostDistantVertex = 0;
			dist_t mostDistantDistance = 0;

			for (size_t j=0; j != landmarkOutputBitmaps[i].size(); ++j)
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


void * PrepareForSearch(std::vector<bool> &bits, int w, int h, const char *filename)
{
	map = bits;
	width = w;
	height = h;

	//vector<int> colorizedMap;
	readVector(colorizedMap, filename);
	//colorizator.setColorizedMap(colorizedMap);

	// this whole function calls dijkstra 6times on the map
	vector<Node_info_t>* landmarkOutputBitmaps = pickLandmarks();
	// pick TOTAL_LANDMARKS/2 random points



	return landmarkOutputBitmaps;
}

const char *GetName() 
{
	return "NovellA*";
}




template<class T>
void printMap(const T& m)
{
	for (int i=0; i != width*height; ++i)
	{
		cout << m[i] <<"\t";
		if (i %width == width-1 && i != 0)
		{
			cout << endl;
		}
	}
	cout << endl;
}

vector<bool> push(const char * str)
{
    vector<bool> ret;
    while (*str != '\0')
    {
        if (*str == '0') 
            ret.push_back(false);
        else
            ret.push_back(true);
        ++str;
    }
    return ret;
}


