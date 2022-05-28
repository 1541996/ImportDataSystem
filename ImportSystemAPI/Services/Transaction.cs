using Data.Models;
using Data.ViewModel;
using Extensions;
using Infra.Service;
using Infra.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ImportSystemAPI.Services
{
    public interface ITransaction
    {
        PagedListClient<tbTransaction> GetList(int pageSize = 10, int page = 1);
        ResponseViewModel Save(CSVRequestModel objs);

    }
    public abstract class Transaction : ITransaction
    {      
        public abstract PagedListClient<tbTransaction> GetList(int pageSize = 10, int page = 1);
        public abstract ResponseViewModel Save(CSVRequestModel objs);
    }

    public class TransactionBase : Transaction
    {
        
        private readonly UnitOfWork uow;
        public TransactionBase(UnitOfWork uow)
        {
            this.uow = uow;
          
        }

        public override PagedListClient<tbTransaction> GetList(int pageSize = 10, int page = 1)
        {
            var objs = uow.transactionRepo.GetAll().OrderByDescending(a => a.AccessTime);    
            var result = PagingService<tbTransaction>.getPaging(page, pageSize, objs);
            PagedListClient<tbTransaction> model = PagingService<tbTransaction>.Convert(page, pageSize, result);

            return model;
        }


        public override ResponseViewModel Save(CSVRequestModel objs)
        {
            ResponseViewModel res = new ResponseViewModel();
            if (objs.Models.Count() > 0)
            {
                try
                {
                    foreach (var item in objs.Models)
                    {
                        var checkInvoice = uow.transactionRepo.GetAll().Where(a => a.TransactionIdentificator == item.TransactionIdentificator).Any();
                        if (!checkInvoice)
                        {
                            tbTransaction obj = new tbTransaction();
                            obj.ID = Guid.NewGuid();
                            obj.TransactionIdentificator = item.TransactionIdentificator;
                            obj.Amount = item.Amount != null ? Convert.ToDecimal(item.Amount) : null;
                            obj.CurrencyCode = item.CurrencyCode;
                            obj.TransactionDate = item.TransactionDate != null ? DateTime.ParseExact(item.TransactionDate, "dd/MM/yyyy hh:mm:ss", null) : null;
                            obj.Status = FixedData.GetStatus(item.Status, objs.FileExtension);
                            obj.AccessTime = MyExtension.GetLocalTime(DateTime.UtcNow);
                            obj.FileExtension = obj.FileExtension;
                            obj = uow.transactionRepo.InsertReturn(obj);
                        }                     
                    }
                    res = new ResponseViewModel()
                    {
                        ReturnMessage = "Data imported successfully.",
                        ReturnStatus = "200"
                    };
                }
                catch(Exception ex)
                {
                    res = new ResponseViewModel()
                    {
                        ReturnMessage = ex.Message,
                        ReturnStatus = "Fail"
                    };
                }
              
            }
            else
            {
                res = new ResponseViewModel()
                {
                    ReturnMessage = "No Data Found",
                    ReturnStatus = "Fail"
                };
            }

            return res;
           
        }

    }
}