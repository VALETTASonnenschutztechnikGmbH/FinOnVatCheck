using System;
using System.Linq;
using System.Reflection;

namespace FinOnVatCheck
{
    public class VLTFinOnVatCheck
    {
        #region InputAttributes

        public string VATNumber { get; set; }
        public string Memberid { get; set; }
        public string UserId { get; set; }
        public string Pin { get; set; }
        public string ProducerVatNum { get; set; }

        /// <summary>
        /// The country code of the uid to check
        /// </summary>
        /// <remarks>
        public string CountryCode { get; set; }

        #endregion InputAttributes

        #region OutputAttributes

        public string SessionId { get; set; }
        public int ReturnCode { get; set; }
        public string ReturnMsg { get; set; }
        public string Address { get; set; }
        public bool IsValid { get; set; }
        public string Name { get; set; }
        public DateTime RetDate { get; set; }
        public string VatService { get; set; }

        #endregion OutputAttributes

        public int GetSessionId()
        {
            if (string.IsNullOrEmpty(Memberid) || string.IsNullOrEmpty(UserId)
                || string.IsNullOrEmpty(Pin) || string.IsNullOrEmpty(ProducerVatNum))
                return (-1);

            string ret = string.Empty;
            string MemberidLoc = Memberid;
            string UserIdLoc = UserId;
            string PinLoc = Pin;
            string ProducerVatNumLoc = ProducerVatNum;

            int ReturnCodeLoc = 0;
            string ReturnMsgLoc = string.Empty;
            string strName = string.Empty;
            string strAddress = string.Empty;

            try
            {
                //x509certificate2 cert = new x509certificate2();
                //x509keystorageflags defaultkeyset = default(x509keystorageflags);
                //cert.import("thawte_primary_root_ca-g3_sha256.pem", "", defaultkeyset);
                sessionService sessionService = new sessionService();
                //sessionservice.clientcertificates.add(cert);
                SessionId = sessionService.login(MemberidLoc, UserIdLoc, PinLoc, ProducerVatNumLoc, out ReturnCodeLoc, out ReturnMsgLoc);
                ReturnCode = ReturnCodeLoc;
                ReturnMsg = ReturnMsgLoc;
                //    MessageBox.Show(text: ReturnMsg);
                return (ReturnCode);
            }
            catch (Exception err)
            {
                System.Diagnostics.Trace.TraceError(err.ToString());
                ReturnCode = -1;
                ReturnMsg = err.ToString();
                //MessageBox.Show(err.Message);
                return (-1);
            }
        }

        public bool CheckVat()
        {
            int retSession = GetSessionId();
            if (retSession == 0)
            {
                if (string.IsNullOrEmpty(VATNumber) || string.IsNullOrEmpty(CountryCode))
                    return (false);

                string strVat = CountryCode + VATNumber;
                string strCountry = CountryCode;

                string strName = string.Empty;
                string strAddress1 = string.Empty;
                string strAddress2 = string.Empty;
                string strAddress3 = string.Empty;
                string strAddress4 = string.Empty;
                string strAddress5 = string.Empty;
                string strAddress6 = string.Empty;
                string strReturn = string.Empty;

                int ReturnCodeLoc = 0;
                string ReturnMsgLoc = string.Empty;
                string[] stuff;
                VatService = "Finanz Online";
                try
                {
                    uidAbfrageService uidAbfrageService = new uidAbfrageService();
                    ReturnCodeLoc = uidAbfrageService.uidAbfrage(Memberid, UserId, SessionId, ProducerVatNum, strVat, uidAbfrageServiceRequestStufe.Item2, out ReturnMsgLoc, out strName, out strAddress1, out strAddress2, out strAddress3, out strAddress4, out strAddress5, out strAddress6);
                    stuff = new string[] { strAddress1, strAddress2, strAddress3, strAddress4, strAddress5, strAddress6 };
                    stuff = stuff.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    Address = string.Join("\n", stuff);
                    Name = strName;
                    //MessageBox.Show(text: Name);
                    //MessageBox.Show(text: Address);
                    //MessageBox.Show(text: strReturn);
                    //MessageBox.Show(text: strAddress1 + '\n\r' + strAddress1);
                    if (ReturnCodeLoc != 0)
                    {
                        ReturnCode = ReturnCodeLoc;
                        ReturnMsg = ReturnMsgLoc;
                        //MessageBox.Show(text: ReturnMsgLoc);
                        //MessageBox.Show(text: ReturnCodeLoc.ToString());
                    }
                    else
                    {
                        IsValid = true;                        
                        ReturnCode = ReturnCodeLoc;
                        ReturnMsg = ReturnMsgLoc;
                    }
                    logout();
                    return (IsValid);
                }
                catch (Exception err)
                {
                    System.Diagnostics.Trace.TraceError(err.ToString());
                    return (false);
                }
            }
            else
            {
                string strVat = VATNumber;
                string strCountry = CountryCode;

                bool bValid = false;
                string strName = string.Empty;
                string strAddress = string.Empty;
                VatService = "EU VIES";
                try
                {
                    checkVatService visService = new checkVatService();

                    RetDate = visService.checkVat(ref strCountry, ref strVat,
                                                  out bValid, out strName, out strAddress);
                    IsValid = bValid;
                    Name = strName;
                    Address = strAddress;
                    if (IsValid)
                    {
                        ReturnCode = 0;
                        ReturnMsg = "Die UID des Erwerbers ist gültig";
                    }
                    else
                    {
                        ReturnCode = -1;
                        ReturnMsg = "Die UID-Nummer des Erwerbers ist falsch";
                    }
                    
                    return (IsValid);
                }
                catch (Exception err)
                {
                    System.Diagnostics.Trace.TraceError(err.ToString());
                    ReturnCode = -1;
                    ReturnMsg = err.ToString();
                    return (false);
                }
            }
        }

        public int logout()
        {
            string MemberidLoc = Memberid;
            string UserIdLoc = UserId;
            string ReturnMsgLoc = string.Empty;
            int ReturnCodeLoc = 0;
            try
            {
                sessionService sessionService = new sessionService();
                ReturnCodeLoc = sessionService.logout(MemberidLoc, UserIdLoc, out ReturnMsgLoc);
            }
            catch (Exception err)
            {
                System.Diagnostics.Trace.TraceError(err.ToString());
            }

            return (ReturnCodeLoc);
        }
    }
}