using QueueingSystemSimulation.Analytics.Base;
using QueueingSystemSimulation.QueueBlocks;

namespace QueueingSystemSimulation.Analytics;

public class RequestInSystemAnalytics : Base.Analytics
{
    private readonly DiscardAnalytics _discardAnalytics = new();
    
    private readonly List<double> _requestsCount = new();
    private readonly HashSet<QueueRequest> _requests = new();

    public double AverageRequestsCount => _requestsCount.Average();

    public override void AnalyzeBeforeTick(TickAnalyticsContext context)
    {
        _discardAnalytics.AnalyzeBeforeTick(context);
    }

    public override void AnalyzeSendingRequest(BlockAnalyticsContext context)
    {
        _discardAnalytics.AnalyzeSendingRequest(context);
        
        if (context.FromBlock is StartBlock)
        {
            _requests.Add(context.SentRequest);
        }
        
        if (context.ToBlock is EndBlock)
        {
            _requests.Remove(context.SentRequest);
        }
    }

    public override void AnalyzeAfterTick(TickAnalyticsContext context)
    {
        _discardAnalytics.AnalyzeAfterTick(context);

        if (_discardAnalytics.DiscardedRequests.Any())
        {
            foreach (var request in _discardAnalytics.DiscardedRequests)
            {
                _requests.Remove(request);
            }
            
            _discardAnalytics.Clear();
        }
        
        _requestsCount.Add(_requests.Count);
    }
}