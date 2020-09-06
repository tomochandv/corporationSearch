using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WindowsFormsApp1
{
    public class Hometax
    {
        private readonly String postUrl = "https://teht.hometax.go.kr/wqAction.do?actionId=ATTABZAA001R08&screenId=UTEABAAA13&popupYn=false&realScreenId=";
        private String xmlRaw = "<map id=\"ATTABZAA001R08\"><pubcUserNo/><mobYn>N</mobYn><inqrTrgtClCd>1</inqrTrgtClCd><txprDscmNo>{CRN}</txprDscmNo><dongCode>15</dongCode><psbSearch>Y</psbSearch><map id=\"userReqInfoVO\"/></map>";

        public Model postCRN(String crn)
        {
            Model model = new Model();
            byte[] contents = System.Text.Encoding.ASCII.GetBytes(xmlRaw.Replace("{CRN}", crn));
            HttpWebRequest request = createHttpWebRequest();
            setContentStream(request, contents);

            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                String resString = new StreamReader(responseStream).ReadToEnd();
                responseStream.Close();
                response.Close();
                model = getCRNresultFromXml(resString, crn);
            }
            response.Close();
            return model;
        }

        private HttpWebRequest createHttpWebRequest()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.ContentType = "text/xml; encoding='utf-8'";
            request.Method = "POST";
            return request;
        }

        private void setContentStream(HttpWebRequest request, byte[] contents)
        {
            request.ContentLength = contents.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(contents, 0, contents.Length);
            requestStream.Close();
        }

        private Model getCRNresultFromXml(String xmlData, string crn)
        {
            Wrkw wrkr = new Wrkw();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlData);
            String crnResult = xmlDocument.SelectNodes("//trtCntn").Item(0).InnerText;
            var info = wrkr.getName(crn);
            Model model = new Model();
            model.name = info.name;
            model.addr = info.addr;
            model.cnt = crn;
            model.status = crnResult;
            model.cname = info.cname;
            return model;
        }


    }
}
