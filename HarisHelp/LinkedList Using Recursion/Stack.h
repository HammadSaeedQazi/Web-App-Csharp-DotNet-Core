#pragma once
#include"Node.h"
#include"Node.cpp"
class Converter;
template<class t1>
class Stack {
	friend Converter;
	Node<t1>* top;
public:
	Stack();
	void push(t1 d);
	bool isEmpty();
	t1 pop();
	void print();
	~Stack();
};