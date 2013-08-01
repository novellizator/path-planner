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

struct dataAboutRectangle2 
{
	dataAboutRectangle2(int l = 0, int s = 0): left(l), sequenceSize(s) 
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


class Colorizator
{
public:

	Colorizator():colorizedMap(width * height, DEFAULT_COLOR)
	{}

	vector<int> colorize() 
	{
		float debug_area = 0;
		int color = DEFAULT_COLOR + 1;
		pair<int, dataAboutRectangle> largestRectangle = findLargestRectangle();
		while (largestRectangle.second.getArea() >= SMALLEST_COLORIZED_RECTANGLE)
		{
			colorizeRectangle(largestRectangle, color++);
			largestRectangle = findLargestRectangle();
			//std::cout << "farba " << color << "obsah" <<
				//largestRectangle.second.getArea() << std::endl;
			debug_area += largestRectangle.second.getArea();
		}

		//std::cout << "podiel je" << width*height/debug_area << std::endl;
		return colorizedMap;
	}

	vector<int> getColorizedMap()
	{
		return colorizedMap;
	}

	void setColorizedMap(vector<int> cMap)
	{
		colorizedMap = cMap;
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
private:
	static const int DEFAULT_COLOR = 9;
	vector<int> colorizedMap;

	bool validLoc(const xyLoc & loc) const
	{
		if (loc.x < 0 || loc.y < 0) 
			return false; 
		return true;
	}
	bool isColorizable(int vertex) const
	{
		if (map[vertex] && colorizedMap[vertex] == DEFAULT_COLOR)
			return true;
		else
			return false;
	}

	int getLefts(const vector<dataAboutRectangle>& data, const xyLoc& loc) const
	{
		if (validLoc(loc)) 
			return data[linearize(loc)].left; 
		else 
			return 0;
	}



	pair<int, dataAboutRectangle> findLargestRectangle() const
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
		xyLoc loc = coordinatize(vertex);

		for (int c = loc.x - rectangle.second.left + 1; c <= loc.x; ++c)		
		{
			for (int r = loc.y - rectangle.second.sequenceSizeUp ; r <= loc.y + rectangle.second.sequenceSizeDown; ++r)
			{
				colorizedMap[linearize(r, c)] = color;
			}
		}
	}

	void colorizeRectangle2(const pair<int, dataAboutRectangle2>& rectangle, int color)
	{
		int vertex = rectangle.first;
		xyLoc loc = coordinatize(vertex);

		for (int c = loc.x - rectangle.second.left + 1; c <= loc.x; ++c)		
		{
			for (int r = loc.y; r < loc.y + rectangle.second.sequenceSize; ++r)
			{
				colorizedMap[linearize(r, c)] = color;
			}
		}
	}
};
#endif
