using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurroundListener : MonoBehaviour
{
    static SurroundListener instance;
    public static SurroundListener Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<SurroundListener>();
            if (instance == null)
            {
                instance = new GameObject("ReverbManager").AddComponent<SurroundListener>();
            }
            return instance;
        }
    }

    public LayerMask detectionMask;
    public LayerMask DetectionMask { get { return detectionMask; } }
    [Range(0, 22000)]
    public float minimumFrequencyForWallthrough = 100;
    public float MinimumFrequencyForWallthrough { get { return minimumFrequencyForWallthrough; } }
    [Range(0, 22000)]
    public float maximumFrequencyForWallthrough = 10000;
    public float MaximumFrequencyForWallthrough { get { return maximumFrequencyForWallthrough; } }
    [Range(0,1)]
    public float changeSpeed = 0.5f;
    public float ChangeSpeed { get { return changeSpeed; } }
}
