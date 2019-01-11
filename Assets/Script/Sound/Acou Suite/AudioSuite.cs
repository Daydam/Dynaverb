using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;

public class AudioSuite : MonoBehaviour
{
    static AudioSuite instance;
    public static AudioSuite Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<AudioSuite>();
            if (instance == null)
            {
                Debug.LogError("There's no AudioSuite object on scene!");
            }
            return instance;
        }
    }
    
    public AudioMixer mixer;
    public AudioSuiteProcess[] processes;
    public AudioSuiteProcess[] Processes { get { return processes; } }

    public SurroundListener GetSurroundListener { get { return (SurroundListener)processes.Where(a => a.GetType() == typeof(SurroundListener)).First(); } }

    void Start ()
	{
        for (int i = 0; i < Processes.Length; i++)
        {
            Processes[i].Initialize();
        }
	}
	

	void Update () 
	{
        for (int i = 0; i < Processes.Length; i++)
        {
            Processes[i].Process(transform, mixer);
        }
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < Processes.Length; i++)
        {
            Processes[i].DrawGizmos(transform);
        }
    }
}
