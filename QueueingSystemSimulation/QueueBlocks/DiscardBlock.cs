using QueueingSystemSimulation.QueueBlocks.Base;

namespace QueueingSystemSimulation.QueueBlocks;

public class DiscardBlock : WrapBlock
{
    public DiscardBlock(SourceQueueBlock block) : base(block)
    {
    }
    
    public override void PerformTick(Func<QueueRequest, bool> tryToSendRequest)
    {
        WrappedBlock.PerformTick(request =>
        {
            tryToSendRequest(request);
            return true;
        });
    }

    public override bool TryAcceptRequest(QueueRequest request, Func<QueueRequest, bool> tryToSendRequest) => 
        WrappedBlock.TryAcceptRequest(request, tryToSendRequest);
}