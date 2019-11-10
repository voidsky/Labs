	/* ***************************************************************************** */
/* BubbleSort test using OpenACC                                                 */	
/* pgcc -fast -ta=nvidia -Minfo=accel BubbleSortACC.c                            */
/* pgcc -fast -ta=multicore -Minfo=accel BubbleSortACC.c                         */
/*  time ./bubblesortacc.exe 100000 -silent                                      */		
/* ***************************************************************************** */
#include <time.h>

void PrintArray(int *arr, int size) {
	for (int i = 0; i < size; i++)
		printf(" %d ", *(arr + i));
	printf("\n");
}

void swap(int *p1, int *p2) {
	int p = *p1;
	*p1 = *p2;
	*p2 = p;
}

/* Same as odd even sort, but parallelizing loops using OpenMP */
void BubbleSortParallel(int *arr, int n)
{
	int swapEvenCount = 1;
	int swapOddCount = 1;
	
	#pragma acc data copy(arr[0:n])  
	while (swapEvenCount > 0 || swapOddCount > 0)
	{
		swapEvenCount = swapOddCount = 0;
		
		#pragma acc kernels loop reduction(+:swapEvenCount) 
			for (int j = 0; j < n - 1 - n % 2; j += 2)
				if (*(arr+j) > *(arr + j + 1)) {
					int tmp = *(arr + j);
					*(arr + j) = *(arr + j + 1);
					*(arr + j + 1) = tmp;
					swapEvenCount++;
				}
		
		#pragma acc kernels loop reduction(+:swapOddCount) 
			for (int j = 1; j < n - 1 - (1 - n % 2); j += 2)
				if (arr[j] > arr[j + 1]) {
					int tmp = *(arr + j);
					*(arr + j) = *(arr + j + 1);
					*(arr + j + 1) = tmp;
					swapOddCount++;
				}

	}
}

int main(int argc, char *argv[])
{
	int size = strtol(argv[1], NULL, 10);

	int silent = 0;
	if (argc == 3 && strcmp(argv[2], "-silent") == 0) {
		silent = 1;
	}

	int * array1 = (int *)malloc(size * sizeof(int));

	srand(time(NULL));
	for (int i = 0; i < size; i++) {
		array1[i] = rand() % 255;
	}

	if (silent == 0) PrintArray(array1, size);

	printf("Start sorting\n");

	BubbleSortParallel(array1, size);

	printf("Finished \n");

	if (silent == 0) PrintArray(array1, size);

	free(array1);
}