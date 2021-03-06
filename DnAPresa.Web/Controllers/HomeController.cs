﻿using System;
using System.Web.Mvc;
using DnAPresa.Web.Models;
using DnAPresa.Common.Utilities;
using DnAPresa.Data.Processors;
using System.IO;
using DnAPresa.Common.Models;

namespace DnAPresa.Web.Controllers
{
    public class HomeController : Controller
    {

        #region Index

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Filter()
        {
            EmployeeModel viewModel = new EmployeeModel();

            try
            {
                // Get Dictionaries for DropDownLists
                ViewBag.EmpClassList = Utilities.Get_EmpClassList();
                ViewBag.EmpTerminalList = Utilities.Get_EmpTerminalList();
            }
            catch (Exception ex)
            {
                // TODO: return processor response/error handling
                Console.WriteLine(ex);
            }

            // Convert partial view to return as Json
            string viewContent = ConvertViewToString("_Filter", viewModel);
            return Json(new { PartialView = viewContent });
        }

        public ActionResult Get_FilteredEmployees(EmployeeModel model)
        {
            EmployeeListModel viewModel = new EmployeeListModel();

            try
            {
                // Convert to Data Model
                Common.Models.Employee mData = new Common.Models.Employee
                {
                    Tier3 = model.Tier3,
                    Terminal = model.Terminal,
                    DrugPool = model.DrugPool,
                    AlcPool = model.AlcPool
                };

                // Go get our filtered list of employees
                var mResponse = EmployeeProcessor.Get_FilteredEmployees(mData);

                // Assignment to view model
                foreach (var emp in mResponse.Data)
                {
                    viewModel.Employees.Add(new EmployeeModel
                    {
                        EmployeeNumber = emp.EmployeeNumber,
                        LastName = emp.LastName,
                        FirstName = emp.FirstName,
                        Tier3 = emp.Tier3,
                        Terminal = emp.Terminal,
                        Drug = emp.Drug,
                        Alcohol = emp.Alcohol,
                        Substitute = emp.Substitute
                    });
                }

                // Convert partial view to return as Json
                string viewContent = ConvertViewToString("_PrintPreview", viewModel);
                return Json(new { PartialView = viewContent, Success = true }); // TODO: do not return here return updated dto json
            }
            catch (Exception ex)
            {
                return Json(new { ex.Message });
            }
        }

        public ActionResult Insert_DnAHistory(EmployeeListModel model)
        {
            JsonResult json = new JsonResult { Data = false };
            Common.Models.EmployeeList mData = new Common.Models.EmployeeList();
            try
            {
                // Convert to Data Model
                foreach (var emp in model.Employees)
                {
                    mData.Employees.Add(new Common.Models.EmployeeHistory
                    {
                        EmployID = emp.EmployeeNumber,
                        lastname = emp.LastName,
                        frstname = emp.FirstName,
                        emplclas = emp.Terminal,
                        testsel = emp.Get_TestSelection(emp.Drug, emp.Alcohol, emp.Substitute),
                        Report_DateTime = DateTime.Now
                    });
                }

                // Post the list of employees into history table
                var mResponse = EmployeeProcessor.Insert_DnAHistory(mData);

                json.Data = mResponse.Success;
            }
            catch (Exception ex)
            {
                return Json(new { ex.Message });
            }

            return json;
        }

        #endregion

        #region History

        public ActionResult History()
        {
            return View();
        }

        public ActionResult _HistoryFilter()
        {
            EmployeeModel viewModel = new EmployeeModel();

            try
            {
                // Get Dictionaries for DropDownLists
                ViewBag.EmpClassList = Utilities.Get_EmpClassList();
                ViewBag.EmpTerminalList = Utilities.Get_EmpTerminalList();
            }
            catch (Exception ex)
            {
                // TODO: return processor response/error handling
                Console.WriteLine(ex);
            }

            // Convert partial view to return as Json
            string viewContent = ConvertViewToString("_HistoryFilter", viewModel);
            return Json(new { PartialView = viewContent });
        }

        public ActionResult Get_FilteredHistory(EmployeeHistoryModel model)
        {
            EmployeeHistoryListModel viewModel = new EmployeeHistoryListModel();

            try
            {
                // Convert to Data Model
                Common.Models.EmployeeHistory mData = new Common.Models.EmployeeHistory
                {
                    EmployID = model.EmployID,
                    lastname = model.lastname,
                    frstname = model.frstname,
                    midlname = model.midlname,
                    emplclas = model.emplclas,
                    db = model.db,
                    testsel = model.testsel,
                    Report_DateTime = model.Report_DateTime,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate
                };

                // Go get our filtered list of history
                var mResponse = EmployeeProcessor.Get_FilteredHistory(mData);

                // Assignment to view model
                foreach (var emp in mResponse.Data)
                {
                    viewModel.Employees.Add(new EmployeeHistoryModel
                    {
                        EmployID = emp.EmployID,
                        lastname = emp.lastname,
                        frstname = emp.frstname,
                        midlname = emp.midlname,
                        emplclas = emp.emplclas,
                        db = emp.db,
                        testsel = emp.testsel,
                        Report_DateTime = emp.Report_DateTime
                    });
                }

                // Convert partial view to return as Json
                string viewContent = ConvertViewToString("_HistoryPreview", viewModel);
                return Json(new { PartialView = viewContent });
            }
            catch (Exception ex)
            {
                return Json(new { ex.Message });
            }
        }

        #endregion

        #region Helpers

        private string ConvertViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (StringWriter writer = new StringWriter())
            {
                ViewEngineResult vResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext vContext = new ViewContext(this.ControllerContext, vResult.View, ViewData, new TempDataDictionary(), writer);
                vResult.View.Render(vContext, writer);
                return writer.ToString();
            }
        }

        #endregion
    }
}