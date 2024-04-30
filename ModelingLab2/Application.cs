using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingLab2
{
    public class Application
    {
        private int _number;
        private double _timeComing;
        private double _timeWaitInBuff;

        public int Number { get => _number; set => _number = value; }
        public double TimeComing { get => _timeComing; set => _timeComing = value; }
        public double TimeWaitInBuff { get => _timeWaitInBuff; set => _timeWaitInBuff = value; }

        public Application(int number, double timeComing, double timeWaitInBuff)
        {
            Number = number;
            TimeComing = timeComing;
            TimeWaitInBuff = timeWaitInBuff;
        }
    }
}
