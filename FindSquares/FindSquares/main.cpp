#include <iostream>
#include <stdint.h>
#include "globals.h"
#include "colorize.h"

using namespace std;


int width, height;
vector<bool> map;
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
//HelpMap m("mapa.map", 1,1,7,1);
HelpMap m("Aftershock.map", 163,428,170,427);
//HelpMap m("Aftershock.map", 1,130,9,113); //20.3136
//HelpMap m("Aftershock.map", 442,8,503,495); //726.247
//HelpMap m("Aftershock.map", 490, 264, 488, 260); //4.82843
int main()
{

	LoadMap(m.map, map, width, height);
	
	string outFilename = m.map;
	//outFilename += itoa(SMALLEST_COLORIZED_RECTANGLE);
	outFilename += ".colorized";

	//freopen( outFilename.c_str(), "w", stdout );
	//printMap(map);
	Colorizator col;
	col.colorize();
	vector<int> cMap = col.getColorizedMap();
	printMap(cMap);

	////vector<Node_info_t> outputMap;
	////PreprocessMap(map, width, height, "foofile");
	////void * reference = PrepareForSearch(map, width, height, "foofile");

	////vector<xyLoc> thePath;
	////for(int i=0; i != 1; ++i)
	////{
	////	xyLoc s, g;
	////	s.x = m.xfrom;
	////	s.y = m.yfrom;
	////	g.x = m.xto;
	////	g.y = m.yto;

	////	bool done = GetPath(reference, s, g, thePath);
	////}


	////cout << "velkost thePath (ideal length=726.247)" << thePath.size() << endl;
	////for (int i=0; i != thePath.size(); ++i)
	////{
	////	//cout << "x=" <<thePath[i].x<<" y="<<thePath[i].y << endl;
	////}


	//printMap(map);


	//cout << "colorized graph" << endl;
	//vector<int> colorizedGraph = Colorize(1);
	//printMap(colorizedMap);
	

	//system("pause");
    return 0;
}



