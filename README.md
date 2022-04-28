# DepCalcsCS
Depreciation calculations in C# and implemented as a simple REST API calls for ease of understanding and usage.  

## Build
Using dotnet 5 and the dotnet 5+ SDK simply build and deploy the resulting service to an Azure Website (or IIS server). 

-----------

## API Usage Examples
To call the restful service, simply call the API using the following patterns:

### GET - Straight Line Depreciation
A simple call that only returns the SL depreciation tables for a single asset.  Example Usage:

`GET /Calculate/SL?PurchasePrice=1000&Residual=0&Life=5&AssetName=SlAssest`

Parameters:
- **PurchasePrice** - Original price of the asset.
- **Residual** - Residual life of the asset at the end of the useful life.
- **Life** - The useful life of the asset.
- **AssetName** - (Optional) A name for the asset.

Returns:
- **200** - Success and returns an Asset object with calculated depreciation expense and accumulated depreciation by year.
- **500** - Any error.

### GET - Double Declining Balance Depreciation
A simple call that only returns the 200% Declining Balance depreciation tables for a single asset.  Example Usage:

`GET /Calculate/DB200?PurchasePrice=1000&Residual=0&Life=5&AssetName=SlAssest`

Parameters:
- **PurchasePrice** - Original price of the asset.
- **Residual** - Residual life of the asset at the end of the useful life.
- **Life** - The useful life of the asset.
- **AssetName** - (Optional) A name for the asset.

Returns:
- **200** - Success and returns an Asset object with calculated depreciation expense and accumulated depreciation by year.
- **500** - Any error.

### GET - 150% Declining Balance Depreciation
A simple call that only returns the 150% Declining Balance depreciation tables for a single asset.  Example Usage:

`GET /Calculate/DB150?PurchasePrice=1000&Residual=0&Life=5&AssetName=SlAssest`

Parameters:
- **PurchasePrice** - Original price of the asset.
- **Residual** - Residual life of the asset at the end of the useful life.
- **Life** - The useful life of the asset.
- **AssetName** - (Optional) A name for the asset.

Returns:
- **200** - Success and returns an Asset object with calculated depreciation expense and accumulated depreciation by year.
- **500** - Any error.

### GET - Sum of the Years Digits Depreciation
A simple call that only returns the Sum of the Years Digits (SYD) depreciation tables for a single asset.  Example Usage:

`GET /Calculate/SYD?PurchasePrice=1000&Residual=0&Life=5&AssetName=SlAssest`

Parameters:
- **PurchasePrice** - Original price of the asset.
- **Residual** - Residual life of the asset at the end of the useful life.
- **Life** - The useful life of the asset.
- **AssetName** - (Optional) A name for the asset.

Returns:
- **200** - Success and returns an Asset object with calculated depreciation expense and accumulated depreciation by year.
- **500** - Any error.

### GET - IRS MACRS Half-Year Depreciation
A simple call that only returns the MACRS Half-Year depreciation tables for a single asset.  Example Usage:

`GET /Calculate/MACRSHY?PurchasePrice=1000&Life=5&Section179=200&AssetName=MacrsHyAsset&PurchaseDate=10%2F01%2F2020`

Parameters:
- **PurchasePrice** - Original price of the asset.
- **Life** - The useful life of the asset (**MUST** be a valid tax life of 3, 5, 7, 10, 15 or 20)
- **PurchaseDate** - A URL encoded date representing the date an asset was purchased.
- **Section179** - The dollar amount of Section 179 depreciation taken in the first year.
- **AssetName** - (Optional) A name for the asset.

Returns:
- **200** - Success and returns an Asset object with calculated depreciation expense and accumulated depreciation by year.
- **422** - Error for an invalid tax life provided (see Life above for valid values).
- **500** - Any other error.

### GET - IRS MACRS Mid-Quarter Depreciation
A simple call that only returns the MACRS Mid-Quarter depreciation tables for a single asset.  Example Usage:

`GET /Calculate/MACRSMQ?PurchasePrice=1000&Life=5&Section179=200&AssetName=MacrsHyAsset&PurchaseDate=10%2F01%2F2020`

Parameters:
- **PurchasePrice** - Original price of the asset.
- **Life** - The useful life of the asset (**MUST** be a valid tax life of 3, 5, 7, 10, 15 or 20)
- **PurchaseDate** - A URL encoded date representing the date an asset was purchased.
- **Section179** - The dollar amount of Section 179 depreciation taken in the first year.
- **AssetName** - (Optional) A name for the asset.

Returns:
- **200** - Success and returns an Asset object with calculated depreciation expense and accumulated depreciation by year.
- **422** - Error for an invalid tax life provided (see Life above for valid values).
- **500** - Any other error.

### POST - Depreciate (Calculate both tax and GAAP depreciation tables)
A comprehensive call that returns an Asset object with both GAAP and Tax depreciation tables for a single asset.  Example Usage:

```
POST /Calculate/Depreciate?GaapMethod=SL&TaxMethod=MACRSHY
Content-Type: application/json

{
    "name": "New Asset",
    "purchaseDate": "2011-01-01",
    "purchasePrice": 1000,
    "residualValue": 0,
    "section179": 0,
    "usefulLife": 5,
    "taxLife": 5
}
```

Parameters:
- **Asset** - A JSON representation of the assets (see example above).  Remember, the Tax Life of the asset must be a valid tax life (**MUST** be one of the following values: 3, 5, 7, 10, 15 or 20)
- **GaapMethod** - The GAAP method of depreciation (**MUST** be one of the following values: SL, DB200, DB150 or SYD)
- **TaxMethod** - The Tax method of depreciation (**MUST** be one of the following values: MACRSHY or MACRSMQ)

Returns:
- **200** - Success and returns an Asset object with calculated depreciation expense and accumulated depreciation by year.
- **422** - Error for an invalid tax life, GAAP Method or Tax Method provided (See returned error description for which).
- **500** - Any other error.

-----------

## Additional Documentation and Examples

### Swagger Usage Documentation
Usage can also be obtained from the running service by calling: https://localhost:5001/swagger/index.html or http://YourServiceUrl/swagger/index.html

###  Testing Usage Examples
A sample set of local REST calls have been provided for localhost testing purposes under the `test/manualtesting.http` file in this repo.  Please refer to that file for more examples of calling the service.  

-------------

## Reusing Code
This repo is shared under the MIT license and anyone is welcome to reuse the code here for any reason, in part or in whole.  All this is provided as-is and without a warranty of any kind.  Please use at your own risk.
