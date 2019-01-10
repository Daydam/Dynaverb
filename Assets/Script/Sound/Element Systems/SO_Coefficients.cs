using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Coefficient calculations", menuName = "Scriptable Objects/Audio/New Coefficient calculations file")]
public class SO_Coefficients : ScriptableObject
{
    public ElementType elementType;

    public float LowDiffusionCoefficient { get { return Mathf.Min(1, diffusionCoefficient * CoefficientIndex.LowCoefficientIndexer[elementType]); } }
    public float LowAbsorptionCoefficient { get { return Mathf.Min(1, absorptionCoefficient * CoefficientIndex.LowCoefficientIndexer[elementType]); } }

    [Range(0,1)]
    public float diffusionCoefficient;
    public float MidDiffusionCoefficient { get { return diffusionCoefficient; } }
    [Range(0,1)]
    public float absorptionCoefficient;
    public float MidAbsorptionCoefficient { get { return absorptionCoefficient; } }
    
    public float HiDiffusionCoefficient { get { return Mathf.Min(1, diffusionCoefficient * CoefficientIndex.HiCoefficientIndexer[elementType]); } }
    public float HiAbsorptionCoefficient { get { return Mathf.Min(1, absorptionCoefficient * CoefficientIndex.HiCoefficientIndexer[elementType]); } }
}