using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Xml.Linq;

namespace Kasa.Models
{
    public static class Xml
    {
        static string path = HostingEnvironment.MapPath(@"~\App_Data\Data.xml");
        public static List<Person> Grouped { get; set; }
        public static List<Person> History { get; set; }
        public static Model Model { get; set; }

        public static void LoadModel()
        {
            XDocument xdoc = XDocument.Load(path);
            History = AllList();
            Grouped = GroupedList();
            Model = new Model();
            Model.Grouped = new List<Person>();
            Model.History = new List<Person>();
            Model.Grouped = Grouped;
            Model.History = History; 
        }
        private static List<Person> AllList()
        {
            List<Person> temp = new List<Person>();
            try
            {
                XDocument xdoc = XDocument.Load(path);
                foreach (var item in xdoc.Element("data").Elements())
                {
                    temp.Add(new Person
                    {
                        Name = item.Attribute("name").Value,
                        Number = item.Attribute("value").Value.ToDecimal(),
                        Date = item.Attribute("date").Value,
                        What = item.Attribute("what").Value
                    });
                }
                return temp.OrderByDescending(a=>a.Date).ToList();
            }
            finally
            {                
            }
        }
        private static List<Person> GroupedList()
        {
            try
            {
                List<Person> temp = AllList();
                return temp
                    .GroupBy(a => a.Name)
                    .Select(b => new Person
                    {
                        Name = b.First().Name,
                        Number = b.Sum(c => c.Number),
                        Date = b.Max(c => c.Date)                        
                    }).ToList();
            }
            finally
            {               
            }
        }
        public static void Add(Person person)
        {
            if (person != null && person.Name != null && person.Number != 0)
            {
                XDocument doc = XDocument.Load(path);
                doc.Descendants("data")
                .Last()
                .Add(new XElement("val",
                new XAttribute("name", person.Name),
                new XAttribute("value", person.Number.ToString()),
                new XAttribute("what", person.What ?? "-"), 
                new XAttribute("date", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"))));
                doc.Save(path);
            }
        }
        public static double ToDecimal(this string val)
        {
            try
            {
                return Convert.ToDouble(val);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static DateTime ToDateTime(this string val)
        {
            try
            {
                return Convert.ToDateTime(val);
            }
            catch (Exception ex)
            {
                return new DateTime(2000, 1, 1);
            }
        }
    }
}