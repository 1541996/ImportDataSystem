using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ViewModel
{
    public class CSVViewModel
    {
        public string No { get; set; }
        public string TransactionIdentificator { get; set; }
        public string Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string TransactionDate { get; set; }
        public string Status { get; set; }
       
    }

    public class CSVRequestModel
    {
        public List<CSVViewModel> Models { get; set; }
        public string FileExtension { get; set; }
    }

    public class ResponseViewModel
    {
        public string ReturnStatus { get; set; }
        public string ReturnMessage { get; set; }
        public List<string> AdditionalDatas { get; set; }
    }

    public class TransactionViewModel
    {
        public string id { get; set; }
        public string payment { get; set; }
        public string Status { get; set; }
    }
}
