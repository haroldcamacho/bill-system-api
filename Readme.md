# Basic Billing System API

The Basic Billing System API is a backend application built using .NET Core that provides endpoints to manage billing and payment data.

## Table of Contents

- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
- [Endpoints](#endpoints)

## Getting Started

### Prerequisites

To run the Basic Billing System API, you need to have the following software installed on your machine:

- .NET 6 SDK

### Installation

1. Clone this repository:

   ```bash
   git clone https://github.com/your-username/basic-billing-system-api.git
2.  Navigate to the project directory:
    ```bash
    cd basic-billing-system-api
3. Build and run the application:
    ```bash
    dotnet restore
    dotnet run
4. Verify the API is running:

    The API should now be running locally on http://localhost:5000.


# Usage

The Basic Billing System API provides endpoints to manage billing and payment data. You can interact with the API using tools like curl, Postman, or your preferred API client.

# Endpoints

### GET /billing/pending

Get a list of pending bills for a specific client.

Query Parameters:
- **clientId (integer, required): The ID of the client.**

Example: http://localhost:5000/billing/pending?clientId=100

### POST /billing/pay

Pay a bill for a client.
- Request Body:

```json 
{
  "clientId": 100,
  "category": "WATER",
  "period": 202308
}
```
Example: **http://localhost:5000/billing/pay**

### POST /clients
Create a new client.

- Request Body:

```json 
{
  "clientId": 100,
  "name": "John Doe"
}
``` 
Example: http://localhost:5000/clients

### POST /billing/bills
Create a new bill.
- Request Body:
```json 
{
    "clientId": 100,
    "period": 202308,
    "category": "AXS",
    "amount": 200
}
```
### GET /billing/search?category=ELECTRICITY
Search bills by category. Replace ELECTRICITY by any category you want to search.

Query Parameters:
- **category (string, required): The Category of the bills.**

### GET /billing/client/200

Search bills by Client ID regardless if they are paid or pending

Query Parameters:
- **clientId (integer, required): The ID of the client.**