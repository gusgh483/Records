# 프로그래밍 일지
- [작성 양식 모음](./Styles/Styles.md)
- [데일리 체크](./DailyCheck.md)

## 이론
1) [키워드 정리](./Theory/1Keywords.md)
2) [유니티](./Theory/2Unity.md)
3) [C#](./Theory/3Csharp.md)
4) [수학](./Theory/4Math.md)
5) [영어](./Theory/5English.md)

## 메모

#### 2026.07.07
---

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


#### 2026.07.14

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

-> 1번 체택
------------

[에러발생]

InvalidOperationException: Collection was modified; enumeration operation may not execute. System.Collections.Generic.HashSet`1+Enumerator[T].MoveNext () (at <5dcb76a18315462e964f1bedee980471>:0) ContinuousWeapon+<ContinousRoutine>d__12.MoveNext () (at Assets/1.Scripts/1.Play/1.Upgrade/0.Weapon/2.ContinuousWeapon/ContinuousWeapon.cs:62) UnityEngine.SetupCoroutine.InvokeMoveNext (System.Collections.IEnumerator enumerator, System.IntPtr returnValueAddress) (at <254a04541d1c4a7b9e9c9e594cf72b4c>:0) 

원인: foreach문에서 새로 생성한 리스트를 참조해야하는데 헤시셋을 참고하여 꼬여버림