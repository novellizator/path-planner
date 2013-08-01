#ifndef GLOBALS_H
#define GLOBALS_H


#include <vector>
#include "Entry.h"

// typedefs
typedef double dist_t;


// constants - unchangable ones
const dist_t tangent = 1.4142;
const dist_t straight = 1.0;
const dist_t maxDistance = 999999.0;


// user-set
const int TOTAL_LANDMARKS = 6;
// do not forget that 0 is considered color - doesn't matter because all paths given in requests are valid
const int SMALLEST_COLORIZED_RECTANGLE = 500; // this is the smallest possible rectangle area to look for when colorizing the graph


// the map with its key attributes: width and height. Defined in Entry.cpp
extern int width, height;
extern std::vector<bool> map;


extern int verticesScanned;


// structures

// used as structure for storing  element in the heap when running dijkstra or A*
struct Node_t
{
	Node_t(int c = 0, dist_t d = 0.0, dist_t sd = 0.0): coords(c), distance(d), supposedDistance(sd)
	{
	}
	
    int coords;
    dist_t distance;
	dist_t supposedDistance; // "lower bound"
};

// this is a structure an array of which is saved after running dijkstra or a*
struct Node_info_t
{
	Node_info_t(): distance(maxDistance), precursorCoords(0), isClosed(false)
	{
	}
	dist_t distance;
	int precursorCoords;
	bool isClosed;
};

inline int linearize(int y, int x)
{
	return y * width + x;
}

inline int linearize(const xyLoc& coords)
{
    return coords.y * width + coords.x;
}


inline xyLoc coordinatize(int linear)
{
	xyLoc coords;
	coords.x = linear % width;
	coords.y = linear / width;
	return coords;
}


extern bool ValidateLoc(const xyLoc& loc);

#endif