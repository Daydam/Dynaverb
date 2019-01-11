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
        Vector3 distanceToListener = AudioSuite.Instance.transform.position - transform.position;
        var hits = Physics.RaycastAll(transform.position, distanceToListener.normalized, distanceToListener.magnitude, AudioSuite.Instance.GetSurroundListener.DetectionMask);
        absorptionFactor = 0;
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                absorptionFactor += hits[i].collider.GetComponent<ColliderElement>().ElementCoefficients.MidAbsorptionCoefficient;
            }
            absorptionFactor = Mathf.Min(1, absorptionFactor);
        }
        float newFrequency = absorptionFactor > 0? Mathf.Lerp(AudioSuite.Instance.GetSurroundListener.MaximumMufflingFrequency, AudioSuite.Instance.GetSurroundListener.MinimumMufflingFrequency, absorptionFactor): 22000;
        lowPassFilter.cutoffFrequency = Mathf.Lerp(lowPassFilter.cutoffFrequency, newFrequency, AudioSuite.Instance.GetSurroundListener.ChangeSpeed);

        currentListenerAngle = Vector3.Angle(-distanceToListener.normalized, AudioSuite.Instance.transform.forward);
        if (currentListenerAngle > 90f)
        {
            hiPassFilter.cutoffFrequency = Mathf.Lerp(0, AudioSuite.Instance.GetSurroundListener.MaximumSurroundFrequency, (currentListenerAngle - 90) / 90);
        }
    }

    void OnDrawGizmos()
    {
        if(Application.isPlaying && AudioSuite.Instance.GetSurroundListener != default(SurroundListener))
        {
            Gizmos.color = Color.Lerp(Color.blue, Color.red, absorptionFactor);
            Gizmos.DrawLine(transform.position, AudioSuite.Instance.transform.position);
        }
    }
}
