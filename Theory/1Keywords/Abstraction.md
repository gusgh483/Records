# 추상화(Abstraction)
1. 현실의 개념에서 필요한 공통점과 핵심 정보만 뽑아내는 과정.
1. 복잡한 내부 구현은 숨기고, 외부에는 필요한 기능과 사용법만 제공하는 설계 방식.
1. 공통된 구조를 만들고, 구체적인 동작은 자식 클래스가 구현하도록 나눌 수 있다.
- 구현 방식: abstract - override

## 추상클래스(Abstract Class)
1. 공통된 특성과 기능을 지니지만, 그 자체로는 애매한 상위 개념을 표현하는 클래스
1. 직접 객체를 만들 수 없고 상속 전용으로 사용한다.
1. 클래스와 동일한데 일부 멤버를 abstract로 설정할 수 있다.
1. 공통 데이터와 기본 구현을 함께 제공하고 싶을 때 사용한다.

1. 특징:
	- 추상 클래스는 직접 객체를 만들 수 없다.
	- 추상 함수
		- 함수의 이름과 형태만 있고, 구현은 없는 함수
		- 추상 함수를 가진 클래스는 반드시 추상 클래스여야 한다.
		- 추상 클래스를 상속받은 자식 클래스는 반드시 추상 함수를 override해서 구현해야 한다.

## 인터페이스(Interface)
1. 클래스가 반드시 구현해야 하는 기능의 약속
1. 함수의 이름과 형태를 미리 정해 둔다.
1. 실제 동작은 인터페이스를 구현하는 클래스에서 작성한다.
1. 서로 다른 클래스라도 같은 인터페이스를 구현하면 같은 타입으로 다룰 수 있다.
1. C# 클래스는 부모 클래스를 하나만 상속할 수 있지만, (단일 상속) <br>
인터페이스는 여러 개 구현할 수 있다.
1. 특징:
    - 인터페이스는 접근 제어자를 생략하면 기본값이 public
    - 인터페이스는 멤버 변수를 가질 수 없다. 프로퍼티는 가능. 이벤트도 가능.
    - 인터페이스 이름은 맨앞에 I를 붙이는 약속이 있다.

### 추상 클래스와 인터페이스
- 공통 데이터와 기능이 필요하면 추상 클래스를 사용.
- 기능 약속만 필요하면 인터페이스를 사용.
<br><br>
1. 추상 클래스 예시
```
public abstract class Item
    {
        // 멤버 변수
        protected string _name;
        protected string _desc;

        // 생성자
        public Item(string name, string desc)
        {
            _name = name;
            _desc = desc;
        }

        // 멤버 함수
        public virtual void PrintInfo()
        {
            Debug.Log("=========");
            Debug.Log($"아이템 이름: {_name}");
            Debug.Log($"아이템 설명: {_desc}");
        }

        // 추상 함수
        public abstract void Use();
    }

    public class Potion : Item
    {
        float _healAmount;      // 힐량
        public Potion(string name, string desc, float healAmount) : base(name, desc)
        {
            _healAmount = healAmount;
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Debug.Log($"효과: 체력 {_healAmount} 회복");
        }

        public override void Use()
        {
            Debug.Log("----------");
            Debug.Log($"{_name} 아이템을 사용합니다.");
            Debug.Log($"체력을 {_healAmount} 회복합니다.");
        }
    }

    public class DamageScroll : Item
    {
        float _damage;
        public DamageScroll(string name, string desc, float damage) : base(name, desc)
        {
            _damage = damage;
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Debug.Log($"효과: 가까운 적에게 {_damage} 피해");
        }

        public override void Use()
        {
            Debug.Log("---------");
            Debug.Log($"{_name} 아이템을 사용했습니다.");
            Debug.Log($"적에게 {_damage} 피해!");
        }
    }

    public class DungeonMap : Item
    {
        public DungeonMap(string name, string desc) : base(name, desc)
        {
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Debug.Log("효과: 던전의 지도를 보여 줍니다.");
        }

        public override void Use()
        {
            Debug.Log("---------");
            Debug.Log($"{_name} 아이템을 사용합니다.");
            Debug.Log($"던전 미니맵이 표시됩니다.");
        }
    }


    public class AbstractionStudy : MonoBehaviour
    {
        private void Start()
        {
            // 추상 클래스는 직접 객체를 만들 수 없다.
            //Item item = new Item("아이템", "설명");

            Item potion = new Potion("체력 포션", "회복 물약입니다.", 10);
            potion.Use();

            List<Item> items = new List<Item>()
            {
                new Potion("힐링 포션", "기초적인 회복 물약입니다.", 20),
                new DamageScroll("파이어볼 주문서", "가까운 적에게 불꽃 피해를 줍니다.", 15),
                new DungeonMap("쥐굴 지도", "던전의 미니맵을 보여 줍니다.")
            };

            foreach(var item in items)
            {
                item.PrintInfo();
                item.Use();
            }
        }
    }
```

<br><br>
1. 인터페이스 예시
```
// 인터페이스는 접근 제어자를 생략하면 기본값이 public
    // 인터페이스는 멤버 변수를 가질 수 없다. 프로퍼티는 가능. 이벤트도 가능.
    // 인터페이스 이름은 맨앞에 I를 붙이는 약속이 있다.
    public interface ITestInterface
    {
        event Action SomeAction;
        string Name { get; }

        void Test();
    }

    /// <summary>
    /// 상호 작용 가능한 인터페이스
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// 상호 작용
        /// </summary>
        void Interact();
    }

    public class Door : IInteractable
    {
        string _name;

        public Door(string name)
        {
            _name = name;
        }

        public void Interact()
        {
            Debug.Log($"{_name}이 열립니다.");
        }
    }

    public class Chest : IInteractable
    {
        public void Interact()
        {
            Debug.Log("상자를 열어 아이템을 획득합니다.");
        }
    }

    public class Npc : IInteractable
    {
        string _name;
        public Npc(string name)
        {
            _name = name;
        }

        public void Interact()
        {
            Debug.Log($"{_name} 캐릭터와 대화를 시작합니다.");
        }
    }


    public class InterfaceStudy : MonoBehaviour
    {
        // 인터페이스는 직접 객체를 만들 수 없다.
        //ITestInterface test = new ITestInterface();

        private void Start()
        {
            IInteractable[] interactables =
            {
                new Door("나무문"),
                new Chest(),
                new Npc("대장장이")
            };

            foreach(var interactable in interactables)
            {
                interactable.Interact();
            }
        }
    }
```

[OOP](./OOP.md)<br>
[뒤로가기](../1Keywords.md)