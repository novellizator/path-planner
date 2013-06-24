#ifndef NSQUARE_H
#define NSQUARE_H

#include <vector>

std::vector<int> colorize();
bool isSameColor(const xyLoc& s, const xyLoc& g);
void setSameColorPath(std::vector<xyLoc>& path, const xyLoc& s, const xyLoc& g);


#endif
