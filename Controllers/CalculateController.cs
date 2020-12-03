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
        public ActionResult<Asset> GetSL([RequiredFromQuery] double fPurchasePrice, [RequiredFromQuery] double fResidual,[RequiredFromQuery]  int iLife)
        {
            Asset asset = new Asset(fPurchasePrice, fResidual, iLife);
            asset.calcSL();
            return asset;
        }

        [HttpGet]
        [Route("[controller]/DB200")]
        public ActionResult<Asset> GetDB200([RequiredFromQuery] double fPurchasePrice, [RequiredFromQuery] double fResidual,[RequiredFromQuery]  int iLife)
        {
            Asset asset = new Asset(fPurchasePrice, fResidual, iLife);
            asset.calcDB200();
            return asset;
        }

        [HttpGet]
        [Route("[controller]/DB150")]
        public ActionResult<Asset> GetDB150([RequiredFromQuery] double fPurchasePrice, [RequiredFromQuery] double fResidual,[RequiredFromQuery]  int iLife)
        {
            Asset asset = new Asset(fPurchasePrice, fResidual, iLife);
            asset.calcDB150();
            return asset;
        }

        [HttpGet]
        [Route("[controller]/SYD")]
        public ActionResult<Asset> GetSYD([RequiredFromQuery] double fPurchasePrice, [RequiredFromQuery] double fResidual,[RequiredFromQuery]  int iLife)
        {
            Asset asset = new Asset(fPurchasePrice, fResidual, iLife);
            asset.calcSYD();
            return asset;
        }

        
    }
}