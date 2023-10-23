using QueueingSystemSimulation.QueueBlocks.Base;

namespace QueueingSystemSimulation.QueueBlocks;

public class EndBlock : IQueueBlock
{
    public void PerformTick(Func<QueueRequest, bool> tryToSendRequest)
    {
    }

    public bool TryAcceptRequest(QueueRequest request, Func<QueueRequest, bool> tryToSendRequest) => true;
}