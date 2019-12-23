using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kniznica;


namespace kajo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<SongItem> temp = Hudba.GetSongTitlesByArtist();
            return View(temp);
        }

        public ActionResult Save()
        {
            if (Hudba.InsertIntoDB(Hudba.GetSongTitlesByArtist()))
            {
                ViewBag.ResultState = "successful";
            }
            else
            {
                ViewBag.ResultState = "failed";
            }
            return View();
        }

        public ActionResult DeleteAll()
        {
            Hudba.DeleteDBtable();
            return View();
        }

        public ActionResult SearchArtist(string searchString)
        {
            var hits = Hudba.Search(searchString);
            return View("Index",hits);
        }
    }
}