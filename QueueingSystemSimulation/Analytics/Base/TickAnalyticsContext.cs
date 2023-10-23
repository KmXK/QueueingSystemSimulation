using QueueingSystemSimulation.QueueBlocks.Base;

namespace QueueingSystemSimulation.Analytics.Base;

public record TickAnalyticsContext(
    List<IQueueBlock> Blocks,
    int TickNumber
);