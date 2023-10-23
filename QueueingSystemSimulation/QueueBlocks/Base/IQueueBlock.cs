namespace QueueingSystemSimulation.QueueBlocks.Base;

public interface IQueueBlock
{
    void PerformTick(Func<QueueRequest, bool> tryToSendRequest);

    bool TryAcceptRequest(QueueRequest request, Func<QueueRequest, bool> tryToSendRequest);
}