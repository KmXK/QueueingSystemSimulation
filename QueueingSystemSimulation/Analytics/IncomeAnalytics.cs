using QueueingSystemSimulation.Analytics.Base;
using QueueingSystemSimulation.QueueBlocks;

namespace QueueingSystemSimulation.Analytics;

public class IncomeAnalytics : Base.Analytics
{
    private readonly double _requestCost;
    
    public double Income { get; private set; }

    public IncomeAnalytics(double requestCost)
    {
        _requestCost = requestCost;
    }
    
    public override void AnalyzeBeforeTick(TickAnalyticsContext context)
    {
    }

    public override void AnalyzeSendingRequest(BlockAnalyticsContext context)
    {
        if (context.ToBlock is EndBlock)
        {
            Income += _requestCost;
        }
    }

    public override void AnalyzeAfterTick(TickAnalyticsContext context)
    {
    }
}