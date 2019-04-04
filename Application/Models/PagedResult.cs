using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App_Plugins.XlsFileUpload.Application.Models
{
    public class PagedResult
    {
        public List<CompanyInventory> CompanyInventoryItems { get; set; }

        public List<CompanyInvetoryAudit> FileImportAuditItems { get; set; }

        public long CurrentPage { get; set; }

        public long ItemsPerPage { get; set; }

        public long TotalPages { get; set; }

        public long TotalItems { get; set; }
    }
}
