#ifndef ENTRY_H
#define ENTRY_H

#include <stdint.h>
#include <vector>

struct xyLoc {
	xyLoc()
	{}

	xyLoc(int16_t xx, int16_t  yy): x(xx), y(yy)
	{}

	int16_t x;
	int16_t y;
};

void PreprocessMap(std::vector<bool> &bits, int width, int height, const char *filename);
void *PrepareForSearch(std::vector<bool> &bits, int width, int height, const char *filename);
bool GetPath(void *data, xyLoc s, xyLoc g, std::vector<xyLoc> &path);
const char *GetName();


#endif