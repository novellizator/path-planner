#ifndef NSQUARE_H
#define NSQUARE_H


#include <vector>


void HorizontallyColorize(std::vector<int>& coloredMap, int vertex, int color);
void VerticallyColorize(std::vector<int>& coloredMap, int vertex, int color);
std::vector<int> Colorize(int type);
int inSameNSquare(const xyLoc& s, const xyLoc& g);
void NSquarePath(std::vector<xyLoc>& path, xyLoc s, const xyLoc& g);


#endif
