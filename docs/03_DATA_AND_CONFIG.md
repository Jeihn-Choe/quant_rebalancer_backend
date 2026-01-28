# Data & Configuration

## 1. appsettings.json
```json
{
  "Portfolio": {
    "Ratios": { "US": 0.35, "KR": 0.20, "COIN": 0.20, "GOLD": 0.25, "BOND": 0.00 },
    "Threshold": 0.10
  },
  "Brokers": {
    "Kis": { "AppKey": "...", "Secret": "...", "Account": "..." },
    "Upbit": { "AccessKey": "...", "Secret": "..." }
  },
  "N8n": { "WebhookUrl": "..." }
}

## 2. Database Schema (SQLite) - The 4 Tables

### Table: PortfolioConfig (설정)
전체 포트폴리오의 목표 비중 설정.
* `Id` (PK, Integer)
* `UsRatio` (Decimal)
* `KrRatio` (Decimal)
* `CoinRatio` (Decimal)
* `GoldRatio` (Decimal)
* `BondRatio` (Decimal)
* `Threshold` (Decimal): 리밸런싱 발동 임계값 (e.g., 0.1)

### Table: TickerMaster (전체 종목 원장)
매일 아침 API로 동기화되는 마스터 데이터.
* `Code` (PK, String): 종목코드 (e.g., "KRW-BTC", "005930")
* `Type` (Enum): 자산군 (US, KR, COIN, GOLD, BOND)
* `Name` (String): 종목명
* `Exchange` (String): 거래소 정보 (e.g., "UPBIT", "KOSPI", "NAS")
* `LastUpdated` (DateTime): 마지막 동기화 시간

### Table: AutoTradingTarget (봇 전략)
봇이 실제로 관리하는 종목 및 내부 가중치.
* `Id` (PK, Integer)
* `TickerCode` (FK -> TickerMaster.Code): 외래키
* `Weight` (Decimal): **섹터 내 비중 (0.0 ~ 1.0). 동일 Type 내 합은 1.0**
* `IsActive` (Bool): 활성화 여부

### Table: TradeLog (매매 일지)
봇의 매매 기록.
* `Id` (PK, Long)
* `Timestamp` (DateTime)
* `Action` (String): "BUY" or "SELL"
* `Code` (String)
* `Price` (Decimal): 체결 단가
* `Qty` (Decimal): 수량
* `Reason` (String): 매매 사유 (e.g., "SmartFilling", "Rebalance")