using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine.Serialization;


public class AudioManager : MonoBehaviour
{
    // public Sound[] sounds;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float outsideClosedVolume = -20;
    public Sound[] sounds;
    private Dictionary<string, Sound> soundMap;
    private HashSet<string> clipNames = new HashSet<string>
    {
        "Error",
    };

     private HashSet<string> pitchedClipNames = new HashSet<string>
    {
        "KeyClick",
        "Blip",
    };


    public static AudioManager Instance { get; private set; }

    void Awake(){
        if(Instance) {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        soundMap = new Dictionary<string, Sound>();
        foreach (Sound s in sounds)
        {
            soundMap[s.name] = s;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.originalVolume = s.volume;
        }

        Play("Ambient");
    }

    public void Play (string name ) {
        if (soundMap.TryGetValue(name, out Sound s))
        {
            if (pitchedClipNames.Contains(name))
            {
                s.source.pitch = Random.Range(0.8f, 1.05f); // add pitch variation
                s.source.PlayOneShot(s.clip);                // overlap-friendly
            }
            else if (clipNames.Contains(name)) {
                s.source.PlayOneShot(s.clip);   
            }
            else
            {
                s.source.Play();         // default play
            }
        }
    }

    public void Stop(string name)
    {
        if (soundMap.TryGetValue(name, out Sound s))
            {
                s.source.Stop();
            }
    }

    public void Mute(string name, bool mute)
    {
        if (soundMap.TryGetValue(name, out Sound s))
        {
            s.source.mute = mute;
        }
    }

    public void Dim(string name, bool dim)
    {
        if (soundMap.TryGetValue(name, out Sound s))
        {
            if (dim)
            {
                if (Mathf.Approximately(s.source.volume, s.originalVolume))
                {
                    s.originalVolume = s.source.volume;
                    s.source.volume *= 0.15f;
                }
            }
            else
            {
                s.source.volume = s.originalVolume;
            }
        }
    }


    public void CloseOutside(bool close)
    {
        audioMixer.SetFloat("MyExposedParam 1", close ? outsideClosedVolume : 0);
    }
}
