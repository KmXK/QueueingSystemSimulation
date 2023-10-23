using QueueingSystemSimulation.Analytics.Base;

namespace QueueingSystemSimulation.Analytics;

public class TestAnalytics : Base.Analytics
{
    public override void AnalyzeBeforeTick(TickAnalyticsContext context)
    {
        if (context.TickNumber == 10000)
        {
            Console.WriteLine(context.Blocks.Count);
        }
    }
}