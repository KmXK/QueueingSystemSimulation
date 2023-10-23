using QueueingSystemSimulation.QueueBlocks.Base;

namespace QueueingSystemSimulation.QueueBlocks;

public class TimerSourceBlock : SourceQueueBlock
{
    private readonly int _period;

    private int _remainingTime;
    
    public int RemainingTime => _remainingTime;

    public TimerSourceBlock(int period)
    {
        _period = period;
        _remainingTime = period;
    }
    
    protected override void TryToProcessRequest(Action processRequest)
    {
        if (_remainingTime == 1)
        {
            processRequest();
            _remainingTime = _period;
        }
        else
        {
            _remainingTime--;
        }
    }
}