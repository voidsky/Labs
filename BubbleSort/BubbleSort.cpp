#include "pch.h"
#include <iostream>
#include <time.h>       
#include <chrono> 
#include <iterator> 
#include <omp.h>

using namespace std::chrono;

using namespace std;

void PrintArray(int *arr, int size) {
	for (int i = 0; i < size; i++)
		cout << arr[i] << " " << flush;
	cout << endl;
}

void swap(int *p1, int *p2) {
	int p = *p1;
	*p1 = *p2;
	*p2 = p;
}

void BubbleSort(int *arr, int n)
{
	int swapCount = 1;

	while (swapCount > 0)
	{
		swapCount = 0;

		// even phase - every even indexed element is compared with next odd indexed element
		for (int j = 0; j < n-1; j+=2)
			if (arr[j] > arr[j + 1]) {
				swap(&arr[j], &arr[j + 1]);
				swapCount++;
			}

		// odd phase - every odd indexed element is compared with next even indexed element
		for (int j = 1; j < n-1; j+=2)
			if (arr[j] > arr[j+1]) {
				swap(&arr[j], &arr[j + 1]);
				swapCount++;
			}
	}
}

/*void BubbleSortParallel(int *arr, int n)
{
	int swapCount = 1;

		while (swapCount > 0)
		{
			#pragma omp parallel shared(arr)
			{
				swapCount = 0;

				// even phase - every even indexed element is compared with next odd indexed element
				#pragma omp for reduction(+: swapCount)
				for (int j = 0; j < n - 1; j += 2)
					if (arr[j] > arr[j + 1]) {
						swap(&arr[j], &arr[j + 1]);
						swapCount++;
					}

				// odd phase - every odd indexed element is compared with next even indexed element
				#pragma omp for reduction(+: swapCount) 
				for (int j = 1; j < n - 1; j += 2)
					if (arr[j] > arr[j + 1]) {
						swap(&arr[j], &arr[j + 1]);
						swapCount++;
					}
			}
		}
}*/

void BubbleSortParallel(int *arr, int n)
{
	int swapCount = 1;
	while(swapCount>0)
	{
		swapCount = 0;
		#pragma omp parallel num_threads(2,4) shared(arr)   
		{
			int num = omp_get_thread_num();
			#pragma omp parallel for reduction(+: swapCount) shared(arr) 
			for (int j = num; j < n - 1; j += 2)
				if (arr[j] > arr[j + 1]) {
					swap(&arr[j], &arr[j + 1]);
					swapCount++;
				}
		}
	}
}


/*void BubbleSortParallel(int *arr, int n)
{
	for (int i = 0; i < n; i++)
	{
		if (i % 2 == 0) {
			#pragma omp parallel for shared(arr) num_threads(8)
			for (int j = 2; j < n; j += 2)
				if (arr[j - 1] > arr[j])
					swap(&arr[j - 1], &arr[j]);
		} else {
			#pragma omp parallel for shared(arr) num_threads(8)
			for (int j = 1; j < n; j += 2)
				if (arr[j - 1] > arr[j])
					swap(&arr[j - 1], &arr[j]);
		}
	}
}*/

bool DoTest() {
	const int testSize = 10;
	cout << "Performing sorting test of " << testSize << " size " << endl;
	int * testArray1 = new int[testSize];
	int * testArray2 = new int[testSize];

	srand(time(NULL));
	for (int i = 0; i < testSize; i++) {
		int r = rand() % 255;
		testArray1[i] = r;
		testArray2[i] = r;
	}
	PrintArray(testArray1, testSize);
	cout << "Serial sort of " << testSize << " size " << endl;
	BubbleSort(testArray1, testSize);
	PrintArray(testArray1, testSize);
	cout << "Parallel sort of " << testSize << " size " << endl;
	BubbleSortParallel(testArray2, testSize);
	PrintArray(testArray2, testSize);
	bool testPass = true;
	for (int i = 0; i < testSize; i++) {
		if (testArray1[i] != testArray2[i]) {
			testPass = false;
			break;
		}
	}

	if (testPass)
		cout << "Test pass, arrays are equal." << endl;
	else
		cout << "Test fails, arrays are not equal." << endl;

	return testPass;
}


int main(int argc, char *argv[])
{
	bool doTest = true;

	/*if (argc < 2) {
		cout << "bubblesort test - performs sorting test" << endl;
		cout << "bubblesort benchmark - performs benchmark from 1000 to 50000 stepping by 5000 " << endl;
		cout << "bubblesort single [arraysize] [cores] - performs single arrayt sort on [arraysize] using number of [cores] " << endl;
		return(0);
	} else {
		if (argv[1]==)
	}*/

	if (doTest) {
		DoTest();
		//return(0);
	}

	for (int size = 1000; size <= 100000; size += 5000) {

		int * array1 = new int[size];
		int * array2 = new int[size];

		srand(time(NULL));
		for (int i = 0; i < size; i++) {
			array1[i] = rand() % 255;
			array2[i] = array1[i];
		}

		auto start = high_resolution_clock::now();
		BubbleSort(array1, size);
		auto stop = high_resolution_clock::now();
		auto durationSerial = duration_cast<milliseconds>(stop - start);
		
		start = high_resolution_clock::now();
		BubbleSortParallel(array2, size);
		stop = high_resolution_clock::now();
		auto durationParallel = duration_cast<milliseconds>(stop - start);
		
		cout << size << ";" << durationSerial.count() << ";" << durationParallel.count() << endl;

		delete array1;
		delete array2;

	}
}

