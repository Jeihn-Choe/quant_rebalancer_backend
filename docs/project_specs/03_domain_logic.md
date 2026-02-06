# Solution Structure

**Solution Name:** `QuantRebalancer.sln`

## 1. Project Hierarchy

```text

QuantRebalancer/
├── src/
│   ├── 1. 📦 QuantRebalancer.Domain/          # [Enterprise Logic] 순수 도메인 (No External Dependencies)
│   │   ├── Aggregates/                        # (e.g., Portfolio, Trade, AutoTradingTarget)
│   │   ├── Enums/                             # (e.g., AssetType, OrderStatus, TradeSide)
│   │   ├── Exceptions/                        # Domain-specific exceptions
│   │   └── Services/                          # Pure domain calculation logic (e.g., PortfolioCalculator)
│   │
│   ├── 2. 📦 QuantRebalancer.Application/     # [Application Logic] 유스케이스 & 포트 (New!)
│   │   ├── Ports/                             # Interfaces for external dependencies (Input/Output Ports)
│   │   │   ├── Input/                         # Input Ports (e.g., UseCase Interfaces)
│   │   │   └── Output/                        # Output Ports (e.g., IBrokerService, INotifier, IRepository)
│   │   ├── Features/                          # Application-specific use case implementations (Commands, Queries, Handlers)
│   │   │   ├── Orders/                        # (e.g., PlaceOrderCommand, PlaceOrderHandler)
│   │   │   └── Rebalancing/                   # (e.g., SmartFillingHandler, RebalanceHandler)
│   │   └── Behaviors/                         # Cross-cutting concerns for Features (e.g., Logging, Validation, Transactional)
│   │
│   ├── 3. 📦 QuantRebalancer.Infrastructure/  # [Adapter] 포트 구현체 (Adapters for Ports)
│   │   ├── Persistence/                       # (e.g., EF Core Repository implementations for IRepository)
│   │   ├── Brokers/                           # (e.g., KisBrokerService, UpbitBrokerService implementing IBrokerService)
│   │   └── Notifications/                     # (e.g., N8nNotifier implementing INotifier)
│   │
│   └── 4. 🚀 QuantRebalancer.Worker/          # [Entry Point] 호스트 / 실행기 (Host / Entry Point)
│       ├── Jobs/                              # Schedulers (e.g., Quartz, Hangfire) that trigger Application Features
│       ├── Program.cs                         # Application startup, DI container assembly
│       └── appsettings.json
│
└── tests/
    └── 🧪 QuantRebalancer.Tests/           # 주로 Domain과 Application을 테스트

## 2. Layer Dependencies (Hexagonal Architecture)

### 1. Domain :
    - 의존성 없음. 순수 POCO & Logic.
    - 가장 안쪽 계층.

### 2. Application :
    - Domain 참조.
    - Infrastructure의 Ports (Interface)에 의존.

### 3. Infrastructure :
    - Application의 Ports (Interface)를 참조하며 구현.
    - 가장 바깥쪽 계층.

### 4. Worker :
    - Application, Infrastructure 참조.
    - 애플리케이션의 호스트이자 엔트리 포인트. 