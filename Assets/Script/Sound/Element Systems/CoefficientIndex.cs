using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoefficientIndex
{
    public static Dictionary<ElementType, float> LowCoefficientIndexer = new Dictionary<ElementType, float>()
    {
        { ElementType.POROUS, 0.1f},
        { ElementType.HEMHOLTZ, 0.2f},
        { ElementType.PANEL, 5f},
    };

    public static Dictionary<ElementType, float> HiCoefficientIndexer = new Dictionary<ElementType, float>()
    {
        { ElementType.POROUS, 1.1f},
        { ElementType.HEMHOLTZ, 0.7f},
        { ElementType.PANEL, 1f},
    };
}

public enum ElementType
{
    POROUS,
    HEMHOLTZ,
    PANEL,
}
