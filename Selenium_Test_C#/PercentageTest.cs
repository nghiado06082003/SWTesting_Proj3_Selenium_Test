using IronXL;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Selenium_Test
{
    internal class PercentageTest
    {
        struct Input
        {
            public string percent;
            public string value;
            public Input(string percent, string value)
            {
                this.percent = percent;
                this.value = value;
            }
        };
        struct Output
        {
            public string result;
            public Output(string result)
            {
                this.result = result;
            }
        }
        struct Testcase
        {
            public Input input;
            public Output output;
            public Testcase(Input input, Output output)
            {
                this.input = input;
                this.output = output;
            }
            public void print()
            {
                Console.WriteLine(this.input.percent);
                Console.WriteLine(this.input.value);
                Console.WriteLine(this.output.result);
            }
        }
        private List<Testcase> loadTestcase(string pathname)
        {
            WorkBook workBook = WorkBook.Load(pathname);
            WorkSheet workSheet = workBook.WorkSheets.First();
            int i = 1;
            Cell cell = workSheet[$"C{i}"].First();
            List<Testcase> testList = new List<Testcase>();
            while (cell.StringValue != "")
            {
                var row = workSheet[$"A{i}:C{i}"].ToList();
                Input input = new Input(row[0].StringValue, row[1].StringValue);
                Output output = new Output(row[2].StringValue);
                Testcase testcase = new Testcase(input, output);
                testList.Add(testcase);
                i++;
                cell = workSheet[$"C{i}"].First();
            }
            return testList;
        }
        private bool testException(ChromeDriver chromeDriver, string excpt)
        {
            var message = chromeDriver.FindElement(By.TagName("font"));
            return message.Text == excpt;
        }

        private bool testNormal(ChromeDriver chromeDriver, Testcase testcase)
        {
            var result = chromeDriver.FindElement(By.ClassName("verybigtext")).FindElement(By.TagName("b")).Text;
            string expected_result = testcase.output.result;
            return result == expected_result;
        }
        public void DoTest(string pathname)
        {
            List<Testcase> testList = this.loadTestcase(pathname);
            ChromeDriver chromeDriver = new ChromeDriver();
            for (int testIndex = 0; testIndex < testList.Count; testIndex++)
            {
                chromeDriver.Url = "https://www.calculator.net/percent-calculator.html";
                chromeDriver.Navigate();
                var calForm = chromeDriver.FindElement(By.Name("calform"));
                var percent = calForm.FindElement(By.Name("cpar1"));
                percent.Clear();
                Thread.Sleep(100);
                percent.SendKeys(testList[testIndex].input.percent);
                var value = calForm.FindElement(By.Name("cpar2"));
                value.Clear();
                Thread.Sleep(100);
                value.SendKeys(testList[testIndex].input.value);
                var calButton = calForm.FindElement(By.Name("x"));
                Thread.Sleep(100);
                calButton.Click();
                Thread.Sleep(100);
                if (Regex.IsMatch(testList[testIndex].output.result, @"[-+]?[0-9]+", RegexOptions.IgnoreCase))
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