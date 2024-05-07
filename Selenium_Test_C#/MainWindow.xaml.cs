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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Probability_Cal_Click(object sender, RoutedEventArgs e)
        {
            ProbabilityTest probabilityTest = new ProbabilityTest();
            probabilityTest.DoTest(@"Testcase_Probability.xlsx");
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

        private void Grade_Cal_Click(object sender, RoutedEventArgs e)
        {
            GradeCalTest gradeCalTest = new GradeCalTest();
            gradeCalTest.DoTest();
        }

        private void Percentage_Cal_Click(object sender, RoutedEventArgs e)
        {
            PercentageTest percentageTest = new PercentageTest();
            percentageTest.DoTest(@"Testcase_Percentage.xlsx");
        }
    }
}
