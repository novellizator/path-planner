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


//Node_t make_node_pair(int coords, int distance)
//{
//    Node_t ret;
//    ret.coords = coords;
//    ret.distance = distance;
//
//    return ret;
//}
//
//
//Node_t make_node_pair(const Coords& coords, int distance)
//{
//    Node_t ret;
//    ret.coords = coords.first * width + coords.second;
//    ret.distance = distance;
//    return ret;
//}

class Comp
{
public:
    bool operator() (const Node_t& a, const Node_t& b)
    {
        return a.distance > b.distance;
    }
};


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
				//printf("%c", c);
			}
			//printf("\n");
		}
		fclose(f);
    }
}

//void loadMap(const char *infile) 
//{
//    FILE * fp = fopen(infile, "r");
//    if (fp == NULL) 
//    {
//        puts("Error - cannot open map file!");
//        exit(1);
//    };
//
//    fscanf(fp, "width %d\n", &width);
//    fscanf(fp, "height %d\n", &height);
//
//    int i, j;
//    for (i = 0; i < height; ++i) 
//    {
//		char c;
//        for(j = 0; j < width; ++j)
//        {
//            c = fgetc(fp);
//            bool val = (c == '.');
//            map[i][j] = val;
//            
//            if (j == width - 1)
//            {
//                fgetc(fp); // '\n'
//            } 
//        }
//    }
//}



vector<Node_info_t> dijkstra(int from, int to)
{
    
    vector<Node_info_t> outputMap(width * height, Node_info_t());
    priority_queue<Node_t, vector<Node_t>, Comp> Q;

    
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
			//int neighborCoords = vertex.coords + neighbors[i];
			Coords vc = linearToCoords(vertex.coords);
			Coords neighborC = make_pair(vc.first + neighbors[i].first,
				vc.second + neighbors[i].second);

			int neighborCoords = linearize(neighborC);

			if (neighborC.first == -1 || neighborC.first == height ||
				neighborC.second == -1 || neighborC.second == width ||
				map[neighborCoords] == false ||
				outputMap[neighborCoords].isClosed == true)
			/* // problem so sikmymi vrcholmi, ci nepretekaju
			if (neighborCoords < 0 || 
				neighborCoords > height * width ||
				((vertex.coords % width == 0) &&(neighbors[i] == -1)) ||
				((vertex.coords % width == width - 1) &&(neighbors[i] == 1)) ||
				map[c.first][c.second] == false ||
				outputMap[neighborCoords].isClosed == true
				
			) */
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
HelpMap m("Aftershock.map", 1,130,9,113);
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


