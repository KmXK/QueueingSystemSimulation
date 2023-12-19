using QueueingSystemSimulation.QueueBlocks.Base;

namespace QueueingSystemSimulation.QueueBlocks;

public class ExponentialSourceBlock : SourceQueueBlock
{
    private readonly double _rate;
    private readonly Random _random = new();
    
    private int _timeLeft;

    public ExponentialSourceBlock(double rate)
    {
        if (rate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rate));
        }

        _rate = rate;
    }

    protected override void TryToProcessRequest(Action processRequest)
    {
        if (_timeLeft == 0)
        {
            _timeLeft = (int)Math.Round(-Math.Log(_random.NextDouble(), Math.E) / _rate);
        }
        else
        {
            _timeLeft--;

            if (_timeLeft == 0)
            {
                processRequest();
            }
        }
    }
}