#include "globals.h"
#include "Entry.h"




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
