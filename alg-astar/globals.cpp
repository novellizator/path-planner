#include "globals.h"
#include "Entry.h";


int linearize(int y, int x)
{
	return y * width + x;
}

int linearize(const xyLoc& coords)
{
    return coords.y * width + coords.x;
}


xyLoc coordinatize(int linear)
{
	xyLoc coords;
	coords.x = linear % width;
	coords.y = linear / width;
	return coords;
}

bool ValidateLoc(const xyLoc& loc)
{
	if (loc.y <= -1 || loc.y >= height ||
		loc.x <= -1 || loc.x >= width)
	{
		return false;
	}

	int locCoords = linearize(loc);
	if(map[locCoords] == false) 
	{
		return false;
	}

	return true;
}