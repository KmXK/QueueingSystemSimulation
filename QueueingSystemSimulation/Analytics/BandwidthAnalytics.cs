using QueueingSystemSimulation.Analytics.Base;
using QueueingSystemSimulation.QueueBlocks;

namespace QueueingSystemSimulation.Analytics;

public class BandwidthAnalytics : Base.Analytics
{
    private int _enteredRequests;
    private int _exitedRequests;
    private int _ticks;
    
    public double AbsoluteBandwidth => 1.0 * _exitedRequests / _ticks;
    
    public double RelativeBandwidth => 1.0 * _exitedRequests / _enteredRequests;
    
    public double FailureRate => 1 - RelativeBandwidth;

    public override void AnalyzeSendingRequest(BlockAnalyticsContext context)
    {
        if (context.FromBlock is StartBlock)
        {
            _enteredRequests++;
        }

        if (context.ToBlock is EndBlock)
        {
            _exitedRequests++;
        }
    }

    public override void AnalyzeAfterTick(TickAnalyticsContext context)
    {
        _ticks++;
    }
}