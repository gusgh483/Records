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
	 Apply