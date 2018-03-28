using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Xml.Linq;
using Kasa.Models;

namespace Kasa.Controllers
{
    public class HomeController : Controller
    {        
        static string pass = ""; 
        public ActionResult Index()
        {
            try
            {                
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                throw;
            }

        }
        public ActionResult Go(string Haslo)
        {
            try
            {                
                if (!String.IsNullOrEmpty(Haslo))
                {
                    pass = Haslo;
                }
                if (pass == "blabla")
                {
                    Xml.LoadModel();
                    return View(Xml.Model);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                throw;
            }
        }
        public ActionResult Add(Person person)
        {
            try
            {
                Xml.Add(person);               
                return RedirectToAction("Go");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                throw;
            }
        }
    }
}