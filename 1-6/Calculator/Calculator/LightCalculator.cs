using System;

namespace CalculatorExample
{
    public class LightCalculator : ICalculator
    {
        public virtual double Add(double a, double b)
        {
            if (a == 0) return b;
            if (b == 0) return a;
            return double.NaN;
        }

        public virtual double Subtract(double a, double b)
        {
            if (b == 0) return a;
            if (a == 0) return -b;
            return double.NaN;
        }

        public virtual double Multiply(double a, double b)
        {
            if (a == 0 || b == 0) return 0;
            return double.NaN;
        }

        public virtual double Divide(double a, double b)
        {
            Console.OutputEncoding = System.Text.Encoding.Default;
            if (b == 0) throw new DivideByZeroException("Ділення на нуль!");
            if (a == 0) return 0;
            return double.NaN;
        }
    }
}