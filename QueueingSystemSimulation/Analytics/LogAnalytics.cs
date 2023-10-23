using QueueingSystemSimulation.Analytics.Base;
using QueueingSystemSimulation.QueueBlocks;
using QueueingSystemSimulation.QueueBlocks.Base;

namespace QueueingSystemSimulation.Analytics;

public class LogAnalytics : Base.Analytics
{
    private readonly int? _tick;
    private readonly Dictionary<IQueueBlock, Dictionary<IQueueBlock, QueueRequest>> _transfers = new();

    public LogAnalytics(int? tick = null)
    {
        _tick = tick;
    }
    
    public override void AnalyzeBeforeTick(TickAnalyticsContext context)
    {
        if (_tick != null && context.TickNumber != _tick)
        {
            return;
        }
        
        foreach (var key in _transfers.Keys)
        {
            _transfers[key].Clear();
        }
    }

    public override void AnalyzeSendingRequest(BlockAnalyticsContext context)
    {
        if (_tick != null && context.TickNumber != _tick)
        {
            return;
        }
        
        var block = context.FromBlock;
        var nextBlock = context.ToBlock;

        if (!_transfers.ContainsKey(block))
        {
            _transfers[block] = new Dictionary<IQueueBlock, QueueRequest>();
        }
        
        _transfers[block].Add(nextBlock, context.SentRequest);
    }

    public override void AnalyzeAfterTick(TickAnalyticsContext context)
    {
        if (_tick != null && context.TickNumber != _tick)
        {
            return;
        }
        
        var blocks = context.Blocks;

        for (var index = 0; index < blocks.Count; index++)
        {
            var block = blocks[index];
            var nextBlock = index < blocks.Count - 1 ? blocks[index + 1] : null;

            var fromBlockType = block.GetType().Name;
            var toBlockType = nextBlock?.GetType().Name ?? " - ";

            if (block is not EndBlock)
            {
                var request = nextBlock != null &&
                              _transfers.TryGetValue(block, out var transfer) &&
                              transfer.TryGetValue(nextBlock, out var r)
                    ? r
                    : null;

                Console.WriteLine($"{context.TickNumber,3} | " +
                                  $"{fromBlockType,22} -> {toBlockType,22} | " +
                                  $"{(request != null ? $"Request is sent ({request.Index})" : " - ")}" +
                                  (nextBlock is AccumulatorBlock a ? $"| Count: {a.QueueSize}" : null));
            }
            else if (block is EndBlock)
            {
                var state = string.Join("", context.Blocks.Select(GetBlockState));
                Console.WriteLine($"State: {state}");
            }

            if (block is EndBlock)
            {
                Console.WriteLine(new string('=', 72));
            }
        }
    }

    private static string GetBlockState(IQueueBlock block)
    {
        while (true)
        {
            switch (block)
            {
                case AccumulatorBlock accumulatorBlock:
                    return accumulatorBlock.QueueSize.ToString();
                case BlockingBlock { IsBlocked: true }:
                    return "0";
                case BlockingBlock blockingBlock:
                    block = blockingBlock.WrappedBlock;
                    continue;
                case DiscardBlock discardBlock:
                    block = discardBlock.WrappedBlock;
                    continue;
                case TimerSourceBlock timerSourceBlock:
                    return timerSourceBlock.RemainingTime.ToString();
                case ProbabilitySourceBlock probabilitySourceBlock:
                    return probabilitySourceBlock.IsBusy ? "1" : "0";
            }

            break;
        }

        return "";
    }
}