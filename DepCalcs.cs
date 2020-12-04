using System;
using System.Collections.Generic;


namespace DepCalcsCS
{
    public static class DepCalcs
    {
        
        /*---------------------------------*/
        /* ------- SL Depreciation ------- */
        /*---------------------------------*/
        public static List<DepYear> CalcSL(Asset asset)
        {
            List<DepYear> years = new List<DepYear>();

            short iYear = 0;
	        double fAD = 0;

            while (iYear < asset.UsefulLife)
		    {
                DepYear oYear = new DepYear();
                oYear.Year = iYear;
                oYear.Expense = ((asset.PurchasePrice - asset.ResidualValue) / asset.UsefulLife);
                
                fAD += oYear.Expense;
                oYear.AccumulatedDepreciation = fAD;

                years.Add(oYear);

    			iYear++;
		    }


            return years;
        }

        /*---------------------------------*/
        /* ------- DDB Depreciation ------- */
        /*---------------------------------*/
        public static List<DepYear> CalcDB200(Asset asset)
        {
            List<DepYear> years = new List<DepYear>();

            short iYear = 0;
	        double fAD = 0;
            double fMultiplier = 0;
            double fBalance = 0;
            double fBasis = asset.PurchasePrice - asset.ResidualValue;

            fBalance = asset.PurchasePrice;
            fMultiplier = 2/(double)asset.UsefulLife;

            for(iYear=0;iYear<asset.UsefulLife;iYear++)
	        {
                DepYear oYear = new DepYear();
                oYear.Year = iYear;
                oYear.Expense = fBalance * fMultiplier;
		        fBalance -= oYear.Expense;
		        fAD += oYear.Expense;
		        oYear.AccumulatedDepreciation = fAD;

                years.Add(oYear);
	        }

            return years;
        }

        /*---------------------------------*/
        /* ------- DB150 Depreciation ------- */
        /*---------------------------------*/
        public static List<DepYear> CalcDB150(Asset asset)
        {
            List<DepYear> years = new List<DepYear>();

            short iYear = 0;
	        double fAD = 0;
            double fMultiplier = 0;
            double fBalance = 0;
            double fBasis = asset.PurchasePrice - asset.ResidualValue;

            fBalance = asset.PurchasePrice;
            fMultiplier = 1.5/(double)asset.UsefulLife;

            for(iYear=0;iYear<asset.UsefulLife;iYear++)
	        {
                DepYear oYear = new DepYear();
                oYear.Year = iYear;
                oYear.Expense = fBalance * fMultiplier;
		        fBalance -= oYear.Expense;
		        fAD += oYear.Expense;
		        oYear.AccumulatedDepreciation = fAD;

                years.Add(oYear);
	        }

            return years;
        }

        /*----------------------------------*/
        /* ------- SYD Depreciation ------- */
        /*----------------------------------*/
        public static List<DepYear> CalcSYD(Asset asset)
        {
            List<DepYear> years = new List<DepYear>();

            short iYear = 0;
            int iYearsLeft = 0;
            short iSigma = 0;
	        double fAD = 0;
            double fBasis = 0;

            fBasis = asset.PurchasePrice - asset.ResidualValue;
            iYearsLeft = asset.UsefulLife;
            iSigma = (short)(asset.UsefulLife * (((double)asset.UsefulLife+(double)1)/(double)2));

            for(iYear=0;iYear<asset.UsefulLife;iYear++)
	        {
                DepYear oYear = new DepYear();
                oYear.Year = iYear;
                oYear.Expense = fBasis * ((double)iYearsLeft/(double)iSigma);
                iYearsLeft -= 1;
		        fAD += oYear.Expense;
		        oYear.AccumulatedDepreciation = fAD;

                years.Add(oYear);
	        }

            return years;
        }
    } 
}