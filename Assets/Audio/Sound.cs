using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound 
{
  public string name;
  public AudioSource source;
  public AudioClip clip;

  public float volume = 1f;

    [HideInInspector] public float originalVolume; // Store original volume
}
