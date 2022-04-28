using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace DepCalcsCS.Controllers
{
    [ApiController]
    public class CalculateController : ControllerBase
    {

        private readonly ILogger<CalculateController> _logger;

        public CalculateController(ILogger<CalculateController> logger)
        {
            _logger = logger;
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
        /// <param name="GaapMethod" example="SL">GAAP Depreciation Method (SL, DB200, DB150 OR SYD)</param>
        /// <param name="TaxMethod" example="MACRSHY">Tax Depreciation Method (MACRSHY or MACRSMQ)</param>
        /// <returns>Asset object with both accounting and tax depreciation calculations added</returns>
        /// <response code="200">Returns asset object</response>
        /// <response code="422">If GAAP Method, Tax Method or Tax Life are invalid</response>
        /// <response code="500">Any other error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [Route("[controller]/Depreciate")]
        public ActionResult<Asset> PostDepreciate([FromBody]Asset asset, [RequiredFromQuery] string GaapMethod, [RequiredFromQuery] string TaxMethod)
        {
            switch(GaapMethod)
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

            switch(TaxMethod)
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
        
    }
}