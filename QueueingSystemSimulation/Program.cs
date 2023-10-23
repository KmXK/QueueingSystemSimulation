using System.Linq.Expressions;
using System.Reflection;
using QueueingSystemSimulation;
using QueueingSystemSimulation.Analytics;
using QueueingSystemSimulation.Analytics.Base;
using QueueingSystemSimulation.QueueBlocks;
using QueueingSystemSimulation.QueueBlocks.Base;

var blocks = new List<IQueueBlock>
{
    new StartBlock(),
    new BlockingBlock(new TimerSourceBlock(2)),
    new AccumulatorBlock(2),
    new DiscardBlock(new ProbabilitySourceBlock(0.48)),
    new ProbabilitySourceBlock(1.0),
    new EndBlock()
};

var analytics = new List<IAnalytics>
{
    // new LogAnalytics(),
    new BandwidthAnalytics(),
    new BlockRateAnalytics(),
    new BusynessAnalytics(),
    new QueueLengthAnalytics(),
    new QueueRequestTimeAnalytics(),
    new RequestInSystemAnalytics(),
    new SystemRequestTimeAnalytics()
};

var system = new QueueSystem(blocks, analytics);

system.Run(100_000);

var indicators = new List<(string, string, Expression)>
{
    ("Абсолютная пропускная способность", "A", (BandwidthAnalytics a) => a.AbsoluteBandwidth),
    ("Относительная пропускная способность", "Q", (BandwidthAnalytics a) => a.RelativeBandwidth),
    ("Вероятность отказа", "P_отк", (BandwidthAnalytics a) => a.FailureRate),
    ("Вероятность блокировки", "P_бл", (BlockRateAnalytics a) => a.BlockRate),
    ("Средняя длина очереди", "L_оч", (QueueLengthAnalytics a) => a.AverageQueueLength),
    ("Среднее число заявок в системе", "L_с", (RequestInSystemAnalytics a) => a.AverageRequestsCount),
    ("Среднее время заявки в очереди", "W_оч", (QueueRequestTimeAnalytics a) => a.AverageRequestTimeInQueue),
    ("Среднее время заявки в системе", "W_с", (SystemRequestTimeAnalytics a) => a.AverageRequestTimeInSystem),
    ("Коэффициент загрузки каналов", "K_кан", (BusynessAnalytics a) =>
        string.Join(", ", a.AverageSourceBusyness.Select(x => x.Value.ToString()))),
};

foreach (var indicator in indicators)
{
    var lambda = (LambdaExpression)indicator.Item3;
    var a = analytics.First(x => x.GetType() == lambda.Parameters.First().Type);
    var result = lambda.Compile().DynamicInvoke(a)?.ToString();
    
    Console.WriteLine($"{indicator.Item1, 40} {"(" + indicator.Item2 + ")", -7} = {result}");
}
