### 📄 6. `05_WORKFLOWS.md`

```markdown
# Workflows & Jobs

## 1. DailyTickerSyncJob (08:00 AM)
* **Goal:** `TickerMaster` 테이블 갱신.
* **Logic:**
    1. `ITickerLoader`를 통해 KIS(주식) 및 Upbit(코인) 전체 종목 데이터 Fetch.
    2. `TickerMaster` 테이블에 Bulk Upsert 수행.
    3. 없는 종목 추가, 있는 종목 업데이트.

## 2. Smart Filling Job (15:00 PM - 장 마감 30분 전)
* **Goal:** 예수금을 사용하여 비율이 부족한 자산 매수.
* **Logic:**
    1. `PortfolioCalculator`로 섹터별/종목별 `Gap` 계산.
    2. `Gap > 0` (부족)인 종목을 `Gap` 크기순 정렬.
    3. 계좌별(KIS/Upbit) `AvailableCash` 확인.
    4. 현금이 있는 곳에서만 순차 매수 실행. (Money Wall로 인해 이체는 안 함)

## 3. RebalanceJob (15:05 PM)
* **Goal:** 리밸런싱 및 자금 이동 요청.
* **Logic:**
    * **Step 1 (Check):** `Drift > Threshold(10%)` 체크.
    * **Step 2 (Sell):** 초과된 자산 매도하여 현금 확보.
    * **Step 3 (Cross-Exchange Action):**
        * 확보된 현금으로 매수해야 할 자산이 **다른 계좌**에 있는 경우:
        * **Upbit 매수 필요:** `TryDepositKrwAsync()` 시도 -> 성공 시 매수, 실패 시 "입금 요망" 알림.
        * **KIS 매수 필요:** `INotifier`로 "KIS로 이체 요망" 알림 발송.
    * **Step 4 (Buy):** 같은 계좌 내 매수라면 즉시 실행.