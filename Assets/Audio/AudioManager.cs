using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;


public class AudioManager : MonoBehaviour
{
    // public Sound[] sounds;

    public Sound[] sounds;
    private Dictionary<string, Sound> soundMap;


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
        }

        Play("Ambient");
    }

    public void Play (string name ) {
        if (soundMap.TryGetValue(name, out Sound s))
        {
            s.source.Play();
        }
    }

}
