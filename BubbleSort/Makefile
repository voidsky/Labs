clean :
	rm -f bubblesort
	rm -f bubblesort.o
bubblesort : bubblesort.o 
	g++ bubblesort.o -o bubblesort -fopenmp
bubblesort.o : bubblesort.cpp	
	g++ -c bubblesort.cpp -fopenmp