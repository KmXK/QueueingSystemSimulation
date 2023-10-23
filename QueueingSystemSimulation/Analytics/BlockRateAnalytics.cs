using QueueingSystemSimulation.Analytics.Base;
using QueueingSystemSimulation.QueueBlocks;

namespace QueueingSystemSimulation.Analytics;

public class BlockRateAnalytics : Base.Analytics
{
    private int _ticks;
    private int _blockTicks;
    
    public double BlockRate => 1.0 * _blockTicks / _ticks;
    
    public override void AnalyzeAfterTick(TickAnalyticsContext context)
    {
        _ticks++;

        if (context.Blocks.Any(x => x is BlockingBlock { IsBlocked: true }))
        {
            _blockTicks++;
        }
    }
}