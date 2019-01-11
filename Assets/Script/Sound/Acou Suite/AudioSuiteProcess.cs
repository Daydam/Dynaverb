using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSuiteProcess : ScriptableObject
{
    public virtual void Initialize() { }
    public virtual void Process(Transform t, AudioMixer reverbFX) { }
    public virtual void DrawGizmos(Transform t) { }
}
