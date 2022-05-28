using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    [Table("tbTransaction")]
    public class tbTransaction
    {
        [Key]
        public Guid ID { get; set; }
        public string TransactionIdentificator { get; set; }
        public decimal? Amount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string Status { get; set; }
        public DateTime? AccessTime { get; set; }
        public string FileExtension { get; set; }


    }
}
