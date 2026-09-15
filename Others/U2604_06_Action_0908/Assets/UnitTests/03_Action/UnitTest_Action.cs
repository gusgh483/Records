using System;
using UnityEngine;

public class UnitTest_Action : MonoBehaviour
{
    public delegate void Run();
    public Run run;

    private void Run1()
    {
        print("Run1");
    }

    private void Run2()
    {
        print("Run2");
    }

    private void ExecuteRun()
    {
        run = Run1;
        print("ExecuteRun1 - 실행전");
        run.Invoke();
        print("ExecuteRun1 - 실행후");

        run = Run2;
        print("ExecuteRun2 - 실행전");
        run.Invoke();
        print("ExecuteRun2 - 실행후");
    }

    
    private void Print(string text)
    {
        print($"Print : {text}");
    }

    private void Log(string str)
    {
        Debug.Log($"Log : {str}");
    }

    private delegate void PrintText(string context);
    private PrintText printText;

    private void ExecutePrintText()
    {
        printText = Print;
        printText.Invoke("Test");

        printText = Log;
        printText.Invoke("Test");
    }


    private Action runAction;
    private Action<string> printAction;

    private void PrintColor(string value, Color color)
    {
        print($"PrintColor : {value}, {color}");
    }

    private void PrintColor2(string value, Color color) => print($"PrintColor : {value}, {color}");


    private void ExecuteAction()
    {
        runAction = Run2;
        printAction = Print;

        runAction.Invoke();
        printAction.Invoke("Hello Action");
    }


    private string Func1(int value, Vector2 value2, string value3)
    {
        return $"Value : {value}, Value2 : {value2}, Value3 : {value3}";
    }

    private void ExecuteFunc()
    {
        PrintColor("Red", Color.red);
        PrintColor2("Blue", Color.blue);

        Func<int, Vector2, string, string> func = Func1;
        string str = func.Invoke(10, new Vector2(1, 2), "Test Func");
        print("Func Result : " + str);
    }


    private bool Predicate1(int value)
    {
        return value > 10;
    }

    private void ExecutePrediate()
    {
        Predicate<int> predicate = Predicate1;
        
        bool result = predicate.Invoke(5);
        print("Predicate Result : " + result);

        bool result2 = predicate.Invoke(20);
        print("Predicate Result2 : " + result2);
    }


    private void Start()
    {
        ExecuteRun();
        ExecutePrintText();
        ExecuteAction();
        ExecuteFunc();
        ExecutePrediate();
    }
}
