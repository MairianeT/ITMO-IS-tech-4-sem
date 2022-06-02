using GeneticAlgo.Shared.Models;

namespace GeneticAlgo.Shared.Tools;

public class SortByDistance : IComparer<PointMap>
{
    public int Compare(PointMap? o1, PointMap? o2)
    { 
        return o2.ToTarget().CompareTo(o1.ToTarget());
    }
}