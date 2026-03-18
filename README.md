# 🛰️ Satellite Telemetry & Alert System

A robust, asynchronous .NET Web API designed to ingest, store, and monitor real-time satellite telemetry data. The system automatically detects critical hardware issues (Battery & Temperature) and provides a filtered interface for alert management.

## 🚀 Features

- **Telemetry Ingestion**: High-throughput endpoint for receiving satellite status updates.
- **Real-time Alerting**: Automatic detection of:
  - **Battery Drop**: Alerts when battery levels fall below 20%.
  - **Temperature Spikes**: Alerts when temperatures exceed 85°C.
- **Advanced Filtering**: Query alerts by `SatelliteId`, `Date Range`, and `Pagination`.
- **Thread-Safe Storage**: In-memory persistence using `ConcurrentDictionary` and `ConcurrentQueue`.
- **Structured Logging**: Detailed execution logs with performance tracking (Stopwatch).

## 🛠️ Tech Stack

- **Framework**: .NET 8 / ASP.NET Core
- **Language**: C# 12
- **Persistence**: Thread-Safe In-Memory Storage
- **Validation**: Data Annotations & Custom Validation Attributes
- **Tooling**: JetBrains Rider, EditorConfig

## 📋 Architecture Decisions

### 1. Thread Safety
Since the API handles concurrent requests, the repository uses `ConcurrentQueue<T>` and `ConcurrentDictionary<K, V>`. This ensures that multiple satellites can send data simultaneously without causing `CollectionModified` exceptions.

### 2. Validation & Quality
- **NotEmptyGuid**: A custom validation attribute was implemented to prevent `Guid.Empty` (0000...) from entering the system, which is a common pitfall in C# GUID handling.
- **EditorConfig**: A strict `.editorconfig` is included to maintain consistent code style and allow readable single-line blocks for simple logic.

### 3. Observability
The project uses **Structured Logging**. Instead of simple strings, logs include parameterized data, allowing for efficient querying in production monitoring tools (like Seq, ELK, or Application Insights).

## 🧪 Testing Strategy

The project includes a comprehensive Unit Test suite to ensure the reliability of the telemetry logic and alert thresholds.

- **Framework**: xUnit
- **Mocking**: Moq
- **Key Test Scenarios**:
    - **Threshold Validation**: Verifies that alerts are correctly triggered at <20% battery and >85°C temperature.
    - **Thread Safety**: High-concurrency tests to ensure `InMemoryRepository` handles multiple simultaneous writes without data loss.
    - **Validation Logic**: Testing the `NotEmptyGuid` attribute against empty and non-empty GUIDs.
    - **Pagination**: Ensuring the skip/take logic in `GetActiveFilteredAlertsAsync` returns the correct data subsets.

## 🚦 Getting Started

### Installation & Run

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/TalyTochner/SatelliteTelemetryAPI.git](https://github.com/TalyTochner/SatelliteTelemetryAPI.git)

2. **Navigate to the project folder:**
  ```bash
    cd SatelliteTelemetryAPI
```

3. **Restore dependencies:**
  ```bash
  dotnet restore
  ```

4. **Run the application:**

 ```bash
dotnet run --project SatelliteTelemetryAPI.API
```

5. **Open Swagger UI:**
    Once the application is running, navigate to:
    http://localhost:[PORT]/swagger
