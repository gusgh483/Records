using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTest_Generic : MonoBehaviour
{
    private class A { }

    private void Print<T>(T value)
    {
        print($"print : {value}");
    }

    private class B<T>
    {
        List<T> datas = new List<T>();

        public void Add(T data)
        {
            datas.Add(data);
        }

        public void PrintAll()
        {
            foreach (T data in datas)
                print($"data : {data}");
        }
    }

    private class C
    {
        List<object> datas = new List<object>();

        public void Add(object data)
        {
            datas.Add(data);
        }

        public void PrintAll()
        {
            foreach (object data in datas)
                print($"data : {data}");
        }
    }

    class D
    {
        private int number = 0;
        private static int count = 0;

        public D()
        {
            number = ++count;
        }

        public override string ToString()
        {
            return $"{number}번 D 클래스 객체";
        }
    }

    private void Start()
    {
        Print<int>(5);
        Print<string>("Unity");
        Print(new A());

        B<int> datas = new B<int>();
        datas.Add(1);
        datas.Add(3);
        datas.Add(2);
        datas.Add(5);
        datas.PrintAll();

        C datas2 = new C();
        datas2.Add(1);
        datas2.Add(3);
        datas2.Add(2);
        datas2.Add(5);
        datas2.PrintAll();

        D d = new D();
        string str = d.ToString();
        print($"d : {str}");


        List<int> list = new List<int>();
        list.Add(10);
        list.Add(20);

        foreach(int data in list)
            print($"list : {data}");

        
        ArrayList arrayList = new ArrayList();
        arrayList.Add(10);
        arrayList.Add("ArrayList");
        arrayList.Add(3.14f);
        arrayList.Add(new D());

        foreach (object data in arrayList)
            print($"arrayList : {data}");

        //IEnumerator enumerator = arrayList.GetEnumerator();
        //while (enumerator.Current != null)
        //{
        //    print(enumerator.Current);
        //    enumerator.MoveNext();
        //}
    }
}
