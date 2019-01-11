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
    float absorptionFactor = 0;
    float currentListenerAngle = 0;

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
        absorptionFactor = 0;
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                absorptionFactor += hits[i].collider.GetComponent<ColliderElement>().ElementCoefficients.MidAbsorptionCoefficient;
            }
            absorptionFactor = Mathf.Min(1, absorptionFactor);
        }
        float newFrequency = absorptionFactor > 0? Mathf.Lerp(SurroundListener.Instance.MaximumMufflingFrequency, SurroundListener.Instance.MinimumMufflingFrequency, absorptionFactor): 22000;
        lowPassFilter.cutoffFrequency = Mathf.Lerp(lowPassFilter.cutoffFrequency, newFrequency, SurroundListener.Instance.ChangeSpeed);

        currentListenerAngle = Vector3.Angle(-distanceToListener.normalized, SurroundListener.Instance.transform.forward);
        if (currentListenerAngle > 90f)
        {
            hiPassFilter.cutoffFrequency = Mathf.Lerp(0, SurroundListener.Instance.MaximumSurroundFrequency, (currentListenerAngle - 90) / 90);
        }
    }

    void OnDrawGizmos()
    {
        if(Application.isPlaying)
        {
            Gizmos.color = Color.Lerp(Color.blue, Color.red, absorptionFactor);
            Gizmos.DrawLine(transform.position, SurroundListener.Instance.transform.position);
        }
    }
}
