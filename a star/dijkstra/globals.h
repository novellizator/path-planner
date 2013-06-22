#ifndef GLOBALS_H
#define GLOBALS_H


#include <vector>
#include "Entry.h"

// typedefs
typedef double dist_t;


// constants
const dist_t tangent = 1.4142;
const dist_t straight = 1.0;
const dist_t maxDistance = 999999.0;
const int TOTAL_LANDMARKS = 6;


extern int width, height;
extern std::vector<bool> map;
extern std::vector<int> NSquares[2]; // novella's squares (colorized map, int=color)


extern int verticesScanned;


// structures
// used in the heap
struct Node_t
{
	Node_t(int c = 0, dist_t d = 0.0, dist_t sd = 0.0): coords(c), distance(d), supposedDistance(sd)
	{
	}
	
    int coords;
    dist_t distance;
	dist_t supposedDistance;
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

extern int linearize(const xyLoc& coords);
extern xyLoc linearToCoords(int linear);
extern bool ValidateLoc(const xyLoc& loc);

#endif