#include "globals.h"
#include "Entry.h";


int linearize(const xyLoc& coords)
{
    return coords.y * width + coords.x;
}


xyLoc linearToCoords(int linear)
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