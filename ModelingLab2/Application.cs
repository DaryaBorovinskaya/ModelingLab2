namespace ModelingLab2
{
    public class Application
    {
        private int _number;
        private decimal _timeComing;
        private decimal _timeWaitInBuff;

        public int Number { get => _number; set => _number = value; }
        public decimal TimeComing { get => _timeComing; set => _timeComing = value; }
        public decimal TimeWaitInBuff { get => _timeWaitInBuff; set => _timeWaitInBuff = value; }

        public Application(int number, decimal timeComing, decimal timeWaitInBuff)
        {
            Number = number;
            TimeComing = timeComing;
            TimeWaitInBuff = timeWaitInBuff;
        }
    }
}
