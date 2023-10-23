using QueueingSystemSimulation.QueueBlocks.Base;

namespace QueueingSystemSimulation.QueueBlocks;

public class StartBlock : IQueueBlock
{
    private QueueRequest _request = new(1);
    
    public void PerformTick(Func<QueueRequest, bool> tryToSendRequest)
    {
        if (tryToSendRequest(_request))
        {
            _request = new QueueRequest(_request.Index + 1);
        }
    }

    public bool TryAcceptRequest(QueueRequest request, Func<QueueRequest, bool> tryToSendRequest) => false;
}