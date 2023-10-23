using QueueingSystemSimulation.Analytics.Base;
using QueueingSystemSimulation.QueueBlocks;

namespace QueueingSystemSimulation.Analytics;

public class QueueRequestTimeAnalytics : Base.Analytics
{
    private readonly Dictionary<QueueRequest, int> _requestLifetimes = new();
    private readonly HashSet<QueueRequest> _requestsInQueue = new();

    public double AverageRequestTimeInQueue => _requestLifetimes.Average(x => x.Value);
    
    public override void AnalyzeSendingRequest(BlockAnalyticsContext context)
    {
        _requestLifetimes.TryAdd(context.SentRequest, 0);
        
        if (context.FromBlock is AccumulatorBlock)
        {
            if (_requestsInQueue.Contains(context.SentRequest))
            {
                _requestsInQueue.Remove(context.SentRequest);
            }
        }
        
        if (context.ToBlock is AccumulatorBlock)
        {   
            _requestsInQueue.Add(context.SentRequest);
        }
    }

    public override void AnalyzeAfterTick(TickAnalyticsContext context)
    {
        foreach (var request in _requestsInQueue)
        {
            _requestLifetimes[request]++;
        }
    }
}