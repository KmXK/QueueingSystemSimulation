using QueueingSystemSimulation.QueueBlocks.Base;

namespace QueueingSystemSimulation.Analytics.Base;

public record BlockAnalyticsContext(
    List<IQueueBlock> Blocks,
    IQueueBlock FromBlock,
    IQueueBlock ToBlock,
    QueueRequest SentRequest,
    int TickNumber
);