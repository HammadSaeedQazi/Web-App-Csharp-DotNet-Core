#pragma once
#include<string>
#include"Token.h"
#include"Stack.h"
#include"Stack.cpp"
class Converter {
	Stack<char>stk;
	Token* tkn;
	int count;
public:
	Converter();
	char* getInput();
	void reverseString(char* ch);
	char* InToPost(char* inExpr);
	char* PostToIn(char* postExpr);
	char* InToPre(char* inExpr);
	char* PreToIn(char* preExpr);
	string PostToPre(string postExpr);
	string PreToPost(string preExpr);
};