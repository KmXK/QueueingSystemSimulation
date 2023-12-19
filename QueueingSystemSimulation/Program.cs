using QueueingSystemSimulation;
using QueueingSystemSimulation.Analytics;
using QueueingSystemSimulation.Analytics.Base;
using QueueingSystemSimulation.QueueBlocks;
using QueueingSystemSimulation.QueueBlocks.Base;

const int ticksCount = 10000;

const double requestGenerationPeriod = 4.0;
const double requestHandleTime = 0.8;
const double requestCost = 4;
const double channelCost = 2;
const double queueSlotCost = 0.3;

const double tickMultiplier = 60;

Console.WriteLine($"Количество каналов \u2502 Размер очереди \u2502 {"Доход", -7} \u2502 {"Расходы", -7} \u2502 {"Прибыль", 7}");
Console.WriteLine($"───────────────────┼────────────────┼{new string('─',9)}┼{new string('─',9)}┼{new string('─',8)}");

for (var i = 2; i <= 10; i++)
{
    var channelCount = i == 3 ? i : 2;
    var queueSize = i > 3 ? i - 3 : 0;
    
    var blocks = GetBlocks(channelCount, queueSize);
    
    var analytics = new List<IAnalytics>
    {
        // new LogAnalytics(),
        new IncomeAnalytics(requestCost),
        new OutcomeAnalytics(channelCost, queueSlotCost)
    };

    var system = new QueueSystem(blocks, analytics);

    system.Run(ticksCount);

    var income = GetAnalytic<IncomeAnalytics>().Income * tickMultiplier / ticksCount;
    var outcome = GetAnalytic<OutcomeAnalytics>().Outcome / ticksCount;
    var profit = income - outcome;

    Console.WriteLine($"{channelCount, -18} \u2502 " +
                      $"{queueSize, -14} \u2502 " +
                      $"{income, 7 : 0.####} \u2502 " +
                      $"{outcome, 7 : 0.####} \u2502 " +
                      $"{profit, 7 : 0.####}");

    continue;

    T GetAnalytic<T>() where T : IAnalytics
    {
        return (T)analytics.First(x => x.GetType() == typeof(T));
    }
}

return;

IEnumerable<IQueueBlock> GetBlocks(int channelCount, int queueSize)
{
    var blocks = new List<IQueueBlock>();
    
    blocks.Add(new StartBlock());
    blocks.Add(new DiscardBlock(new ExponentialSourceBlock(requestGenerationPeriod / tickMultiplier)));
    
    if (queueSize > 0)
    {
        blocks.Add(new AccumulatorBlock(queueSize));
    }

    var channels = Enumerable
        .Repeat(1, channelCount)
        .Select(_ => new TimerSourceBlock((int)(requestHandleTime * tickMultiplier)));
    
    blocks.Add(new UnionBlock(channels));
    
    blocks.Add(new EndBlock());

    return blocks;
}