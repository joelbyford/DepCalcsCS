# DepCalcsCS
Depreciation calculations in C# and implemented as a simple REST API call for ease of understanding and usage.  

## Build
Using dotnet 5 and the dotnet 5+ SDK simply build and deploy the resulting service to an Azure Website (or IIS server).  

## Usage
To call the sample restful service, simply call the API using the following pattern:

- **Simple API Calls** - Sample API's using just 3 required parameters: Purchase Price, Residual Value and Asset Useful Life.  If the usefull life doesn't match a valid Tax Class, the closest rounded down asset class will be selected (e.g. 8 year GAAP life -> 7 year MACRS asset class) 
  - **Straight Line Calculation** - https://localhost:5001/Calculate/SL?fPurchasePrice=2000&fResidual=0&iLife=5
  - **DDB** - https://localhost:5001/Calculate/DB200?fPurchasePrice=2000&fResidual=0&iLife=5
  - **150% DB** - https://localhost:5001/Calculate/DB150?fPurchasePrice=2000&fResidual=0&iLife=5
  - **SYD** - https://localhost:5001/Calculate/SYD?fPurchasePrice=2000&fResidual=0&iLife=5
  
- **Comprehensive API Calls** - Complete API calls with all variables provided
  - WIP

- **MACRS API Calls** - Tax calculations using MACRS tables for claculations.
  - WIP
  
## Reusing Code
Anyone is welcome to reuse the code here for any reason, in part or in whole.  All this is provided as-is and without warranty.  
