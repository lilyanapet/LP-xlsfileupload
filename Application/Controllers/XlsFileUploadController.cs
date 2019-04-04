using App_Plugins.XlsFileUpload.Application.Models;
using App_Plugins.XlsFileUpload.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using Umbraco.Web.WebApi.Filters;

namespace App_Plugins.XlsFileUpload.Application.Controllers
{    
    /// <summary>
     /// Controller for returning log data to authenticated developers
     /// </summary>
    [UmbracoApplicationAuthorize("xlsFileUpload")]
    [PluginController("xlsFileUpload")]
    [IsBackOffice]
    public class XlsFileUploadController : UmbracoApiController
    {
        private readonly DbService dbService;

        /// <summary>
        /// Instantiate a new audit log controller and configure the log service with the Umbraco database
        /// </summary>
        public XlsFileUploadController()
        {
            this.dbService = new DbService(UmbracoContext.Application.DatabaseContext.Database, UmbracoContext.Application.ApplicationCache.RuntimeCache);
        }


        /// <summary>
        /// Gets a list of all inventories
        /// </summary>
        /// <param name="itemsPerPage">A number of items to show on a page</param>
        /// <param name="pageNumber">Current Page Number</param>
        /// <param name="sortColumn">The name of the column to sort on</param>
        /// <param name="sortOrder">The type of sort order</param>
        /// <returns>a list of inventories</returns>
        public PagedResult GetInventoryList(int itemsPerPage = 50, int pageNumber = 1, string sortColumn = "sci.CompanyName", string sortOrder = "asc")
        {
            var request = new ItemsSearchRequest()
            {
                ItemsPerPage = itemsPerPage,
                PageNumber = pageNumber,
                SortColumn = sortColumn,
                SortOrder = sortOrder
            };

            var paged = this.dbService.GetInventoryList(request);

            PagedResult result = new PagedResult()
            {
                CurrentPage = paged.CurrentPage,
                ItemsPerPage = paged.ItemsPerPage,
                CompanyInventoryItems = paged.Items,
                TotalItems = paged.TotalItems,
                TotalPages = paged.TotalPages
            };

            return result;

        }

        /// <summary>
        /// Gets a list of all imports
        /// </summary>
        /// <param name="itemsPerPage">A number of items to show on a page</param>
        /// <param name="pageNumber">Current Page Number</param>
        /// <param name="sortColumn">The name of the column to sort on</param>
        /// <param name="sortOrder">The type of sort order</param>
        /// <returns>A list of audits</returns>
		public PagedResult GetFileImportDateList(int itemsPerPage = 50, int pageNumber = 1, string sortColumn = "scia.Id", string sortOrder = "asc")
        {
            var request = new ItemsSearchRequest()
            {
                ItemsPerPage = itemsPerPage,
                PageNumber = pageNumber,
                SortColumn = sortColumn,
                SortOrder = sortOrder
            };

            var paged = this.dbService.GetFileImportDateList(request);

            PagedResult result = new PagedResult()
            {
                CurrentPage = paged.CurrentPage,
                ItemsPerPage = paged.ItemsPerPage,
                FileImportAuditItems = paged.Items,
                TotalItems = paged.TotalItems,
                TotalPages = paged.TotalPages
            };

            return result;
        }

    }
}
