using QueueingSystemSimulation.Analytics.Base;
using QueueingSystemSimulation.QueueBlocks;

namespace QueueingSystemSimulation.Analytics;

public class OutcomeAnalytics : Base.Analytics
{
    private readonly double _channelCost;
    private readonly double _queueSlotCost;

    private UnionBlock? _unionBlock;
    private AccumulatorBlock? _queue;

    public double Outcome { get; private set; }

    public OutcomeAnalytics(double channelCost, double queueSlotCost)
    {
        _channelCost = channelCost;
        _queueSlotCost = queueSlotCost;
    }

    public override void AnalyzeAfterTick(TickAnalyticsContext context)
    {
        _unionBlock ??= context.Blocks
                .First(x => x is UnionBlock) as UnionBlock;

        _queue ??= context.Blocks.FirstOrDefault(x => x is AccumulatorBlock) as AccumulatorBlock;
        
        Outcome += _unionBlock!.BlocksCount * _channelCost + (_queue?.Capacity ?? 0) * _queueSlotCost;
    }
}