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
    internal class GradeCalDecisionTable
    {
        public struct Input
        {
            public string name;
            public string grade;
            public string weight;
            public Input(string name, string grade, string weight)
            {
                this.name = name;
                this.grade = grade;
                this.weight = weight;
            }
        }
        public struct Testcase
        {
            public List<Input> input;
            public List<string> output;
            public Testcase(List<Input> input, List<string> output)
            {
                this.input = input;
                this.output = output;

            }
            public void print()
            {
                for (int i = 0; i < this.input.Count; i++) Console.WriteLine(this.input[i]);
                for (int i = 0; i < this.output.Count; i++) Console.WriteLine(this.output[i]);
            }
        }
        public List<Testcase> loadTestcase(string pathname)
        {
            WorkBook workBook = WorkBook.Load(pathname);
            WorkSheet workSheet = workBook.WorkSheets.First();
            int testnum = 1;
            int i = 1;
            Cell cell = workSheet[$"B{i}"].First();
            List<Testcase> testList = new List<Testcase>();
            while (cell.StringValue != "")
            {
                List<Input> inputs = new List<Input>();
                do
                {
                    var row = workSheet[$"B{i}:D{i}"].ToList();
                    Input input = new Input(row[0].StringValue, row[1].StringValue, row[2].StringValue);
                    inputs.Add(input);
                    i++;
                }
                while (workSheet[$"A{i-1}"].First().StringValue == "");
                List<string> outputs = workSheet[$"E{i - 1}:G{i - 1}"].Where(x => x.StringValue != "").Select(x => x.StringValue).ToList();
                Testcase testcase = new Testcase(inputs, outputs);
                testList.Add(testcase);
                testnum++;
                cell = workSheet[$"B{i}"].First();
            }

            return testList;
        }

        public bool testNormal(ChromeDriver chromeDriver, Testcase testcase)
        {
            //var resultTable = chromeDriver.FindElement(By.ClassName("cinfoT"));
            IReadOnlyList<IWebElement> rows = chromeDriver.FindElements(By.CssSelector(".verybigtext"));
            string exp_result = testcase.output[0];
            var data = rows[0].Text.Trim().Replace("\r", "").Replace("\n", "");
            if (data != exp_result) return false;
            if (testcase.output.Count == 2)
            {
                rows = chromeDriver.FindElements(By.CssSelector("p:nth-child(6)"));
                string exp_error = testcase.output[1];
                data = rows[0].Text.Trim().Replace("\r", "").Replace("\n", "");
                if (data != exp_error) return false;
            }
            return true;
        }
        public bool testException(ChromeDriver chromeDriver, Testcase testcase)
        {
            //var resultTable = chromeDriver.FindElement(By.ClassName("cinfoT"));
            if (Regex.IsMatch(testcase.output[0], @"Please provide positive weight for the Assignments/Exams", RegexOptions.IgnoreCase) == true)
            {
                IReadOnlyList<IWebElement> rows = chromeDriver.FindElements(By.CssSelector($"p:nth-child(5)"));
                string exp_result = testcase.output[0];
                var data = rows[0].Text.Trim().Replace("\r", "").Replace("\n", "");
                if (data != exp_result) return false;
            }
            else
            {
                for (int i = 0; i < testcase.output.Count; i++)
                {
                    IReadOnlyList<IWebElement> rows = chromeDriver.FindElements(By.CssSelector($"div:nth-child({i + 4})>font)"));
                    string exp_result = testcase.output[i];
                    var data = rows[0].Text.Trim().Replace("\r", "").Replace("\n", "");
                    if (data != exp_result) return false;
                }
            }

            return true;
        }
        public void DoTest()
        {
            List<Testcase> testList = this.loadTestcase(@"Testcase_Grade.xlsx");
            ChromeDriver chromeDriver = new ChromeDriver();
            for (int testIndex = 0; testIndex < testList.Count; testIndex++)
            {
                //todo
                Testcase testcase = testList[testIndex];
                chromeDriver.Url = "https://www.calculator.net/grade-calculator.html";
                chromeDriver.Navigate();
                for (int inputIndex = 0; inputIndex < testcase.input.Count; inputIndex++)
                {
                    var name = chromeDriver.FindElement(By.Name($"d{inputIndex + 1}"));
                    name.Clear();
                    Thread.Sleep(100);
                    name.SendKeys(testList[testIndex].input[inputIndex].name);
                    var grade = chromeDriver.FindElement(By.Name($"s{inputIndex + 1}"));
                    grade.Clear();
                    Thread.Sleep(100);
                    grade.SendKeys(testList[testIndex].input[inputIndex].grade);
                    var weight = chromeDriver.FindElement(By.Name($"w{inputIndex + 1}"));
                    weight.Clear();
                    Thread.Sleep(100);
                    weight.SendKeys(testList[testIndex].input[inputIndex].weight);
                }
                var calButton = chromeDriver.FindElement(By.Name("x"));
                calButton.Click();
                Thread.Sleep(100);
                if (Regex.IsMatch(testList[testIndex].output[0], @"Average Grade", RegexOptions.IgnoreCase) == true) //Mean not error testcase
                {
                    if (this.testNormal(chromeDriver, testList[testIndex]))
                        Console.WriteLine("Test case {0}: PASS", testIndex + 1);
                    else
                        Console.WriteLine("Test case {0}: FAIL", testIndex + 1);
                }
                else
                {
                    if (this.testException(chromeDriver, testList[testIndex]))
                        Console.WriteLine("Test case {0}: PASS", testIndex + 1);
                    else
                        Console.WriteLine("Test case {0}: FAIL", testIndex + 1);
                }
            }
            chromeDriver.Close();
        }
    }

}
