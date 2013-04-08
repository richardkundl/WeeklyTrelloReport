using System.Collections.Generic;
using TrelloNet;

/// <summary>
/// Trello card comparer
/// </summary>
public class CardComparer: IComparer<List<Card.Label>>
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
}