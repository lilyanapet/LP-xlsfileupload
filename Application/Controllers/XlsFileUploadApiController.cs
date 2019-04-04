using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.WebApi;
using Umbraco.Core;
using System.Linq;
using System.IO;
using System.Collections.Specialized;
using System.Collections.Generic;
using ExcelDataReader;
using System.Data;
using App_Plugins.XlsFileUpload.Application.Models;
using System;
using log4net;
using App_Plugins.XlsFileUpload.Application.Services;

namespace App_Plugins.XlsFileUpload.Application.Controllers
{
    public class XlsFileUploadApiController : UmbracoApiController
    {
        private readonly DbService dbService;
        private static readonly ILog log = LogManager.GetLogger("fileUploadLog");
        public XlsFileUploadApiController()
        {
            this.dbService = new DbService(UmbracoContext.Application.DatabaseContext.Database, UmbracoContext.Application.ApplicationCache.RuntimeCache);
        }
        // POST: XlsFileUploadApi
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<HttpResponseMessage> SaveData()
        {
            Umbraco.Core.Services.IContentService _contentService = ApplicationContext.Current.Services.ContentService;
            UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var httpCurrentUser = HttpContext.Current.Request.Form.AllKeys.Count() > 0 ?
                HttpContext.Current.Request.Form["CurrentUserId"] : "0";
            var httpCurrentUserName = HttpContext.Current.Request.Form.AllKeys.Count() > 0 ?
                HttpContext.Current.Request.Form["CurrentUserName"] : "admin";

            var httpPostedFile = HttpContext.Current.Request.Files["UploadedFile"];
            if (httpPostedFile != null)
            {
                int lastPos = httpPostedFile.FileName.LastIndexOf('\\');
                string[] fileName = httpPostedFile.FileName.Substring(++lastPos).Split(new char[] { '.' });
                string name = fileName[0];
                string extension = fileName[1];

                if (extension != "xls" && extension != "xlsx")
                {
                    log.Info(string.Format("The Provided File is not an xls file."));
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                                    "The Provided File is not an xls file.",
                                    "application/json");
                }

                // Get the complete file path
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/UploadedFiles"), httpPostedFile.FileName);
                // Save the uploaded file to "UploadedFiles" folder
                if (!System.IO.File.Exists(fileSavePath))
                {
                    httpPostedFile.SaveAs(fileSavePath);
                }
                FileStream stream = File.Open(fileSavePath, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                DataSet result = excelReader.AsDataSet();
                excelReader.Close();
                int row_no = 1;
                //delete all old Company records
                var deletedRecords = dbService.DeleteOldCompanyInvetoryItems();
                if (deletedRecords == -1)
                {
                    log.Info(string.Format("There is a problem deleting the records"));
                }
                else
                {
                    log.Info(string.Format("The number of deleted records is {0}", deletedRecords));
                }

                var saveAuditImportReport = dbService.SaveCompanyInvetoryAuditItem(new CompanyInvetoryAudit
                {
                    ImportedDate = DateTime.Now.ToString(),
                    UserId = int.Parse(httpCurrentUser),
                    UserName = httpCurrentUserName
                });


                var recordsCount = 0;
                var allRecordsCount = 0;
                if (saveAuditImportReport != -1)
                {
                    while (row_no < result.Tables[0].Rows.Count)
                    {
                        #region date convertion
                        var _dateOfIncorporation = string.Empty;
                        if (!string.IsNullOrEmpty(result.Tables[0].Rows[row_no][6].ToString()))
                        {
                            double DateOfIncorporation;
                            if (double.TryParse(result.Tables[0].Rows[row_no][6].ToString(), out DateOfIncorporation))
                            {
                                _dateOfIncorporation = string.Format("{0}/{1}/{2}", DateTime.FromOADate(DateOfIncorporation).Day, DateTime.FromOADate(DateOfIncorporation).Month, DateTime.FromOADate(DateOfIncorporation).Year);
                            }
                        }

                        var _month = string.Empty;
                        if (!string.IsNullOrEmpty(result.Tables[0].Rows[row_no][7].ToString()))
                        {
                            double Month;
                            if (double.TryParse(result.Tables[0].Rows[row_no][7].ToString(), out Month))
                            {
                                _month = string.Format("{0}/{1}/{2}", DateTime.FromOADate(Month).Day, DateTime.FromOADate(Month).Month, DateTime.FromOADate(Month).Year);
                            }
                        }

                        var _potentialStrikeOffDate = string.Empty;
                        if (!string.IsNullOrEmpty(result.Tables[0].Rows[row_no][9].ToString()))
                        {
                            double PotentialStrikeOffDate;
                            if (double.TryParse(result.Tables[0].Rows[row_no][9].ToString(), out PotentialStrikeOffDate))
                            {
                                _potentialStrikeOffDate = string.Format("{0}/{1}/{2}", DateTime.FromOADate(PotentialStrikeOffDate).Day, DateTime.FromOADate(PotentialStrikeOffDate).Month, DateTime.FromOADate(PotentialStrikeOffDate).Year);
                            }
                        }

                        var _reservedDate = string.Empty;
                        if (!string.IsNullOrEmpty(result.Tables[0].Rows[row_no][10].ToString()))
                        {
                            double ReservedDate;
                            if (double.TryParse(result.Tables[0].Rows[row_no][10].ToString(), out ReservedDate))
                            {
                                _reservedDate = string.Format("{0}/{1}/{2}", DateTime.FromOADate(ReservedDate).Day, DateTime.FromOADate(ReservedDate).Month, DateTime.FromOADate(ReservedDate).Year);
                            }
                        }

                        var _soldDate = string.Empty;
                        if (!string.IsNullOrEmpty(result.Tables[0].Rows[row_no][12].ToString()))
                        {
                            double SoldDate;
                            if (double.TryParse(result.Tables[0].Rows[row_no][12].ToString(), out SoldDate))
                            {
                                _soldDate = string.Format("{0}/{1}/{2}", DateTime.FromOADate(SoldDate).Day, DateTime.FromOADate(SoldDate).Month, DateTime.FromOADate(SoldDate).Year);
                            }
                        }

                        var _companyStruckOffDate = string.Empty;
                        if (!string.IsNullOrEmpty(result.Tables[0].Rows[row_no][14].ToString()))
                        {
                            double CompanyStruckOffDate;
                            if (double.TryParse(result.Tables[0].Rows[row_no][14].ToString(), out CompanyStruckOffDate))
                            {
                                _companyStruckOffDate = string.Format("{0}/{1}/{2}", DateTime.FromOADate(CompanyStruckOffDate).Day, DateTime.FromOADate(CompanyStruckOffDate).Month, DateTime.FromOADate(CompanyStruckOffDate).Year);
                            }
                        }
                        #endregion date convertion

                        var inventoryItem = new CompanyInventory();
                        inventoryItem.CompanyClient = result.Tables[0].Rows[row_no][0].ToString();
                        inventoryItem.CompanyName = result.Tables[0].Rows[row_no][1].ToString();
                        inventoryItem.IsOn = result.Tables[0].Rows[row_no][2].ToString();
                        inventoryItem.BC = result.Tables[0].Rows[row_no][3].ToString();
                        inventoryItem.Availability = result.Tables[0].Rows[row_no][4].ToString();
                        inventoryItem.CorporationStatus = result.Tables[0].Rows[row_no][5].ToString();
                        inventoryItem.DateOfIncorporation = _dateOfIncorporation;
                        inventoryItem.Month = _month;
                        inventoryItem.FirstOrSecondHalf = !string.IsNullOrEmpty(result.Tables[0].Rows[row_no][8].ToString()) ? int.Parse(result.Tables[0].Rows[row_no][8].ToString()) : 1;
                        inventoryItem.PotentialStrikeOffDate = _potentialStrikeOffDate;
                        inventoryItem.ReservedDate = _reservedDate;
                        inventoryItem.ReservedBy = result.Tables[0].Rows[row_no][11].ToString();
                        inventoryItem.SoldDate = _soldDate;
                        inventoryItem.SoldBy = result.Tables[0].Rows[row_no][13].ToString();
                        inventoryItem.CompanyStruckOffDate = _companyStruckOffDate;
                        inventoryItem.Note = result.Tables[0].Rows[row_no][15].ToString();
                        inventoryItem.AuditId = saveAuditImportReport;
                        var saveShelfCompany = -1;
                        try
                        {
                            saveShelfCompany = dbService.SaveInventoryItem(inventoryItem);
                        }
                        catch (Exception ex)
                        {
                            log.Info(ex.Message);
                        }
                        recordsCount++;
                        allRecordsCount++;
                        if (saveShelfCompany == -1)
                        {
                            log.Info(string.Format("The Company {0} was not added to the DB", inventoryItem.CompanyName));
                            recordsCount--;
                        }
                        row_no++;
                    }
                    var updateAuditWithRecordsCount = dbService.UpdateCompanyInvetoryAuditItem(saveAuditImportReport, recordsCount);
                    if (!updateAuditWithRecordsCount)
                    {
                        log.Info(string.Format("The Audit with Id={0} was not updated with the Company records count.", saveAuditImportReport));
                    }
                }
                else
                {
                    log.Info(string.Format("The Audit table was not updated with the information with the upload"));
                }
                log.Info(string.Format("Company Record imported {1}, Total Company Records {0}", allRecordsCount, recordsCount));

                //close the file after all is read
                stream.Close();
                return Request.CreateResponse(HttpStatusCode.OK,
                                  string.Format("Successfully uploaded {0} records from {1} rows!", recordsCount, allRecordsCount),
                                  "application/json");
            }
            else
            {
                log.Info(string.Format("No File was posted!"));
                return Request.CreateResponse(HttpStatusCode.NotFound,
                  "There is no file posted!",
                  "application/json");
            }


        }
    }
}
