using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Storage.WebApi.Controllers
{
    public class ProductsInfoController : Controller
    {
        // GET: EmployeeInfo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddNewProduct()
        {
            return PartialView("AddProduct");
        }
        public ActionResult ShowProducts()
        {
            return PartialView("ShowProducts");
        }

        public ActionResult EditProduct()
        {
            return PartialView("EditProduct");
        }

        public ActionResult DeleteProduct()
        {
            return PartialView("DeleteProduct");
        }

        public ActionResult DetailsProduct()
        {
            return PartialView("DetailsProduct");
        }
    }
}