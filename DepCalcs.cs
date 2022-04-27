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

            int iYear = 0;
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

            int iYear = 0;
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

            int iYear = 0;
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

            int iYear = 0;
            int iYearsLeft = 0;
            int iSigma = 0;
	        double fAD = 0;
            double fBasis = 0;

            fBasis = asset.PurchasePrice - asset.ResidualValue;
            iYearsLeft = asset.UsefulLife;
            iSigma = (int)(asset.UsefulLife * (((double)asset.UsefulLife+(double)1)/(double)2));

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

        /*---------------------------------------*/
        /* ------- MACRS-HY Depreciation ------- */
        /*---------------------------------------*/

        public static List<DepYear> CalcMacrsHY(Asset asset)
        {
            List<DepYear> years = new List<DepYear>();

            // locals
            int iYear = 0;
	        double fAD = 0;
            double fBasis = 0;

            // using the MACRS HY table published by the IRS
            double[] HYtable3 = new double[4] {33.33, 44.45, 14.81,  7.41};

            double[] HYtable5 = new double[6] {20.00, 32.00, 19.20, 11.52, 11.52, 5.76};

            double[] HYtable7 = new double[8] {14.29, 24.49, 17.49, 12.49,  8.93, 8.92,
                                    8.93,  4.46};

            double[] HYtable10 = new double[11] {10.00, 18.00, 14.40, 11.52,  9.22, 7.37,
                                    6.55,  6.55,  6.56,  6.55,  3.28};

            double[] HYtable15 = new double[16] { 5.00,  9.50,  8.55,  7.70,  6.93, 6.23,
                                    5.90,  5.90,  5.91,  5.90,  5.91, 5.90,
                                    5.91,  5.90,  5.91,  2.95};

            double[] HYtable20 = new double[21] {3.750, 7.219, 6.677, 6.177, 5.713, 5.285,
                                    4.888, 4.522, 4.462, 4.461, 4.462, 4.461,
                                    4.462, 4.461, 4.462, 4.461, 4.462, 4.461,
                                    4.462, 4.461, 2.231};

            //calculate the basis given SS179
            fBasis = asset.PurchasePrice - asset.Section179;

            for(iYear=0;iYear<=asset.TaxLife;iYear++)
	        {
                DepYear oYear = new DepYear();
                oYear.Year = iYear;

                switch(asset.TaxLife)
                {
                    case 3:
                        oYear.Expense = fBasis * (HYtable3[iYear]/100);
                        break;
                    
                    case 5:
                        oYear.Expense = fBasis * (HYtable5[iYear]/100);
                        break;

                    case 7:
                        oYear.Expense = fBasis * (HYtable7[iYear]/100);
                        break;
                    
                    case 10:
                        oYear.Expense = fBasis * (HYtable10[iYear]/100);
                        break;

                    case 15:
                        oYear.Expense = fBasis * (HYtable15[iYear]/100);
                        break;

                    case 20:
                        oYear.Expense = fBasis * (HYtable20[iYear]/100);
                        break;

                    default:
                        oYear.Expense = 0;
                        break;
                }
                //add in SS179 (in first year)
                if (iYear==0)
                    oYear.Expense += asset.Section179;                    

                //Adjust for Rounding (in last year)
                if ((fAD + oYear.Expense) > asset.PurchasePrice)
                    oYear.Expense = asset.PurchasePrice - fAD;

                //update the AD
                fAD += oYear.Expense;
                oYear.AccumulatedDepreciation = fAD;


                //add the year to the array
                years.Add(oYear);
            }

            return years;
        }

        /*---------------------------------------*/
        /* ------- MACRS-MQ Depreciation ------- */
        /*---------------------------------------*/

        public static List<DepYear> CalcMacrsMQ(Asset asset)
        {
            List<DepYear> years = new List<DepYear>();

            // locals
            int iYear = 0;
	        double fAD = 0;
            double fBasis = 0;
            short iQtr = 0;

            // using the MACRS MQ table published by the IRS
            double[,] MQtable3 = new double[4,4]  { {58.33, 27.78, 12.35, 1.54},
                                                    {41.67, 38.89, 14.14, 5.30},
                                                    {25.00, 50.00, 16.67, 8.33},
                                                    {8.33, 61.11, 20.37, 10.19}};

	        double[,] MQtable5 = new double[4,6]  {{35.00, 26.00, 15.60, 11.01, 11.01, 1.38},
                                                    {25.00, 30.00, 18.00, 11.37, 11.37, 4.26},
                                                    {15.00, 34.00, 20.40, 12.24, 11.30, 7.06},
                                                    {5.00, 38.00, 22.80, 13.68, 10.94, 9.58}};

	        double[,] MQtable7 = new double[4,8]  {{25.00, 21.43, 15.31, 10.93, 8.75, 8.74, 8.75, 1.09},
                                                    {17.85, 23.47, 16.76, 11.97, 8.87, 8.87, 8.87, 3.33},
                                                    {10.71, 25.51, 18.22, 13.02, 9.30, 8.85, 8.86, 5.53},
                                                    {3.57, 27.55, 19.68, 14.06, 10.04, 8.73, 8.73, 7.64}};

	        double[,] MQtable10 = new double[4,11] {{17.50, 16.50, 13.20, 10.56, 8.45, 6.76, 6.55, 6.55, 6.56, 6.55, 0.82},
                                                    {12.50, 17.50, 14.00, 11.20, 8.96, 7.17, 6.55, 6.55, 6.56, 6.55, 2.46},
                                                    {7.50, 18.50, 14.80, 11.84, 9.47, 7.58, 6.55, 6.55, 6.56, 6.55, 4.10},
                                                    {2.50, 19.50, 15.60, 12.48, 9.98, 7.99, 6.55, 6.55, 6.56, 6.55, 5.74}};

	        double[,] MQtable15 = new double[4,16] {{8.75, 9.13, 8.21, 7.39, 6.65, 5.99, 5.90, 5.91, 5.90, 5.91, 5.90, 5.91, 5.90, 5.91, 5.90, 0.74},
                                                    {6.25, 9.38, 8.44, 7.59, 6.83, 6.15, 5.91, 5.90, 5.91, 5.90, 5.91, 5.90, 5.91, 5.90, 5.91, 2.21},
                                                    {3.75, 9.63, 8.66, 7.80, 7.02, 6.31, 5.90, 5.90, 5.91, 5.90, 5.91, 5.90, 5.91, 5.90, 5.91, 3.69},
                                                    {1.25, 9.88, 8.89, 8.00, 7.20, 6.48, 5.90, 5.90, 5.90, 5.91, 5.90, 5.91, 5.90, 5.91, 5.90, 5.17}};
	
	        double[,] MQtable20 = new double[4,21] {{6.563, 7.000, 6.482, 5.996, 5.546, 5.130, 4.746, 4.459, 4.459, 4.459, 4.459, 4.460, 4.459, 4.460, 4.459, 4.460, 4.459, 4.460, 4.459, 4.460, 0.557},
                                                    {4.688, 7.148, 6.612, 6.116, 5.658, 5.233, 4.841, 4.478, 4.463, 4.463, 4.463, 4.463, 4.463, 4.463, 4.462, 4.463, 4.462, 4.463, 4.462, 4.463, 1.673},
                                                    {2.813, 7.289, 6.742, 6.237, 5.769, 5.336, 4.936, 4.566, 4.460, 4.460, 4.460, 4.460, 4.461, 4.460, 4.461, 4.460, 4.461, 4.460, 4.461, 4.460, 2.788},
                                                    {0.938, 7.430, 6.872, 5.880, 5.439, 5.031, 4.654, 4.458, 4.458, 4.458, 4.458, 4.458, 4.458, 4.458, 4.458, 4.458, 4.458, 4.459, 4.458, 4.459, 3.901}};

            //calculate the basis given SS179
            fBasis = asset.PurchasePrice - asset.Section179;

            //figure out the quarter by ignoring the fraction through casting to a short int
	        iQtr = (short)(asset.PurchaseDate.Month/12);

            for(iYear=0;iYear<=asset.TaxLife;iYear++)
	        {
                DepYear oYear = new DepYear();
                oYear.Year = iYear;

                switch(asset.TaxLife)
                {
                    case 3:
                        oYear.Expense = fBasis * (MQtable3[iQtr,iYear]/100);
                        break;
                    
                    case 5:
                        oYear.Expense = fBasis * (MQtable5[iQtr,iYear]/100);
                        break;

                    case 7:
                        oYear.Expense = fBasis * (MQtable7[iQtr,iYear]/100);
                        break;
                    
                    case 10:
                        oYear.Expense = fBasis * (MQtable10[iQtr,iYear]/100);
                        break;

                    case 15:
                        oYear.Expense = fBasis * (MQtable15[iQtr,iYear]/100);
                        break;

                    case 20:
                        oYear.Expense = fBasis * (MQtable20[iQtr,iYear]/100);
                        break;

                    default:
                        oYear.Expense = 0;
                        break;
                }
                //add in SS179 (in first year)
                if (iYear==0)
                    oYear.Expense += asset.Section179;                    

                //Adjust for Rounding (in last year)
                if ((fAD + oYear.Expense) > asset.PurchasePrice)
                    oYear.Expense = asset.PurchasePrice - fAD;

                //update the AD
                fAD += oYear.Expense;
                oYear.AccumulatedDepreciation = fAD;


                //add the year to the array
                years.Add(oYear);
            }

            return years;
        }
    } 
}