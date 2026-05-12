using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace DepCalcsCS.Controllers
{
    [ApiController]
    public class CalculateController : ControllerBase
    {
        private static readonly Dictionary<string, (double RecoveryPeriod, string Convention)> AdsClassDefaults = new Dictionary<string, (double RecoveryPeriod, string Convention)>(StringComparer.OrdinalIgnoreCase)
        {
            { "PersonalProperty3Year", (3, "HALFYEAR") },
            { "PersonalProperty5Year", (5, "HALFYEAR") },
            { "PersonalProperty7Year", (7, "HALFYEAR") },
            { "PersonalProperty10Year", (10, "HALFYEAR") },
            { "PersonalProperty15Year", (15, "HALFYEAR") },
            { "PersonalProperty20Year", (20, "HALFYEAR") },
            { "ResidentialRentalProperty", (30, "MIDMONTH") },
            { "NonResidentialRealProperty", (40, "MIDMONTH") }
        };
        private static readonly HashSet<string> AdsConventions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "HALFYEAR",
            "MIDQUARTER",
            "MIDMONTH",
            "FULLYEAR"
        };

        private readonly ILogger<CalculateController> _logger;

        public CalculateController(ILogger<CalculateController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns an array of Assets with calculated Accounting and Tax Depreciation 'tables' along with the asset information originally submitted.
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     POST /Calculate/DepreciateArray?GaapMethod=SL&amp;TaxMethod=MACRSHY
        ///     [
        ///     {
        ///         "name": "New Asset",
        ///         "purchaseDate": "2011-01-01",
        ///         "purchasePrice": 1000,
        ///         "residualValue": 0,
        ///         "section179": 0,
        ///         "usefulLife": 5,
        ///         "taxLife": 5
        ///     },
        ///     {
        ///         "name": "New Asset",
        ///         "purchaseDate": "2011-01-01",
        ///         "purchasePrice": 1000,
        ///         "residualValue": 0,
        ///         "section179": 0,
        ///         "usefulLife": 5,
        ///         "taxLife": 5
        ///     }
        ///     ]
        /// </remarks>
        /// <param name="assets">Array of Asset JSON dictionaries in the body of the post</param>
        /// <returns>Array of Asset objects with both accounting and tax depreciation calculations added</returns>
        /// <response code="200">Returns array of asset objects</response>
        /// <response code="422">If GAAP Method, Tax Method or Tax Life are invalid</response>
        /// <response code="500">Any other error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [Route("[controller]/DepreciateArray")]
        public ActionResult<Asset[]> PostDepreciateArray([FromBody]Asset[] assets)
        {
            foreach(Asset asset in assets)
            {
                switch(asset.GaapMethod)
                {
                    case "SL":
                        asset.calcSL();
                        break;
                    case "DB200":
                        asset.calcDB200();
                        break;
                    case "DB150":
                        asset.calcDB150();
                        break;
                    case "SYD":
                        asset.calcSYD();
                        break;
                    default:
                        throw new Exception("INVALID_GAAP_METHOD");
                }

                switch(asset.TaxMethod)
                {
                    case "MACRSHY":
                        asset.calcMacrsHY();
                        break;
                    case "MACRSMQ":
                        asset.calcMacrsMQ();
                        break;
                    default:
                        throw new Exception("INVALID_TAX_METHOD");
                }
            }

            return assets;
        }

        /// <summary>
        /// Returns an Asset with calculated Accounting and Tax Depreciation 'tables' along with the asset information originally submitted.
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     POST /Calculate/Depreciate?GaapMethod=SL&amp;TaxMethod=MACRSHY
        ///     {
        ///         "name": "New Asset",
        ///         "purchaseDate": "2011-01-01",
        ///         "purchasePrice": 1000,
        ///         "residualValue": 0,
        ///         "section179": 0,
        ///         "usefulLife": 5,
        ///         "taxLife": 5
        ///     }
        /// </remarks>
        /// <param name="asset">Asset JSON dictionary in the body of the post</param>
        /// <returns>Asset object with both accounting and tax depreciation calculations added</returns>
        /// <response code="200">Returns asset object</response>
        /// <response code="422">If GAAP Method, Tax Method or Tax Life are invalid</response>
        /// <response code="500">Any other error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [Route("[controller]/Depreciate")]
        public ActionResult<Asset> PostDepreciate([FromBody]Asset asset)
        {
            switch(asset.GaapMethod)
            {
                case "SL":
                    asset.calcSL();
                    break;
                case "DB200":
                    asset.calcDB200();
                    break;
                case "DB150":
                    asset.calcDB150();
                    break;
                case "SYD":
                    asset.calcSYD();
                    break;
                default:
                    throw new Exception("INVALID_GAAP_METHOD");
            };

            switch(asset.TaxMethod)
            {
                case "MACRSHY":
                    asset.calcMacrsHY();
                    break;
                case "MACRSMQ":
                    asset.calcMacrsMQ();
                    break;
                default:
                    throw new Exception("INVALID_TAX_METHOD");
            }
            
            return asset;
        }

        /// <summary>
        /// Returns an Asset with calculated Straight Line Depreciation 'tables' along with the asset information originally submitted.
        /// </summary>
        /// <param name="PurchasePrice" example="1000">Original price of the asset</param>
        /// <param name="Residual" example="50">Residual value of the asset at the end of the depreciation period</param>
        /// <param name="Life" example="10">The GAAP useful life of the asset</param>
        /// <param name="AssetName" example="MyAsset">(Optional) Name of the asset</param>
        /// <returns>Asset object with both accounting and tax depreciation calculations added</returns>
        /// <response code="200">Returns asset object</response>
        /// <response code="500">Any error (e.g. purchase price less than residual)</response>
        [HttpGet]
        [Route("[controller]/SL")]
        public ActionResult<Asset> GetSL([RequiredFromQuery] double PurchasePrice, [RequiredFromQuery] double Residual,[RequiredFromQuery]  int Life, string AssetName="")
        {
            Asset asset = new Asset(PurchasePrice, Residual, Life, AssetName);
            asset.calcSL();
            return asset;
        }

        /// <summary>
        /// Returns an Asset with calculated Double Declining Balance Depreciation 'tables' along with the asset information originally submitted.
        /// </summary>
        /// <param name="PurchasePrice" example="1000">Original price of the asset</param>
        /// <param name="Residual" example="50">Residual value of the asset at the end of the depreciation period</param>
        /// <param name="Life" example="10">The GAAP useful life of the asset</param>
        /// <param name="AssetName" example="MyAsset">(Optional) Name of the asset</param>
        /// <returns>Asset object with both accounting and tax depreciation calculations added</returns>
        /// <response code="200">Returns asset object</response>
        /// <response code="500">Any error (e.g. purchase price less than residual)</response>
        [HttpGet]
        [Route("[controller]/DB200")]
        public ActionResult<Asset> GetDB200([RequiredFromQuery] double PurchasePrice, [RequiredFromQuery] double Residual,[RequiredFromQuery]  int Life, string AssetName="")
        {
            Asset asset = new Asset(PurchasePrice, Residual, Life, AssetName);
            asset.calcDB200();
            return asset;
        }

        /// <summary>
        /// Returns an Asset with calculated 150% Declining Balance Depreciation 'tables' along with the asset information originally submitted.
        /// </summary>
        /// <param name="PurchasePrice" example="1000">Original price of the asset</param>
        /// <param name="Residual" example="50">Residual value of the asset at the end of the depreciation period</param>
        /// <param name="Life" example="10">The GAAP useful life of the asset</param>
        /// <param name="AssetName" example="MyAsset">(Optional) Name of the asset</param>
        /// <returns>Asset object with both accounting and tax depreciation calculations added</returns>
        /// <response code="200">Returns asset object</response>
        /// <response code="500">Any error (e.g. purchase price less than residual)</response>
        [HttpGet]
        [Route("[controller]/DB150")]
        public ActionResult<Asset> GetDB150([RequiredFromQuery] double PurchasePrice, [RequiredFromQuery] double Residual,[RequiredFromQuery]  int Life, string AssetName="")
        {
            Asset asset = new Asset(PurchasePrice, Residual, Life, AssetName);
            asset.calcDB150();
            return asset;
        }

        /// <summary>
        /// Returns an Asset with calculated 150% Declining Balance Depreciation 'tables' along with the asset information originally submitted.
        /// </summary>
        /// <param name="PurchasePrice" example="1000">Original price of the asset</param>
        /// <param name="Residual" example="50">Residual value of the asset at the end of the depreciation period</param>
        /// <param name="Life" example="10">The GAAP useful life of the asset</param>
        /// <param name="AssetName" example="MyAsset">(Optional) Name of the asset</param>
        /// <returns>Asset object with both accounting and tax depreciation calculations added</returns>
        /// <response code="200">Returns asset object</response>
        /// <response code="500">Any error (e.g. purchase price less than residual)</response>
        [HttpGet]
        [Route("[controller]/SYD")]
        public ActionResult<Asset> GetSYD([RequiredFromQuery] double PurchasePrice, [RequiredFromQuery] double Residual,[RequiredFromQuery]  int Life, string AssetName="")
        {
            Asset asset = new Asset(PurchasePrice, Residual, Life, AssetName);
            asset.calcSYD();
            return asset;
        }

        /// <summary>
        /// Returns an Asset with calculated MACRS Half-Year depreciation 'tables' along with the asset information originally submitted.
        /// </summary>
        /// <param name="PurchasePrice" example="1000">Original price of the asset</param>
        /// <param name="Life" example="10">The Tax useful life of the asset</param>
        /// <param name="PurchaseDate" example="10%2F01%2F2020">The date an asset was purchased</param>
        /// <param name="AssetName" example="MyAsset">(Optional) Name of the asset</param>
        /// <param name="Section179" example="1000">(Optional) The section 179 depreciation amount taken on the asset in the first year</param>
        /// <returns>Asset object with both accounting and tax depreciation calculations added</returns>
        /// <response code="200">Returns asset object</response>
        /// <response code="422">If Tax Life are invalid</response>
        /// <response code="500">Any error (e.g. purchase price less than residual)</response>
        [HttpGet]
        [Route("[controller]/MACRSHY")]
        public ActionResult<Asset> GetMacrsHY([RequiredFromQuery] double PurchasePrice, [RequiredFromQuery]  int Life, [RequiredFromQuery] DateTime PurchaseDate, string AssetName="", double Section179=0)
        {            
            Asset asset = new Asset (AssetName, PurchaseDate, PurchasePrice, 0, Life, Life, Section179, "" );
            asset.calcMacrsHY();
            return asset;
        }

        /// <summary>
        /// Returns an Asset with calculated MACRS Mid-Quarter depreciation 'tables' along with the asset information originally submitted.
        /// </summary>
        /// <param name="PurchasePrice" example="1000">Original price of the asset</param>
        /// <param name="Life" example="10">The Tax useful life of the asset</param>
        /// <param name="PurchaseDate" example="10%2F01%2F2020">The date an asset was purchased</param>
        /// <param name="AssetName" example="MyAsset">(Optional) Name of the asset</param>
        /// <param name="Section179" example="1000">(Optional) The section 179 depreciation amount taken on the asset in the first year</param>
        /// <returns>Asset object with both accounting and tax depreciation calculations added</returns>
        /// <response code="200">Returns asset object</response>
        /// <response code="422">If Tax Life are invalid</response>
        /// <response code="500">Any error (e.g. purchase price less than residual)</response>

        [HttpGet]
        [Route("[controller]/MACRSMQ")]
        public ActionResult<Asset> GetMacrsMQ([RequiredFromQuery] double PurchasePrice, [RequiredFromQuery]  int Life, [RequiredFromQuery] DateTime PurchaseDate, string AssetName="", double Section179=0)
        {
            Asset asset = new Asset (AssetName, PurchaseDate, PurchasePrice, 0, Life, Life, Section179, "" );
            asset.calcMacrsMQ();
            return asset;
        }

        /// <summary>
        /// Returns an ADS (Alternative Depreciation System) straight-line depreciation table for a single asset.
        /// </summary>
        /// <param name="PurchasePrice" example="1000">Original price of the asset</param>
        /// <param name="PurchaseDate" example="10%2F01%2F2020">The date an asset was placed in service</param>
        /// <param name="AssetClass" example="NonResidentialRealProperty">ADS asset class used to default recovery period and convention</param>
        /// <param name="AssetName" example="AdsAsset">(Optional) Name of the asset</param>
        /// <param name="Residual" example="0">(Optional) Residual value used for depreciation basis</param>
        /// <param name="RecoveryPeriod" example="40">(Optional) Explicit ADS recovery period override in years</param>
        /// <param name="Convention" example="MIDMONTH">(Optional) Explicit ADS convention override (HALFYEAR, MIDQUARTER, MIDMONTH, FULLYEAR)</param>
        /// <returns>ADS depreciation details and depreciation table</returns>
        /// <response code="200">Returns ADS depreciation object</response>
        /// <response code="422">If asset class, recovery period, or convention are invalid</response>
        [HttpGet]
        [Route("[controller]/MACRSADS")]
        public ActionResult<AdsDepreciationResult> GetMacrsADS([RequiredFromQuery] double PurchasePrice, [RequiredFromQuery] DateTime PurchaseDate, [RequiredFromQuery] string AssetClass, string AssetName = "", double Residual = 0, double? RecoveryPeriod = null, string Convention = null)
        {
            (double recoveryPeriod, string convention) = ResolveAdsRule(AssetClass, RecoveryPeriod, Convention);
            List<DepYear> table = DepCalcs.CalcMacrsADS(PurchasePrice, Residual, PurchaseDate, recoveryPeriod, convention);

            AdsDepreciationResult result = new AdsDepreciationResult();
            result.AssetName = AssetName;
            result.AssetClass = AssetClass;
            result.PurchaseDate = PurchaseDate;
            result.PurchasePrice = PurchasePrice;
            result.ResidualValue = Residual;
            result.RecoveryPeriod = recoveryPeriod;
            result.Convention = convention;
            result.DepreciationTable = table;

            return result;
        }

        private static (double RecoveryPeriod, string Convention) ResolveAdsRule(string assetClass, double? recoveryPeriodOverride, string conventionOverride)
        {
            if (recoveryPeriodOverride.HasValue || !String.IsNullOrWhiteSpace(conventionOverride))
            {
                string convention = NormalizeAndValidateConvention(conventionOverride);
                double recoveryPeriod = 0;

                if (recoveryPeriodOverride.HasValue)
                {
                    if (recoveryPeriodOverride.Value <= 0)
                    {
                        throw new Exception("INVALID_ADS_RECOVERY_PERIOD");
                    }

                    recoveryPeriod = recoveryPeriodOverride.Value;
                }
                else if (!String.IsNullOrWhiteSpace(assetClass) && AdsClassDefaults.ContainsKey(assetClass))
                {
                    recoveryPeriod = AdsClassDefaults[assetClass].RecoveryPeriod;
                }
                else
                {
                    throw new Exception("INVALID_ADS_RECOVERY_PERIOD");
                }

                return (recoveryPeriod, convention);
            }

            if (String.IsNullOrWhiteSpace(assetClass) || !AdsClassDefaults.ContainsKey(assetClass))
            {
                throw new Exception("INVALID_ADS_ASSET_CLASS");
            }

            return AdsClassDefaults[assetClass];
        }

        private static string NormalizeAndValidateConvention(string convention)
        {
            if (String.IsNullOrWhiteSpace(convention))
            {
                return null;
            }

            string normalized = convention.Trim().ToUpperInvariant();
            if (!AdsConventions.Contains(normalized))
            {
                throw new Exception("INVALID_ADS_CONVENTION");
            }

            return normalized;
        }
        
    }
}
