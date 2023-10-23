using QueueingSystemSimulation.Analytics.Base;
using QueueingSystemSimulation.QueueBlocks;

namespace QueueingSystemSimulation.Analytics;

public class QueueLengthAnalytics : Base.Analytics
{
    private readonly List<int> _averageQueueLengths = new();

    public double AverageQueueLength => _averageQueueLengths.Average();

    public override void AnalyzeAfterTick(TickAnalyticsContext context)
    {                
        _averageQueueLengths.Add(context.Blocks
            .Where(x => x is AccumulatorBlock)
            .Sum(x => (x as AccumulatorBlock)!.QueueSize));
    }
}