using QueueingSystemSimulation.QueueBlocks.Base;

namespace QueueingSystemSimulation.QueueBlocks;

public class BlockingBlock : WrapBlock
{
    private QueueRequest? _blockedRequest;

    public bool IsBlocked => _blockedRequest != null;

    public BlockingBlock(IQueueBlock block) : base(block)
    {
    }

    public override void PerformTick(Func<QueueRequest, bool> tryToSendRequest)
    {
        if (_blockedRequest != null)
        {
            if (tryToSendRequest(_blockedRequest))
            {
                _blockedRequest = null;
            }
            
            return;
        }
        
        WrappedBlock.PerformTick(request =>
        {
            if (!tryToSendRequest(request))
            {
                _blockedRequest = request;
            }
            
            return true;
        });
    }

    public override bool TryAcceptRequest(QueueRequest request, Func<QueueRequest, bool> tryToSendRequest)
    {
        if (_blockedRequest != null)
        {
            return false;
        }
        
        return WrappedBlock.TryAcceptRequest(request, tryToSendRequest);
    }
}