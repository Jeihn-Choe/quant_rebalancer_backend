# QuantRebalancer

**QuantRebalancer**는 미국주식, 국내주식, 암호화폐, 금, 채권 등 5대 자산군을 자동으로 관리하는 **Headless Multi-Asset Trading Bot**입니다.
.NET 8 Worker Service 기반으로 구축되었으며, Oracle Cloud 상에서 Docker 컨테이너로 동작하여 24시간 자산을 모니터링하고 리밸런싱을 수행합니다.

---

## 📑 목차 (Table of Contents)

1. [프로젝트 개요 (Overview)](#1-프로젝트-개요-overview)
2. [아키텍처 설계 (Architecture Design)](#2-아키텍처-설계-architecture-design)
3. [핵심 전략 및 안전장치 (Core Strategies)](#3-핵심-전략-및-안전장치-core-strategies)
4. [프로젝트 구조 (Project Structure)](#4-프로젝트-구조-project-structure)
5. [기술 스택 (Tech Stack)](#5-기술-스택-tech-stack)

---

## 1. 프로젝트 개요 (Overview)

이 프로젝트의 목표는 **"감정을 배제한 기계적 자산 배분"**입니다.
프론트엔드(UI) 없이 백그라운드 서비스로만 동작하며, 정해진 스케줄에 따라 자산 상태를 진단하고 비중을 조절합니다.

*   **다중 거래소 지원:** KIS(한국투자증권)와 Upbit(업비트)를 동시에 제어합니다.
*   **하이브리드 데이터:** API 인증 정보는 `appsettings.json`으로, 자산 비중과 타겟 종목은 `SQLite`로 관리합니다.
*   **유연한 확장성:** Clean Architecture 적용으로 거래소가 추가되거나 로직이 변경되어도 유연하게 대응 가능합니다.

---

## 2. 아키텍처 설계 (Architecture Design)

본 프로젝트는 **Clean Architecture** 원칙을 철저히 준수하여 의존성 방향이 항상 **외부에서 내부(Domain)**로 향하도록 설계되었습니다.

```mermaid
graph TD
    User((User/Scheduler)) --> Worker(QuantRebalancer.Worker)
    
    subgraph Infrastructure
        Worker --> Infra(QuantRebalancer.Infrastructure)
        Infra --> KIS[KIS Broker]
        Infra --> Upbit[Upbit Broker]
        Infra --> DB[(SQLite)]
    end
    
    subgraph Domain ["Core Domain (Pure C#)"]
        Infra -.->|Implements| Interfaces
        Worker --> Services
        Services --> Interfaces[Interfaces (Broker/Repo)]
        Services --> Entities[Entities]
    end
```

### 2.1. Domain Layer (`QuantRebalancer.Domain`)
*   **역할:** 프로젝트의 심장. 외부 기술(HTTP, DB 등)에 대해 전혀 모르는 순수 영역입니다.
*   **구성:**
    *   **Entities:** 데이터 모델 (`Asset`, `PortfolioConfig`).
    *   **Services:** 핵심 비즈니스 로직 (`PortfolioCalculator` - 리밸런싱 수량 계산).
    *   **Interfaces:** 외부와의 소통을 위한 설계도 (`IBrokerService`, `ITradeRepository`).

### 2.2. Infrastructure Layer (`QuantRebalancer.Infrastructure`)
*   **역할:** 도메인의 인터페이스를 실제로 구현하는 손발.
*   **특징:**
    *   **3-Level Communication:** `GenericHttpClient` (순수 통신) -> `ApiClient` (인증 관리) -> `Broker` (도메인 변환)의 3단 구조로 통신 책임을 분리했습니다.
    *   `IBrokerService`의 구현체(`KisBroker`, `UpbitBroker`)가 위치합니다.

### 2.3. Worker Layer (`QuantRebalancer.Worker`)
*   **역할:** 어플리케이션의 진입점.
*   **기능:** `Quartz.NET`을 이용해 스케줄링(08:00 데이터 동기화, 15:00 스마트 필링 등)을 수행하고 도메인 로직을 실행합니다.

---

## 3. 핵심 전략 및 안전장치 (Core Strategies)

### 3.1. Money Wall (자금 이동 차단)
보안과 사고 방지를 위해 봇은 **거래소 간 자금 이체(Transfer)를 직접 수행하지 않습니다.**
*   리밸런싱을 위해 타 거래소의 매수가 필요할 경우, `N8n Webhook`을 통해 사용자에게 **"이체 요청 알림"**만 발송합니다.
*   사용자가 수동으로 이체하면, 다음 실행 주기(Smart Filling)에 봇이 이를 감지하여 매수를 진행합니다.

### 3.2. Strict Asset Protection (엄격한 자산 보호)
*   **Allow-List 방식:** `AutoTradingTarget` 테이블에 등록된 종목 외에는 **절대로 매도하지 않습니다.**
*   사용자가 수동으로 산 개별 주식이나 잡코인을 봇이 오판하여 파는 것을 원천 봉쇄합니다.

### 3.3. Factory Pattern & Dependency Injection
*   **`IBrokerFactory`:** 자산 타입(`US`, `KR`, `COIN`)에 따라 적절한 브로커 구현체를 런타임에 주입합니다.
*   도메인 로직은 상대방이 KIS인지 Upbit인지 알 필요 없이, 추상화된 `PlaceOrderAsync` 명령만 내립니다.

---

## 4. 프로젝트 구조 (Project Structure)

```text
src/
├── 📦 QuantRebalancer.Domain/          # [Core] 순수 비즈니스 로직 & 인터페이스
│   ├── 📂 Entities/                    # 데이터 모델
│   ├── 📂 Services/                    # 핵심 연산 (PortfolioCalculator)
│   └── 📂 Interfaces/                  # 외부 통신 규약
│       ├── 📂 Brokers/                 # IBrokerService, IBrokerFactory
│       ├── 📂 MarketData/              # IMarketDataService
│       ├── 📂 Notifications/           # INotifier
│       └── 📂 Repositories/            # ITradeRepository
│
├── 📦 QuantRebalancer.Infrastructure/  # [Infra] 외부 연동 구현체
│   ├── 📂 Brokers/                     # KisBroker, UpbitBroker
│   ├── 📂 External/                    # KisApiClient, UpbitApiClient
│   ├── 📂 Common/Http/                 # GenericHttpClient (순수 통신)
│   └── 📂 Persistence/                 # EF Core Repository 구현
│
└── 🚀 QuantRebalancer.Worker/          # [App] 실행 호스트 & 스케줄러
    ├── 📂 Jobs/                        # DailyTickerSync, SmartFilling, Rebalance
    └── Program.cs                      # DI 설정 및 실행
```

---

## 5. 기술 스택 (Tech Stack)

*   **Framework:** .NET 8 (Worker Service)
*   **Language:** C# 12
*   **Database:** SQLite (Entity Framework Core 8)
*   **Scheduling:** Quartz.NET
*   **Logging:** Serilog
*   **Utils:** FluentValidation, MediatR (Optional)
*   **Deployment:** Docker on Oracle Cloud

