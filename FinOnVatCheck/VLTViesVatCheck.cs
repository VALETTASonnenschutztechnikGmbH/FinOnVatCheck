using System;

namespace VLTVATChecker
{

    public class VLTViesVatCheck
    {
        #region InputAttributes
        public string VATNumber { get; set; }

        /// <summary>
        /// The country code of the uid to check
        /// </summary>
        /// <remarks>
        /// This parameter can be one of these country codes
        /// 
        /// country --> code to use
        /// ************************************************
        /// Austria --> AT 
        /// Belgium --> BE 
        /// Bulgaria --> BG 
        /// Cyprus --> CY 
        /// Czech Republic --> CZ 
        /// Germany --> DE 
        /// Denmark --> DK 
        /// Estonia EE 
        /// Greece EL 
        /// Spain ES 
        /// Finland FI 
        /// France FR 
        /// United Kingdom GB 
        /// Hungary HU 
        /// Ireland IE 
        /// Italy IT 
        /// Lithuania LT 
        /// Luxembourg LU 
        /// Malta MT 
        /// The Netherlands NL 
        /// Poland PL 
        /// Portugal PT 
        /// Romania RO 
        /// Sweden SE 
        /// Slovenia SI 
        /// Slovakia SK
        /// </remarks>
        public string CountryCode { get; set; }
        #endregion

        #region OutputAttributes
        public string Address { get; set; }
        public bool IsValid { get; set; }
        public string Name { get; set; }
        public DateTime RetDate { get; set; }
        #endregion

        /// <summary>
        /// Check if the VAT number is valid or not
        /// </summary>
        /// <returns>True if the VAT number could be validated otherwise false</returns>
        public bool CheckVat()
        {
            if (string.IsNullOrEmpty(VATNumber) || string.IsNullOrEmpty(CountryCode))
                return (false);

            //If the country code is AT for Austria we need a U before the UID
            if (CountryCode.ToUpper().Equals("AT"))
            {
                if (!VATNumber.StartsWith("U"))
                    VATNumber = "U" + VATNumber;
            }

            string strVat = VATNumber;
            string strCountry = CountryCode;

            bool bValid = false;
            string strName = string.Empty;
            string strAddress = string.Empty;

            try
            {
                checkVatService visService = new checkVatService();

                RetDate = visService.checkVat(ref strCountry, ref strVat,
                                              out bValid, out strName, out strAddress);
                IsValid = bValid;
                Name = strName;
                Address = strAddress;

                return (IsValid);
            }
            catch (Exception err)
            {
                System.Diagnostics.Trace.TraceError(err.ToString());
                return (false);
            }
        }

    }
}
