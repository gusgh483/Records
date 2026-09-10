# 유니티 안드로이드 빌드
---
[뒤로가기](../../2Unity.md)

---
## 빌드
1) File - Build Profile - 안드로이드 플랫폼 선택 후 Swtich Platform
2) 빌드 전 씬이 추가되어 있는지 확인 (Open Scene List)

## 프로젝트
ProjectSettings - Player - <br>
- Company Name (회사 이름)
- Product Name (앱 이름)
- Default Icon (앱 아이콘 이미지)<br><br>

- Resolution and Presentation
	- Fullscreen Mode => Fullscreen Window(꽉찬 화면)
	- Orientation
		- Default Orientation => Auto Rotation(자동 회전) / Portrait(세로) / LandScape(가로) <br><br>

- Splash Image
	- Virtual Reality Splash Image (시작로고)<br><br>

- Other Settings
	- Identification
		- Minimum API Level (구버전 작동 범위)
	- Configuration
		- Scripting Backend => IL2CPP
	- Active Input Handling => Input System Package (Both <- X)
	- Android Application Configuration
		- Target Architectures - ARM64 (무조건 체크)<br><br>

- Publishing Settings
	- Project Keystore (테스트 시 x  / 정식 출시할 때 반드시 필요)<br><br>
---
[뒤로가기](../../2Unity.md)