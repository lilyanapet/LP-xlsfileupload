using App_Plugins.XlsFileUpload.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Persistence;

namespace App_Plugins.XlsFileUpload.Application.Services
{
    public class DbService
    {

        private readonly UmbracoDatabase db;
        private readonly IRuntimeCacheProvider cache;

        /// <summary>
        /// Instantiates the db service with the Umbraco database and a caching provider
        /// </summary>
        /// <param name="db">The Umbraco database</param>
        /// <param name="cache">A caching provider</param>
        public DbService(UmbracoDatabase db, IRuntimeCacheProvider cache)
        {
            if (db == null)
                throw new ArgumentNullException("db");

            this.db = db;
            this.cache = cache;
        }

        /// <summary>
        /// Gets a list of company inventories for the umbraco dashboard
        /// </summary>
        /// <param name="request">Page data</param>
        public Page<CompanyInventory> GetInventoryList(ItemsSearchRequest request)
        {
            const string sql = @"SELECT sci.CompanyName, 
                                sci.Availability, sci.DateOfIncorporation, sci.ReservedDate, sci.SoldDate, sci.CompanyStruckOffDate,
                                CONVERT(datetime, sci.DateOfIncorporation, 103) as Incorporation, 
                                CONVERT(datetime, sci.ReservedDate, 103) as Reserved, 
                                CONVERT(datetime, sci.SoldDate, 103) as Sold, 
                                CONVERT(datetime, sci.CompanyStruckOffDate, 103) as  StruckOffDate
                                FROM CompanyInventory sci";

            Sql query = new Sql(sql);

            if (request.SortColumn.InvariantEquals("id"))
                request.SortColumn = "sci.Id";

            var sortColumn = request.SortColumn;
            switch (request.SortColumn)
            {
                case "sci.DateOfIncorporation":
                    sortColumn = "Incorporation";
                    break;
                case "sci.ReservedDate":
                    sortColumn = "Reserved";
                    break;
                case "sci.SoldDate":
                    sortColumn = "Sold";
                    break;
                case "sci.CompanyStruckOffDate":
                    sortColumn = "StruckOffDate";
                    break;
                default:
                    sortColumn = request.SortColumn;
                    break;
            }
            query = query.Append(" ORDER BY " + sortColumn + " " + request.SortOrder);

            return db.Page<CompanyInventory>(request.PageNumber, request.ItemsPerPage, query);
        }


        /// <summary>
        /// Gets the file import audit
        /// </summary>
        /// <param name="request">Page data</param>
        public Page<CompanyInvetoryAudit> GetFileImportDateList(ItemsSearchRequest request)
        {
            const string sql = @"SELECT CONVERT(datetime, scia.ImportedDate, 103) as ImportDate, 
                                scia.RecordsCount, scia.ImportedDate,
                                scia.UserName
                                FROM CompanyInvetoryAudit scia";

            Sql query = new Sql(sql);

            if (request.SortColumn.InvariantEquals("id"))
                request.SortColumn = "scia.Id";

            var sortColumn = request.SortColumn;
            switch (request.SortColumn)
            {
                case "scia.ImportedDate":
                    sortColumn = "ImportDate";
                    break;
                default:
                    sortColumn = request.SortColumn;
                    break;
            }

            query = query.Append(" ORDER BY " + sortColumn + " " + request.SortOrder);

            return db.Page<CompanyInvetoryAudit>(request.PageNumber, request.ItemsPerPage, query);
        }

        /// <summary>
        /// Gets a list of company inventories for the company landing page
        /// </summary>
        /// <param name="number">The number of items to select from the database</param>
        public List<CompanyInventory> GetInventoryList()
        {
            const string sql = "SELECT sci.CompanyName, sci.DateOfIncorporation FROM CompanyInventory sci";
            
            return db.Fetch<CompanyInventory>(sql).ToList();
        }

        /// <summary>
        ///  Save each row from the uploaded file in the CompanyInventory db table
        /// </summary>
        /// <param name="item">The item to be saved in the CompanyInventory db table</param>
        public int SaveInventoryItem(CompanyInventory item)
        {
            try
            {
                db.Save(item);
            }
            catch (Exception ex)
            {
                return -1;
            }
            return item.Id;
        }


        /// <summary>
        /// Update in the CompanyInvetoryAuditdb table the number of records uploaded
        /// </summary>
        /// <param name="auditId">The id of the audit record to update</param>
        /// <param name="recordsCount">The number of items uploaded throught the file import</param>
        public bool UpdateCompanyInvetoryAuditItem(int auditId, int recordsCount)
        {
            var sqlQuery = string.Format("UPDATE CompanyInvetoryAudit SET RecordsCount={0} WHERE Id ={1}", recordsCount, auditId);
            try
            {
                db.Execute(new Sql(sqlQuery));
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }

        /// <summary>
        /// Save audit data for the upload in the CompanyInvetoryAudit db table
        /// </summary>
        /// <param name="number">The number of items to select from the database</param>
        public int SaveCompanyInvetoryAuditItem(CompanyInvetoryAudit item)
        {
            try
            {
                db.Save(item);
            }
            catch (Exception ex)
            {
                return -1;
            }
            return item.Id;

        }

        /// <summary>
        /// deletes inventory list on new upload
        /// </summary>
        public int DeleteOldCompanyInvetoryItems()
        {
            //get the list of items to delete from the CompanyInventory db table
            const string sql = @"SELECT *
                                FROM CompanyInventory sci
                                LEFT JOIN CompanyInvetoryAudit scia ON sci.AuditId = scia.id";
            
            //delete all items form the CompanyInventory db table
            const string sqlDelete = @"DELETE FROM CompanyInventory WHERE Id>0";


            int recordesToDelete = db.Query<int>(new Sql(sql)).Count();
            try
            {
                if (recordesToDelete > 0)
                    db.Execute(new Sql(sqlDelete));
            }
            catch (Exception ex)
            {
                return -1;
            }
            return recordesToDelete;
        }

    }
}
