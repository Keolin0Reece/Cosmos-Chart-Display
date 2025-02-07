# CosmosChartDisplay

## Overview
CosmosChartDisplay is a Windows Forms application built in C# that retrieves and visualizes time-series data from a custom API. The application fetches data using HTTP requests, processes the received JSON response, and plots the values on a chart for analysis.

## Features
- Fetches time-series data from an API.
- Displays response time for API queries.
- Parses JSON data and extracts key-value pairs.
- Plots data dynamically using Windows Forms Charting.
- Displays time intervals between data points.

## Getting Started
### Prerequisites
- .NET Framework (Version compatible with Windows Forms)
- Visual Studio (for development and debugging).

## UI
![image](https://github.com/user-attachments/assets/7f27000e-3346-4baa-88e6-ebecd0a04084)

## Usage
1. Enter the required parameters:
   - **Device ID**: Identifier for fetching specific device data.
   - **Start Date & End Date**: Time range for data retrieval.
   - **Data Key**: Specifies the parameter to fetch from the API.
2. Click the fetch button to retrieve and display the data.
3. View the plotted chart and response metrics.

## API Information
- The API takes device data and sends it to a **Cosmos DB**, where the data is stored.
- The stored data can then be retrieved through various **GET requests** based on different parameters.
- Ensure proper API authentication via Bearer Token before querying data.

## Code Structure
- **Form1.cs**: Contains application logic, API interaction, and charting implementation.
- **ApiResponse.cs**: Defines the API response model.
