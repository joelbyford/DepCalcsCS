using System;
using DepCalcsCS;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        [HttpPost]
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

        [HttpGet]
        [Route("[controller]/SL")]
        public ActionResult<Asset> GetSL([RequiredFromQuery] double PurchasePrice, [RequiredFromQuery] double Residual,[RequiredFromQuery]  int Life, string AssetName="")
        {
            Asset asset = new Asset(PurchasePrice, Residual, Life, AssetName);
            asset.calcSL();
            return asset;
        }

        [HttpGet]
        [Route("[controller]/DB200")]
        public ActionResult<Asset> GetDB200([RequiredFromQuery] double PurchasePrice, [RequiredFromQuery] double Residual,[RequiredFromQuery]  int Life, string AssetName="")
        {
            Asset asset = new Asset(PurchasePrice, Residual, Life, AssetName);
            asset.calcDB200();
            return asset;
        }

        [HttpGet]
        [Route("[controller]/DB150")]
        public ActionResult<Asset> GetDB150([RequiredFromQuery] double PurchasePrice, [RequiredFromQuery] double Residual,[RequiredFromQuery]  int Life, string AssetName="")
        {
            Asset asset = new Asset(PurchasePrice, Residual, Life, AssetName);
            asset.calcDB150();
            return asset;
        }

        [HttpGet]
        [Route("[controller]/SYD")]
        public ActionResult<Asset> GetSYD([RequiredFromQuery] double PurchasePrice, [RequiredFromQuery] double Residual,[RequiredFromQuery]  int Life, string AssetName="")
        {
            Asset asset = new Asset(PurchasePrice, Residual, Life, AssetName);
            asset.calcSYD();
            return asset;
        }

        [HttpGet]
        [Route("[controller]/MACRSHY")]
        public ActionResult<Asset> GetMacrsHY([RequiredFromQuery] double PurchasePrice, [RequiredFromQuery]  int Life, [RequiredFromQuery] DateTime PurchaseDate, string AssetName="", double Section179=0)
        {            
            Asset asset = new Asset (AssetName, PurchaseDate, PurchasePrice, 0, Life, Life, Section179, "" );
            asset.calcMacrsHY();
            return asset;
        }

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