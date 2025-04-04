# TransactionManager
A microservices-based system for processing and validating financial transactions.

## Flow Diagram
```mermaid
graph TD;

%% Servicios
TRANSACTION[Transaction Service] --> TRANSACTION_DB[(Transaction Database)];
ANTI_FRAUD[Anti-Fraud Service] --> ANTI_FRAUD_DB[(Anti-Fraud Database)];

%% Subgráfico de Kafka
subgraph Kafka Topics
    CREATED_EVENT["transactions.created"]
    VALIDATED_EVENT["transactions.validated"]
end

%% Publicación de eventos en Kafka
TRANSACTION --> CREATED_EVENT;
TRANSACTION --> VALIDATED_EVENT;

%% Consumo de eventos desde Kafka
ANTI_FRAUD --> CREATED_EVENT;
ANTI_FRAUD --> VALIDATED_EVENT;

