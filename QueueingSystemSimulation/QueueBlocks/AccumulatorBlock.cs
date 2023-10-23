using QueueingSystemSimulation.QueueBlocks.Base;

namespace QueueingSystemSimulation.QueueBlocks;

public class AccumulatorBlock : IQueueBlock
{
    private readonly int _capacity;
    private readonly Queue<QueueRequest> _queue;

    public int QueueSize => _queue.Count;

    public AccumulatorBlock(int capacity)
    {
        if (capacity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity));
        }

        _capacity = capacity;
        _queue = new Queue<QueueRequest>(capacity);
    }
    
    public void PerformTick(Func<QueueRequest, bool> tryToSendRequest)
    {
        if (_queue.Any() && tryToSendRequest(_queue.Peek()))
        {
            _queue.Dequeue();
        }
    }

    public bool TryAcceptRequest(QueueRequest request, Func<QueueRequest, bool> tryToSendRequest)
    {
        if (_queue.Count == _capacity)
        {
            return false;
        }

        if (!tryToSendRequest(request))
        {
            _queue.Enqueue(request);
        }

        return true;
    }
}