using System;

namespace Lab5.Services
{
    public class CalculationService
    {
        public double TrapezoidalIntegrator(Func<double, double> function, double A, double B, int n)
        {
            double h = (B - A) / n;
            double sum = (function(A) + function(B)) / 2.0;
            for (int i = 1; i < n; i++)
            {
                double x = A + i * h;
                sum += function(x);
            }
            return sum * h;
        }

        public double Function(double x)
        {
            if (x <= 0)
                throw new ArgumentException("x must be greater than 0.", nameof(x));
            return Math.Log10(x) * x + 2 * x + 1;
        }
    }
}