using System.Runtime.CompilerServices;
using QueueingSystemSimulation.Analytics.Base;
using QueueingSystemSimulation.QueueBlocks;

namespace QueueingSystemSimulation.Analytics;

public class SystemRequestTimeAnalytics : Base.Analytics
{
    private readonly DiscardAnalytics _discardAnalytics = new();
    
    private readonly Dictionary<QueueRequest, int> _requestLifetimes = new();
    private readonly Dictionary<QueueRequest, int> _requestsInSystem = new();

    private readonly HashSet<QueueRequest> _exitedRequests = new();

    public double AverageRequestTimeInSystem => _requestLifetimes.Any()
        ? _requestLifetimes.Average(x => x.Value)
        : 0;

    public override void AnalyzeBeforeTick(TickAnalyticsContext context)
    {
        _discardAnalytics.AnalyzeBeforeTick(context);

        _exitedRequests.Clear();
    }

    public override void AnalyzeSendingRequest(BlockAnalyticsContext context)
    {
        _discardAnalytics.AnalyzeSendingRequest(context);
        
        _requestLifetimes.TryAdd(context.SentRequest, 0);
        
        if (context.FromBlock is StartBlock)
        {   
            _requestsInSystem.Add(context.SentRequest, 0);
        }
        
        if (context.ToBlock is EndBlock)
        {
            _exitedRequests.Add(context.SentRequest);
        }
    }

    public override void AnalyzeAfterTick(TickAnalyticsContext context)
    {
        _discardAnalytics.AnalyzeAfterTick(context);

        foreach (var request in _requestsInSystem.Keys
                     .Where(x => _discardAnalytics.DiscardedRequests.Contains(x)))
        {
            _exitedRequests.Add(request);
        }

        foreach (var exitedRequest in _exitedRequests)
        {
            if (_requestsInSystem.TryGetValue(exitedRequest, out var requestTimeInSystem))
            {
                _requestsInSystem.Remove(exitedRequest);
            }
        }
        foreach (var request in _requestsInSystem.Keys)
        {
            _requestLifetimes[request]++;
        }
    }
}