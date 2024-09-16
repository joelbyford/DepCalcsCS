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

        public string GaapMethod {get; set;}

        public string TaxMethod {get; set;}

        private TAX_Lifes _TaxLife;
        public int TaxLife {
            get
            {
                return (int) this._TaxLife;
            }

            set
            {
                if (value == 20)
                {
                    _TaxLife = TAX_Lifes.TWENTY;
                }
                else if (value == 15)
                {
                    _TaxLife = TAX_Lifes.FIFTEEN;
                }
                else if (value == 10)
                {
                    _TaxLife = TAX_Lifes.TEN;
                }
                else if (value == 7)
                {
                    _TaxLife = TAX_Lifes.SEVEN;
                }
                else if (value ==5)
                {
                    _TaxLife = TAX_Lifes.FIVE;
                }
                else if (value ==3)
                {
                    _TaxLife = TAX_Lifes.THREE;
                }
                else if (value ==0)
                {
                    _TaxLife = TAX_Lifes.NONE;
                }
                else
                {
                    throw new Exception("INVALID_TAX_YEAR");
                }
            }
        }

        public List<DepYear> GaapDepreciation {get; set;}

        public List<DepYear> TaxDepreciation {get; set;}

        //Empty Constructor
        public Asset()
        {
            Name = "";
            PurchaseDate = DateTime.Now;
            PurchasePrice = 0;
            ResidualValue = 0;
            UsefulLife = 0;
            TaxLife = (int)TAX_Lifes.NONE;
            Section179 = 0;
            return;
        }

        //Full Constructor
        public Asset (string sName, DateTime dtPurchaseDate, double fPurchasePrice, double fResidualValue, int iUSefulLife,int iTaxLife, double fSection179=0, string sDescription="" )
        {
            Name = sName;
            //Description = sDescription;
            PurchaseDate = dtPurchaseDate;
            PurchasePrice = fPurchasePrice;
            ResidualValue = fResidualValue;
            UsefulLife = iUSefulLife;
            Section179 = fSection179;
            TaxLife = iTaxLife;
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
            TaxLife = (int)TAX_Lifes.NONE;
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