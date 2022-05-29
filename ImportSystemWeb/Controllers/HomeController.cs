using CsvHelper;
using Data;
using ImportSystemWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data.ViewModel;
using Infra.Service;
using Data.Models;
using Infra.Helper;
using System.Xml.Linq;
using Extensions;

namespace ImportSystemWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        [Obsolete]
        private IHostingEnvironment Environment;

        [Obsolete]
        public HomeController(ILogger<HomeController> logger, IHostingEnvironment _environment)
        {
            _logger = logger;
            Environment = _environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Import()
        {
            return View();
        }

        public async Task<ActionResult> List(int pageSize = 10, int page = 1, string currency = null, DateTime? fromdate = null, DateTime? todate = null, string status = null)
        {
            PagedListClient<tbTransaction> result = await TransactionApiRequestHelper.List(pageSize, page, currency, fromdate, todate, status);
            ViewBag.page = page;
            ViewBag.pageSize = pageSize;
            return PartialView("_list", result);
        }

        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> Upload(IFormFile postedFile)
        {
            var fileextension = "";
            var IsSave = true;
            ResponseViewModel res = new ResponseViewModel();
            List<string> resList = new List<string>();
            List<CSVViewModel> saveDataList = new List<CSVViewModel>();

            string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = Path.GetFileName(postedFile.FileName);
            string filePath = Path.Combine(path, fileName);
            fileextension = Path.GetExtension(filePath);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                postedFile.CopyTo(stream);
            }
            string csvData = System.IO.File.ReadAllText(filePath);

            if (fileextension == SettingConfig.CSVExtension)
            {
                #region csv import

                try
                {
                    using (var reader = new StreamReader(filePath))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {

                        var records = csv.GetRecords<CSVViewModel>();
                        records = records.ToList();
                        if (records.Count() > 0)
                        {
                            foreach (var record in records)
                            {
                                saveDataList.Add(record);

                                if (record.TransactionIdentificator != null)
                                {
                                    var CheckTextLength = MyExtension.CheckTextGreaterThanLength(50, record.TransactionIdentificator);
                                    if (CheckTextLength)
                                    {
                                        var ReturnMessage = MyExtension.GetInvalidMessage("Transaction Identificator", record.No);
                                        IsSave = false;
                                        resList.Add(ReturnMessage);
                                    }


                                }

                                if (record.Amount != null)
                                {
                                    var CheckAmountIsDecimal = MyExtension.CheckAmountIsDecimal(record.Amount);
                                    if (!CheckAmountIsDecimal)
                                    {
                                        var ReturnMessage = MyExtension.GetInvalidMessage("Amount", record.No);
                                        IsSave = false;
                                        resList.Add(ReturnMessage);
                                    }


                                }

                                if (record.CurrencyCode != null)
                                {
                                    var CheckISOFormat = MyExtension.CheckISOFormat(record.CurrencyCode);
                                    if (!CheckISOFormat)
                                    {
                                        var ReturnMessage = MyExtension.GetInvalidMessage("Currency Code", record.No);
                                        IsSave = false;
                                        resList.Add(ReturnMessage);
                                    }
                                }

                                if (record.TransactionDate != null)
                                {
                                    var chValidity = MyExtension.CheckDateFormatValid(record.TransactionDate);
                                    if (chValidity != true)
                                    {
                                        var ReturnMessage = MyExtension.GetInvalidMessage("Transaction Date", record.No);
                                        IsSave = false;
                                        resList.Add(ReturnMessage);
                                    }

                                }


                                if (record.Status != null)
                                {
                                    var statuscheck = FixedData.StatusList(record.Status, fileextension);
                                    if (statuscheck != true)
                                    {
                                        var ReturnMessage = MyExtension.GetInvalidMessage("Status", record.No);
                                        IsSave = false;
                                        resList.Add(ReturnMessage);
                                    }
                                }
                            }

                        }
                        else
                        {
                            IsSave = false;
                            res = new ResponseViewModel()
                            {
                                ReturnStatus = SettingConfig.FailErrorCode,
                                ReturnMessage = SettingConfig.NoDataMessage,

                            };
                        }

                    }
                }
                catch
                {
                    IsSave = false;
                    res = new ResponseViewModel()
                    {
                        ReturnStatus = SettingConfig.FailErrorCode,
                        ReturnMessage = SettingConfig.NoDataMessage,

                    };
                }

            

                #endregion
            }
            else if (fileextension == SettingConfig.XMLExtension)
            {
                #region xml import
                if (postedFile.ContentType.Equals("application/xml") || postedFile.ContentType.Equals("text/xml"))
                {
                    try
                    {
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await postedFile.CopyToAsync(fileStream);
                            fileStream.Dispose();
                            XDocument xDoc = XDocument.Load(filePath);
                            var xmlList = xDoc.Descendants("Transaction").ToList();
                            if (xmlList.Count() > 0)
                            {
                                foreach (var item in xmlList)
                                {
                                    var Status = item.Element("Status").Value;
                                    var TransactionIdentificator = item.Attribute("id").Value;
                                    var Amount = item.Descendants("PaymentDetails").Descendants("Amount").FirstOrDefault()?.Value;
                                    var CurrencyCode = item.Descendants("PaymentDetails").Descendants("CurrencyCode").FirstOrDefault()?.Value;
                                    var TransactionDate = item.Element("TransactionDate")?.Value;
                                    TransactionDate = TransactionDate.Replace("T", " ");
                                    //TransactionDate = TransactionDate.Replace("-", "/");
                                    //TransactionDate = TransactionDate != null ? Convert.ToDateTime(TransactionDate).ToString() : null;

                                    if (TransactionIdentificator != null)
                                    {
                                        var CheckTextLength = MyExtension.CheckTextGreaterThanLength(50, TransactionIdentificator);
                                        if (CheckTextLength)
                                        {
                                            var ReturnMessage = MyExtension.GetInvalidMessage("Transaction Identificator", TransactionIdentificator);
                                            IsSave = false;
                                            resList.Add(ReturnMessage);
                                        }


                                    }

                                    if (Amount != null)
                                    {
                                        var CheckAmountIsDecimal = MyExtension.CheckAmountIsDecimal(Amount);
                                        if (!CheckAmountIsDecimal)
                                        {
                                            var ReturnMessage = MyExtension.GetInvalidMessage("Amount", TransactionIdentificator);
                                            IsSave = false;
                                            resList.Add(ReturnMessage);
                                        }


                                    }

                                    if (CurrencyCode != null)
                                    {
                                        var CheckISOFormat = MyExtension.CheckISOFormat(CurrencyCode);
                                        if (!CheckISOFormat)
                                        {
                                            var ReturnMessage = MyExtension.GetInvalidMessage("Currency Code", CurrencyCode);
                                            IsSave = false;
                                            resList.Add(ReturnMessage);
                                        }
                                    }

                                    if (TransactionDate != null)
                                    {
                                        //  TransactionDate = DateTime.ParseExact(TransactionDate, "yyyy/MM/dd hh:mm:ss", CultureInfo.InvariantCulture).ToString();
                                        var chValidity = MyExtension.CheckXMLDateFormatValid(TransactionDate);
                                        if (chValidity != true)
                                        {
                                            var ReturnMessage = MyExtension.GetInvalidMessage("Transaction Date", TransactionIdentificator);
                                            IsSave = false;
                                            resList.Add(ReturnMessage);
                                        }
                                        else
                                        {
                                            TransactionDate = Convert.ToDateTime(TransactionDate).ToString("dd/MM/yyyy hh:mm:ss");
                                        }

                                    }


                                    if (Status != null)
                                    {
                                        var statuscheck = FixedData.StatusList(Status, fileextension);
                                        if (statuscheck != true)
                                        {
                                            var ReturnMessage = MyExtension.GetInvalidMessage("Status", TransactionIdentificator);
                                            IsSave = false;
                                            resList.Add(ReturnMessage);
                                        }
                                    }


                                    CSVViewModel obj = new CSVViewModel();
                                    obj.TransactionIdentificator = TransactionIdentificator;
                                    obj.Status = Status;
                                    obj.Amount = Amount;
                                    obj.CurrencyCode = CurrencyCode;
                                    obj.TransactionDate = TransactionDate;
                                    saveDataList.Add(obj);

                                }
                            }
                            else
                            {
                                IsSave = false;
                                res = new ResponseViewModel()
                                {
                                    ReturnStatus = SettingConfig.FailErrorCode,
                                    ReturnMessage = SettingConfig.NoDataMessage,

                                };
                            }

                        }
                    }
                    catch (Exception)
                    {
                        IsSave = false;
                        res = new ResponseViewModel()
                        {
                            ReturnStatus = SettingConfig.FailErrorCode,
                            ReturnMessage = SettingConfig.XMLFail,

                        };
                    }
                }
                else
                {
                    IsSave = false;
                    res = new ResponseViewModel()
                    {
                        ReturnStatus = SettingConfig.FailErrorCode,
                        ReturnMessage = SettingConfig.UnknownFormat,

                    };
                }
                #endregion
            }
            else
            {
                IsSave = false;
                res = new ResponseViewModel()
                {
                    ReturnStatus = SettingConfig.FailErrorCode,
                    ReturnMessage = SettingConfig.UnknownFormat,

                };
            }

            #region data save
                if (IsSave)
                {
                    CSVRequestModel model = new CSVRequestModel()
                    {
                        Models = saveDataList,
                        FileExtension = fileextension
                    };
                    var result = await TransactionApiRequestHelper.Save(model);
                    return Ok(result);
                }
                else if(resList.Count() > 0)
                {
                    res = new ResponseViewModel()
                    {
                        ReturnStatus = SettingConfig.BadErrorCode,
                        ReturnMessage = "",
                        AdditionalDatas = resList,
                    };
                }

            #endregion


            return Json(res);

        }

      

    }
}
