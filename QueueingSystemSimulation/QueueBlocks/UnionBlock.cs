using QueueingSystemSimulation.QueueBlocks.Base;

namespace QueueingSystemSimulation.QueueBlocks;

public class UnionBlock : IQueueBlock
{
    private readonly Dictionary<IQueueBlock, QueueRequest?> _requests = new();

    public IReadOnlyDictionary<IQueueBlock, QueueRequest?> Requests => _requests;
    
    public int BlocksCount => _requests.Count;

    public UnionBlock(params IQueueBlock[] blocks) : this((IEnumerable<IQueueBlock>)blocks)
    {
    }

    public UnionBlock(IEnumerable<IQueueBlock> blocks)
    {
        foreach (var block in blocks)
        {
            _requests[block] = null;
        }
    }

    public void PerformTick(Func<QueueRequest, bool> tryToSendRequest)
    {
        foreach (var block in _requests.Keys)
        {
            block.PerformTick(tryToSendRequest);
        }
    }

    public bool TryAcceptRequest(QueueRequest request, Func<QueueRequest, bool> tryToSendRequest)
    {
        var freeBlocks = _requests.Where(x => x.Value is null);
        
        foreach (var (block, _) in freeBlocks)
        {
            if (block.TryAcceptRequest(request, tryToSendRequest))
            {
                return true;
            }
        }

        return false;
    }
}