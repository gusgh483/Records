# Unity Graphics

링크: https://unitygraphics.web.app


## 0. Home

### 1) CPU
- 역할: 게임 상태를 갱신하고, 이번 프레임에 무엇을 어떤 순서와 설정으로 그릴지 준비하 GPU에 명령을 제출
- 주요 작업: 입력, C# 스크립트, 물리, AI, 애니메이션, 게임 로직 등, Transform 갱신 등등

### 2) GPU
- 역할: CPU가 전달한 랜더링 명령에 따라 수많은 정점과 픽셀을 병렬로 계산하여 최종 화면을 만든다.
- 주요 작업: 정점 변환, 레스터라이징, 픽셀 셰이딩, 텍스쳐 샘플링, 조명과 글미자, 깊이 테스트, 블랜딩, 후처리 등등<br> <br> 

---
## 1. 3D 모델 구조
#### - 3D 모델은 크게 모양을 담당하는 Mesh와 표면을 담당하는 Material로 볼 수 있다.
#### - Mesh에는 Vertex와 Index 정보가 포함된다.
#### - Vertex는 Position, Normal, Tangent, UV 같은 속성을 가진 데이터이다.
#### - Index는 Vertex를 가리키는 번호이고, 3개가 모이면 하나의 삼각형을 이룬다.
#### - Material은 표면 표현에 필요한 값 묶음으고, Shader는 그 값들을 사용해 GPU가 최종 색 계산을 하도록 하는 프로그램이다.<br> <br> 

### 1) Vertex는 위치 정보만 갖고있다?
- 위치(Position)뿐 아니라 Normal, Tangent, UV처럼 랜더링에 필요한 여러 속성을 함께 가진 단위 데이터이다.

### 2) Vertex가 많으면 무조건 더 좋은 3D 모델인가?
- Vertex가 많으면 더 많은 삼각형으로 더 세밀한 형태를 만들 수 있지만 처리 비용도 늘어나므로 품질과 성능의 균형이 필요하다.

### 3) 왜 사각형이 아니고 삼각형인가?(3D 모델이 기본 도형 단위가 왜 삼각형인가?)
- GPU는 점, 선, 삼각형 같은 Primitive를 처리할 수 있다.
- 삼각형은 세 점으로 항상 하나의 평면이 결정되기 때문에 GPU가 표면을 안정적으로 보간하고 처리하기 좋다.

### 4) Index의 필요성
- Index를 사용하면 여러 삼각형이 같은 Vertex 데이터를 번호로 재사용하는 게 가능하고 삼각형 연결 순서도 명확하게 지정할 수 있다.

### 5) 같은 위치에 Vertex가 여러 개 있을 수도 있나요?
- 같은 위치라도 Normal이나 UV가 다르면 랜더링에서는 서로 다른 Vertex로 분리될 수 있다.

### 6) UV는 왜 x, y가 아니라 u, v로 사용하는가?
- 3D 공간이 x, y, z 좌표와 구분된 2D Texture 좌표이므로 관습적으로 u, v로 사용한다.

### 7) Normal과 Tangent 방향은 무엇인가?
- Normal은 표면 바깥 방향, Tangent는 표면 위의 U축 기준 방향
- 서로 다른 조명 계산 역할을 가진다.

### 8) Texture 외에 Material이 굳이 필요한 이유?
- Material은 Texture를 포함한 색상, 금속성, 거칠기 등 랜더링과 관련된 여러 데이터를 Shader에 전달하는 데이터 묶음이다.

### 9) Material과 Shader의 차이
- Material은 Shader에 넣는 값의 묶음이고, Shader는 그 값으로 최종 화면 색을 계산하는 방법이다.

### 10) 휴머노이드 애니메이션(스켈레탈 애니메이션)을 쓰면 Mesh 데이터 자체가 계속 변경되는가?
- 원본 Mesh를 매번 새로 만드는 것이 아니라 뼈대와 가중치로 현재 자세의 정점 위치를 매 프레임 변형해 그린다.

### 11) 이 내용이 실제 Unity 작업에 필요한가?
- Mesh 구조를 알면 Texture, Light, Normal Map, Shader 등 문제가 어떤 데이터에서 발생했는지 추론 가능

### 12) 유니티 6 URP에서 한 화면에 정점 몇 개 정도까지 괜찮은가?
- 안전한 정점 수는 고정되어 있지 않고, 목표 기기에서 전체 랜더링 조건을 포함해 측정하고 판단해야 한다.<br> <br> 

---
## 2. 3D 모델 시각화
#### - Vertex의 Position은 처음부터 화면상의 좌표가 아니라 Local Space 좌표이다.
#### - Transform의 Scale, Rotation, Translation은 Model Matrix를 만들고,<br>Model Matrix는 Local 좌표를 World 좌표로 변환한다.   
#### - Camera의 위치와 회전은 View Matrix와 연결되고, View Matrix는 World 좌표를 카메라 기준으로 변환한다. 
#### - Projection Matrix는 FOV, Near, Far, Aspect를 반영해 View 좌표를 Clip 좌표로 변환한다. 
#### - 이후 Clip 좌표를 NDC, Viewport, Screen 변환을 거쳐 실제 화면 픽셀 위치로 변환한다. 
#### - 3D 모델이 화면에 보이는 것은 정점 위치가 여러 좌표계 변환을 거쳐 최종적으로 Screen 좌표가 되기 때문이다.<br> <br> 

### 1) Transform을 바꾸면 Mesh의 원본 Vertex Position이 변경되는가?
- 일반적인 Transform 조작은 Mesh 원본을 바꾸지 않고 랜더링 할 때 Local 좌표를 World 좌표로 변환한다.

### 2) Model = T * R * S 인데 왜 적용 순서는 S, R, T인가요?
- 오른쪽 행렬부터 순서대로 계산되므로 실제 적용 순서는 S, R, T이다.

### 3) 유니티 인스펙터뷰의 Rotation 값은 행렬인가요?
- 인스펙터에는 오일러각으로 보이지만 내부적으로는 Matrix(행렬)로 변환되어 계산에 사용된다.

### 4) Viewport 좌표와 Screen 좌표의 차이
- Viewport 좌표는 카메라 화면 안의 정규화된 위치(0~1 사이)이고 Screen 좌표는 실제 해상도에 대응하는 픽셀 위치이다.

### 5) Normal은 Position과 같이 행렬 변환 계산이 이루어지는가?
- Normal은 이동(Translation)의 영향을 받지 않는 방향이기에 Position과 다르게 변환이 이루어진다.

### 6) 2D 카메라(Orthopraphic Camera)도 MVP(Model, View, Projection) 변환이 이루어지는가?
- 2D 카메라도 MVP 변환을 사용하지만 원근 축소가 없는 Orthopraphic Projection Matrix를 사용한다.

### 7) UI 좌표와 Screen 좌표가 같은 것인가?
- Screen 좌표는 픽셀 기준이지만 UI 좌표는 Canvas, Canvas Scaler, Anchor, Pivot 설정에 따라 다르게 해석된다.

### 8) 3D 애니메이션도 이 과정을 거치는가?
- 3D 애니메이션으로 변형된 정점도 최종적으로 Model, View, Projection 변환을 거쳐 화면에 표시된다.

### 9) 왜 좌표 변환에서 행렬을 사용하는가?
- 행렬은 여러 좌표 변환을 같은 형식으로 결합해 GPU가 많은 정점들에 효율적으로 반복 적용하게 해 준다.

```
[정리]
Local(중심부로부터의 상대적 좌표) --Model Matrix( Scale, Rotation, Translation)-->
World(월드 상의 좌표) --View Matrix-->
View(카메라로부터의 상대적 좌표) --Projection Matrix(FOV, Near, Far, Aspect)-->
Clip(원근법 계산을 적용한 좌표) --NDC(정규화 좌표) -- Viewport 변환-->
Screen(원근법이 적용된 스크린상의 최종 좌표)
```
---
## 3. GPU 랜더 파이프라인
#### - Mesh 데이터가 GPU 안에서 어떤 과정을 거쳐 최종 픽셀이 되는지
#### - CPU가 그리기 명령(Draw Call)을 보내면 GPU는 IA, VS, RS, PS, OM 단계를 거친다.
#### - IA는 Vertex와 Index를 읽어 Primitive를 준비한다.
#### - VS는 정점마다 실행되어 위치와 전달 값을 계산한다.
#### - RS는 삼각형을 화면의 픽셀 후보(Fragment)로 바꾸고. UV와 Normal 같은 값을 보간한다.
#### - PS는 픽셀 후보마다 Texture, Material, Light 정보 등을 사용해 색을 계산한다.
#### - OM는 Depth Test(깊이 테스트), Stencil Test, Blending 등을 거쳐 최종 Render Target에 기록한다.
#### - 핵심은 단순 단계 이름 암기가 아니라,<br>데이터가 정점에서 삼각형, 픽셀 후보, 최종 픽셀로 바뀌는 흐름을 파악하는 것.<br> <br> 

### 1) GPU 랜더 파이프라인과 Unity URP?
- GPU 랜더 파이프라인은 GPU 내부 처리 흐름이고,<br>
  URP는 그 흐름을 활용해 랜더링 절차와 품질을 구성하는 Unity의 프레임워크이다.

### 2) VS와 PS는 왜 둘 다 Shader라고 하는가?
- VS와 PS는 실행 대상과 역할은 다르지만 모두 GPU에서 실행되는 프로그래밍 가능한 Shader 단계이다.

### 3) Resterizer 단계에서 하는 일
- 삼각형이 덮는 픽셀 후보를 만들고 정점의 UV와 Normal 같은 속성 값들을 픽셀 후보마다 보간하여 계산한다.

### 4) Fragment와 Pixel의 차이
- Fragment는 최종 기록 전의 픽셀 후보이고, Pixel은 Render Target에 실제로 기록된 화면상의 점에 가깝다.

### 5) Compute Shader도 랜더 파이프라인 단계인가?
- Compute Shader는 그래픽 파이프라인 단계가 아니라, GPU가 범용 계산을 수행하고 그 결과를 제공할 수 있는 별도 Shader이다.

### 6) Pixel Shader에서 계산한 색이 무조건 화면에 출력되는가?
- Pixel Shader가 색을 계산해도 Depth/Stencil Test와 Blending 결과에 따라 최종 화면에 기록되지 않거나, 다르게 기록될 수 있다.

### 7) CPU가 Draw Call을 많이 보내면 GPU만 부하가 발생하는가?
- GPU뿐 아니라 CPU의 명령 준비와 랜더링 상태 변경 비용도 함꼐 발생할 수 있다.<br> <br> 

---
## 4. 셰이더와 메터리얼
#### - Shader는 GPU가 정점과 픽셀을 계산하는 방법, Material은 그 Shader에 넣을 값 묶음.<br> <br> 

#### - Pixel Shader는 UV로 Texture를 샘플링하고, Base Color를 곱해 Surface Color를 계산한다.
#### - Normal Map은 실제 Mesh를 바꾸지 않고 빛 계산에 사용할 표면 방향을 바꾼다.
#### - Metalic, Smoothness는 빛 계산에 영향을 준다.
#### - Emission은 자체 발광 색(실제로 조명 역할을 하는 건 아니다.)
#### - Alpha는 투명도 계산에 영향을 준다.<br> <br> 

#### - 유니티에서 Material Inspector에서 값을 바꾸는 것은 Shader 계산의 입력값을 바꾸는 일이다.<br> <br> 

### 1) 같은 Texture인데 왜 표면이 다르게 보일 수 있는가?
- Texture는 입력 값 중 하나이므로 다른 Material 값과 조명 환경 등이 달라지면<br>
  같은 Texture도 다르게 보인다.

### 2) Base Map과 Base Color는 어떻게 계산되는가?
- 일반적으로 UV로 읽은 Base Map 색에 Base Color를 곱해 표면의 기본 색을 만든다.

### 3) Normal Map은 실제 Mesh를 변경하는가?
- Mormal Map은 실제 Vertex의 Position을 바꾸지 않고 조명 계산에 사용할 표면 방향만 변경한다.

### 4) Normal Map 파일은 왜 그림 파일처럼 보이나요?
- RGB 값을 사용하기 때문에 그림 파일로 볼 수는 있지만,<br>
  실제로는 Shader가 해석할 표면 방향 백터 데이터를 저장한 파일이다.

### 5) Emission은 빛을 실제로 내는가?
- 표면 자체를 밝게 보이게 할 뿐, 주변을 밝히려면 조명이나 GI 설정 같은 별도 처리가 필요하다.

### 6) Base Color Alpha 값을 수정했는데 Unity에서 투명해지지 않는 이유
- Alpha 값만 조절하는 것으로는 부족하고 Surface Type이 Transparent여야 하고 Blending이 동작해야 한다.

### 7) Material이 너무 많으면 성능에 영향이 있을 수 있는가?
- Material이 과도하게 나뉘면 상태 변경과 Draw Call, Set Pass 준비 비용이 증가할 수 있다.

### 8) 유니티 Shader Graph를 쓰는 것과 Shader를 직접 코딩하는 것의 차이
- Shader Graph는 Shader를 노드 기반으로 작성하는 도구이고<br>
  최종적으로 GPU에서 실행되는 Shader 코드로 변환된다.

### 9) URP Lit과 Unlit은 무엇이 다른가?
- Lit은 조명 반응 계산을 하지만, Unlit은 조명 영향을 거의 받지 않고 색과 Texture를 직접 보여 준다.

### 10) 분홍색 오브젝트는 왜 생기는가?
- 분홍색 Material은 대게 Shader 누락, 랜더 파이프라인 비호환, Shader 컴파일 실패 등
  주로 Pixel Shader 단계 문제를 뜻한다.

### 11) Material 값은 CPU, GPU 어느 쪽에 있는가?
- Material은 프로젝트 데이터로 관리되지만(CPU), 랜더링에 필요한 값과 Texture는 GPU로 전달되어 사용된다.<br> <br> 

---
## 5. 라이트와 그림자
#### - Material이 Surface Color를 만들면, 여기에 Ambient, Diffuse, Specular가 더해진다.
#### - Ambient는 방향과 관계없는 기본 밝기이다.
#### - Diffuse는 Normal과 라이트 방향이 마주 보는 정도에 따라 생기는 밝기이다.(벡터 내적, Dot Product)
#### - Specular는 라이트 반사 방향과 카메라 방향이 맞을 때 생기는 하이라이트이다.
#### - 그림자는 라이트가 직접 닿는지 판단해서 Diffuse, Specular와 같은 직접광을 줄인다.
#### - 라이트 베이킹은 정적ㅇ니 조명 결과를 미리 계산해 Lightmap이나 Light Probe로 저장함으로써 런타임 조명 계산 비용을 줄이는 방법이다.<br> <br> 

### 1) Ambient는 진짜 조명(빛)인가?
- Ambient는 하나의 광원이라기보다 주변 밝기를 단순화해 표현하는 조명 성분이다.

### 2) Diffuse의 Dot Product 사용
- Normal과 라이트 방향이 얼마나 마주 보는지 수치로 나타내므로 Diffuse 밝기 계산에 적합히다.

### 3) Normal이 이상하면 조명에 따른 결과로 이상해지는가?
- Diffuse와 Specular가 Normal을 사용하므로 Normal이 잘못되면 조명의 밝기와 반사 방향도 잘못될 수 있다.

### 4) Specular는 무엇인가?
- Specular는 빛이 표면에서 반사되어 카메라 방향으로 들어오는 하이라이트 성분이다.

### 5) Directional Light 위치가 의미 있는가?
- 무한히 먼 광원을 가정하므로 조명  결과에는 위치보다 Rotation으로 정한 빛의 방향이 중요하다.

### 6) Point Light, Spot Light의 Range(범위)
- Range(범위) 내의 오브젝트가 조명 계산 대상이 된다.

### 7) Soft Shadow는 대강 어떤 방식인가?
- Soft Shadow는 경계를 부드럽게 만들기 위해 Shadow Map을 여러 번 샘플링, 필터링  하기에 GPU 비용이 증가한다.

### 8) Baked Light는 실시간으로 바뀌지 않는가?
- Baked Light는 미리 계산해 저장해 둔 결과이므로 런타임의 변화가 자동으로 반영되지 않는다.

### 9) Lightmap은 무엇인가?
- Lightmap도 이미지처럼 샘플링되지만 텍스쳐와 달리 표면 무늬가 아니라 미리 계산된 조명과 그림자 정보를 저장한다.

### 10) 라이트를 많이 사용하면 왜 성능이 저하되는가?
- 오브젝트와 픽셀마다 계산해야 할 조명 정보가 늘어난다.

### 11) 그림자 안에서는 Diffuse와 Specular가 모두 사라지는가?
- 그림자 안에서는 직접광인 Diffuse와 Specular가 줄어들지만, Ambient는 남을 수 있다.<br> <br> 

---
## 6. 드로우콜과 배칭
#### - 많은 오브젝트는 GPU의 픽셀 계산뿐 아니라 CPU의 그리기 명령 준비 비용도 만든다.
#### - Draw Call은 CPU가 GPU에 보내는 그리기 명령이고, Batch는 유니티에서 묶어서 처리하는 랜더링 작업 단위이다.
#### - SetPass는 랜더 상태 준비와 연결된다.<br> <br> 

#### - Material 정렬은 상태 변경을 줄이고, Static Batching은 움직이지 않는 오브젝트를 미리 묶어 Draw Call 부담을 줄인다.
#### - GPU Instancing은 같은 Mesh와 Material이 반복될 때 부담을 줄일 수 있다.
#### - SRP Batcher는 SRP 환경에서 상태 세팅 비용을 줄인다.
#### - 중요한 것은 측정이다. CPU 병목인지, GPU 병목인지 확인하여 그에 맞는 최적화를 해야 한다.<br> <br> 


### 1) Draw Call과 Batch가 같은 말인가?
- Draw Call은 GPU에 보내는 그리기 명령이고, <br>
Batch는 유니티가 묶어 처리한 랜더링 작업 단위이다.

### 2) SetPass Call은 Draw Call과 무엇이 다른가?
- Draw Call은 실제 그리기 명령이고,<br>
SetPass Call은 그 전에 렌더 상태를 준비하는 작업이다.

### 3) 오브젝트 수가 많으면 항상 Draw Call도 많아지는가?
- 오브젝트가 많아도 Mesh와 Material을 공유하고,<br>
배칭  조건이 맞으면 더 적은 Draw Call로 처리할 수 있다.

### 4) Material이 다르면 왜 묶기 어려운가?
- Material이 다르면 Shader, Texture 등 렌더 상태를<br>
다시 설정해야 하므로 같은 상태로 묶기 어렵다.

### 5) Static Batching은 어떻게 이루어지는가?
- 움직이지 않는 오브젝트의 Mesh 정보를 미리 결합해<br>
더 적은 그리기 단위로 처리하는 것이다.
- 다만, 결합된 데이터 때문에 메모리 사용은 증가할 수 있다.

### 6) Dynamic Batching은 덜 중요한가?
- 조건이 까다롭고 런타임 CPU 비용도 있어
현재 프로젝트에서는 다른 방식에 비해 덜 중요하다.

### 7) GPU Instancing은 어떤 방식인가?
- 같은 Mesh와 Material의 반복 오브젝트를 인스턴스 데이터로 그린다. (ex. 나무)

### 8) SRP Batcher가 무엇을 하는가?
- Draw Call 수보다는 렌더 상태 세팅을 효율적으로 하게 하는 역할이다.

### 9) Transparent(투명) 오브젝트는 왜 배칭이 어려운가?
- Blending 결과가 그리기 순서에 의존하기 때문에<br>
자유롭게 정렬하고 묶기 어렵다.

### 10) Profiler와 Frame Debugger
- Profiler는 시간이 오래 걸리는 영역을 찾고,<br>
Frame Debugger는 한 프레임의 Draw Call과 실행 흐름을 추적할 수 있다.<br> <br> 

---
## 7. 유니티 URP
#### - URP는 3단원의 IA, VS, RS, PS, OM 같은 GPU 내부 단계가 아니라,<br>유니티가 장면을 어떤 설정과 옵션으로 랜더링할지 정하는 랜더링 환경
#### - Built-in은 유니티 6 기준 구버전 렌더 파이프라인이고,<br> HDRP는 고사양 고품질 프로젝트용인데 새 기능 추가가 미정
#### - 따라서 유니티 6에서는 URP가 기본이다.
#### - URP 설정은 Graphic Settings, Quality Settings, URP Asset, Render Asset으로 나뉜다.
#### - URP Asset에서 Render Scale, MSAA(계단 현상 방지), HDR, Light, Shadow 같은 옵션들로<br>성능과 품질을 조절할 수 있다.
#### - Post-Processing(후처리)은 완성된 화면에 더 풍부한 효과를 제공하는 후처리 기능이다.
#### - Quality Settings에서 여러 URP Asset을 플렛폼, 유저 선택에 따라 다르게 제공할 수 있다.<br> <br> 

#### - 게임을 거의 완성한 상태에서 성능과 품질 사이의 균형을 잘 맞추면 된다.<br> <br> 

### 1) URP를 쓰면 무조건 성능이 좋은가?
- URP도 품질 옵션을 무겁게 설정하면 느려질 수 있다.

### 2) URP Asset은 하나만 만들면 되는가?
- 플렛폼, 품질 단계별 옵션이 다르면 Mobile, PC와 같이<br>
여러 URP Asset으로 나누어 관리할 수 있다.
- Quality Settings에서 품질 단계를 나누고<br> 
각 단계에서 사용할 실제 렌더링 옵션을 URP Asset으로 설정할 수 있다.

### 3) Render Scale과 게임 해상도가 같은 것인가?
- 게임 해상도는 출력 크기이고,<br>
Render Scale은 출력에 사용할 내부 렌더링  해상도 비율이다.

### 4) Shadow Distance를 줄이면 게임 성능이 왜 좋아지는가?
- 그림자를 만들고 적용할 공간이 줄어들어 부담도 줄어든다.

### 5) URP 옵션을 바꿨는데 화면 차이가 별로 없는 경우
- 그 옵션을 사용하는 효과가 현재 화면에 없을 수 있다.