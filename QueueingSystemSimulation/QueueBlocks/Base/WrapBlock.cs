namespace QueueingSystemSimulation.QueueBlocks.Base;

public abstract class WrapBlock : IQueueBlock
{
    public IQueueBlock WrappedBlock { get; private set; }
    
    protected WrapBlock(IQueueBlock block)
    {
        WrappedBlock = block;
    }
    
    public abstract void PerformTick(Func<QueueRequest, bool> tryToSendRequest);

    public abstract bool TryAcceptRequest(QueueRequest request, Func<QueueRequest, bool> tryToSendRequest);
}