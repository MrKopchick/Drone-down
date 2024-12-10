using UnityEngine;
using System.Collections.Generic;

public class BaseManager : MonoBehaviour
{
    private List<BaseBuilder> basesUnderConstruction = new List<BaseBuilder>();
    private List<BaseBuilder> completedBases = new List<BaseBuilder>();

    public void RegisterBaseUnderConstruction(BaseBuilder builder)
    {
        if (!basesUnderConstruction.Contains(builder))
        {
            basesUnderConstruction.Add(builder);
        }
    }

    public void RegisterCompletedBase(BaseBuilder builder)
    {
        if (!completedBases.Contains(builder))
        {
            completedBases.Add(builder);
            basesUnderConstruction.Remove(builder);
        }
    }

    public List<BaseBuilder> GetBasesUnderConstruction()
    {
        return basesUnderConstruction;
    }

    public List<BaseBuilder> GetCompletedBases()
    {
        return completedBases;
    }
}