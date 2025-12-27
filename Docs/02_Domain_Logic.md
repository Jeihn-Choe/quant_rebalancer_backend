# Solution Structure

**Solution Name:** `QuantRebalancer.sln`

## 1. Project Hierarchy

```text

QuantRebalancer/
├── src/
│   ├── 📦 QuantRebalancer.Domain/          # [Core] Entities, Enums, Logic (No Dependencies)
│   │   ├── Entities/ (Asset, Portfolio, TickerMaster, TradeLog...)
│   │   ├── Enums/ (AssetType, OrderSide...)
│   │   ├── Services/ (PortfolioCalculator)
│   │   └── Interfaces/ (IBrokerService, INotifier, ITickerLoader)
│   │
│   ├── 📦 QuantRebalancer.Infrastructure/  # [Infra] Implementation
│   │   ├── Persistence/ (EF Core DbContext, Migrations)
│   │   ├── Brokers/ (KisBroker, UpbitBroker)
│   │   └── Notifications/ (N8nNotifier)
│   │
│   └── 🚀 QuantRebalancer.Worker/          # [App] Background Service
│       ├── Jobs/ (DailyTickerSyncJob, SmartFillingJob, RebalanceJob)
│       ├── appsettings.json
│       └── Program.cs
│
└── tests/
    └── 🧪 QuantRebalancer.Tests/           # [Test] Domain Logic TDD

## 2. Layer Dependencies

### Domain : 의존성 없음. 순수 POCO & Logic.
### Infrastructure : Domain 참조. 외부 API 구현
### Worker : Domain, Infrastructure 참조. 