using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using HtmlParser.Lexer;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApplication8
{
    class Program
    {



        static string GetContent(string url)
        {
            using (var client = new WebClient())
            {
                var request = url;
                return client.DownloadString(request);
                    
            }
        }

        static IEnumerable<string> GetLinks(string content)
        {
            Regex reg = new Regex(@"The [a-z]+ element");

            HtmlLexer a = new HtmlLexer();
            a.Load(content);
            return a.Parse().Where(w => w.TokenType == TokenType.Text).Select(w => new String(w.Source).Replace("\0", "")).Where(w =>
                {
                    return w.Contains("The <code>") && w.Contains("</code> element");
                });


        }


        static IEnumerable<string> TagCount(string content)
        {

            List<string> elements = new List<string>();

            //int session = content.IndexOf("</section>");

            while (content.IndexOf("The <code>") > -1)
            {
                int i1 = content.IndexOf("The <code>");
                int i2 = content.IndexOf("</code> element</a>") + 15;

                //if (i2 > session) break;
                if (i2 < i1)
                {
                    content = content.Remove(i2, 7);
                    continue;
                }

                int length = i2 - i1;
                string substr = content.Substring(i1, length);
                elements.Add(substr);
                content = content.Replace(substr, "");
            }

            return elements;
        }


        static IEnumerable<string> GetHtml51(string content)
        {
            Regex reg = new Regex(@"The <code>\w+</code> element</a>");
            var sss = content.Replace("<li>", "@").Split('@');
            return sss.Where(w => reg.IsMatch(w)).Select(w=>
                {
                    int index1 = w.IndexOf("<code>");
                    int index2 = w.IndexOf("</code>");
                    return "<"+w.Substring(index1 + 6, index2-index1-6)+">";
                }).Distinct().OrderBy(w=>w);
        }






        static IEnumerable<string> GetDataHtml(string content)
        {

            List<string> elements = new List<string>();


            while (content.IndexOf("&lt;") > -1)
            {
                int i1 = content.IndexOf("&lt;");
                int i2 = content.IndexOf("&gt;") + 4;
                int length = i2 - i1;
                string substr = content.Substring(i1, length);
                elements.Add(substr.Replace("&lt;", "<").Replace("&gt;", ">"));
                content = content.Replace(substr, "");
            }

            return elements;
        }

        static IEnumerable<string> GetDataHtml2(string content)
        {

            List<string> elements = new List<string>();


            while (content.IndexOf("\"><code>&lt;") > -1)
            {
                int i1 = content.IndexOf("\"><code>&lt;");
                int i2 = content.IndexOf("&gt;</code></a></td>") + 15;
                if (i2 < i1) break;
                int length = i2 - i1;
                string substr = content.Substring(i1, length);
                elements.Add(substr.Replace("&lt;", "<").Replace("&gt;", ">"));
                content = content.Replace(substr, "");
            }

            return elements;
        }

        static IEnumerable<string> GetDataHtml5(string content)
        {

            List<string> elements = new List<string>();


            int index = content.IndexOf("<article>");
            content = content.Remove(0, index + 9);


            while (content.IndexOf("&lt;") > -1)
            {
                int i1 = content.IndexOf("&lt;");
                int i2 = content.IndexOf("&gt;") + 4;
                int length = i2 - i1;
                string substr = content.Substring(i1, length);
                string substr2 = content.Substring(i1, length + 30);
                if (substr2.Contains("html5"))
                {
                    elements.Add(substr.Replace("&lt;", "<").Replace("&gt;", ">"));
                }
                content = content.Replace(substr, "");
            }

            return elements;
        }

        



        static void Main(string[] args)
        {
            var cc = GetContent("http://www.w3.org/TR/html51/");
            /*var c = GetContent("http://htmlbook.ru/html");
            var html = GetContent("http://www.w3.org/TR/html5/");
            var html5 = GetDataHtml5(c);
            var htmlbook = GetDataHtml(c);
            
            var html5 = GetHtml51(html);

            foreach (var item in html51)
            {
                Console.Write(item);
                if(!html5.Contains(item))
                {
                    Console.Write("\tHTML5.1");
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var item in htmlbook)
            {
                if(!html51.Contains(item))
                {
                    Console.WriteLine(item);
                }
            }*/


            //var newSite = GetContent("https://developer.mozilla.org/en-US/docs/Web/HTML/Element");
            //var data = GetDataHtml2(newSite);
            var html51 = GetHtml51(cc);




            var site = GetContent("http://www.w3.org/TR/html51/semantics.html");
            int sub1 = site.IndexOf("<section><h4 id=\"the-base-element\">4.2.3 The <dfn><code>base</code></dfn> element</h4>");


            /*foreach (var item in str)
            {
                Console.Write(item);
                if(!html51.Contains(item))
                {
                    Console.Write("!!!!!!");
                }
                Console.WriteLine();
            }*/





            Console.Read();
        }
    }
}
