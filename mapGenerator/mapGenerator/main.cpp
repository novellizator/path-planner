#include <cstdio>

using namespace std;

int main()
{
	freopen( "empty_map.map", "w", stdout );


	int width = 2048;
	int height = 2048;


	for (int i=0; i < height; ++i)
	{
		for (int j= 0; j < width; ++j)
		{
			printf("%c", '.');
		}
		printf("\n");
	}
	return 0;

}