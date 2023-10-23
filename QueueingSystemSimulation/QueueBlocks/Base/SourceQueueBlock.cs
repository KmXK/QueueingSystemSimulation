namespace QueueingSystemSimulation.QueueBlocks.Base;

public abstract class SourceQueueBlock : IQueueBlock
{
    private QueueRequest? _request;

    public bool IsBusy => _request != null;
    public QueueRequest? Request => _request;

    public void PerformTick(Func<QueueRequest, bool> tryToSendRequest)
    {
        if (_request == null)
        {
            return;
        }

        TryToProcessRequest(() =>
        {
            if (tryToSendRequest(_request))
            {
                _request = null;
            }
        });
    }

    public bool TryAcceptRequest(QueueRequest request, Func<QueueRequest, bool> tryToSendRequest)
    {
        if (_request != null)
        {
            return false;
        }
        
        _request = request;
        return true;

    }

    protected abstract void TryToProcessRequest(Action processRequest);
}