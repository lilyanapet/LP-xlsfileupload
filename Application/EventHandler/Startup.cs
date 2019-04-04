using App_Plugins.XlsFileUpload.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace App_Plugins.XlsFileUpload.Application.EventHandler
{
    public class Startup : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            DatabaseContext ctx = ApplicationContext.Current.DatabaseContext;
            DatabaseSchemaHelper dbSchema = new DatabaseSchemaHelper(ctx.Database, ApplicationContext.Current.ProfilingLogger.Logger, ctx.SqlSyntax);


            if (!dbSchema.TableExist("CompanyInvetoryAudit")) dbSchema.CreateTable<CompanyInvetoryAudit>(false);

            if (!dbSchema.TableExist("CompanyInventory")) dbSchema.CreateTable<CompanyInventory>(false);

        }
    }
}
