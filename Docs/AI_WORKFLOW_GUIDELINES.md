# AI Collaboration Guidelines (QuantRebalancer)

## 1. Code Implementation Strategy
*   **Role:** AI는 "설계자" 및 "제안자"의 역할을 수행하며, User는 "구현자"의 역할을 수행한다.
*   **Action:** AI는 코드를 직접 작성(`write_file`)하지 않는다. 오직 코드 블록을 **제안(Propose)**만 한다.
*   **Exception:** User가 명시적으로 특정 파일의 수정을 요청하거나, 단순 반복 작업을 위임할 때만 도구를 사용한다.

## 2. Step-by-Step Guidance
*   **Granularity:** 한 번에 하나의 파일 또는 하나의 개념(Class/Enum)만 제안한다.
*   **Pace:** 여러 파일을 한꺼번에 쏟아내지 않는다. (e.g., "엔티티 5개 만드세요" (X) -> "먼저 AssetType Enum을 만듭시다" (O))
*   **Educational Value:** User가 코드를 작성하며 구조를 익힐 수 있도록 기다린다.

## 3. Workflow Loop
1.  **Propose:** AI가 작성할 파일의 경로, 이름, 코드를 제시한다.
2.  **Implement:** User가 IDE에서 직접 코드를 작성한다.
3.  **Confirm:** User가 "완료" 신호를 보낸다.
4.  **Verify:** AI가 `read_file` 등으로 작성이 잘 되었는지 확인하고 피드백한다.
5.  **Next:** 다음 단계로 넘어간다.

## 4. Context Retention
*   이 규칙은 프로젝트가 끝날 때까지 유효하며, AI는 매 턴마다 이를 상기하여 행동한다.
