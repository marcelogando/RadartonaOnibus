using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RadartonaOnibus.Models;
using System.Linq;

namespace RadartonaOnibus.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public ActionResult Index()
        {
            string apiCredentials = Login();

            string url = "http://api.olhovivo.sptrans.com.br/v2.1/Posicao";


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            OnibusSPTrans onibus = new OnibusSPTrans();

            Cookie cookie = new Cookie("apiCredentials", apiCredentials, "/", "api.olhovivo.sptrans.com.br");
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(cookie);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                onibus = JsonConvert.DeserializeObject<OnibusSPTrans>(reader.ReadToEnd());
            }

            List<Alerta> lstAlerta = GetAlerta();

            List<Onibus> listaOnibus = new List<Onibus>();
            int i = 1;
            decimal tolerancia = 0.001M;

            foreach (LinhaOnibus linha in onibus.l)
            {
                foreach (Onibus on in linha.vs)
                {
                    Alerta teste = lstAlerta.Where(d => Math.Abs(d.lat - on.py) <= tolerancia && Math.Abs(d.lon - on.px) <= tolerancia).FirstOrDefault();
                    Alerta teste2 = lstAlerta.Where(d => Math.Abs(d.lat - on.py) <= tolerancia && Math.Abs(d.lon - on.px) <= tolerancia).LastOrDefault();

                    if (lstAlerta.Where(d => Math.Abs(d.lat - on.py) <= tolerancia && Math.Abs(d.lon - on.px) <= tolerancia).Count() > 0)
                    {
                        on.ocorrencia = true;
                    }
                    
                    on.nome = "onibus" + i.ToString(); 
                    listaOnibus.Add(on);

                    i++;
                }
            }

            listaOnibus = listaOnibus.Take(300).ToList();

            return View(listaOnibus);
        }

        private string Login()
        {
            String url = "http://api.olhovivo.sptrans.com.br/v2.1/Login/Autenticar?token=token-api-olho-vivo";

            var request = (HttpWebRequest)WebRequest.Create(url);
            var data = Encoding.ASCII.GetBytes("");

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return response.Headers[4].ToString().Replace("apiCredentials=", "").Replace("; path=/; HttpOnly", "");
        }

        public List<Alerta> GetAlerta()
        {
            string connString = "connect-string-do-banco";
            List<Alerta> lstRetorno = new List<Alerta>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand comm = conn.CreateCommand();

                comm.CommandType = CommandType.Text;
                comm.CommandText = "select type, substring(lat_lon,7,charindex(' ', lat_lon) - 7) as lon,\n" +
		                           "substring(lat_lon, charindex(' ', lat_lon) + 1,len(lat_lon) - charindex(' ', lat_lon) - 1) as lat\n" +
                                   "from waze_alerts\n" +
                                   "where cast(pubdate as date) = '2018-02-01' and type = 'ACCIDENT'";

                conn.Open();

                SqlDataReader dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    Alerta ett = new Alerta();
                    
                    ett.tipo = dr["type"].ToString();
                    ett.lon = Convert.ToDecimal(dr["lon"], CultureInfo.GetCultureInfo("en-US"));
                    ett.lat = Convert.ToDecimal(dr["lat"], CultureInfo.GetCultureInfo("en-US"));

                    lstRetorno.Add(ett);
                }
            }

            return lstRetorno;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
