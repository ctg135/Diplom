namespace Client.DataModels
{
    public class Plan
    {
        public string DateSet { get; set; }
        public string StartDay { get; set; }
        public string EndDay { get; set; }
        public string TypePlan { get; set; }
        public static Plan Empty()
        {
            return new Plan() { DateSet = "-", EndDay = "-", StartDay = "-", TypePlan = "0" };
        }
    }
}
