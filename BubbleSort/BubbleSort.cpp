#include "pch.h"
#include <iostream>
#include <time.h>       
#include <chrono> 
#include <iterator> 
#include <omp.h>
#include <string> 

using namespace std::chrono;
using namespace std;

void PrintArray(int *arr, int size) {
	for (int i = 0; i < size; i++)
		cout << arr[i] << " " << flush;
}

void swap(int *p1, int *p2) {
	int p = *p1;
	*p1 = *p2;
	*p2 = p;
}

/* Simple bubble sort */
void BubbleSort1(int *arr, int n)
{
	int swapCount = 1;
	while (swapCount > 0)
	{
		swapCount = 0;
		for (int j = 0; j < n - 1; j++)
			if (arr[j] > arr[j + 1]) {
				swap(&arr[j], &arr[j + 1]);
				swapCount++;
			}
	}
}

/* Bubble sort by splitting odd even */
void BubbleSort2(int *arr, int n)
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

/* Same as odd even sort, but parallelizing loops using OpenMP */
void BubbleSortParallel(int *arr, int n, int num_threads = 8, bool detailedOutput = false)
{
	int swapEvenCount = 1;
	int swapOddCount = 1;
	
	omp_set_num_threads(num_threads);

	while (swapEvenCount > 0 || swapOddCount > 0)
	{
		swapEvenCount = swapOddCount = 0;
		
		#pragma omp parallel for reduction(+: swapEvenCount)  
		for (int j = 0; j < n-1-n%2; j += 2)
			if (arr[j] > arr[j + 1]) {
				swap(arr[j], arr[j + 1]);
				swapEvenCount++;
			}
		
		if (detailedOutput) {
			PrintArray(arr, n);
			cout << "Swapped even(0) " << swapEvenCount << endl;
		}

		#pragma omp parallel for reduction(+: swapOddCount) 
		for (int j = 1; j < n-1-(1-n%2); j += 2)
			if (arr[j] > arr[j + 1]) {
				swap(arr[j], arr[j + 1]);
				swapOddCount++;
			}

		if (detailedOutput) {
			PrintArray(arr, n);
			cout << "Swapped odd(1) " << swapOddCount << endl;
		}

	}
}

/* Helper routine to make test */
bool DoTest(int testSize, bool detailedOutput) {
	cout << "Performing sorting test of " << testSize << " size " << endl;
	int * testArray1 = new int[testSize];
	int * testArray2 = new int[testSize];
	int * testArray3 = new int[testSize];

	srand(time(NULL));
	for (int i = 0; i < testSize; i++) {
		int r = rand() % 255;
		testArray1[i] = r;
		testArray2[i] = r;
		testArray3[i] = r;
	}

	BubbleSort1(testArray1, testSize);
	BubbleSort2(testArray2, testSize);

	PrintArray(testArray1, testSize);
	cout << endl;

	BubbleSortParallel(testArray3, testSize, omp_get_num_threads(), detailedOutput);

	if (detailedOutput) {
		PrintArray(testArray3, testSize); 
		cout << endl;
	}

	bool testPass = true;
	for (int i = 0; i < testSize; i++) {
		if (!(testArray1[i] == testArray3[i] )) {
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

/* Do benchmark using provided number of threads */
void DoBenchamrk(int numThreads) {

	cout << "DataSize;NumThreads;SimpleSort;OddEvenSort;ParallelSort" << endl;

	for (int size = 1000; size <= 100000; size += 5000) {

		int * array1 = new int[size];
		int * array2 = new int[size];
		int * array3 = new int[size];

		srand(time(NULL));
		for (int i = 0; i < size; i++) {
			array1[i] = rand() % 255;
			array2[i] = array1[i];
			array3[i] = array1[i];
		}

		auto start = high_resolution_clock::now();
		BubbleSort1(array1, size);
		auto stop = high_resolution_clock::now();
		auto durationSerial = duration_cast<milliseconds>(stop - start);

		auto start2 = high_resolution_clock::now();
		BubbleSort2(array2, size);
		auto stop2 = high_resolution_clock::now();
		auto durationSerial2 = duration_cast<milliseconds>(stop2 - start2);

		auto start3 = high_resolution_clock::now();
		BubbleSortParallel(array3, size, numThreads);
		auto stop3 = high_resolution_clock::now();
		auto durationParallel = duration_cast<milliseconds>(stop3 - start3);

		cout << size << ";" << numThreads << 
			";" << durationSerial.count() <<
			";" << durationSerial2.count() <<
			";" << durationParallel.count() << endl;

		delete array1;
		delete array2;
	}
}

int main(int argc, char *argv[])
{
	if (argc < 2) {
		cout << "Naudojimas:" << endl;
		cout << "BUBBLESORT test [size] [detailed_output] - perform sorting test, check if it sorts correctly." << endl;
		cout << "BUBBLESORT benchmark [num_threads] - performs benchmark from 1000 to 100000 stepping by 5000 " << endl;
		return(0);
	} else {
		if (string(argv[1]) == "test") {
			int size = stoi(argv[2]);
			bool detailed = true; // string(argv[3]) == "true";
			DoTest(size,detailed);
			return (0);
		}
		else if (string(argv[1]) == "benchmark") {
			int numThreads = stoi(argv[2]);
			DoBenchamrk(numThreads);
			return (0);
		}
		else {
			cout << "Unknown argument " << argv[1] << endl;
			return (0);
		}
	}	
}

