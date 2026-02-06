QuantRebalancer.Domain/
├── Aggregates/                           # 애그리거트 루트들을 위한 최상위 폴더
│   ├── Portfolio/                        # **Portfolio 애그리거트**
│   │   ├── Portfolio.cs                  # 애그리거트 루트 (현재 자산 보유 현황 및 그 안의 Asset 엔티티들을 관리)
│   │   ├── Asset.cs                      # Portfolio 애그리거트에 속한 엔티티 (개별 자산)
│   │   └── Events/                       # Portfolio 관련 도메인 이벤트
│   │       ├── PortfolioRebalancedEvent.cs # 포트폴리오 리밸런싱 완료 이벤트
│   │       └── AssetUpdatedEvent.cs      # 자산 정보 업데이트 이벤트
│   ├── AutoTradingTarget/                # **AutoTradingTarget 애그리거트**
│   │   ├── AutoTradingTarget.cs          # 애그리거트 루트 (자동매매 대상 종목 목록 및 관리)
│   │   └── Events/
│   │       └── AutoTradingTargetChangedEvent.cs # 자동매매 대상 변경 이벤트
│   ├── Trade/                            # **Trade 애그리거트**
│   │   ├── Trade.cs                      # 애그리거트 루트 (체결된 거래 내역, 주문 등을 관리)
│   │   ├── Order.cs                      # Trade 애그리거트에 속한 엔티티 (실제 제출된 주문)
│   │   ├── TradeLog.cs                   # Trade 애그리거트에 속한 엔티티 (상세 거래 로그)
│   │   └── Events/
│   │       ├── OrderPlacedEvent.cs       # 주문 생성 이벤트
│   │       └── TradeExecutedEvent.cs     # 거래 체결 이벤트
│   └── Configuration/                    # **Configuration 애그리거트**
│       ├── PortfolioConfiguration.cs     # 애그리거트 루트 (목표 비율, 리밸런싱 임계값 등 시스템 설정 관리)
│       └── Events/
│           └── ConfigurationUpdatedEvent.cs # 설정 변경 이벤트
├── Services/                             # 도메인 서비스 (단일 애그리거트로는 처리하기 어려운 복잡한 비즈니스 로직, 여러 애그리거트 조정 등)
│   ├── PortfolioCalculator.cs            # 핵심 계산 로직 (Gap 계산, 매수/매도 수량 결정 등)
│   └── RebalancingOrchestrator.cs        # 리밸런싱 프로세스 전체를 조율 (Calculator, Broker Service Interface 활용)
├── Interfaces/                           # 외부 인프라스트럭처에 대한 인터페이스 (Dependency Inversion Principle)
│   ├── IBrokerService.cs                 # 브로커 API (자산 조회, 주문)
│   ├── INotifier.cs                      # 알림 (N8n Webhook 등)
│   ├── ITickerLoader.cs                  # 종목 마스터 데이터 로드
│   └── IUnitOfWork.cs                    # (선택 사항: 여러 애그리거트의 영속성 관리를 위한 UoW 패턴)
├── ValueObjects/                         # 여러 애그리거트에서 공유될 수 있는 값 객체
│   ├── Money.cs                          # 금액 (통화 단위 포함)
│   ├── Percentage.cs                     # 백분율
│   ├── Price.cs                          # 가격
│   └── Quantity.cs                       # 수량
├── Enums/                                # 도메인 전반에 사용되는 열거형
│   ├── AssetType.cs
│   ├── TradeSide.cs
│   └── OrderStatus.cs
└── Exceptions/                           # 도메인 전용 예외 클래스
    ├── InsufficientFundsException.cs     # 자금 부족 예외
    └── InvalidTradeRequestException.cs   # 유효하지 않은 거래 요청 예외