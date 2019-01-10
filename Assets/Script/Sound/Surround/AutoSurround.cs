using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioLowPassFilter))]
[RequireComponent(typeof(AudioHighPassFilter))]
public class AutoSurround : MonoBehaviour
{
    AudioLowPassFilter lowPassFilter;
    AudioHighPassFilter hiPassFilter;

    void Start()
    {
        lowPassFilter = GetComponent<AudioLowPassFilter>();
        lowPassFilter.cutoffFrequency = 22000;
        hiPassFilter = GetComponent<AudioHighPassFilter>();
        hiPassFilter.cutoffFrequency = 0;
    }

    void Update()
    {
        Vector3 distanceToListener = SurroundListener.Instance.transform.position - transform.position;
        var hits = Physics.RaycastAll(transform.position, distanceToListener.normalized, distanceToListener.magnitude, SurroundListener.Instance.DetectionMask);
        float lowAbsorptionFactor = 0;
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                lowAbsorptionFactor += hits[i].collider.GetComponent<ColliderElement>().ElementCoefficients.MidAbsorptionCoefficient;
            }
            lowAbsorptionFactor = Mathf.Min(1, lowAbsorptionFactor);
        }
        float newFrequency = lowAbsorptionFactor > 0? Mathf.Lerp(SurroundListener.Instance.MaximumFrequencyForWallthrough, SurroundListener.Instance.MinimumFrequencyForWallthrough, lowAbsorptionFactor): 22000;
        lowPassFilter.cutoffFrequency = Mathf.Lerp(lowPassFilter.cutoffFrequency, newFrequency, SurroundListener.Instance.ChangeSpeed);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, SurroundListener.Instance.transform.position);
    }
}
