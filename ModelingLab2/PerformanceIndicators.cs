using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingLab2
{
    /// <summary>
    /// Показатели эффективности
    /// </summary>
    public class PerformanceIndicators
    {
        public double CalculateCoeffWorkload( double Tstagn1,   double Tstagn2,  double Tmod)
        {
            return Tstagn1 / (Tmod - Tstagn1) * Tstagn2 / (Tmod - Tstagn2);
        }
        public double CalculateT_averServ( double Tserv,  double Twait1,  double Twait2,  double Nserv)
        {
            return (Tserv + Twait1 + Twait2) / Nserv;
        }
        public double CalculateP_noServ( double N_noServ,  double N)
        {
            return N_noServ / N;
        }
    }
}
