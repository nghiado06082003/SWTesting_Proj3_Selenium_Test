using IronXL;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
    struct Input
    {
        public string pA;
        public string pB;
        public Input(string pA, string pB)
        {
            this.pA = pA;
            this.pB = pB;
        }
    };
    struct Output
    {
        public string not_A;
        public string not_B;
        public string and_A_B;
        public string or_A_B;
        public string xor_A_B;
        public string not_both;
        public string have_A_not_B;
        public string have_B_not_A;
        public Output(
            string not_A, 
            string not_B, 
            string and_A_B, 
            string or_A_B, 
            string xor_A_B, 
            string not_both, 
            string have_A_not_B, 
            string have_B_not_A)
        {
            this.not_A = not_A;
            this.not_B = not_B;
            this.and_A_B = and_A_B;
            this.or_A_B = or_A_B;
            this.xor_A_B = xor_A_B;
            this.not_both = not_both;
            this.have_A_not_B = have_A_not_B;
            this.have_B_not_A = have_B_not_A;
        }
    };
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
            Console.WriteLine(this.input.pA);
            Console.WriteLine(this.input.pB);
            Console.WriteLine(this.output.not_A);
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private List<Testcase> loadTestcase(string pathname)
        {
            WorkBook workBook = WorkBook.Load(pathname);
            WorkSheet workSheet = workBook.WorkSheets.First();
            int i = 1;
            Cell cell = workSheet[$"A{i}"].First();
            List<Testcase> testList = new List<Testcase>();
            while (cell.StringValue != "")
            {
                var row = workSheet[$"A{i}:J{i}"].ToList();
                Input input = new Input(row[0].StringValue, row[1].StringValue);
                Output output = new Output(
                    row[2].StringValue,
                    row[3].StringValue,
                    row[4].StringValue,
                    row[5].StringValue,
                    row[6].StringValue,
                    row[7].StringValue,
                    row[8].StringValue,
                    row[9].StringValue
                    );
                Testcase testcase = new Testcase(input, output);
                testList.Add(testcase);
                i++;
                cell = workSheet[$"A{i}"].First();
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
            var resultTable = chromeDriver.FindElement(By.ClassName("cinfoT"));
            IReadOnlyList<IWebElement> rows = resultTable.FindElements(By.TagName("tr"));
            string[] expected_result = {
                testcase.output.not_A,
                testcase.output.not_B,
                testcase.output.and_A_B,
                testcase.output.or_A_B,
                testcase.output.xor_A_B,
                testcase.output.not_both,
                testcase.output.have_A_not_B,
                testcase.output.have_B_not_A
            };
            for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                IReadOnlyList<IWebElement> datas = rows[rowIndex].FindElements(By.TagName("td"));
                var data = datas[1].Text;
                if (data != expected_result[rowIndex]) return false;
            }
            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Testcase> testList = this.loadTestcase(@"Testcase_Probability.xlsx");
            ChromeDriver chromeDriver = new ChromeDriver();
            for (int testIndex = 0; testIndex < testList.Count; testIndex++)
            {
                chromeDriver.Url = "https://www.calculator.net/probability-calculator.html";
                chromeDriver.Navigate();
                var calForm = chromeDriver.FindElement(By.Name("calform"));
                var prob_A = calForm.FindElement(By.Name("cal1pa"));
                prob_A.Clear();
                Thread.Sleep(100);
                prob_A.SendKeys(testList[testIndex].input.pA);
                var prob_B = calForm.FindElement(By.Name("cal1pb"));
                prob_B.Clear();
                Thread.Sleep(100);
                prob_B.SendKeys(testList[testIndex].input.pB);
                var calButton = calForm.FindElement(By.Name("x"));
                Thread.Sleep(100);
                calButton.Click();
                Thread.Sleep(100);
                if (Regex.IsMatch(testList[testIndex].output.not_A, @"^\d", RegexOptions.IgnoreCase))
                {
                    if (this.testNormal(chromeDriver, testList[testIndex]))
                        Console.WriteLine("Test case {0}: PASS", testIndex + 1);
                    else
                        Console.WriteLine("Test case {0}: FAIL", testIndex + 1);
                }
                else
                {
                    if (this.testException(chromeDriver, testList[testIndex].output.not_A))
                        Console.WriteLine("Test case {0}: PASS", testIndex + 1);
                    else
                        Console.WriteLine("Test case {0}: FAIL", testIndex + 1);
                }
            }
            chromeDriver.Close();
        }

        private void Age_Cal_Decision_Table_Click(object sender, RoutedEventArgs e)
        {
            AgeCalDecisionTable ageCalDecisionTable = new AgeCalDecisionTable();
            ageCalDecisionTable.DoTest();
        }

        private void Age_Cal_Boundary_Click(object sender, RoutedEventArgs e)
        {
            AgeCalBoundary ageCalBoundary = new AgeCalBoundary();
            ageCalBoundary.DoTest();
        }

        private void Grade_Cal_Boundary_Click(object sender, RoutedEventArgs e)
        {
            GradeCalDecisionTable gradeCalDecisionTable = new GradeCalDecisionTable();
            gradeCalDecisionTable.DoTest();
        }
    }
}
