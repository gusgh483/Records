# 임시 메모장

[뒤로가기](../README.md)

---
### 2026.07.07


- Toggle의 SetIsOnWithoutNotify() 함수 - UnityEngine.UI <br>
각 변경에 대한 이벤트 발행 없이 isOn 값만 변경하는 함수
```
_bgmToggle.SetIsOnWithoutNotify(!_soundManager.IsBgmMute);
```

-  어떤 객체의 Awake()가 먼저 실행될지 불분명하므로 먼저 실행되기 원하는 특정 객체에 대해 <br>
Project Settings 에서 우선순위를 설정해줄 수 있다.<br>
(다만, 남발하지 않는 것이 좋다.)
	- ▶ Project Settings <br>▶ Script Execution Order <br>▶ ( + ) 아이콘 클릭 (Add script to custom order)<br>
    ▶ 원하는 객체명 클릭하여 추가<br> ▶ 낮은 숫자일 수록 더 빨리 실행되므로 0보다 작은 숫자로 설정 (-1) <br>▶
	 Apply<br> 
---

### 2026.07.14

[개선점] 각 개별 Enemy 객체에 대한 Coroutine 함수를 일일히 수행하면
과부하가 생길 우려가 있다.

첫번째 방법 - Event Action 구독 / HashSet 사용

1) 공격 코루틴은 시작과 동시에 실행
2) OnTriggerEnter2D - 범위 내에 닿은 적 HashSet에 담기
3) OnTriggerExit2D - 범위를 벗어난 적 HashSet에서 빼기
4) 적이 범위 내에서 죽어 null이 될 시 HashSet에서 빠지도록 설계

두번째 방법 - Physics.OverlapSphere 사용

1) 범위 내 닿은 적이 배열로 반환되고 그 배열 안에 존재하는 적들만 때리기
2) 적이 범위 내에서 때리기도 전에 죽어 null이 될 시 무시하고 넘어가도록 설계
= 충돌체의 사이즈를 별도로 계산하는 번거로움

-> 첫번째 방법 체택


[에러발생]

InvalidOperationException: Collection was modified; enumeration operation may not execute. System.Collections.Generic.HashSet`1+Enumerator[T].MoveNext () (at <5dcb76a18315462e964f1bedee980471>:0) ContinuousWeapon+<ContinousRoutine>d__12.MoveNext () (at Assets/1.Scripts/1.Play/1.Upgrade/0.Weapon/2.ContinuousWeapon/ContinuousWeapon.cs:62) UnityEngine.SetupCoroutine.InvokeMoveNext (System.Collections.IEnumerator enumerator, System.IntPtr returnValueAddress) (at <254a04541d1c4a7b9e9c9e594cf72b4c>:0) 

원인: foreach문에서 새로 생성한 리스트를 참조해야하는데 헤시셋을 참고하여 꼬여버림

---
### 2026.07.16

직렬화(Serialization)

16 core 32Thread(4Ghz)

직렬: 통로(Stream)가 하나 - 선입선출 / 순차적
병렬: 통로가 여러개

<br><br>

CPU(Register)<br> 
ㅣ<br> 
Cache<br> 
ㅣ<br> 
RAM<br> 
ㅣ<br> 
I/O(입출력장치) : Keyboard / SSD / Network . . . .<br> 
ㄴ Inout용 통로<br> 
ㄴ Output용 통로
<br> 
최소단위(Byte)로 통로를 지나간다 - ByteStream

<br><br>

입력방식)
text => Json : 읽어들이는 속도는 느리지만 편집 용이
binary : 배포 시 Json을 binary로 변환(뜯어볼 수 없다)
<br> 
ByteStream에 통과시키기 위해 Byte 단위로 변환하는 작업 = 직렬화


---
### 2026.07.20

https://unitygraphics.web.app/#home

<br> <br> 
0. Home
1) CPU
- 역할: 게임 상태를 갱신하고, 이번 프레임에 무엇을 어떤 순서와 설정으로 그릴지 준비하 GPU에 명령을 제출
- 주요 작업: 입력, C# 스크립트, 물리, AI, 애니메이션, 게임 로직 등, Transform 갱신 등등

2) GPU
- 역할: CPU가 전달한 랜더링 명령에 따라 수많은 정점과 픽셀을 병렬로 계산하여 최종 화면을 만든다.
- 주요 작업:정점 변환, 레스터라이징, 픽셀 셰이딩, 텍스쳐 샘플링, 조명과 글미자, 깊이 테스트, 블랜딩, 후처리 등등
<br> <br> 

1. 3D 모델 구조
- 3D 모델은 크게 모양을 담당하는 Mesh와 표면을 담당하는 Material로 볼 수 있다.
- Mesh에는 Vertex와 Index 정보가 포함된다.
- Vertex는 Position, Normal, Tangent, UV 같은 속성을 가진 데이터이다.
- Index는 Vertex를 가리키는 번호이고, 3개가 모이면 하나의 삼각형을 이룬다.
- Material은 표면 표현에 필요한 값 묶음으고, Shader는 그 값들을 사용해 GPU가 최종 색 계산을 하도록 하는 프로그램이다.

1) Vertex는 위치 정보만 갖고있다?
- 위치(Position)뿐 아니라 Normal, Tangent, UV처럼 랜더링에 필요한 여러 속성을 함께 가진 단위 데이터이다.

2) Vertex가 많으면 무조건 더 좋은 3D 모델인가?
- Vertex가 많으면 더 많은 삼각형으로 더 세밀한 형태를 만들 수 있지만 처리 비용도 늘어나므로 품질과 성능의 균형이 필요하다.

3) 왜 사각형이 아니고 삼각형인가?(3D 모델이 기본 도형 단위가 왜 삼각형인가?)
- GPU는 점, 선, 삼각형 같은 Primitive를 처리할 수 있다.
- 삼각형은 세 점으로 항상 하나의 평면이 결정되기 때문에 GPU가 표면을 안정적으로 보간하고 처리하기 좋다.

4) Index의 필요성
- Index를 사용하면 여러 삼각형이 같은 Vertex 데이터를 번호로 재사용하는 게 가능하고 삼각형 연결 순서도 명확하게 지정할 수 있다.

5) 같은 위치에 Vertex가 여러 개 있을 수도 있나요?
- 같은 위치라도 Normal이나 UV가 다르면 랜더링에서는 서로 다른 Vertex로 분리될 수 있다.

6) UV는 왜 x, y가 아니라 u, v로 사용하는가?
- 3D 공간이 x, y, z 좌표와 구분된 2D Texture 좌표이므로 관습적으로 u, v로 사용한다.

7) Normal과 Tangent 방향은 무엇인가?
- Normal은 표면 바깥 방향, Tangent는 표면 위의 U축 기준 방향
- 서로 다른 조명 계산 역할을 가진다.

8) Texture 외에 Material이 굳이 필요한 이유?
- Material은 Texture를 포함한 색상, 금속성, 거칠기 등 랜더링과 관련된 여러 데이터를 Shader에 전달하는 데이터 묶음이다.

9) Material과 Shader의 차이
- Material은 Shader에 넣는 값의 묶음이고, Shader는 그 값으로 최종 화면 색을 계산하는 방법이다.

10) 휴머노이드 애니메이션(스켈레탈 애니메이션)을 쓰면 Mesh 데이터 자체가 계속 변경되는가?
- 원본 Mesh를 매번 새로 만드는 것이 아니라 뼈대와 가중치로 현재 자세의 정점 위치를 매 프레임 변형해 그린다.

11) 이 내용이 실제 Unity 작업에 필요한가?
- Mesh 구조를 알면 Texture, Light, Normal Map, Shader 등 문제가 어떤 데이터에서 발생했는지 추론 가능

12) 유니티 6 URP에서 한 화면에 정점 몇 개 정도까지 괜찮은가?
- 안전한 정점 수는 고정되어 있지 않고, 목표 기기에서 전체 랜더링 조건을 포함해 측정하고 판단해야 한다.
<br> <br> 

2. 3D 모델 시각화
- Vertex의 Position은 처음부터 화면상의 좌표가 아니라 Local Space 좌표이다.
- Transform의 Scale, Rotation, Translation은 Model Matrix를 만들고,<br> 
  Model Matrix는 Local 좌표를 World 좌표로 변환한다. 
- Camera의 위치와 회전은 View Matrix와 연결되고, View Matrix는 World 좌표를 카메라 기준으로 변환한다. 
- Projection Matrix는 FOV, Near, Far, Aspect를 반영해 View 좌표를 Clip 좌표로 변환한다. 
- 이후 Clip 좌표를 NDC, Viewport, Screen 변환을 거쳐 실제 화면 픽셀 위치로 변환한다. 
- 3D 모델이 화면에 보이는 것은 정점 위치가 여러 좌표계 변환을 거쳐 최종적으로 Screen 좌표가 되기 때문이다.

1) Transform을 바꾸면 Mesh의 원본 Vertex Position이 변경되는가?
- 일반적인 Transform 조작은 Mesh 원본을 바꾸지 않고 랜더링 할 때 Local 좌표를 World 좌표로 변환한다.

2) Model = T * R * S 인데 왜 적용 순서는 S, R, T인가요?
- 오른쪽 행렬부터 순서대로 계산되므로 실제 적용 순서는 S, R, T이다.

3) 유니티 인스펙터뷰의 Rotation 값은 행렬인가요?
- 인스펙터에는 오일러각으로 보이지만 내부적으로는 Matrix(행렬)로 변환되어 계산에 사용된다.

4) Viewport 좌표와 Screen 좌표의 차이
- Viewport 좌표는 카메라 화면 안의 정규화된 위치(0~1 사이)이고 Screen 좌표는 실제 해상도에 대응하는 픽셀 위치이다.

5) Normal은 Position과 같이 행렬 변환 계산이 이루어지는가?
- Normal은 이동(Translation)의 영향을 받지 않는 방향이기에 Position과 다르게 변환이 이루어진다.

6) 2D 카메라(Orthopraphic Camera)도 MVP(Model, View, Projection) 변환이 이루어지는가?
- 2D 카메라도 MVP 변환을 사용하지만 원근 축소가 없는 Orthopraphic Projection Matrix를 사용한다.

7) UI 좌표와 Screen 좌표가 같은 것인가?
- Screen 좌표는 픽셀 기준이지만 UI 좌표는 Canvas, Canvas Scaler, Anchor, Pivot 설정에 따라 다르게 해석된다.

8) 3D 애니메이션도 이 과정을 거치는가?
- 3D 애니메이션으로 변형된 정점도 최종적으로 Model, View, Projection 변환을 거쳐 화면에 표시된다.

9) 왜 좌표 변환에서 행렬을 사용하는가?
- 행렬은 여러 좌표 변환을 같은 형식으로 결합해 GPU가 많은 정점들에 효율적으로 반복 적용하게 해 준다.
<br> <br> <br> 

Local(중심부로부터의 상대적 좌표) --Model Matrix( Scale, Rotation, Translation)--><br> 
World(월드 상의 좌표) --View Matrix--><br> 
View(카메라로부터의 상대적 좌표) --Projection Matrix(FOV, Near, Far, Aspect)--><br> 
Clip(원근법 계산을 적용한 좌표) --NDC(정규화 좌표) -- Viewport 변환--><br> 
Screen(원근법이 적용된 스크린상의 최종 좌표)<br> 


3. GPU 랜더 파이프라인
- Mesh 데이터가 GPU 안에서 어떤 과정을 거쳐 최종 픽셀이 되는지
- CPU가 그리기 명령(Draw Call)을 보내면 GPU는 IA, VS, RS, PS, OM 단계를 거친다.
- IA는 Vertex와 Index를 읽어 Primitive를 준비한다.
- VS는 정점마다 실행되어 위치와 전달 값을 계산한다.
- RS는 삼각형을 화면의 픽셀 후보(Fragment)로 바꾸고. UV와 Normal 같은 값을 보간한다.
- PS는 픽셀 후보마다 Texture, Material, Light 정보 등을 사용해 색을 계산한다.
- OM는 Depth Test(깊이 테스트), Stencil Test, Blending 등을 거쳐 최종 Render Target에 기록한다.
- 핵심은 단순 단계 이름 암기가 아니라,
  데이터가 정점에서 삼각형, 픽셀 후보, 최종 픽셀로 바뀌는 흐름을 파악하는 것.

1) GPU 랜더 파이프라인과 Unity URP?
- GPU 랜더 파이프라인은 GPU 내부 처리 흐름이고,
  URP는 그 흐름을 활용해 랜더링 절차와 품질을 구성하는 Unity의 프레임워크이다.

2) VS와 PS는 왜 둘 다 Shader라고 하는가?
- VS와 PS는 실행 대상과 역할은 다르지만 모두 GPU에서 실행되는 프로그래밍 가능한 Shader 단계이다.

3) Resterizer 단계에서 하는 일
- 삼각형이 덮는 픽셀 후보를 만들고 정점의 UV와 Normal 같은 속성 값들을 픽셀 후보마다 보간하여 계산한다.

4) Fragment와 Pixel의 차이
- Fragment는 최종 기록 전의 픽셀 후보이고, Pixel은 Render Target에 실제로 기록된 화면상의 점에 가깝다.

5) Compute Shader도 랜더 파이프라인 단계인가?
- Compute Shader는 그래픽 파이프라인 단계가 아니라, GPU가 범용 계산을 수행하고 그 결과를 제공할 수 있는 별도 Shader이다.

6) Pixel Shader에서 계산한 색이 무조건 화면에 출력되는가?
- Pixel Shader가 색을 계산해도 Depth/Stencil Test와 Blending 결과에 따라 최종 화면에 기록되지 않거나, 다르게 기록될 수 있다.

7) CPU가 Draw Call을 많이 보내면 GPU만 부하가 발생하는가?
- GPU뿐 아니라 CPU의 명령 준비와 랜더링 상태 변경 비용도 함꼐 발생할 수 있다.


4. 셰이더와 메터리얼
- Shader는 GPU가 정점과 픽셀을 계산하는 방법, Material은 그 Shader에 넣을 값 묶음.

- Pixel Shader는 UV로 Texture를 샘플링하고, Base Color를 곱해 Surface Color를 계산한다.
- Normal Map은 실제 Mesh를 바꾸지 않고 빛 계산에 사용할 표면 방향을 바꾼다.
- Metalic, Smoothness는 빛 계산에 영향을 준다.
- Emission은 자체 발광 색(실제로 조명 역할을 하는 건 아니다.)
- Alpha는 투명도 계산에 영향을 준다.

- 유니티에서 Material Inspector에서 값을 바꾸는 것은 Shader 계산의 입력값을 바꾸는 일이다.

1) 같은 Texture인데 왜 표면이 다르게 보일 수 있는가?
- Texture는 입력 값 중 하나이므로 다른 Material 값과 조명 환경 등이 달라지면<br> 
  같은 Texture도 다르게 보인다.

2) Base Map과 Base Color는 어떻게 계산되는가?
- 일반적으로 UV로 읽은 Base Map 색에 Base Color를 곱해 표면의 기본 색을 만든다.

3) Normal Map은 실제 Mesh를 변경하는가?
- Mormal Map은 실제 Vertex의 Position을 바꾸지 않고 조명 계산에 사용할 표면 방향만 변경한다.

4) Normal Map 파일은 왜 그림 파일처럼 보이나요?
- RGB 값을 사용하기 때문에 그림 파일로 볼 수는 있지만,
  실제로는 Shader가 해석할 표면 방향 백터 데이터를 저장한 파일이다.

5) Emission은 빛을 실제로 내는가?
- 표면 자체를 밝게 보이게 할 뿐, 주변을 밝히려면 조명이나 GI 설정 같은 별도 처리가 필요하다.

6) Base Color Alpha 값을 수정했는데 Unity에서 투명해지지 않는 이유
- Alpha 값만 조절하는 것으로는 부족하고 Surface Type이 Transparent여야 하고 Blending이 동작해야 한다.

7) Material이 너무 많으면 성능에 영향이 있을 수 있는가?
- Material이 과도하게 나뉘면 상태 변경과 Draw Call, Set Pass 준비 비용이 증가할 수 있다.

8) 유니티 Shader Graph를 쓰는 것과 Shader를 직접 코딩하는 것의 차이
- Shader Graph는 Shader를 노드 기반으로 작성하는 도구이고
  최종적으로 GPU에서 실행되는 Shader 코드로 변환된다.

9) URP Lit과 Unlit은 무엇이 다른가?
- Lit은 조명 반응 계산을 하지만, Unlit은 조명 영향을 거의 받지 않고 색과 Texture를 직접 보여 준다.

10) 분홍색 오브젝트는 왜 생기는가?
- 분홍색 Material은 대게 Shader 누락, 랜더 파이프라인 비호환, Shader 컴파일 실패 등<br> 
  주로 Pixel Shader 단계 문제를 뜻한다.

11) Material 값은 CPU, GPU 어느 쪽에 있는가?
- Material은 프로젝트 데이터로 관리되지만(CPU), 랜더링에 필요한 값과 Texture는 GPU로 전달되어 사용된다.

<br> 

---
### 2026.07.21

[할당 영역]<br> 
Automatic(자동) - Stack<br> 
Static(정적) - Data / 초기값이 없거나 0 혹은 null인 변수 - BSS (예약공간)<br> 
Dynamic(동적) - Heap<br> 
<br> 
정적 생성자 보유 => static class<br> 
<br>
C -> 기계어 : 컴파일링<br>
C# -> ByteCode(중간언어) -> 기계어 : 인터프리팅<br> 
<br> 
값형식을 참조형식으로 - Boxing<br> 
참조형식을 값형식으로 - Unboxing<br> 

<br> 

---
### 2026.07.22

```
// Sequential -> 데이터를 순차적으로
// Pack = 1 -> 1 byte 씩 (바이트패깅)

[StructLayout(LayoutKind.Sequential, Pack = 1)] 
public struct WeaponData
{
    public WeaponType Type;
    public float Power;
    public float Speed;
    public float Durabillity;
    public float CriticalRatio;
}
```
<br><br>

- Dictionary

| key(string) - 균형2진트리 | Value(int) |- Pair => Array
|:--- |:--- |
| "Oak" | 100 | 
| "Elf" | 250 |
| "Dwarf" | 80 |
| "Human" | 200 |
| "Poring" | 150 |

<br>

2진트리<br>
  . Root<br>
 . . Branch<br>
.. .. Leaf<br>
완전2진트리 , 포화2진트리
<br>

C# => AVR트리

<br> 

---
### 2026.07.23
-> 주말에 찾아서 공부할 것

#### 이진 트리
- 순회 방식(전위, 중위, 후위)<br> 
<br> 
- 시간복잡도 (최악 O)
```
O(1) > O(logn) > O(n) > O(n*logn) > O(n^2) > O(n^3)
```
<br> 

- O(n)
```
for (int i = 0; i < 101; i++)
{
    if (i == 100) 
        break;
}
```
<br> 

- O(n^2^)
```
for (int y = 0; y < n; y++)
{
    for (int x = 0; x < n; x++)
    {
            
    }
}
```
- 이진탐색 - O(logn)
    - 이진탐색 트리 - O(logn)
- 이진트리 종류
    - 완전이진트리
    - 포화이진트리
- 힙(Heap)
- 균형이진트리
    - AVL Tree
    - Red Black Tree
    - B Tree
<br><br>
#### 자료구조 (학문적)
- 선형
    - 배열(Array)
        - 고정형
        - 가변형(List)
            - 연결 리스트(Linked List)
                - 이중 연결 리스트(Duplex)
                - 원형 연결 리스트(Circular)
- 스택(Stack)
- 큐(Queue)
    - 스크롤(Scroll)
    - 쉘프(Shelf)
    - 데큐(Dequeue)
    - 힙(Heap)
<br><br>
- 비선형
    - 트리
        - 일반형 트리
        - n진형 트리
            - 2진 트리(Binary)
                - 순회(전, 중, 후)
                - 2진 탐색(Binary Search)
                - 2인 탐색트리(BST)
                - 힙(Heap)
                - 균형 2진트리
                    - AVL, RB, B
            - 4진 트리(Quad)
            - 8진 트리(Oct)
    - 그래프
        - 탐색
            - 깊이 우선 탐색(DFS)
            - 너비 우선 탐색(BFS)
        - 위상 정렬(Topology Sort)
        - 최소 비용 트리(MST)
            - 프림(Prim)
            - 크루스탈(Kruskal)
            - 솔린(Sollin) - 보루부카(Boruvka)
- 파일
    - 텍스트 파일(json, XML)
    - 바이너리 파일
    - 직렬화(Serialization)
<br><br>
#### 알고리즘
- 복잡도
    - 시간 복잡도
    - 공간 복잡도
- 탐색
    - 순차 탐색
    - 2진 탐색
    - 문자열 탐색
- 정렬
    - O(n^2)
        - 버블 정렬
        - 선택 정렬
        - 삽입 정렬
    - O(m*logn)
        - 병합 정렬
        - 퀵 정렬
- 길찾기(최단거리)
    - 다익스트라(데이크스트라)
    - A-Star(A*)
        - Nevigation Mesh
    - 벨만-포드 - O(n^2)
    - 플로워드-워셜 - O(n^3)

- 설계방식(구상방식)
    - 분할 / 정복
        - 분할된 문제끼리 영향을 주지 않음 -> 병렬 프로그래밍 가능
- 동적 프로그래밍
    - 메모이제이션
    - 병렬 프로그래밍 X
- 탐욕(Greedy)
- 백트래킹(Back Tracking)
<br> 

---
### 2026.07.27







<br> 

---
[뒤로가기](../README.md)