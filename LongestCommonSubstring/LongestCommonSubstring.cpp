// LongestCommonSubstring.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "pch.h"
#include <iostream>
#include <string> // for string class 
#include <algorithm>
#include <vector>

using namespace std;
int lcs(string str1, string str2, bool returnString, vector<char> & v) {
	if (!str1.size() || !str2.size()) return 0;
	if (str1[str1.size()-1] == str2[str2.size()-1]) {
		if (returnString) v.push_back(str1[str1.size() - 1]);
		return 1 + lcs(str1.substr(0,str1.size() - 1), str2.substr(0,str2.size() - 1), returnString, v);
	} else {
		vector<char> vec1, vec2;
		int res1 = lcs(str1.substr(0, str1.size() - 1), str2, returnString, vec1);
		int res2 = lcs(str1, str2.substr(0, str2.size() - 1), returnString, vec2);
		if (res1 > res2) {
			if (returnString) v.insert(v.end(), vec1.begin(), vec1.end());
			return res1;
		}
		else
		{
			if (returnString) v.insert(v.end(), vec2.begin(), vec2.end());
			return res2;
		}
	}
}

int main()
{
	string str1 = "labasvaxkaras";
	string str2 = "vakaraslabas";
	//cout << lcs(str1, str2) << endl;
	vector<char> myvector;
	cout << lcs(str1, str2, true, myvector);
	reverse(myvector.begin(), myvector.end());
	string s(myvector.begin(), myvector.end());
	cout << " " << s << endl;
}
