using System;
using System.Collections.Generic;

namespace DepCalcsCS
{
    public enum TAX_Lifes
    {
	NONE	=0,
	THREE	=3,
	FIVE	=5,
	SEVEN	=7,
	TEN		=10,
	FIFTEEN	=15,
	TWENTY	=20
    };

    public class Asset
    {
        //Public Properties
        public String Name {get; set;}

        //public String Description {get; set;}

         public DateTime PurchaseDate {get; set;}

        public double PurchasePrice {get; set;}

        public double ResidualValue {get; set;}

        public double Section179 {get; set;}

        public int UsefulLife {get; set;}

        public TAX_Lifes TaxLife {get; set;}

        public List<DepYear> GaapDepreciation {get; set;}

        public List<DepYear> TaxDepreciation {get; set;}

        

        //Full Constructor
        public Asset (string sName, DateTime dtPurchaseDate, double fPurchasePrice, double fResidualValue, int iUSefulLife,TAX_Lifes iTaxLife, double fSection179=0, string sDescription="" )
        {
            Name = sName;
            //Description = sDescription;
            PurchaseDate = dtPurchaseDate;
            PurchasePrice = fPurchasePrice;
            ResidualValue = fResidualValue;
            UsefulLife = iUSefulLife;
            TaxLife = iTaxLife;
            Section179 = fSection179;
            return;
        }

        //Limited Constructor
        public Asset (double fPurchasePrice, double fResidualValue, int iUSefulLife, string sName="Unnamed Asset", double fSection179=0)
        {
            Name = sName;
            //Description = "";
            PurchaseDate = DateTime.Now;
            PurchasePrice = fPurchasePrice;
            ResidualValue = fResidualValue;
            UsefulLife = iUSefulLife;
            Section179 = fSection179;
            //guess useful life since it wasn't provided to us
            if (iUSefulLife >= 20)
            {
                TaxLife = TAX_Lifes.TWENTY;
            }
            else if (iUSefulLife >= 15)
            {
                TaxLife = TAX_Lifes.FIFTEEN;
            }
            else if (iUSefulLife >= 10)
            {
                TaxLife = TAX_Lifes.TEN;
            }
            else if (iUSefulLife >= 7)
            {
                TaxLife = TAX_Lifes.SEVEN;
            }
            else if (iUSefulLife >=5)
            {
                TaxLife = TAX_Lifes.FIVE;
            }
            else if (iUSefulLife >=3)
            {
                TaxLife = TAX_Lifes.THREE;
            }
            else
            {
                TaxLife = TAX_Lifes.NONE;
            }
            return;
        }

        // SL Calc
        public void calcSL()
        {
            GaapDepreciation = DepCalcs.CalcSL(this);
        }

        public void calcDB200()
        {
            GaapDepreciation = DepCalcs.CalcDB200(this);
        }

        public void calcDB150()
        {
            GaapDepreciation = DepCalcs.CalcDB150(this);
        }

        public void calcSYD()
        {
            GaapDepreciation = DepCalcs.CalcSYD(this);
        }

        public void calcMacrsHY()
        {
            TaxDepreciation = DepCalcs.CalcMacrsHY(this);
        }

        public void calcMacrsMQ()
        {
            TaxDepreciation = DepCalcs.CalcMacrsMQ(this);
        }
    }
}