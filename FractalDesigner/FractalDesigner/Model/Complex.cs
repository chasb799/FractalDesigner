using System;

namespace FractalDesigner.Model
{
    /// <summary>
    /// Represents a complex number.
    /// </summary>
    public class Complex
    {
        public double Real { get; }
        public double Imaginary { get; }
        public double Magnitude { get; }

        public Complex(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
            Magnitude = Math.Sqrt(Real * Real + Imaginary * Imaginary);
        }

        public static Complex operator +(Complex c1, Complex c2)
        {
            return new Complex(c1.Real + c2.Real, c1.Imaginary + c2.Imaginary);
        }

        public static Complex operator *(Complex c1, Complex c2)
        {
            return new Complex(c1.Real * c2.Real - c1.Imaginary * c2.Imaginary, c1.Real * c2.Imaginary + c1.Imaginary * c2.Real);
        }
    }
}