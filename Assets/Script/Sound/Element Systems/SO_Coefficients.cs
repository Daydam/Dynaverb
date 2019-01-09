using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Coefficient calculations", menuName = "Scriptable Objects/Audio/New Coefficient calculations file")]
public class SO_Coefficients : ScriptableObject
{
    public ElementType elementType;

    public float LowDiffusionCoefficient { get { return diffusionCoefficient * CoefficientIndex.LowCoefficientIndexer[elementType]; } }
    public float LowAbsorptionCoefficient { get { return absorptionCoefficient * CoefficientIndex.LowCoefficientIndexer[elementType]; } }

    public float diffusionCoefficient;
    public float MidDiffusionCoefficient { get { return diffusionCoefficient; } }
    public float absorptionCoefficient;
    public float MidAbsorptionCoefficient { get { return absorptionCoefficient; } }
    
    public float HiDiffusionCoefficient { get { return diffusionCoefficient * CoefficientIndex.HiCoefficientIndexer[elementType]; } }
    public float HiAbsorptionCoefficient { get { return absorptionCoefficient * CoefficientIndex.HiCoefficientIndexer[elementType]; } }
}