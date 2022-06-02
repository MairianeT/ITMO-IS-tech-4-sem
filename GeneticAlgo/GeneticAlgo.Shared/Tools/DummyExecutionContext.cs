using GeneticAlgo.Shared.Models;

using BenchmarkDotNet.Attributes;

namespace GeneticAlgo.Shared.Tools;


public class DummyExecutionContext : IExecutionContext
{
    private readonly int _pointCount;
    private readonly int _maximumValue;
    private readonly double _maxLenght;
    private readonly BarrierCircle[] _barriers;
    private readonly List<PointMap> _pointMaps;

    public DummyExecutionContext(int pointCount, int maximumValue, double maxLenght, BarrierCircle[] barrierCircles)
    {
        _pointCount = pointCount;
        _maximumValue = maximumValue;
        _barriers = barrierCircles;
        _maxLenght = maxLenght;
        _pointMaps = new List<PointMap>(_pointCount);
    }

    public void Reset()
    {
        _pointMaps.Clear();
    }
    
    
    private void Sort()
    {
        _pointMaps.Sort(new SortByDistance());
    }

    
    private void Update(int percent)
    {
        Sort();

        for (int i = 0; i < _pointCount * percent / 100; i++)
        {
            _pointMaps[i].NewWay(_pointMaps[^(i+1)].PointWay);
        }
    }

    
    public void NextStep()
    {
        foreach (var point in _pointMaps)
        {
            point.Next();
        }
    }

    public Task<IterationResult> ExecuteIterationAsync()
    {
        return Task.FromResult(IterationResult.IterationFinished);
    }

    
    public bool ReportStatistics(IStatisticsConsumer statisticsConsumer)
    {
        Statistic[] statistics = new Statistic[_pointCount];
        if (_pointMaps.Count < _pointCount)
        {
            for (int j = 0; j < _pointCount; j++)
            {
                _pointMaps.Add(new PointMap(_maxLenght, _maximumValue, _barriers));
            }
        }

        NextStep();
        Update(10);

        for (int i = 0; i < _pointCount; i++)
        {
            statistics[i] = new Statistic(i, _pointMaps[i].PointWay, _pointMaps[i].ToTarget());
        }


        statisticsConsumer.Consume(statistics, _barriers);

        return !_pointMaps.All(point => point.ToTarget() < 0.0004);
    }
}