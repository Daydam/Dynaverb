using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Surround Listener", menuName = "Scriptable Objects/Audio/Surround Listener")]
public class SurroundListener : AudioSuiteProcess
{
    public LayerMask detectionMask;
    public LayerMask DetectionMask { get { return detectionMask; } }
    [Range(0, 22000)]
    public float minimumMufflingFrequency = 100;
    public float MinimumMufflingFrequency { get { return minimumMufflingFrequency; } }
    [Range(0, 22000)]
    public float maximumMufflingFrequency = 10000;
    public float MaximumMufflingFrequency { get { return maximumMufflingFrequency; } }
    [Range(0, 22000)]
    public float maximumSurroundFrequency = 125;
    public float MaximumSurroundFrequency { get { return maximumSurroundFrequency; } }
    [Range(0,1)]
    public float changeSpeed = 0.5f;
    public float ChangeSpeed { get { return changeSpeed; } }
}
