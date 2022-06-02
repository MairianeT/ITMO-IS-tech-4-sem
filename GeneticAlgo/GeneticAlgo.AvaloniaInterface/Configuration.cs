using System.Collections.Generic;
using GeneticAlgo.Shared.Models;

namespace GeneticAlgo.AvaloniaInterface;

public class Configuration
{
    public Configuration(int pointsCount, double maxLenght, int maxValue, BarrierCircle[] circles)
    {
        PointsCount = pointsCount;
        MaxLenght = maxLenght;
        MaxValue = maxValue;
        Circles = circles;
    }
    public int PointsCount { get; set; }
    public double MaxLenght { get; set; }

    public int MaxValue { get; set; }
    
    public BarrierCircle[] Circles { get; set; }
}