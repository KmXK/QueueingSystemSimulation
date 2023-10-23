using QueueingSystemSimulation.QueueBlocks.Base;

namespace QueueingSystemSimulation.QueueBlocks;

public class ProbabilitySourceBlock : SourceQueueBlock
{
    private readonly double _probability;
    private readonly Random _random = new(4);

    public ProbabilitySourceBlock(double probability)
    {
        if (probability is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(probability));
        }
        
        _probability = probability;
    }

    protected override void TryToProcessRequest(Action processRequest)
    {
        if (CompareDouble(_probability, 0) || _probability <= _random.NextDouble())
        {
            processRequest();
        }
    }

    private static bool CompareDouble(double d1, double d2)
    {
        return Math.Abs(d1 - d2) < 0.001;
    }
}