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
const int SMALLEST_COLORIZED_RECTANGLE = 500; // naturally, the smallest possible value is 2; FIXME all others (not found ones) will have the same value???


extern int width, height;
extern std::vector<bool> map;
//extern std::vector<int> colorizedMap; // novella's squares (colorized map, int=color)

// extern Colorizator col;

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

// FIXME speed-up tip: inline
extern int linearize(const xyLoc& coords);
extern int linearize(int y, int x);
extern xyLoc coordinatize(int linear);



extern bool ValidateLoc(const xyLoc& loc);

#endif