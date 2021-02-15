#pragma once
#include<iostream>
#include<cstdlib>
#include<string>
using namespace std;
class Converter;
class Token {
	friend Converter;
	char value;
	int priority;
public:
	Token() {
		value = ' ';
		priority = 0;
	}
	void setPriority() {
		if (value == '*' || value == '%' || value == '/')
			priority = 2;
		else if (value == '+' || value == '-')
			priority = 1;
	}
	char getValue() {
		return value;
	}
};