using QueueingSystemSimulation.Analytics.Base;
using QueueingSystemSimulation.QueueBlocks;
using QueueingSystemSimulation.QueueBlocks.Base;

namespace QueueingSystemSimulation;

public class QueueSystem
{
    private readonly List<IQueueBlock> _blocks;
    private readonly List<IAnalytics> _analytics;

    public QueueSystem(
        IEnumerable<IQueueBlock> blocks,
        IEnumerable<IAnalytics> analytics)
    {
        _blocks = blocks.ToList();
        _analytics = analytics.ToList();
    }

    public void Run(int tickNumber)
    {
        for (var i = 0; i < tickNumber; i++)
        {
            AnalyzeBeforeTick(i);
            
            for (var blockIndex = _blocks.Count - 1; blockIndex >= 0; blockIndex--)
            {
                var block = _blocks[blockIndex];
                
                block.PerformTick(r =>
                {
                    var transfers = new List<(IQueueBlock from, IQueueBlock to, QueueRequest req)>();
                    
                    var result = TrySendRequestToNextBlock(r, blockIndex);

                    transfers.Reverse();
                    transfers.ForEach(t => AnalyzeSendingRequest(t.req, t.from, t.to, i));
                    
                    return result;

                    bool TrySendRequestToNextBlock(QueueRequest request, int index)
                    {
                        if (index >= _blocks.Count - 1)
                        {
                            return false;
                        }
                        
                        var nextBlock = _blocks[index + 1];
                        
                        var result = nextBlock.TryAcceptRequest(
                            request,
                            req => TrySendRequestToNextBlock(req, index + 1));

                        if (result)
                        {
                            transfers.Add((_blocks[index], nextBlock, request));
                        }
                        
                        return result;
                    }
                });
            }
            
            AnalyzeAfterTick(i);
        }
    }

    private void AnalyzeBeforeTick(int tickNumber)
    {
        _analytics.ForEach(a =>
            a.AnalyzeBeforeTick(new TickAnalyticsContext(
                _blocks,
                tickNumber)));
    }
    
    private void AnalyzeAfterTick(int tickNumber)
    {
        _analytics.ForEach(a =>
            a.AnalyzeAfterTick(new TickAnalyticsContext(
                _blocks,
                tickNumber)));
    }

    private void AnalyzeSendingRequest(
        QueueRequest request,
        IQueueBlock fromBlock,
        IQueueBlock? toBlock,
        int tickNumber)
    {
        _analytics.ForEach(a =>
            a.AnalyzeSendingRequest(new BlockAnalyticsContext(
                _blocks,
                fromBlock,
                toBlock,
                request,
                tickNumber)));
    }
}