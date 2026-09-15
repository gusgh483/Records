using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitTest_Lambda : MonoBehaviour
{
    private void Test(int a, string b)
    {
        print($"Test : {a}, {b}");
    }

    private void Start()
    {
        Action<int, string> action;
        action = Test;
        action.Invoke(1, "Hello");
        action(1, "Hello");

        action = (a, b) => print($"Lambda : {a}, {b}");
        action(2, "World");


        Action action1 = () => print("Hello Lambda");
        action1();

        
        //파라미터가 1개인 경우 괄호 생략 가능
        Action<int> action2 = (a) => print($"Lambda : {a}"); 
        action2(3);


        //내부 함수
        int Square(int x) //캡처
        {
            return x * x; //클로저
        }
        Func<int, int> square = Square;
        int result = square(4);
        print($"Square : {result}");


        int outer = 15;
        int Add(int x) //x => 캡처되는 변수
        {
            return x + outer; //클로저
        }
        Func<int, int> add = Add;
        print($"add = {add(5)}");

        Func<int, int> add2 = Add;
        print($"add = {add2(15)}");


        List<Action> actions = new List<Action>();
        for (int i = 0; i < 3; i++)
        {
            int temp = i;
            //actions.Add(() => print(i)); //3, 3, 3
            actions.Add(() => print(temp)); //0, 1, 2
        }
        
        foreach(Action act in actions)
            act();
        

        //람다식 : (input paramters) => expression
        Action lambda1 = () => print("Hello Lambda");
        lambda1();

        Func<int> lambda2 = () => 5 * 5;
        print($"lambda2 : {lambda2()}");

        Func<int, int> lambda3 = (x) => x * x;
        print($"lambda3 : {lambda3(5)}");

        Func<int, int> lambda4 = (int x) => x * outer;
        print($"lambda4 : {lambda4(5)}");

        Action<string> append = (x) => print($"Append : {x}");
        append("Test");


        //람다문 : (input parameters) => { expression }
        Action<string> lambda5 = name =>
        {
            string hello = $"Hello {name}";
            print("Lambda5 : " + hello);
        };
        lambda5("Unit");


        Func<string, int, bool> lambda6 = (x, y) =>
        {
            return x.Length > y;
        };
        print($"Lambda6 : {lambda6("Unity", 3)}");


        Predicate<int> lambda7 = (x) =>
        {
            string str = "Unity";
            return str.Length > x;
        };
        print($"Lambda7 : {lambda7(3)}");


        
    }
}
