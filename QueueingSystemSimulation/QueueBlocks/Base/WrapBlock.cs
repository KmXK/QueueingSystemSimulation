namespace QueueingSystemSimulation.QueueBlocks.Base;

public abstract class WrapBlock : IQueueBlock
{
    public SourceQueueBlock WrappedBlock { get; private set; }
    
    protected WrapBlock(SourceQueueBlock block)
    {
        WrappedBlock = block;
    }
    
    public abstract void PerformTick(Func<QueueRequest, bool> tryToSendRequest);

    public abstract bool TryAcceptRequest(QueueRequest request, Func<QueueRequest, bool> tryToSendRequest);
}