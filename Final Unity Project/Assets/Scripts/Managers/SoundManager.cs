using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;

    public static SoundManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("GameMusic");
    }

    private void Update()
    {
        
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.Log("Failed to play " + name);
            return;
        }
        s.source.Play(); 
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Failed to stop" + name);
            return;
        }
        s.source.Stop();
    }
    public void PitchChange(string name, float pitchChange)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Failed to change pitch " + name);
            return;
        }
        s.source.pitch = pitchChange;
    }
    
}
