﻿using Data.Models;
using Data.ViewModel;
using Infra.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Infra.Helper
{
    public class TransactionApiRequestHelper
    {
        public static async Task<PagedListClient<tbTransaction>> List(int pageSize = 10, int page = 1)
        {
            string url = string.Format("api/transaction/list?pageSize={0}&page={1}", pageSize, page);
            var data = await ApiRequest<PagedListServer<tbTransaction>>.GetRequest(url);

            var model = new PagedListClient<tbTransaction>();
            var pagedList = new StaticPagedList<tbTransaction>(data.Results, page, pageSize, data.TotalCount);
            model.Results = pagedList;
            model.TotalCount = data.TotalCount;
            model.TotalPages = data.TotalPages;
            return model;
        }

        public static async Task<ResponseViewModel> Save(CSVRequestModel objs)
        {
            var url = "api/transaction/save";
            var result = await ApiRequest<CSVRequestModel, ResponseViewModel>.PostDiffRequest(url, objs);
            return result;
        }

    }
}
