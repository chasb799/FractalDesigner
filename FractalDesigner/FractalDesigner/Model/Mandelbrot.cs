using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalDesigner.Model
{
    internal class Mandelbrot
    {
        private const double Boundary = 30;

        /// <summary>
        /// Number of iterations to reach a magnitude boundary.
        /// </summary>
        public int CalculateIterations(Complex c, int maxIterations = 100)
        {
            var z = new Complex(0, 0);
            var n = 0;

            while (z.Magnitude < Boundary && n < maxIterations)
            {
                z = z*z + c;
                n++;
            }

            return n;
        }
    }
}
