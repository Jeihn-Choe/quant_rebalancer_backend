# Project Spec: QuantRebalancer.Backend

## 1. 개요
* **목적:** 주식/코인 자산 관리 및 하이브리드 자동 리밸런싱 시스템
* **핵심 역할:**
    1. **API Server:** 프론트엔드 요청 처리 (설정, 조회, 수동 매수)
    2. **Background Bot:** 정해진 시간에 자산 비율 점검 및 자동 매매 (Smart Filling)
* **Tech Stack:**
    * Framework: ASP.NET Core 8.0 Web API
    * Language: C#
    * Architecture: Clean Architecture (Domain / Infrastructure / Web / Tests)
    * Notification: n8n Webhook 위임

## 2. 시스템 아키텍처
### 2.1 계층 구조
* **Domain:** 비즈니스 로직, 엔티티 (Asset, Portfolio), 인터페이스 (IBroker, INotifier)
    * *의존성 없음, 순수 C# 로직*
* **Infrastructure:** 외부 시스템 통신 (KIS API, Upbit API, N8n Webhook)
* **Web:** 컨트롤러(Controller), 백그라운드 서비스(HostedService), DI 설정
* **Tests:** xUnit을 이용한 도메인 로직(계산기) 단위 테스트

### 2.2 핵심 로직: 하이브리드 리밸런싱
1. **자산 구분:**
    * **Manual (수동):** 사용자가 직접 매수한 종목. **(Read-Only, 매도 절대 불가)**
    * **Auto (자동):** 봇이 관리하는 ETF/코인. (Buffer 역할)
2. **목표 비율 산출 (Dynamic Target):**
    * `Auto Target` = `Total Target` - `Current Manual Value`
3. **Smart Filling (물타기):**
    * 예수금 충전 시, 목표 비중보다 **부족한 자산(Gap > 0)**만 골라서 매수.
    * 매도 없이 매수만으로 비율을 맞춤.
4. **Safety Net (가치기):**
    * 스케줄러 실행 시 괴리율이 `Threshold` (예: 10%)를 초과하면 **ETF 매도**를 동반한 강제 리밸런싱 수행.

## 3. 기능 명세 (Features)

### A. API Endpoints (For Frontend)
| Method | Endpoint | 설명 |
| :--- | :--- | :--- |
| `GET` | `/api/config` | 현재 설정된 목표 비율 조회 (US/KR/Coin) |
| `POST` | `/api/config` | 목표 비율 수정 |
| `GET` | `/api/assets/summary` | 현재 포트폴리오 요약 (차트용 데이터) |
| `GET` | `/api/stocks/search` | 종목 검색 (KIS 마스터 파일 기반) |
| `POST` | `/api/orders/buy` | 개별주 수동 매수 주문 (시장가) |

### B. Background Worker (Bot)
* **스케줄링:**
    * 매일 국내장 마감 30분 전 (15:00)
    * 매일 미국장 마감 30분 전 (05:30 or 04:30)
* **알림 처리 (Event-Driven):**
    * 매매 발생, 에러, 리밸런싱 필요 시 `N8nWebhookNotifier`를 통해 JSON 페이로드 전송.

## 4. 데이터 흐름 (Data Flow)
1. **Polling:** KIS/Upbit API 호출 → 잔고 조회.
2. **Mapping:** `Manual` vs `Auto` 자산 분류.
3. **Calculation:** `PortfolioCalculator`가 현재 상태 vs 목표 상태 비교.
4. **Action:**
    * `Manual Buy`: 즉시 KIS API 주문 전송.
    * `Auto Rebalance`: 매수/매도 주문 생성 후 순차 실행.

## 5. 개발 환경 및 라이브러리
* **NuGet Packages:**
    * `System.Text.Json`: JSON 처리
    * `Microsoft.Extensions.Hosting`: 백그라운드 서비스
    * `Serilog`: 로깅
    * `xUnit`, `Moq`, `FluentAssertions`: 테스트