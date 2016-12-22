using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.Controllers
{
    public class ConfigurationController : Controller
    {
        // GET: Configuration
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UnosPodataka(Dashboard.Models.Configuration model)
        {
            // MySql connection string
            MySqlConnection con = new MySqlConnection("server=localhost;user id=root;persistsecurityinfo=True;database=siem_sql_database");
            // Kreiram novu MySql naredbu
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = con;

            // konfiguracija pravila (rules)
            cmd.CommandText = "INSERT INTO rules VALUES('" 
                + model.Name + "', '"
                + model.MinLevel + "', '"
                + model.WriteTo + "', '', '" 
                + model.RuleName + "', '')";

            // otvori konekciju
            con.Open();
            // izvrši prvu naredbu
            cmd.ExecuteNonQuery();

            // konfiguracija odredišta (targets)
            cmd.CommandText = "INSERT INTO targets VALUES('" 
                + model.Type + "', '" 
                + model.Name + "', '" 
                + model.Address + "', '" 
                + model.FileName + "', '" 
                + model.Layout 
                + "', '', '', '', '', '', '', '" 
                + model.TargetName + "', '')";

            // izvrši drugu naredbu
            cmd.ExecuteNonQuery();

            // zatvori konekciju na bazu
            con.Close();

            // preusmjeri na idući prikaz
            return View();
        }
    }
}