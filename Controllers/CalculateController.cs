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
        public ActionResult<Asset> GetMacrsHY([RequiredFromQuery] double PurchasePrice, [RequiredFromQuery]  int Life, string AssetName="", double Section179=0)
        {
            Asset asset = new Asset(PurchasePrice, 0, Life, AssetName, Section179);
            asset.calcMacrsHY();
            return asset;
        }

        [HttpGet]
        [Route("[controller]/MACRSMQ")]
        public ActionResult<Asset> GetMacrsMQ([RequiredFromQuery] double PurchasePrice, [RequiredFromQuery]  int Life, [RequiredFromQuery] DateTime PurchaseDate, string AssetName="", double Section179=0)
        {
            TAX_Lifes TaxLife;

            if (Life >= 20)
            {
                TaxLife = TAX_Lifes.TWENTY;
            }
            else if (Life >= 15)
            {
                TaxLife = TAX_Lifes.FIFTEEN;
            }
            else if (Life >= 10)
            {
                TaxLife = TAX_Lifes.TEN;
            }
            else if (Life >= 7)
            {
                TaxLife = TAX_Lifes.SEVEN;
            }
            else if (Life >=5)
            {
                TaxLife = TAX_Lifes.FIVE;
            }
            else if (Life >=3)
            {
                TaxLife = TAX_Lifes.THREE;
            }
            else
            {
                TaxLife = TAX_Lifes.NONE;
            }
            
            Asset asset = new Asset (AssetName, PurchaseDate, PurchasePrice, 0, Life, TaxLife, Section179, "" );
            asset.calcMacrsMQ();
            return asset;
        }
        
    }
}