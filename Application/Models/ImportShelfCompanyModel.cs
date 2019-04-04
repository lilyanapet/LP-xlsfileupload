using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App_Plugins.XlsFileUpload.Application.Models
{
    public class ImportShelfCompanyModel
    {
        public string CompanyName { get; set; }
        public string Availability { get; set; }
        public string Incorporation { get; set; }
        public string Reserved { get; set; }
        public string Sold { get; set; }
        public string StruckOff { get; set; }
    }

    public class FileImportAuditModel
    {
        public string ImportDate { get; set; }
        public string User { get; set; }
        public int RecordsCount { get; set; }
    }
}
