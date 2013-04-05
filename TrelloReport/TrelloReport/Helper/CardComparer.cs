using System.Collections.Generic;
using TrelloNet;

public static class CardComparer
{
    public static int CompareByLabel(List<Card.Label> labelsA, List<Card.Label> labelsB)
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