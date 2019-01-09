using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColliderElement : MonoBehaviour
{
    public SO_Coefficients elementCoefficients;
    public SO_Coefficients ElementCoefficients { get { return elementCoefficients; } }
}
