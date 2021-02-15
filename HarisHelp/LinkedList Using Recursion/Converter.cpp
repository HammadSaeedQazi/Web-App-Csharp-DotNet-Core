#include"Converter.h"
Converter::Converter() {
	tkn = NULL;
	count = 0;
}
char* Converter::getInput() {
	char* t;
	cout << "Enter Expression : ";
	string str;
	getline(cin, str);
	count = str.length();
	t = new char[str.length()];
	count = str.length();
	for (int j = 0; j < str.length(); j++)
		t[j] = str[j];
	return t;
}
char* Converter::InToPost(char* inExpr) {
	tkn = new Token[count];
	int j = 0;
	for (int i = 0; i < count; i++) {
		if (inExpr[i] == '(')
			stk.push('(');
		else if (inExpr[i] != '/' && inExpr[i] != '*' && inExpr[i] != '+' && inExpr[i] != '-' && inExpr[i] != '%' && inExpr[i] != ')') {
			tkn[j++].value = inExpr[i];
		}
		else if (inExpr[i] == ')') {
			char ch = stk.pop();
			while (ch != '(') {
				tkn[j++].value = ch;
				ch = stk.pop();
			}
		}
		else {
		D:
			if (!stk.isEmpty()) {
				char ch = stk.pop();
				Token t1;
				Token t2;
				t1.value = ch;
				t1.setPriority();
				t2.value = inExpr[i];
				t2.setPriority();
				if (t2.priority <= t1.priority) {
					tkn[j++].value = t1.value;
					goto D;
				}
				else {
					stk.push(t1.value);
					stk.push(inExpr[i]);
				}
			}
			else
				stk.push(inExpr[i]);
		}
	}
	for (int i = j; j < count && !stk.isEmpty(); i++)
		tkn[i].value = stk.pop();
	char* temp = new char[count];
	for (int i = 0; i < count; i++)
		temp[i] = tkn[i].value;
	for (int i = 0; i < count; i++)
	cout << tkn[i].value;
	return temp;
}
char* Converter::PostToIn(char* postExpr) {
	int cc = 0;
	for (int i = 0; i < count; i++)
		if (postExpr[i] == '/' || postExpr[i] == '*' || postExpr[i] == '%' || postExpr[i] == '+' || postExpr[i] == '-')
			cc++;
	char* arr = new char[count + cc * 2];
	int newCount = count + cc * 2;
	for (int i = 0; i < newCount; i++)
		arr[i] = ' ';
	char ch = ' ';
	int iter = newCount - 1;
	for (int i = count - 1; i >= 0; i--) {
		char ch = ' ';
		if (postExpr[i] != '/' && postExpr[i] != '*' && postExpr[i] != '%' && postExpr[i] != '+' && postExpr[i] != '-') {
			arr[iter--] = postExpr[i];
			do {
				ch = stk.pop();
				arr[iter--] = ch;
			} while (ch != '/' && ch != '*' && ch != '%' && ch != '+' && ch != '-' && !stk.isEmpty());
		}
		else {
			stk.push('(');
			stk.push(postExpr[i]);
			arr[iter--] = ')';
		}
	}
	count = newCount;
	for (int i = 0; i < newCount; i++)
	cout << arr[i];
	return arr;
}
void Converter::reverseString(char* temp) {
	char* ch;
	ch = new char[count];
	int j = 0;
	for (int i = 0; i < count; i++)
		ch[i] = temp[i];
	for (int i = count - 1; i >= 0; i--) {
		if (ch[i] == '(')
			temp[j++] = ')';
		else if (ch[i] == ')')
			temp[j++] = '(';
		else
			temp[j++] = ch[i];
	}
	return;
}
char* Converter::InToPre(char* inExpr) {
	reverseString(inExpr);
	char* temp = InToPost(inExpr);
	reverseString(temp);
	for (int i = 0; i < count; i++)
		cout << temp[i];
	return temp;
}
char* Converter::PreToIn(char* preExpr) {
	reverseString(preExpr);
	char* temp = PostToIn(preExpr);
	reverseString(temp);
	for (int i = 0; i < count; i++)
		cout << temp[i];
	return temp;
}
string Converter::PostToPre(string postExpr) {
	string temp = "";
	Stack<string> s;
	for (int i = 0; i < postExpr.length(); i++) {
		if (postExpr[i] != '/' && postExpr[i] != '*' && postExpr[i] != '%' && postExpr[i] != '+' && postExpr[i] != '-')
			s.push(string(1, postExpr[i]));
		else {
			string s1 = s.pop();
			string s2 = s.pop();
			temp = postExpr[i] + s2 + s1;
			s.push(temp);
		}
	}
	return temp;
}
string Converter::PreToPost(string preExpr) {
	string temp = "";
	Stack<string> s;
	for (int i = preExpr.length() - 1; i >= 0; i--) {
		if (preExpr[i] != '/' && preExpr[i] != '*' && preExpr[i] != '%' && preExpr[i] != '+' && preExpr[i] != '-')
			s.push(string(1, preExpr[i]));
		else {
			string s1 = s.pop();
			string s2 = s.pop();
			temp = s1 + s2 + preExpr[i];
			s.push(temp);
		}
	}
	for (int i = 0; i < temp.length(); i++)
		cout << temp[i];
	return temp;
}