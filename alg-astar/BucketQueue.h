#ifndef BUCKETQUEUE_H
#define BUCKETQUEUE_H


#include <vector>
#include "globals.h"


using std::vector;


// special data structure that makes the heap linear
// does not work with landmarks... - when I make this work - it will be awesome!
class BucketQueue
{
public:
	BucketQueue():size(0), baseDistance(1), currentIndex(0)
	{
		for (int i = 0; i != 3; ++i)
		{
			baseElements[i] = new vector<Node_t>();
			baseIndexes[i] = 0;
		}
	}


	bool empty()
	{
		return size == 0;
	}


	void push(const Node_t & element)
	{
		++size;
		if (element.distance > baseDistance + 1.999)
		{
			pushOrInsert(2, element);
		}
		else if(element.distance > baseDistance + 0.999)
		{
			pushOrInsert(1, element);
		}
		else
		{
			pushOrInsert(0, element);
		}
	}


	Node_t & top()
	{
		//if (empty())
		//	throw std::exception("empty queue!!!");

		return (*baseElements[0])[currentIndex]; // FIXME? in the beginnign there must me at least one element here baseElements[0]
	}


	// pops the element and sets currentIndex to valid value
	void pop()
	{
		//if (empty())
		//	throw std::exception("empty!!!");

		--size;

		++currentIndex; // move to the next thing

		// check if correct
		for (int i=0; i<3; ++i)
			if (currentIndex == baseIndexes[0])
			{
				// baseIndexes[0] = 0;
				currentIndex = 0;
				++baseDistance;

				vector<Node_t>* tmpVecPtr = baseElements[0];

				for (int oldBase = 1; oldBase <= 2; ++oldBase)
				{
					baseElements[oldBase - 1] = baseElements[oldBase];
					baseIndexes[oldBase - 1] = baseIndexes[oldBase];
				}
				baseElements[2] = tmpVecPtr;
				baseIndexes[2] = 0;
			}
			else
				break;


	}

private:
	int size;
	int baseDistance;

	int currentIndex;
	// <baseDistance, baseDistance+1), 
	// <baseDistance+1, baseDistance+2),
	// <baseDistance2, baseDistance+2.41)
	vector<Node_t>* baseElements[3];
	unsigned int baseIndexes[3]; // index in the baseElementsx vector to which the element is to be pushed

	
	

	// arrayIndex \element {0, 1, 2}
	void pushOrInsert(int arrayIndex, const Node_t& element)
	{
		// should I push back?
		if (baseIndexes[arrayIndex] >= baseElements[arrayIndex]->size())
		{
			baseElements[arrayIndex]->push_back(element);
		}
		else
		{
			(*baseElements[arrayIndex])[baseIndexes[arrayIndex]] = element;
		}
		++baseIndexes[arrayIndex];
	}
};

#endif