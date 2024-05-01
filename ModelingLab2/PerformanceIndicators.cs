namespace ModelingLab2
{
    /// <summary>
    /// Показатели эффективности
    /// </summary>
    public class PerformanceIndicators
    {
        public decimal CalculateCoeffWorkload( decimal Tstagn1,   decimal Tstagn2,  decimal Tmod)
        {
            return 1 - (Tstagn1+Tstagn2) / Tmod;
        }
        public decimal CalculateT_averServ( decimal Tserv,  decimal Twait_serv1,  decimal Twait_serv2,  decimal Nserv)
        {
            return (Tserv + Twait_serv1 + Twait_serv2) / Nserv;
        }
        public decimal CalculateP_noServ( decimal N_noServ,  decimal N)
        {
            return N_noServ / N;
        }
    }
}
