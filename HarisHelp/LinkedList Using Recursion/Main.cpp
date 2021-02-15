#include"Converter.h"
#include<string>
int main() {
	char* ch;
	Converter c1;
	ch = c1.getInput();
	//char* post = c1.InToPost(ch);
	char* infix = c1.PostToIn(ch);
	//char* pre = c1.InToPre(ch);
	//char* pretoin = c1.PreToIn(ch);
	//string s;
	//getline(cin, s);
	//string ptop = c1.PostToPre(s);
	//string ptop = c1.PreToPost(s);
	//cout << ptop;
	system("pause");
	return 0;
}