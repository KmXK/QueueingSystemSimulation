using QueueingSystemSimulation.Analytics.Base;
using QueueingSystemSimulation.QueueBlocks;
using QueueingSystemSimulation.QueueBlocks.Base;

namespace QueueingSystemSimulation.Analytics;

public class BusynessAnalytics : Base.Analytics
{
    private readonly Dictionary<IQueueBlock, int> _sources = new();
    
    private int _ticks;

    public IReadOnlyDictionary<IQueueBlock, double> AverageSourceBusyness =>
        _sources.ToDictionary(x => x.Key, x => 1.0 * x.Value / _ticks);
    
    public override void AnalyzeAfterTick(TickAnalyticsContext context)
    {
        _ticks++;
        
        foreach (var block in context.Blocks.Skip(2).Where(x => x is SourceQueueBlock or WrapBlock))
        {
            _sources.TryAdd(block, 0);
        }

        foreach (var block in _sources.Keys)
        {
            var currentBlock = block;
            
            while (true)
            {
                switch (currentBlock)
                {
                    case BlockingBlock { IsBlocked: true }:
                        _sources[block]++;
                        break;
                    case WrapBlock wrapBlock:
                        currentBlock = wrapBlock.WrappedBlock;
                        continue;
                    case SourceQueueBlock {IsBusy: true}:
                        _sources[block]++;
                        break;
                }

                break;
            }
        }
    }
}