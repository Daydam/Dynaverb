using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;

[RequireComponent(typeof(AudioListener))]
public class ReverbManager : MonoBehaviour
{
    static ReverbManager instance;
    public static ReverbManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<ReverbManager>();
            if (instance == null)
            {
                instance = new GameObject("ReverbManager").AddComponent<ReverbManager>();
            }
            return instance;
        }
    }

    //We need the Raycast detection system. Maybe use Gizmos to show the hits.
    DetectionRay[] detectionRays;
    public AudioMixer reverbFX;
    public int maxRayAmount = 8;
    public float shortDistanceThreshold;
    public float longDistanceThreshold;
    [Range(-1f, 0f)]
    public float minimumYDetection = -0.2f;
    [Range(-100f, 0f)]
    public float maxReflectionVolume = 0;
    [Range(0, 1)]
    public float absorptionCoeficient = 0;
    float maxVolumeW;
    [Range(0f,1f)]
    public float changeSensitivity = 1f;
    [Range(0f, 20f)]
    public float maxDecayTime = 1.6f;

    void Start()
    {
        detectionRays = new DetectionRay[maxRayAmount];
        for (int i = 0; i < detectionRays.Length; i++)
        {
            detectionRays[i] = new DetectionRay();
        }

        maxVolumeW = Mathf.Pow(10, (maxReflectionVolume + 100) / 20);
    }

    void Update()
    {
        longDistanceThreshold = Mathf.Max(longDistanceThreshold, shortDistanceThreshold);
        for (int i = 0; i < detectionRays.Length; i++)
        {
            detectionRays[i].detectionRay.origin = transform.position;
            detectionRays[i].detectionRay.direction = new Vector3(Random.Range(-1f, 1f), Random.Range(minimumYDetection, 1f), Random.Range(-1f, 1f));
            if(Physics.Raycast(detectionRays[i].detectionRay, out detectionRays[i].hitInfo))
            {
                detectionRays[i].detectedHit = true;
                //Check the hit distance, angle maybe?
                //hitInfo[i].distance

                //Angle
                /*float cosine = Vector3.Dot(detectionRays[i].detectionRay.direction, detectionRays[i].hitInfo.normal);
                float angle = Mathf.Acos(cosine);*/
            }
            else
            {
                detectionRays[i].detectedHit = false;
            }
        }
        var hitsOrderedByDistance = detectionRays.Where(a => a.detectedHit).Select(a => a.hitInfo.distance).OrderBy(a => a);
        float tempForGetValues;

        #region Early Reflections
        var shortDistanceHits = hitsOrderedByDistance.SkipWhile(a => a < shortDistanceThreshold).TakeWhile(a => a < longDistanceThreshold);
        float shortestDistance = shortDistanceHits.Count() > 0? shortDistanceHits.First() : longDistanceThreshold;

        float preDelayER = shortestDistance / 342;

        float volumePercentage = 1 - shortestDistance / longDistanceThreshold;

        float shortPercentage = shortDistanceHits.Count() / (float)detectionRays.Length;
        float newVolumeW = Mathf.Lerp(1, maxVolumeW, volumePercentage * shortPercentage * (1 - absorptionCoeficient));
        float newVolumeDB = 20 * Mathf.Log10(newVolumeW);
        newVolumeDB -= 100;

        reverbFX.GetFloat("rvEarlyReflectionsDelay", out tempForGetValues);
        reverbFX.SetFloat("rvEarlyReflectionsDelay", Mathf.Lerp(tempForGetValues, preDelayER, changeSensitivity));
        reverbFX.GetFloat("rvEarlyReflectionsLevel", out tempForGetValues);
        reverbFX.SetFloat("rvEarlyReflectionsLevel", Mathf.Lerp(tempForGetValues,newVolumeDB*100, changeSensitivity));
        #endregion

        #region Reverb
        float preDelayMain = 0;
        if(hitsOrderedByDistance.Count() > 0)
        {
            foreach (var hit in hitsOrderedByDistance)
            {
                preDelayMain += hit;
            }
            preDelayMain /= hitsOrderedByDistance.Count();
            preDelayMain /= 342;
        }

        float hitPercentage = hitsOrderedByDistance.Count() / (float)detectionRays.Length;

        reverbFX.GetFloat("rvPreDelay", out tempForGetValues);
        reverbFX.SetFloat("rvPreDelay", Mathf.Lerp(tempForGetValues, preDelayER, changeSensitivity));
        reverbFX.GetFloat("rvDecayTime", out tempForGetValues);
        reverbFX.SetFloat("rvDecayTime", Mathf.Lerp(tempForGetValues, maxDecayTime * hitPercentage * (1 - absorptionCoeficient), changeSensitivity));
        #endregion

        #region Density and Diffusion
        reverbFX.GetFloat("rvDiffusion", out tempForGetValues);
        reverbFX.SetFloat("rvDiffusion", Mathf.Lerp(tempForGetValues, shortPercentage*100, changeSensitivity));
        //Missing density value.
        #endregion
    }

    void OnDrawGizmos()
    {
        if(Application.isPlaying)
        {
            for (int i = 0; i < detectionRays.Length; i++)
            {
                if(detectionRays[i].detectedHit)
                {
                    /*Gizmos.color = Color.blue;
                    Gizmos.DrawLine(detectionRays[i].detectionRay.origin, detectionRays[i].hitInfo.point);*/
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawSphere(detectionRays[i].hitInfo.point, 0.5f);
                }
            }
        }
    }
}

public class DetectionRay
{
    public Ray detectionRay;
    public RaycastHit hitInfo;
    public bool detectedHit;

    public DetectionRay()
    {
        detectionRay = new Ray();
        hitInfo = new RaycastHit();
        detectedHit = false;
    }
}