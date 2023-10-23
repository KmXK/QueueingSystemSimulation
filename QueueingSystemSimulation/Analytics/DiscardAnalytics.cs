using QueueingSystemSimulation.Analytics.Base;
using QueueingSystemSimulation.QueueBlocks;

namespace QueueingSystemSimulation.Analytics;

public class DiscardAnalytics : Base.Analytics
{
    private readonly Dictionary<DiscardBlock, QueueRequest?> _requests = new();
    private readonly HashSet<QueueRequest> _discardedRequests = new();

    public IReadOnlySet<QueueRequest> DiscardedRequests => _discardedRequests;
    
    public override void AnalyzeBeforeTick(TickAnalyticsContext context)
    {
        _requests.Clear();

        var blocks = context.Blocks
            .Where(x => x is DiscardBlock)
            .Select(x =>
            {
                var discardBlock = (DiscardBlock)x;

                return (discardBlock, request: discardBlock.WrappedBlock.Request);
            })
            .Where(x => x.request != null);
        
        foreach (var (discardBlock, request) in blocks)
        {
            _requests.Add(discardBlock, request!);
        }
    }

    public override void AnalyzeSendingRequest(BlockAnalyticsContext context)
    {
        if (context.FromBlock is DiscardBlock d && _requests.ContainsKey(d))
        {
            _requests[d] = null;
        }
    }

    public override void AnalyzeAfterTick(TickAnalyticsContext context)
    {
        var discardedRequests = _requests
            .Where(kvp => ((DiscardBlock)context.Blocks
                    .First(x => ReferenceEquals(x, kvp.Key)))
                .WrappedBlock.Request != kvp.Value)
            .Select(x => x.Value);

        foreach (var discardedRequest in discardedRequests)
        {
            _discardedRequests.Add(discardedRequest);
        }
    }

    public void Clear()
    {
        _discardedRequests.Clear();
    }
}