using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WindowsFormsApp1
{
    public class Wrkw
    {


        public Model getName(String crn)
        {
            string html = string.Empty;
            string url = string.Format("http://www.ftc.go.kr/bizCommPop.do?wrkr_no={0}", crn);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            return parseHtml(html);
        }

        private Model parseHtml(string html)
        {
            Model model = new Model();
            try
            {
                var suppliesPage = new HtmlAgilityPack.HtmlDocument();
                suppliesPage.LoadHtml(html);
                var name = suppliesPage.DocumentNode.SelectNodes("/html[1]/body[1]/table[1]/tbody[1]/tr[4]/td")[0].InnerHtml;
                var cname = suppliesPage.DocumentNode.SelectNodes("/html[1]/body[1]/table[1]/tbody[1]/tr[3]/td")[0].InnerHtml;
                var addr = suppliesPage.DocumentNode.SelectNodes("/html[1]/body[1]/table[1]/tbody[1]/tr[8]/td")[0].InnerHtml.Replace("              ", "").Replace("\r\n", "").Replace("                  ", "");
                model.addr = addr;
                model.name = name;
                model.cname = cname;
            }
            catch (Exception ex)
            {
                model.addr = "정보가 없습니다.";
                model.name = "정보가 없습니다.";
                model.cname = "정보가 없습니다.";
            }
            return model;
        }
    }
}
