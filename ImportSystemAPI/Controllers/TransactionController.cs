using Data.Models;
using Data.ViewModel;
using ImportSystemAPI.Services;
using Infra.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImportSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransaction iTransaction;
        private readonly UnitOfWork uow;
        private readonly TransactionsDBContext ctx;
        public TransactionController(TransactionsDBContext db)
        {
            ctx = db;
            uow = new UnitOfWork(ctx);
            this.iTransaction = new TransactionBase(uow);
        }

        [Route("list")]
        [HttpGet]
        public IActionResult GetList(int page = 1, int pageSize = 10)
        {
            var result = this.iTransaction.GetList(pageSize, page);
            return Ok(result);
        }

        [Route("save")]
        [HttpPost]
        public IActionResult save(CSVRequestModel objs)
        {
            var result = this.iTransaction.Save(objs);
            return Ok(result);
        }

        [Route("listbycurrency")]
        [HttpGet]
        public IActionResult listbycurrency([Required] string Currency)
        {
            var result = this.iTransaction.GetListWithoutPaging(Currency);
            return Ok(result);
        }

        [Route("listbydaterange")]
        [HttpGet]
        public IActionResult listbycurrency([Required] DateTime FromDate, [Required] DateTime ToDate)
        {
            var result = this.iTransaction.GetListWithoutPaging(FromDate: FromDate,ToDate: ToDate);
            return Ok(result);
        }

        [Route("listbystatus")]
        [HttpGet]
        public IActionResult listbystatus([Required] string Status)
        {
            var result = this.iTransaction.GetListWithoutPaging(Status: Status);
            return Ok(result);
        }
    }
}
