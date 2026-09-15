using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitTest_Lambda2 : MonoBehaviour
{
    public class Person
    {
        public string Name;
        public int Age;

        public override string ToString()
        {
            return $"Name : {Name}, Age : {Age}";
        }   
    }

    private void Start()
    {
        //람다 특성 정리
        //1. 델리게이트를 통해서만 정의 가능
        // - 델리게이트의 타입을 통해 파라미터 타입과 리턴 타입을 생성
        //Func<int, string> lambda = (x) => x.ToString();

        //2. 람다는 타입 자체가 없으므로, object의 어떤 멤버도 접근 불가
        //Func<int, string> lambda = ((int x) => x).ToString();

        //3. 타입이 없으므로 캐스팅도 불가능
        //4. 델리게이트에 의해 타입이 결정되므로 델리게이트 형식과 반드시 일치
        //Func<int, bool> lambda = (int x) => x; //리턴이 int형이므로 bool과 일치하지 않아 오류 발생

        //5. 타입이 없으므로, 형식 추론 불가
        //var a = 10; //형식 추론 가능
        //var lambda = (int x) => x; //형식 추론 불가, 델리게이트를 통해서만 가능


        int Add(int x)
        {
            int a = 10;
            return x + a;
        }
        print($"Add : {Add(10)}");


        void Add2(out int a, int b)
        {
            a = b + 10;
        }

        int value;
        Add2(out value, 20);
        print($"value : {value}");


        //6. 클로저는 out 파라미터를 사용 가능, 람다는 불가능
        //Action<int> lambda = (out int x) => x = 10;

        //7. 클로저는 ref 파라미터를 사용 가능, 람다는 불가능
        //Action<int> lambda = (ref int x) => x = 10;

        //8. 캡처 자체는 레퍼런스로 동작하지만,
        //람다 내부에서 값형식의 변수를 변경하면,
        //캡처된 변수는 복사본이므로 외부 변수에는 영향을 주지 않음


        List<string> items = new List<string>()
        {
            "Potion", "Scroll", "Granade", "Sword", "Shield",
        };
        items.ForEach((x) => print($"items : {x}"));

        
        List<string> names = new List<string>()
        {
            "이순신", "강감찬", "김유신", "박문수", "장보고",
        };
        names.ForEach((x) =>
        {
            print("위인 이름");
            print(x);
        });


        List<Action> actions = new List<Action>();
        foreach(string name in names)
            actions.Add(() => print($"이름 : {name}"));

        actions.ForEach((action) => action());


        List<int> numbers = new List<int>();
        for(int i = 0; i < 10; i++)
            numbers.Add(UnityEngine.Random.Range(1, 100 + 1));

        //numbers.Sort();
        numbers.Sort((x, y) => y.CompareTo(x));
        numbers.ForEach(x => print(x));


        List<Person> persons = new List<Person>()
        {
            new Person() { Name = "홍길동", Age = 50 },
            new Person() { Name = "이순신", Age = 32 },
            new Person() { Name = "강감찬", Age = 64 },
            new Person() { Name = "김유신", Age = 75 },
            new Person() { Name = "박문수", Age = 20 },
        };
        persons.Sort((x, y) => x.Age.CompareTo(y.Age));
        persons.ForEach(x => print(x));
    }
}
