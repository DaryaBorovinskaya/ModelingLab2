
namespace ModelingLab2
{
    public class StatisticalStability
    {
        private ModelingProcess _modelingProcess = new();
        public decimal[] FindProbability(out int n)
        {
            n = 50;

            int nN;
            decimal inaccuracy = 0.04m,
                t = 1.751m,
                pMax = decimal.MinValue,
                pMin = decimal.MaxValue,
                pMiddle = 0;

            while (true)
            {
                decimal[] p = new decimal[n];
                for (int i = 0; i < n; i++)
                {
                    p[i] = _modelingProcess.StartProcess().coeffWorkload;
                    pMiddle += p[i];
                    if (pMax < p[i]) pMax = p[i];
                    if (pMin > p[i]) pMin = p[i];
                }
                pMiddle /= n;
                if ((pMax - pMin) / pMax >= inaccuracy) throw new ArgumentException("Значения показателя эффективности не соответствуют требуемой точности");
                nN = (int)((pMiddle * (1 - pMiddle) / (decimal)Math.Pow((double)inaccuracy, 2)) * (decimal)Math.Pow((double)t, 2));
                
                if (nN <= n)
                {
                    return p;
                }
                n = nN;
                pMax = decimal.MinValue;
                pMin = decimal.MaxValue;
                pMiddle = 0;
            }



        }
    }
}
