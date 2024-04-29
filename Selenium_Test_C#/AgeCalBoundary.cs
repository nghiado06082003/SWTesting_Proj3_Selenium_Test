using IronXL;
using Microsoft.Extensions.Primitives;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Controls.Primitives;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;
using System.IO.Packaging;

namespace Selenium_Test
{
    internal class AgeCalBoundary
    {
        public struct Input
        {
            public string m1,d1,y1;
            public string m2,d2,y2;
            public Input(string m1, string d1, string y1, string m2, string d2, string y2)
            {
                this.m1 = m1;
                this.d1 = d1;
                this.y1 = y1;
                this.m2 = m2;
                this.d2 = d2;
                this.y2 = y2;
            }
        };
        public struct Output
        {
            public string result;
            public Output(
                string result)
            {
                this.result = result;
            }
        };
        public struct Testcase
        {
            public Input input;
            public Output output;
            public Testcase(Input input, Output output)
            {
                this.input = input;
                this.output = output;
            }
            //public void print()
            //{
            //    Console.WriteLine(this.input.pA);
            //    Console.WriteLine(this.input.pB);
            //    Console.WriteLine(this.output.not_A);
            //}
        }

        public List<Testcase> loadTestcase(string pathname)
        {
            WorkBook workBook = WorkBook.Load(pathname);
            WorkSheet workSheet = workBook.WorkSheets.First();
            int i = 1;
            Cell cell = workSheet[$"A{i}"].First();
            List<Testcase> testList = new List<Testcase>();
            while (cell.StringValue != "")
            {
                var row = workSheet[$"A{i}:J{i}"].ToList();
                Input input = new Input(row[0].StringValue, row[1].StringValue, row[2].StringValue, row[3].StringValue, row[4].StringValue, row[5].StringValue);
                Output output = new Output(
                    row[6].StringValue
                    );
                Testcase testcase = new Testcase(input, output);
                testList.Add(testcase);
                i++;
                cell = workSheet[$"A{i}"].First();
            }
            return testList;
        }
        public bool testException(ChromeDriver chromeDriver, string excpt)
        {
            var message = chromeDriver.FindElement(By.CssSelector("font"));
            return message.Text == excpt;
        }

        public bool testNormal(ChromeDriver chromeDriver, Testcase testcase)
        {
            //var resultTable = chromeDriver.FindElement(By.ClassName("cinfoT"));
            IReadOnlyList<IWebElement> rows = chromeDriver.FindElements(By.CssSelector(".verybigtext"));
            string[] expected_result = {
                testcase.output.result
            };
            //for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            //{
            //    IReadOnlyList<IWebElement> datas = rows[rowIndex].FindElements(By.TagName("td"));
            //    var data = datas[1].Text;
            //    if (data != expected_result[rowIndex]) return false;
            //}
            var data = rows[0].Text.Trim().Replace("\r", "").Replace("\n", ""); ;
            if (data != expected_result[0]) return false;
            return true;
        }
        public void DoTest()
        {
            List<Testcase> testList = this.loadTestcase(@"Testcase_Age_Calculator_Boundary.xlsx");
            ChromeDriver chromeDriver = new ChromeDriver();
            for (int testIndex = 0; testIndex < testList.Count; testIndex++)
            {
                //todo
                chromeDriver.Url = "https://www.calculator.net/age-calculator.html";
                chromeDriver.Navigate();
                var m1 = chromeDriver.FindElement(By.Id("today_Month_ID"));
                var d1 = chromeDriver.FindElement(By.Id("today_Day_ID"));
                var y1 = chromeDriver.FindElement(By.Id("today_Year_ID"));
                var m2 = chromeDriver.FindElement(By.Id("ageat_Month_ID"));
                var d2 = chromeDriver.FindElement(By.Id("ageat_Day_ID"));
                var y2 = chromeDriver.FindElement(By.Id("ageat_Year_ID"));

                SelectElement selector = new SelectElement(m1);
                selector.SelectByText(testList[testIndex].input.m1);
                Thread.Sleep(100);
                selector = new SelectElement(d1);
                selector.SelectByText(testList[testIndex].input.d1);
                Thread.Sleep(100);
                while (y1.GetAttribute("value") != "")
                {
                    y1.SendKeys(Keys.Backspace);
                }
                Thread.Sleep(100);
                y1.SendKeys(testList[testIndex].input.y1);
                Thread.Sleep(100);

                selector = new SelectElement(m2);
                selector.SelectByText(testList[testIndex].input.m2);
                Thread.Sleep(100);
                selector = new SelectElement(d2);
                selector.SelectByText(testList[testIndex].input.d2);
                Thread.Sleep(100);
                while (y2.GetAttribute("value") != "")
                {
                    y2.SendKeys(Keys.Backspace);
                }
                y2.SendKeys(testList[testIndex].input.y2);
                Thread.Sleep(100);

                var calButton = chromeDriver.FindElement(By.Name("x"));
                calButton.Click();
                Thread.Sleep(100);

                if (Regex.IsMatch(testList[testIndex].output.result, @"Date", RegexOptions.IgnoreCase) == false) //Mean not error testcase
                {
                    if (this.testNormal(chromeDriver, testList[testIndex]))
                        Console.WriteLine("Test case {0}: PASS", testIndex + 1);
                    else
                        Console.WriteLine("Test case {0}: FAIL", testIndex + 1);
                }
                else
                {
                    if (this.testException(chromeDriver, testList[testIndex].output.result))
                        Console.WriteLine("Test case {0}: PASS", testIndex + 1);
                    else
                        Console.WriteLine("Test case {0}: FAIL", testIndex + 1);
                }
            }
            chromeDriver.Close();
        }
    }
}
