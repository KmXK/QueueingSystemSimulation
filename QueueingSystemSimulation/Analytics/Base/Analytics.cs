namespace QueueingSystemSimulation.Analytics.Base;

public abstract class Analytics : IAnalytics
{
    public virtual void AnalyzeSendingRequest(BlockAnalyticsContext context)
    {
    }
    
    public virtual void AnalyzeBeforeTick(TickAnalyticsContext context)
    {
    }

    public virtual void AnalyzeAfterTick(TickAnalyticsContext context)
    {
    }
}