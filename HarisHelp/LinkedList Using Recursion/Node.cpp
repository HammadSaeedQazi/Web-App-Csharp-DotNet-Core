#include"Node.h"
template<class t>
Node<t>::Node(t d, Node* n) {
	next = n;
	data = d;
}
template<class t>
void Node<t>::print() {
	cout << data;
}
template<class t>
t Node<t>::getData() {
	return data;
}
template<class t>
Node<t>* Node<t>::getNext() {
	return next;
}