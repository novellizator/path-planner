#include <cstdio>
#include <iomanip>
#include <cstdlib>
#include <queue>
#include <iostream>


using namespace std;


typedef pair<int, int> Coords;
typedef double dist_t;

int width, height;
const dist_t tangent = 1.4142;
const dist_t straight = 1.0;
const dist_t maxDistance = 999999.0;
vector<bool> map;


// used in heap
struct Node_t
{
	Node_t(int c = 0, dist_t d = 0.0): coords(c), distance(d)
	{
	}
	
    int coords;
    dist_t distance;
};

// special data structure that makes the heap linear
class BucketQueue
{
public:
	BucketQueue():size(0), baseDistance(1), currentIndex(0)
	{
		for (int i = 0; i != 3; ++i)
		{
			baseElements[i] = new vector<Node_t>();
			baseIndexes[i] = 0;
		}
	}


	bool empty()
	{
		return size == 0;
	}


	void push(Node_t & element)
	{
		++size;
		if (element.distance > baseDistance + 1.999)
		{
			pushOrInsert(2, element);
		}
		else if(element.distance > baseDistance + 0.999)
		{
			pushOrInsert(1, element);
		}
		else
		{
			pushOrInsert(0, element);
		}
	}


	Node_t & top()
	{
		if (empty())
			throw std::exception("empty queue!!!");

		return (*baseElements[0])[currentIndex]; // FIXME? in the beginnign there must me at least one element here baseElements[0]
	}


	// pops the element and sets currentIndex to valid value
	void pop()
	{
		if (empty())
			throw std::exception("empty!!!");

		--size;

		++currentIndex; // move to the next thing

		// check if correct
		for (int i=0; i<3; ++i)
			if (currentIndex == baseIndexes[0])
			{
				// baseIndexes[0] = 0;
				currentIndex = 0;
				++baseDistance;

				vector<Node_t>* tmpVecPtr = baseElements[0];

				for (int oldBase = 1; oldBase <= 2; ++oldBase)
				{
					baseElements[oldBase - 1] = baseElements[oldBase];
					baseIndexes[oldBase - 1] = baseIndexes[oldBase];
				}
				baseElements[2] = tmpVecPtr;
				baseIndexes[2] = 0;
			}
			else
				break;


	}

private:
	int size;
	int baseDistance;

	int currentIndex;
	// <baseDistance, baseDistance+1), 
	// <baseDistance+1, baseDistance+2),
	// <baseDistance2, baseDistance+2.41)
	vector<Node_t>* baseElements[3];
	int baseIndexes[3]; // index in the baseElementsx vector to which the element is to be pushed

	
	

	// arrayIndex \element {0, 1, 2}
	void pushOrInsert(int arrayIndex, Node_t& element)
	{
		// should I push back?
		if (baseIndexes[arrayIndex] >= baseElements[arrayIndex]->size())
		{
			baseElements[arrayIndex]->push_back(element);
		}
		else
		{
			(*baseElements[arrayIndex])[baseIndexes[arrayIndex]] = element;
		}
		++baseIndexes[arrayIndex];
	}
};

struct Node_info_t
{
	Node_info_t(): distance(maxDistance), precursorCoords(0), isClosed(false)
	{
	}
	dist_t distance;
	int precursorCoords;
	bool isClosed;
};


int linearize(int y, int x)
{
    return y * width + x;
}
int linearize(Coords& coords)
{
    return coords.first * width + coords.second;
}

Coords linearToCoords(int linear)
{
	return make_pair(linear / width, linear % width);
}


//class Comp
//{
//public:
//    bool operator() (const Node_t& a, const Node_t& b)
//    {
//        return a.distance > b.distance;
//    }
//};


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
vector<Node_info_t> dijkstra(int from, int to)
{
    
    vector<Node_info_t> outputMap(width * height, Node_info_t());
    //priority_queue<Node_t, vector<Node_t>, Comp> Q;
	BucketQueue Q;
    
    Node_t fromV(from, 0);
    Q.push(fromV);
	outputMap[from].distance = 0.0;

	const int neighborsCount = 8;

	// check out all the neighbors && push them
	// starting with the one up clockwise
	pair<int, int> neighbors[] = { // [y, x]
		make_pair(-1, 0),
		make_pair(-1, 1),
		make_pair(0, 1),
		make_pair(1, 1),
		make_pair(1, 0),
		make_pair(1, -1),
		make_pair(0, -1),
		make_pair(-1, -1),
	};

	dist_t neighborDistances[] = {
		straight,
		tangent,
		straight,
		tangent,
		straight,
		tangent,
		straight,
		tangent
	};

	// row by row

/*	int neighbors[] = {
		-width - 1,
		-width,
		-width + 1,
		-1,
		+1,
		+width - 1,
		+width,
		+width + 1
	};
	double neighborDistances[] = {
		tangent,
		straight,
		tangent,
		
		straight,
		straight,
		
		tangent,
		straight,
		tangent
	};
*/
    while(!Q.empty()) 
    {
        // select vertex with the shortest distance
        Node_t vertex = Q.top();
        Q.pop();
		//cout << "som vo vertexe" << vertex.coords << endl;
		if (vertex.coords == to)
		{
			//break;
		}


		outputMap[vertex.coords].isClosed = true;

		// check all the neighbors
		for (int i = 0; i < neighborsCount; ++i)
		{
			Coords vc = linearToCoords(vertex.coords);
			Coords neighborC = make_pair(vc.first + neighbors[i].first,
				vc.second + neighbors[i].second);

			int neighborCoords = linearize(neighborC);

			if (neighborC.first == -1 || neighborC.first == height ||
				neighborC.second == -1 || neighborC.second == width ||
				map[neighborCoords] == false ||
				outputMap[neighborCoords].isClosed == true)
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


    
    // printf("%d\n", sizeof(*mapa));
    return outputMap;
}



const char *GetName() 
{
	return "NovellA*";
}






struct HelpMap {
	HelpMap(char *m, int a, int b, int c, int d): map(m), xfrom(a), yfrom(b),xto(c), yto(d)
	{
	}
	char * map;
	int xfrom;
	int yfrom;
	int xto;
	int yto;
};
//HelpMap m("mapa.map", 1,1,1,3);
//HelpMap m("Aftershock.map", 163,428,170,427);
HelpMap m("Aftershock.map", 1,130,9,113); //20.3136
int main()
{
	LoadMap(m.map, map, width, height);

	vector<Node_info_t> outputMap;
	for (int i=0; i!= 300; ++i)
		outputMap = dijkstra(linearize(m.yfrom,m.xfrom), linearize(m.yto,m.xto));


	cout << outputMap[linearize(m.yto,m.xto)].distance << endl;

  //  for(int i=0; i< width*height ; ++i) 
  //  {
		//Node_info_t node = outputMap[i];
  //   
		//cout << std::right << std::setw(8) <<node.distance ;
  //      if ((i+1) % width == 0) 
  //          putchar('\n');
  //  }

	system("pause");
    return 0;
}


