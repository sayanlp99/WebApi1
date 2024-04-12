namespace WebApi1
{
    public class TrafficForecast
    {
        public DateOnly Date { get; set; }

        public int Count { get; set; }

        public int Delay => (Count * 60) / 42;

        public string? Summary { get; set; }
    }
}
