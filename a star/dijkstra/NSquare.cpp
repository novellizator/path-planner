#include "globals.h"
#include "Entry.h"
#include "NSquare.h"

using namespace std;

// ------- START OF NSquare algorithm
void HorizontallyColorize(vector<int>& coloredMap, int vertex, int color)
{
	xyLoc startCoords = linearToCoords(vertex);

	// start with the first row
	xyLoc vertexC = linearToCoords(vertex);
	while (ValidateLoc(vertexC)) 
	{
		if (map[vertex] == 1 && coloredMap[vertex] == 0)
		{
			coloredMap[vertex] = color;

			++vertexC.x;
			vertex = linearize(vertexC);
		}
		else break;
	}
	xyLoc endCoords = linearToCoords(vertex); // one coord AFTER the last valid coord

	xyLoc vertexIter = startCoords;
	++vertexIter.y;
	while(ValidateLoc(vertexIter))
	{
		// check whole line
		bool validLine = true;
		for (int i = startCoords.x; i < endCoords.x; ++i)
		{
			vertexIter.x = i;
			int v = linearize(vertexIter);
			if (map[v] == 0 || coloredMap[v] != 0)
			{
				validLine = false;
				break;
			}
		}

		if (validLine)
		{
			for (int i = startCoords.x; i < endCoords.x; ++i)
			{
				vertexIter.x = i;
				int v = linearize(vertexIter);
				coloredMap[v] = color;
			}
		}
		++vertexIter.y;
	}
}


void VerticallyColorize(vector<int>& coloredMap, int vertex, int color)
{
	xyLoc startCoords = linearToCoords(vertex);

	// start with the first col
	xyLoc vertexC = linearToCoords(vertex);
	while (ValidateLoc(vertexC)) 
	{
		if (map[vertex] == 1 && coloredMap[vertex] == 0)
		{
			coloredMap[vertex] = color;

			++vertexC.y;
			vertex = linearize(vertexC);
		}
		else break;
	}
	xyLoc endCoords = linearToCoords(vertex); // one coord AFTER the last valid coord

	xyLoc vertexIter = startCoords;
	++vertexIter.x;
	while(ValidateLoc(vertexIter))
	{
		// check whole line
		bool validLine = true;
		for (int i = startCoords.y; i < endCoords.y; ++i)
		{
			vertexIter.y = i;
			int v = linearize(vertexIter);
			if (map[v] == 0 || coloredMap[v] != 0)
			{
				validLine = false;
				break;
			}
		}

		if (validLine)
		{
			for (int i = startCoords.y; i < endCoords.y; ++i)
			{
				vertexIter.y = i;
				int v = linearize(vertexIter);
				coloredMap[v] = color;
			}
		}
		++vertexIter.x;
	}
}

// Novella's decomposition ;-)
vector<int> Colorize(int type)
{
	vector<int> coloredMap(width * height, 0);
	int color = 1;
	for (int i=0; i < width*height; ++i)
	{
		if (map[i] == 1 && coloredMap[i] == 0)
		{
			if (type == 0)
			{
				VerticallyColorize(coloredMap, i, color);
			}
			else 
			{
				HorizontallyColorize(coloredMap, i, color);
			}
			++color;
		}
	}

	return coloredMap;
}


// -1 -> different squares, otherwise return which NSquare
int inSameNSquare(const xyLoc& s, const xyLoc& g)
{
	int linS = linearize(s);
	int linG = linearize(g);

	for (int i = 0; i < 2; ++i)
	{
		if (NSquares[i][linS] == NSquares[i][linG])
			return i;
	}
	return -1;
}
	
void NSquarePath(vector<xyLoc>& path, xyLoc s, const xyLoc& g)
{
	path.push_back(s);

	while ((s.x - g.x != 0) || (s.y - g.y != 0))
	{
		if (s.x < g.x) ++s.x;
		if (s.x > g.x) --s.x;

		if (s.y < g.y) ++s.y;
		if (s.y > g.y) --s.y;

		path.push_back(s);
	}
}
// ------- END OF NSquare algorithm