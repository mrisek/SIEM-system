using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";


            SqlConnection con = new SqlConnection(WebConfigurationManager.AppSettings["SqlDatabaseConnectionString"]);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "insert Logs values(12, 'neki_tip', 'neki_source')";
            con.Open();
            cmd.ExecuteNonQuery();

            cmd.CommandText = "select * from Logs";
            string text = cmd.ExecuteNonQuery().ToString();

            con.Close();

            


            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult UnosPodataka(Dashboard.Models.Logs model) 
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.AppSettings["SqlDatabaseConnectionString"]);

            SqlCommand cmd = new SqlCommand();

            cmd.Connection = con;

            //cmd2.CommandText = "select KorisnikID from korisnik where Ime = '" + User.Identity.GetUserName() + "'";

            //insert Korisnik values((select KorisnikID from korisnik where Ime = 'mrisek'),        'fg','fg', 1);

            cmd.CommandText = "insert Logs values(12, 'neki_tip', 'neki_source')";

            //cmd.CommandText = "insert Dnevnica values((select KorisnikID from korisnik where KorisnickoIme = '"
            //    + User.Identity.GetUserName() + "'), '"
            //    + model.Odrediste + "', '"
            //    + model.Polaziste + "', '"
            //    + model.VrijemePolaska + "', '"
            //    + model.VrijemeDolaska + "');";

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            return View();
        }

    }
}