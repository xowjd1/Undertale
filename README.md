<img src="https://github.com/user-attachments/assets/8911a2c7-34c2-4f98-b776-197a9b3a9bab" alt="Undertale Battle 01" width="400"/> <img src="https://github.com/user-attachments/assets/f999a504-4283-4305-8d93-fa01a7fca1fe" alt="Undertale Battle 02" width="400"/>
<img src="https://github.com/user-attachments/assets/94d89af8-b12c-4ff0-bca6-0be65f1c8566" alt="Undertale Battle 03" width="400"/> <img src="https://github.com/user-attachments/assets/e38a8f4f-ffb3-4f7b-8d99-607406a4460d" alt="Undertale Battle 04" width="400"/>
<img src="https://github.com/user-attachments/assets/ae6a54f1-2b3f-49b9-8a48-2e9ae65695c4" alt="Undertale Battle 05" width="400"/> <img src="https://github.com/user-attachments/assets/64898a57-c7a6-4db4-8d08-74d9972e58d9" alt="Undertale Battle 06" width="400"/>

---

# 💀 Undertale Battle

> 인디 게임 **UNDERTALE**의 메타톤, 언다인, 샌즈 전투 모작

---

## 🎮 프로젝트 개요

- **프로젝트 명** : 언더테일 모작
- **개발 기간** : 2025년 4월 14일 ~ 2025년 4월 30일
- **개발 인원** : 유니티 개발 3명
- **개발 도구** : Unity 6, Aseprite, Github Desktop, Rider, Notion

원작 UNDERTALE의 대표적인 전투 구조인

- **턴제 커맨드 선택(ATTACK)**과
- 상대 턴에 **하트를 조작해 탄막을 피하는 실시간 회피 파트**

를 Unity에서 직접 구현해 본 전투 시스템입니다.

---

## 🎯 주요 목표

- 상태 전환이 복잡한 턴제 전투를 **추상 FSM(Abstract Finite State Machine)**으로 정리해서 구현
- “플레이어 턴 ↔ 보스 턴” 사이의 흐름을 **명확한 상태 다이어그램**으로 관리
- 패턴별 탄막, 히트 판정 등 **전투 핵심 로직**을 구조화

---

## 🕹 게임 플레이 흐름

1. **플레이어 턴**
   - 커맨드 UI에서 `ATTACK` 선택
   - 공격 선택 시 : 타이밍 바 공격 UI를 통해 데미지 결정

2. **전환 단계 (Transition)**
   - 화면 연출과 함께 보스 턴으로 전환
   - 내부 FSM에서 `PlayerTurn → EnemyTurn` 상태 전환

3. **보스 턴 – 탄막 회피**
   - 플레이어 하트를 방향키로 조작
   - 일정 시간 동안 떨어지는 탄/레이저/장애물을 회피

4. **턴 반복**
   - 보스 체력 또는 패턴 종료에 따라 전투 종료

---

## 👨‍💻 담당 업무

### 🧠 전투 FSM 설계 & 구현

- 전투 전체를 추상 상태로 나누어 관리
  - 각 상태는 공통으로 `Enter() / Update() / Exit()` 인터페이스를 구현하도록 설계
- **상태 전환 규칙**
  - 플레이어 턴에서 커맨드 처리 완료 시 → `EnemyTurnState`로 전환
  - 탄막 패턴이 끝나면 → 다시 `PlayerTurnState`로 전환
  - 체력/조건 만족 시 → `EndState`(Victory/Defeat)로 전환

### 🎯 커맨드 UI & 전투 로직

- UNDERTALE 스타일의 전투 커맨드 UI를 Unity UI로 구현
  - 화살표 키/선택 버튼으로 커맨드 이동
  - 선택 중인 항목에 하이라이트 적용
- 공격 처리 로직
  - 타이밍 바를 기반으로 데미지 계산
  - 보스 체력 UI와 연동하여 피격 연출 처리

### 🔺 탄막 패턴 시스템

- 보스 턴에서 사용할 **탄막 패턴을 별도 클래스로 분리**
- 각 패턴은
  - 탄 프리팹 풀링
  - 탄 생성 위치/각도/속도
  - 지속 시간/간격
  를 파라미터로 받아 동작하도록 설계

### ❤️ 플레이어 컨트롤 & 판정

- 플레이어 하트 이동
  - 제한된 박스 영역 내에서만 이동 가능하도록 AABB 영역 제한
- 피격 판정
  - 탄막과 플레이어 하트 간 충돌 체크
  - 피격 시 체력 감소
 
### 🛠 에디터 확장 (Custom Editor)

- 전용 커스텀 에디터를 구현하고, EnemyType enum 값을 기준으로 메타톤 / 언다인 각 타입에 필요한 필드만 그룹별로 그려주도록 구성

