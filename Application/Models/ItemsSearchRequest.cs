using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App_Plugins.XlsFileUpload.Application.Models
{
    public class ItemsSearchRequest
    {
        public ItemsSearchRequest()
        {
            this.ItemsPerPage = 50;
            this.PageNumber = 1;
            this.SortColumn = "Id";
            this.SortOrder = "asc";
        }

        
        public int ItemsPerPage { get; set; }

        public int PageNumber { get; set; }

        public string SortColumn { get; set; }

        public string SortOrder { get; set; }
        
    }
}
