#ifndef NSQUARE_H
#define NSQUARE_H

#include <vector>
#include <stack>
#include <iostream>
using std::stack;
using std::pair;
using std::vector;


struct dataAboutRectangle 
{
	dataAboutRectangle(int l = 0, int u = 0, int d = 0): left(l), sequenceSizeUp(u), sequenceSizeDown(d) 
	{}


	int getArea() const
	{
		return left * (sequenceSizeUp + sequenceSizeDown + 1); // 1 = myself
	}
	
	pair<int, int> getTLPoint() const // TODO, also getWidth, getHeight...
	{
		return std::make_pair(1, 1);
	}
	int left;
	int sequenceSizeUp;
	int sequenceSizeDown; 

};



struct StackStruct {
	StackStruct(int r=-1, int v=0):row(r), val(v)
	{}

	int row;
	int val;
};



bool validLoc(const xyLoc & loc);
bool isColorizable(int vertex) ;
int getLefts(const vector<dataAboutRectangle>& data, const xyLoc& loc) ;
pair<int, dataAboutRectangle> findLargestRectangle();
void colorizeRectangle(const pair<int, dataAboutRectangle>& rectangle, int color);
vector<int> colorize();
bool isSameColor(const xyLoc& s, const xyLoc& g);
void setSameColorPath(vector<xyLoc>& path, const xyLoc& source, const xyLoc& g);




#endif
