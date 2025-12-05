# 프로젝트 이름 SignalZero

## 📖 목차
1. [프로젝트 소개](#프로젝트-소개)
2. [게임조작 설명](#게임-설명서)
3. [주요기능](#주요기능)
4. [개발기간](#개발기간)
5. [기술스택](#기술스택)
6. [프로젝트 파일 구조](#프로젝트-파일-구조)
7. [Trouble Shooting](#trouble-shooting)


## 👨‍🏫 프로젝트 소개
- 프로젝트 명 : SignalZero
- 프로젝트 설명 : 3D 탑뷰 시점으로 진행되는 액션/슈팅 장르 게임
- 프로젝트 시작 계기 : 심화 팀프로젝트 팀 빌딩을 통해 기획자 & 개발자로 구성해 작업하는 첫 협업 프로젝트
- 프로젝트 구성 인원 : 김상혁, 김주원, 조현일, 송덕희, 유원영


## 💜 주요기능

-GameManager : 전역 매니저 생성 및 신 전용 일부 매니저 관리를 담당

-AudioManager : 전역 매니저로 각 신의 사운드 출력을 담당

-Charactermanager : 플레이어 프리펩을 생성하는 기능을 담당

-UIManager : MainScene 전용 UI패널들을 관리

-FieldManager : MainScene의 랜덤 필드생성 기능을 담당

-MonsterSpawnManager : 몬스터/중간보스 몬스터/보스 몬스터 생성을 담당

-ObjectPoolManager : 신에서 생성할 오브젝트들의 생성 및 회수를 담당

-BulletManager : BulletController에게 전달할 발사 명령 인풋 및 FX 출력을 담당

-PlayerWeaponManager : 플레이어의 공격 인풋을 받아 무기에게 발사명령을 전달하는 기능을 담당

-PlayerController : 플레이어의 움직임을 마우스 이동값을 통해 구현

-SectionDetector : 플레이어의 필드상 위치를 인식해 몬스터들의 생성로직을 구현

## 게임-설명서

<img width="776" height="424" alt="Image" src="https://github.com/user-attachments/assets/82742b93-d40d-4d0e-a842-53e91c3c7ed9" />

<img width="775" height="430" alt="Image" src="https://github.com/user-attachments/assets/a9fd0f58-5ed1-4e7e-8208-7c0ddce3873f" />

<img width="774" height="419" alt="Image" src="https://github.com/user-attachments/assets/12e9c73d-bead-4590-8cd3-7cd72799fa32" />

<img width="769" height="410" alt="Image" src="https://github.com/user-attachments/assets/5bb2fa75-56af-43cb-9243-2b1f532e64f6" />



## ⏲️ 개발기간
- 총 5일   { 2025.12.01(월) ~ 2025.12.05(금) }


## 🔧 기술스택

###  Language
*  C#

###  Version Control
*  Git + GitHubDesktop + Fork

###  IDE
* Visual Studio + Visual Studio Code

###  Framework
* net9.0


### 🚀 배포 (Deploy)
- **빌드 환경:** Unity 2022.3.62f2
- **배포 방식:** 
- **결과물:** 


## 프로젝트 파일 구조
```
📦Assets
 ┣ 📂01_Scenes
 ┃ ┣ 📜MainScene.unity
 ┃ ┗  📜TitleScene.unity
 ┣ 📂02_Scripts
 ┃ ┣ 📂Managers
 ┃ ┃ ┣ 📜AudioManager.cs
 ┃ ┃ ┣ 📜BulletManager.cs
 ┃ ┃ ┣ 📜CharacterManager.cs
 ┃ ┃ ┣ 📜FieldManager.cs
 ┃ ┃ ┣ 📜GameManager.cs
 ┃ ┃ ┣ 📜MonsterSpawnManager.cs
 ┃ ┃ ┣ 📜ObjectPoolManager.cs
 ┃ ┃ ┣ 📜PlayerWeaponManager.cs
 ┃ ┃ ┗ 📜UIManager.cs
 ┃ ┣ 📂Player
 ┃ ┃ ┣ 📜CameraFollow.cs
 ┃ ┃ ┣ 📜PlayerCombat.cs
 ┃ ┃ ┣ 📜PlayerController.cs
 ┃ ┃ ┣ 📜PlayerDash.cs
 ┃ ┃ ┗ 📜PlayerMovement.cs
 ┃ ┣ 📂UI
 ┃ ┃ ┣ 📂Tutorial
 ┃ ┃ ┃ ┗ 📜Tutorial.cs
 ┃ ┃ ┣ 📜CharacterUI.cs
 ┃ ┃ ┣ 📜Field.cs
 ┃ ┃ ┣ 📜GameStartUI.cs
 ┃ ┃ ┣ 📜Option.cs
 ┃ ┃ ┣ 📜UIPlayerController.cs
 ┃ ┃ ┣ 📜UISoundController.cs
 ┃ ┣ 📂Weapons
 ┃ ┃ ┣ 📜BulletController.cs
 ┃ ┃ ┣ 📜DropItem_Weapon.cs
 ┃ ┃ ┗📜Weapon.cs
 ┃ ┣ 📜GlobalBootstrap.cs
 ┣ 📂08_Inputs
 ┃ ┣ 📜PlayerInputActions.cs
 ┃ ┣ 📂Scripts
 ┃ ┃ ┣ 📂Cs
 ┃ ┃ ┃ ┣ 📜Monster.cs
 ┃ ┃ ┃ ┣ 📜MonsterAttackState.cs
 ┃ ┃ ┃ ┣ 📜MonsterBaseState.cs
 ┃ ┃ ┃ ┣ 📜MonsterDatas.cs
 ┃ ┃ ┃ ┣ 📜MonsterFsm.cs
 ┃ ┃ ┃ ┣ 📜MonsterIdleState.cs
 ┃ ┃ ┃ ┣ 📜MonsterMoveState.cs
 ┃ ┃ ┃ ┣ 📜MonsterSpawner.cs
 ┃ ┃ ┃ ┣ 📜MonsterUIRotator.cs
 ┃ ┃ ┃ ┗ 📜SectionDetector.cs
 ┗ ┗ ┗ 
 ```


 ## Trouble-shooting

 1. Trouble: 오브젝트 풀링 매니저에서 무기가 발사할 때마다 탄환이 부족하면 무한히 생성하는 현상
    
    Cause: 오브젝트 풀 매니저에서 설정해둔 초기 생성량보다 무기가 매초 마다 사용해야 하는 탄환량이 더 많았음.
    
    Solve: 무기 당 한번 쏠 때 얼마의 총알이 필요한지 미리 계산 후 탄환 종류별로 넉넉하게 잡아서 생성해둠.