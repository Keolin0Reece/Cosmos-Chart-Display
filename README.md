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
- Visual Studio (for development and debugging)

### Installation
1. Clone this repository:
   ```sh
   git clone https://github.com/your-username/CosmosChartDisplay.git
   ```
2. Open the solution in Visual Studio.
3. Restore dependencies if needed.
4. Run the application.

## Usage
1. Enter the required parameters:
   - **Device ID**: Identifier for fetching specific device data.
   - **Start Date & End Date**: Time range for data retrieval.
   - **Data Key**: Specifies the parameter to fetch from the API.
2. Click the fetch button to retrieve and display the data.
3. View the plotted chart and response metrics.

## API Information
- This application interacts with a **custom-built API**.
- The API is **not intended for external use** and serves only for documentation and demonstration purposes.
- Ensure proper API authentication via Bearer Token before querying data.

## Code Structure
- **Form1.cs**: Contains application logic, API interaction, and charting implementation.
- **ApiResponse.cs**: Defines the API response model.
