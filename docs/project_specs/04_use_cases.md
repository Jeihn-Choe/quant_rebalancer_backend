# 고객 요구사항: 유스 케이스 및 인수 조건

이 문서는 QuantRebalancer 백엔드 봇이 제공하는 주요 기능들에 대한 고객(투자자)의 관점에서의 요구사항과, 해당 기능이 성공적으로 구현되었음을 판단하는 기준(인수 조건)을 정의합니다.

## 1. Daily Ticker Sync (매일 종목 동기화)

*   **관련 워크플로우:** `04_WORKFLOWS.md` - `DailyTickerSyncJob`
*   **고객 목표 (Customer Goal):** 매일 최신 종목 정보(주식, 코인 등)를 바탕으로 포트폴리오를 관리하고 싶다. 오래된 정보로 인한 거래 오류를 방지하고 싶다.
*   **인수 조건 (Acceptance Criteria):**
    *   **Given:** KIS와 Upbit API가 정상 작동하고, 새로운 종목 정보가 있거나 기존 종목 정보에 변경사항이 있을 때
    *   **When:** Daily Ticker Sync 작업이 매일 오전 8시에 실행되면
    *   **Then:** `TickerMaster` 테이블에는 최신 종목 정보가 정확히 반영(추가/업데이트)되어야 한다.
    *   **Given:** KIS 또는 Upbit API 응답에 일시적인 오류가 발생할 때
    *   **When:** Daily Ticker Sync 작업이 실행되면
    *   **Then:** 오류 알림이 전송되고, 기존 `TickerMaster` 데이터는 변경되지 않아야 한다.

## 2. Smart Filling (예수금 자동 매수)

*   **관련 워크플로우:** `04_WORKFLOWS.md` - `Smart Filling Job`
*   **고객 목표 (Customer Goal):** 새로운 자금(예수금)이 입금되면, 내가 정한 목표 비중에 따라 부족한 자산을 자동으로 매수하여 포트폴리오의 균형을 맞추고 싶다. 수동으로 계산하고 주문하는 번거로움을 줄이고 싶다.
*   **인수 조건 (Acceptance Criteria):**
    *   **Given:** KIS 계좌에 100만원의 현금이 있고, 주식 ETF 비중이 목표(예: 70%)보다 낮은 (예: 65%) 상태일 때
    *   **When:** Smart Filling 작업이 실행되면 (매일 15:00)
    *   **Then:** `PortfolioCalculator`가 주식 ETF를 매수 대상으로 추천하고, KIS를 통해 주식 ETF 매수 주문이 실행되어야 한다. (부족한 비중을 채우는 방향으로)
    *   **Given:** KIS 계좌에 현금이 없고, Upbit 계좌에 50만원의 현금이 있으며, 코인 비중이 목표(예: 30%)보다 낮은 (예: 25%) 상태일 때
    *   **When:** Smart Filling 작업이 실행되면
    *   **Then:** `PortfolioCalculator`가 코인을 매수 대상으로 추천하고, Upbit을 통해 코인 매수 주문이 실행되어야 한다.
    *   **Given:** 모든 자산이 목표 비중을 만족하거나 초과하는 상태이고, 현금이 있을 때
    *   **When:** Smart Filling 작업이 실행되면
    *   **Then:** 어떤 매수 주문도 발생하지 않아야 한다.
    *   **Given:** 매수 주문 실행 중 KIS 또는 Upbit API에 일시적인 문제가 발생할 때
    *   **When:** Smart Filling 작업이 실행되면
    *   **Then:** 오류 알림이 전송되고, 이미 실행된 주문은 유지하며, 실패한 주문은 재시도하지 않아야 한다 (또는 특정 정책에 따라 처리).

## 3. Rebalancing (자산 비중 재조정)

*   **관련 워크플로우:** `04_WORKFLOWS.md` - `RebalanceJob`
*   **고객 목표 (Customer Goal):** 포트폴리오의 자산 비중이 크게 벗어났을 때, 자동으로 균형을 맞추어 위험을 관리하고 싶다. 필요 시 자금 이동 요청을 받아 수동으로 조치할 수 있어야 한다.
*   **인수 조건 (Acceptance Criteria):**
    *   **Given:** 코인 비중이 목표보다 15% 초과하고, 주식 비중이 15% 부족한 상태(`Drift` > `Threshold`)일 때
    *   **When:** Rebalancing 작업이 실행되면 (매일 15:05)
    *   **Then:** 초과된 코인을 매도하여 현금을 확보하고, 해당 현금으로 주식을 매수해야 한다.
    *   **Given:** KIS 계좌에 있는 매도 금액으로 Upbit에서 코인을 매수해야 하는 상황이 발생했을 때
    *   **When:** Rebalancing 작업이 실행되면
    *   **Then:** `INotifier`를 통해 "Upbit으로 N원 이체 요망" 알림이 전송되어야 한다.
    *   **Given:** 모든 자산의 비중 `Drift`가 설정된 `Threshold` 이내일 때
    *   **When:** Rebalancing 작업이 실행되면
    *   **Then:** 어떤 매도/매수 주문도 발생하지 않아야 한다.
