# Project Overview: QuantRebalancer

## 1. Goal
미국주식, 국내주식, 코인, 금, 채권 등 5대 자산군을 관리하는 **Headless Trading Bot** 개발.
프론트엔드 없이 **.NET Worker Service**로 동작하며, 단일 서버에서 KIS(한투)와 Upbit(업비트) API를 동시에 제어하여 자산을 배분한다.

## 2. Tech Stack
* **Platform:** .NET 8.0 SDK (Worker Service Template)
* **Language:** C# 12
* **Database:** SQLite (Entity Framework Core 8.0)
* **Architecture:** Clean Architecture (Domain / Infrastructure / Application Worker)
* **Key Libraries:**
    * `Microsoft.EntityFrameworkCore.Sqlite`
    * `Quartz` (Job Scheduling)
    * `Serilog` (Logging)
    * `FluentValidation` (Logic Validation)

## 3. Key Constraints (Strict Rules)
1.  **Manual Asset Protection:** `AutoTradingTarget` 테이블에 없는 종목(개별주/잡코인)은 **절대 매도하지 않는다.**
2.  **ETF/Coin Only:** 봇은 오직 설정된 Target 종목만 매수/매도한다.
3.  **Money Wall Strategy (Human-in-the-loop):**
    * 봇은 KIS <-> Upbit 간 자금 이체를 직접 할 수 없다.
    * 자금 이동이 필요할 경우 `N8n Webhook`으로 **사용자에게 이체 요청 알림**을 보낸다.
4.  **Master Data:** 전 종목 데이터는 매일 아침 배치(Batch)로 동기화하여 로컬 DB에 저장한다.