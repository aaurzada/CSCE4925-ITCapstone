﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace SQLSolutions.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/admin/styles")
                .Include("~/content/styles/bootstrap.css")
                .Include("~/content/styles/admin.css")
                .Include("~/content/styles/jquery-ui.css")
                .Include("~/content/PagedList.css"));

            bundles.Add(new StyleBundle("~/styles")
                .Include("~/content/styles/bootstrap.css")
                .Include("~/content/styles/Site.css")
                .Include("~/content/PagedList.css"));

            bundles.Add(new ScriptBundle("~/admin/scripts")
                .Include("~/scripts/jquery-2.1.3.js")
                .Include("~/scripts/jquery-ui-1.11.4.js")
                .Include("~/scripts/jquery.validate.js")
                .Include("~/scripts/jquery.validate.unobtrusive.js")
                .Include("~/scripts/bootstrap.js")
                .Include("~/areas/admin/scripts/forms.js")
                .Include("~/areas/admin/scripts/DatePicker.js")
                .Include("~/areas/admin/scripts/focusField.js"));
              

            bundles.Add(new ScriptBundle("~/scripts")
                .Include("~/scripts/jquery-2.1.3.js")
                .Include("~/scripts/jquery.validate.js")
                .Include("~/scripts/jquery.validate.unobtrusive.js")
                .Include("~/scripts/bootstrap.js"));
        }
    }
}