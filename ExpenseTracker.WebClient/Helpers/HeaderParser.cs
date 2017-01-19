﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using Newtonsoft.Json;

namespace ExpenseTracker.WebClient.Helpers
{
    public class HeaderParser
    {

        public static PagingInfo FindAndParsePagingInfo(HttpResponseHeaders responseHeaders)
        {
            // find the "X-Pagination" info in header
            if (responseHeaders.Contains("X-Pagination"))
            {
                var xPag = responseHeaders.First(ph => ph.Key == "X-Pagination").Value;

                // parse the value - this is a JSON-string.
                return JsonConvert.DeserializeObject<PagingInfo>(xPag.First());
            }
            return null;
        }
    }
}