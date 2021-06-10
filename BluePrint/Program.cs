using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using System.Xml.Linq;

namespace BluePrint
{
    class Program

    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"Please Enter the roots for loading XMl file :");

            var path = Console.ReadLine();

            ReadXmlFile(path);

            Console.WriteLine(@"EmployeeName | EmployeeTitle | UnitName");
            Console.WriteLine(@"------------------------------------------------");
            Console.WriteLine();
            WriteXmlFile();

            Console.WriteLine();
            Console.WriteLine(@"-----------------Jason File------------------------------");
            Console.WriteLine();

            SwitchUnits(path );
            Console.ReadLine();
        }

        private static void ReadXmlFile(string path)
        {
            try
            {

                if (path == null)
                {
                    Console.WriteLine(@"Please insert valid Route");
                }

                var xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                var dbContext = new Context();
                var title = string .Empty;
                var unitName = string.Empty;
                var unitNodes = xmlDoc.GetElementsByTagName("Unit");
                if (dbContext .Organizations.Any())
                {
                    Console.WriteLine();
                    Console.WriteLine(@"Already there is a file in DataBase Do you want to replace it ?? Please enter Yes or No ");
                }

                var result = Console.ReadLine();
            
                if (result.ToUpper( )=="YES")
                {
                    dbContext .Database.ExecuteSqlCommand("TRUNCATE TABLE Organization");

                    foreach (XmlElement unit in unitNodes)
                    {
                        unitName = unit.Attributes["Name"].Value;
                        var employeeNodes = xmlDoc.GetElementsByTagName("Employee");
                        foreach (XmlElement employee in employeeNodes)
                        {
                            string employeeName = string.Empty;

                            if (employee.ParentNode.Attributes["Name"].Value == unitName)
                            {
                                var unitOfOrganization = new Organization();
                                unitOfOrganization.EmployeeName = employee.FirstChild.Value;
                                unitOfOrganization.EmployeeTitle = employee.Attributes["Title"].Value;
                                unitOfOrganization.UnitName = unitName;

                                dbContext.Entry(unitOfOrganization).State = EntityState.Added;
                                dbContext.SaveChanges();
                            }
                        }
                    }
                }
               

                
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Your path is null or invalid Please insert valid route");

            }

        }
        private static void WriteXmlFile()
        {
            var dbContext = new Context();
            var listOfEmployee = dbContext.Organizations.ToList();
            foreach (var list in listOfEmployee)
            {
                Console.WriteLine(list.EmployeeName + @" | " + list.EmployeeTitle + @" | " + list.UnitName);
            }
        }
        private static void SwitchUnits(string path)
        {

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException($"Path is Empty");
            }
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNodeList unitNodes = xmlDoc.GetElementsByTagName("Unit");
            var unitFirst = "";
            var unitSecond = "";
            foreach (XmlElement unit in unitNodes)
            {
                unitFirst = "Platform Team";

                unitSecond = "Maintenance Team";
                //  }
                unitFirst = unitFirst + unitSecond;
                unitSecond = unitFirst.Remove(unitFirst.IndexOf(unitSecond, StringComparison.Ordinal));
                unitFirst = unitFirst.Substring(unitSecond.Length);

                if (unit.Attributes["Name"].Value == "Platform Team")
                {
                    unit.Attributes["Name"].Value = unitFirst;
                }
                if (unit.Attributes["Name"].Value == "Maintenance Team")
                {
                    unit.Attributes["Name"].Value = unitSecond;
                }

            }

            var XmlFileSwitchFilePath = (@"C:\\XML File\\switchFile.txt");
            xmlDoc.Save(XmlFileSwitchFilePath);

            ConvertToJasonFile(XmlFileSwitchFilePath);
        }
        private static void ConvertToJasonFile(string xmlFileSwitchFilePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFileSwitchFilePath);
            string json = JsonConvert.SerializeXmlNode(doc);

            Console.WriteLine(json);
        }
    }
}
