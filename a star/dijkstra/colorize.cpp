#include <stack>

#include "globals.h"
#include "Entry.h"
#include "colorize.h"

using namespace std;


const int DEFAULT_COLOR = 0;


struct dataAboutRectangle {
	dataAboutRectangle(int l = 0, int s = 0): left(l), sequenceSize(s) 
	{}


	int getArea() const
	{
		return left * sequenceSize;
	}

	int left;
	int sequenceSize; 

};
struct StackStruct {
	StackStruct(int r=-1, int v=0):row(r), val(v)
	{}

	int row;
	int val;
};

bool validLoc(const xyLoc & loc)
{
	if (loc.x < 0 || loc.y < 0) 
		return false; 
	return true;
}

int getLefts(const vector<dataAboutRectangle>& data, const xyLoc& loc)
{
	if (validLoc(loc)) 
		return data[linearize(loc)].left; 
	else 
		return 0;
}



pair<int, dataAboutRectangle> findLargestRectangle()
{
	vector<dataAboutRectangle> data(map.size(), dataAboutRectangle());

	// fill the data vector
    for(int i = 0; i != map.size(); ++i)
    {
        if (map[i] == 0)
        {
            data[i].left = 0;
        } else
        {
            xyLoc loc = coordinatize(i);
			xyLoc leftLoc = loc;
			--leftLoc.x;

            data[i].left = getLefts(data, leftLoc) + 1;
        }
	}


	for (int c = 0; c < width; ++c)
	{
		stack<StackStruct> S;
		for (int r = 0; r < height; ++r)
		{
			int val = data[linearize(r,c)].left;

			while (!S.empty())
			{
				// pop all the elements greater than the element given now
				StackStruct stackTop = S.top();
				if (stackTop.val > val)
				{
					data[linearize(stackTop.row, c)].sequenceSize = r - stackTop.row;
					S.pop();
				}
				else break;
			}

			// push the given element
			S.push(StackStruct(r, val));

		}

		// empty the stack
		while (!S.empty())
		{
			// pop all the elements greater than the element given now
			StackStruct stackTop = S.top();
		
			data[linearize(stackTop.row, c)].sequenceSize = height - stackTop.row;
			S.pop();
		}
	}

	// sums bude hovorit v pravom hornom rohu, kde zacina obdlznik
	vector<int> sums(map.size(), 0);
	int maxSum = 0;
	int maxI=0;
	// vector<int> sums(map.size(), 0);
	for (int i = 0; i != map.size(); ++i)
	{
		//int x = (i % width) - data[i].left + 1;
		//int y = i / width;
		//xyLoc c; c.x = x; c.y = y;
		//sums[i] = data[i].getArea();
		int sum = data[i].getArea();
		if (sum > maxSum)
		{
			maxSum = sum;
			maxI = i;
		}
	}


	pair<int, dataAboutRectangle> ret;
	ret.first = maxI;
	ret.second = data[maxI];

	return ret;
}
void colorizeRectangle(const pair<int, dataAboutRectangle>& rectangle, int color)
{
	int vertex = rectangle.first;
	// ... TODO
}


vector<int> colorize() 
{
	vector<int> colorizedMap(width * height, DEFAULT_COLOR);

	int color = DEFAULT_COLOR + 1;
	pair<int, dataAboutRectangle> largestRectangle = findLargestRectangle();
	while (largestRectangle.second.getArea() >= SMALLEST_COLORIZED_RECTANGLE)
	{
		colorizeRectangle(largestRectangle, color++);
	}

	return colorizedMap;
}


//int main()
//{
//	
//	width = 4;
//	height = 3;
//
//  //  for(int l= 0; l < 16; ++l)
//		//cout << "lokace" << getLoc(l).x <<" "<< getLoc(l).y << endl;
//
//    // vector<bool> map = push("011100110111"); // ok
//    // vector<bool> map = push("111010101111"); // ok
//    // vector<bool> map = push("110111111111");
//	vector<bool> map = push("110110111011");
//    findRectangle(map);
//    system("pause");
//    return 0;
//}
//-------------------------------------------------



bool isSameColor(const xyLoc& s, const xyLoc& g)
{
	int linS = linearize(s);
	int linG = linearize(g);

	
	if (colorizedMap[linS] == colorizedMap[linG] && colorizedMap[linS] != DEFAULT_COLOR)
		return true;
	
	return false;
}
	
void setSameColorPath(vector<xyLoc>& path, const xyLoc& source, const xyLoc& g)
{
	xyLoc s = source;
	path.push_back(s);

	while ((s.x - g.x != 0) || (s.y - g.y != 0))
	{
		if (s.x < g.x) ++s.x;
		if (s.x > g.x) --s.x;

		if (s.y < g.y) ++s.y;
		if (s.y > g.y) --s.y;

		path.push_back(s);
	}
}