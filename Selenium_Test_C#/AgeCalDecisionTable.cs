using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium_Test
{
    internal class AgeCalDecisionTable
    {
        public struct Input
        {
            public string pA;
            public string pB;
            public Input(string pA, string pB)
            {
                this.pA = pA;
                this.pB = pB;
            }
        };
        public struct Output
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
        public struct Testcase
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
    }
}
