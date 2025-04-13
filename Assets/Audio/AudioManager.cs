using UnityEngine;
using UnityEngine.Audio;
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
            s.source.clip = s.clip;
        }

        Play("Ambient");
    }

    public void Play (string name ) {
        if (soundMap.TryGetValue(name, out Sound s))
        {
           if (name == "KeyClick")
            {
                s.source.pitch = Random.Range(0.8f, 1.05f); // add pitch variation
                s.source.PlayOneShot(s.clip);                // overlap-friendly
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

}
