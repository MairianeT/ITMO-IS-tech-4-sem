using GeneticAlgo.Shared.Models;

namespace GeneticAlgo.Shared.Tools;

public class PointMap
{
    
    private readonly Random _random;
    private readonly double _maxLenght;
    private readonly int _maxValue;
    private readonly BarrierCircle[] _barriers;
    public PointMap(double maxLenght, int maxValue, BarrierCircle[] barrierCircles)
    {
        _random = Random.Shared;
        PointWay = new Point(_random.NextDouble() * _maxLenght, _random.NextDouble() * _maxLenght);
        _maxLenght = maxLenght;
        _maxValue = maxValue;
        _barriers = barrierCircles;
    }

    public Point PointWay { get; private set; }

    public double GetLastX()
    {
        return PointWay.X;
    }
    
    public double GetLastY()
    {
        return PointWay.Y;
    }

    public void Next()
    {
        if (ToTarget() < 0.0004)
        {
            return;
        }

        var x = GetLastX() + _random.NextDouble() * _maxLenght * Math.Pow(-1, _random.Next());
        var y = GetLastY() + _random.NextDouble() * _maxLenght * Math.Pow(-1, _random.Next());
        
        if (x > _maxValue)
            x = _maxValue;
        if (y > _maxValue)
            y = _maxValue;
        if (x < 0)
            x = 0;
        if (y < 0)
            y = 0;


        foreach (var barrier in _barriers)
        {
            if (!IsCloseToBarrier(x, y, barrier)) continue;
            x = GetLastX();
            y = GetLastY();
        }

        PointWay = new Point(x, y);
    }

    private bool IsCloseToBarrier(double x, double y, BarrierCircle barrier)
    {
        return Math.Sqrt((
            (x - barrier.Center.X) * (x - barrier.Center.X)
            + (y - barrier.Center.Y) * (y - barrier.Center.Y)
        )) < barrier.Radius;
    }

    public double ToTarget()
    {
        return Math.Sqrt((_maxValue - PointWay.X) * (_maxValue - PointWay.X)
                         + (_maxValue - PointWay.Y) * (_maxValue - PointWay.Y));
    }

    public void NewWay(Point newWay)
    {
        PointWay = newWay;
    }
}