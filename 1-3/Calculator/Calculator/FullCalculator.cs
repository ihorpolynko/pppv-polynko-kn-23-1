using System;

namespace CalculatorExample
{
    public class FullCalculator : ICalculator
    {
        private readonly ICalculator _calculator;

        public FullCalculator(ICalculator calculator)
        {
            _calculator = calculator;
        }

        public double Add(double a, double b)
        {
            double result = _calculator.Add(a, b);
            return double.IsNaN(result) ? a + b : result;
        }

        public double Subtract(double a, double b)
        {
            double result = _calculator.Subtract(a, b);
            return double.IsNaN(result) ? a - b : result;
        }

        public double Multiply(double a, double b)
        {
            double result = _calculator.Multiply(a, b);
            return double.IsNaN(result) ? a * b : result;
        }

        public double Divide(double a, double b)
        {
            double result = _calculator.Divide(a, b);
            if (double.IsNaN(result))
            {
                if (b == 0) throw new DivideByZeroException("Ділення на нуль!");
                return a / b;
            }
            return result;
        }
    }
}