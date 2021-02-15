#pragma once
#include<iostream>
#include<cstdlib>
#include<string>
using namespace std;
template<class t>
class Node {
	Node* next;
	t data;
public:
	Node(t d, Node* n);
	void print();
	t getData();
	Node* getNext();
};