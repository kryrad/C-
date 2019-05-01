using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HtmlAgilityPack;

namespace Lab01
{
    class WikiParser
    {
        public static WikiDataEntry Parse(string stream)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(stream);

            WikiDataEntry result = new WikiDataEntry()
            {
                Title = string.Empty, 
                Picture = string.Empty,
            };
            do
            {    
                String title = (from x in doc.DocumentNode.Descendants()
                                where x.Name.ToLower() == "title"
                                select x.InnerText).FirstOrDefault();

                result.Title = title;
                result.Title = result.Title.Replace(" - Wikipedia", "");

                List<String> imgs = (from x in doc.DocumentNode.Descendants()
                                      where x.Name.ToLower() == "img"
                                      select x.Attributes["src"].Value).ToList<String>();

                 result.Picture = imgs[0];

            } while ((String.IsNullOrEmpty(result.Picture)) && (result.Picture!= "//en.wikipedia.org/wiki/Special:CentralAutoLogin/start?type=1x1"));
            result.Picture = "https:" + result.Picture;
            return result;
        }


    }

}
