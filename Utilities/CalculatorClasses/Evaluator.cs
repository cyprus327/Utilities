using System;
using System.Collections.Generic;

namespace Utilities {
	internal static class Evaluator {
		public static double EvaluateExpression(string expression) {
	        if (expression.Length == 1 && double.TryParse(expression, out double num)) {
	            return num;
	        }
	
	        Stack<double> numbers = new Stack<double>();
	        Stack<char> operators = new Stack<char>();
	
	        for (int i = 0; i < expression.Length; i++) {
	            char currentChar = expression[i];
	
	            if (char.IsDigit(currentChar) || currentChar == '.') {
	                string currentNumberStr = currentChar.ToString();
	                while (i + 1 < expression.Length && (char.IsDigit(expression[i + 1]) || expression[i + 1] == '.')) {
	                    currentNumberStr += expression[i + 1];
	                    i++;
	                }
	                if (!double.TryParse(currentNumberStr, out double currentNumber)) {
	                    throw new ArgumentException("Invalid number format");
	                }
	                numbers.Push(currentNumber);
	            }
	            else if (currentChar == '(') {
	                operators.Push(currentChar);
	            }
	            else if (currentChar == ')') {
	                while (operators.Peek() != '(') {
	                    double secondOperand = numbers.Pop();
	                    double firstOperand = numbers.Pop();
	                    char operatorToApply = operators.Pop();
	                    numbers.Push(ApplyOperator(firstOperand, secondOperand, operatorToApply));
	                }
	                operators.Pop();
	            }
	            else if (currentChar == '+' || currentChar == '-' || currentChar == '*' || currentChar == '/') {
	                while (operators.Count > 0 && HasPrecedence(currentChar, operators.Peek())) {
	                    double secondOperand = numbers.Pop();
	                    double firstOperand = numbers.Pop();
	                    char operatorToApply = operators.Pop();
	                    numbers.Push(ApplyOperator(firstOperand, secondOperand, operatorToApply));
	                }
	                operators.Push(currentChar);
	            }
	        }
	
	        while (operators.Count > 0) {
	            double secondOperand = numbers.Pop();
	            double firstOperand = numbers.Pop();
	            char operatorToApply = operators.Pop();
	            numbers.Push(ApplyOperator(firstOperand, secondOperand, operatorToApply));
	        }
	        return numbers.Pop();
	    }

		private static bool HasPrecedence(char op1, char op2) {
	        if (op2 == '(' || op2 == ')')
	            return false;
	        if ((op1 == '*' || op1 == '/') && (op2 == '+' || op2 == '-'))
	            return false;
	        else
	            return true;
	    }

	    private static double ApplyOperator(double a, double b, char op) {
	        switch (op) {
	            case '+': return a + b;
	            case '-': return a - b;
	            case '*': return a * b;
	            case '/': return a / b;
				default: throw new ArgumentException("Invalid operator: " + op);
	        }
	    }
	}
}