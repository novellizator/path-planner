#include <stack>

#include "globals.h"
#include "Entry.h"
#include "colorize.h"

using namespace std;





	bool validLoc(const xyLoc & loc) 
	{
		if (loc.x < 0 || loc.y < 0) 
			return false; 
		return true;
	}
	bool isColorizable(int vertex) 
	{
		if (map[vertex] && colorizedMap[vertex] == DEFAULT_COLOR)
			return true;
		else
			return false;
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
			if (!isColorizable(i))
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

			// 1) go from the top all the way down
			for (int r = 0; r < height; ++r)
			{
				int val = data[linearize(r,c)].left;

				while (!S.empty())
				{
					// pop all the elements greater than the element given now
					StackStruct stackTop = S.top();
					if (stackTop.val > val)
					{
						data[linearize(stackTop.row, c)].sequenceSizeDown = r - stackTop.row - 1; //excluding actual row
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
		
				data[linearize(stackTop.row, c)].sequenceSizeDown = height - stackTop.row - 1;
				S.pop();
			}


			// 2) and now reverse it
			for (int r = height-1; r >= 0; --r)
			{
				int val = data[linearize(r,c)].left;

				while (!S.empty())
				{
					// pop all the elements greater than the element given now
					StackStruct stackTop = S.top();
					if (stackTop.val > val)
					{
						data[linearize(stackTop.row, c)].sequenceSizeUp = stackTop.row - r - 1; //excluding actual row
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
		
				data[linearize(stackTop.row, c)].sequenceSizeUp = stackTop.row;// == stackTop.row - (-1) - 1;
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
		xyLoc loc = coordinatize(vertex);

		for (int c = loc.x - rectangle.second.left + 1; c <= loc.x; ++c)		
		{
			for (int r = loc.y - rectangle.second.sequenceSizeUp ; r <= loc.y + rectangle.second.sequenceSizeDown; ++r)
			{
				colorizedMap[linearize(r, c)] = color;
			}
		}
	}
	vector<int> colorize() 
	{
		float debug_area = 0;
		int color = DEFAULT_COLOR + 1;
		pair<int, dataAboutRectangle> largestRectangle = findLargestRectangle();
		while (largestRectangle.second.getArea() >= SMALLEST_COLORIZED_RECTANGLE)
		{
			colorizeRectangle(largestRectangle, color++);
			largestRectangle = findLargestRectangle();
			debug_area += largestRectangle.second.getArea();
		}

		//std::cout << "podiel je" << width*height/debug_area << std::endl;
		return colorizedMap;
	}


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

