#include"Stack.h"
template<class t1>
Stack<t1>::Stack() {
	top = NULL;
}
template<class t1>
void Stack<t1>::push(t1 d) {
	Node<t1>* temp = new Node<t1>(d, top);
	top = temp;
}
template<class t1>
bool Stack<t1>::isEmpty() {
	return top == NULL;
}
template<class t1>
t1 Stack<t1>::pop() {
	if (!isEmpty()) {
		t1 d = top->getData();
		Node<t1>* temp = top;
		top = top->getNext();
		delete temp;
		return d;
	}
	exit(1);
}
template<class t1>
void Stack<t1>::print() {
	Node<t1>* temp = top;
	if (isEmpty())
		return;
	while (temp != NULL) {
		cout << temp->getData() << ' ';
		temp = temp->getNext();
	}
}
template<class t1>
Stack<t1>::~Stack() {
	while (!isEmpty()) {
		Node<t1>* temp = top;
		top = top->getNext();
		delete temp;
	}
}