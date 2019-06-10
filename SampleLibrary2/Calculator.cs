using SampleMathLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleLibrary2
{
    class Calculator
    {
        public int Sum(int a, int b)
        {
            return SampleMath.Sum(a, b);
        }

        public int Substract(int a, int b)
        {
            return SampleMath.Substract(a, b);
        }
    }
}
