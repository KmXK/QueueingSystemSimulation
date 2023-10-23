namespace QueueingSystemSimulation.Analytics.Base;

public interface IAnalytics
{
    void AnalyzeSendingRequest(BlockAnalyticsContext context);
    
    void AnalyzeBeforeTick(TickAnalyticsContext context);
    
    void AnalyzeAfterTick(TickAnalyticsContext context);
}