using System;

namespace CalculatorExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Default;
            ICalculator light = new LightCalculator();
            ICalculator full = new FullCalculator(light);

            Console.WriteLine("=== Консольний калькулятор ===");
            Console.WriteLine("Доступні операції: +, -, *, /");
            Console.WriteLine("Для виходу введіть 'exit'");

            while (true)
            {
                try
                {
                    Console.Write("\nВведіть перше число: ");
                    string inputA = Console.ReadLine();
                    if (inputA?.ToLower() == "exit") break;
                    double a = double.Parse(inputA);

                    Console.Write("Введіть друге число: ");
                    string inputB = Console.ReadLine();
                    if (inputB?.ToLower() == "exit") break;
                    double b = double.Parse(inputB);

                    Console.Write("Введіть операцію (+, -, *, /): ");
                    string op = Console.ReadLine();
                    if (op?.ToLower() == "exit") break;

                    double result = op switch
                    {
                        "+" => full.Add(a, b),
                        "-" => full.Subtract(a, b),
                        "*" => full.Multiply(a, b),
                        "/" => full.Divide(a, b),
                        _ => throw new InvalidOperationException("Невідома операція")
                    };

                    Console.WriteLine($"Результат: {result}");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Помилка: введено не число!");
                }
                catch (DivideByZeroException ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
                catch (NotSupportedException ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
            }
        }
    }
}