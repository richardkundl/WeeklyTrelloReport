using System.Collections.Generic;
using TrelloNet;

/// <summary>
/// Trello card comparer
/// </summary>
public class CardComparer : IComparer<List<Card.Label>>, IEqualityComparer<List<Card.Label>>
{
    /// <summary>
    /// Custom compare to card labels
    /// </summary>
    /// <param name="labelsA">(A) Card labels</param>
    /// <param name="labelsB">(B) Card labels</param>
    /// <returns>A-B relation</returns>
    public int Compare(List<Card.Label> labelsA, List<Card.Label> labelsB)
    {
        if ((labelsA == null || labelsA.Count == 0) &&
            (labelsB == null || labelsB.Count == 0))
        {
            return 0;
        }

        if ((labelsA != null || labelsA.Count > 0) &&
            (labelsB == null || labelsB.Count == 0))
        {
            return -1;
        }

        if ((labelsA == null || labelsA.Count == 0) &&
            (labelsB != null || labelsB.Count > 0))
        {
            return 1;
        }

        if (labelsA.Count > labelsB.Count)
        {
            return -1;
        }

        if (labelsA.Count < labelsB.Count)
        {
            return 1;
        }

        for (int i = 0; i < labelsA.Count; i++)
        {
            var compareVal = labelsA[i].Name.CompareTo(labelsB[i].Name);
            if (compareVal != 0)
            {
                return compareVal;
            }
        }

        return 0;
    }

    /// <summary>
    /// Override base.Equals
    /// </summary>
    /// <param name="x">Card labels</param>
    /// <param name="y">Card labels</param>
    /// <returns>is equals</returns>
    public bool Equals(List<Card.Label> x, List<Card.Label> y)
    {
        if (Compare(x, y) == 0)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Generate custom hashcode from labels
    /// </summary>
    /// <param name="obj">card labels</param>
    /// <returns>hash code</returns>
    public int GetHashCode(List<Card.Label> obj)
    {
        int hash = 13;
        foreach (var label in obj)
        {
            hash = (hash * 7) + label.Name.GetHashCode();
            hash = (hash * 7) + label.Color.GetHashCode();
        }

        return hash;
    }
}