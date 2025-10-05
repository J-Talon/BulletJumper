using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    private GameObject channelPrefab;
    public static SoundManager instance;

    private const string PATH = "SoundEffects";
    private Dictionary<string, AudioClip> clips;
        
    
    private void Awake()
    {

        instance = this;
        DontDestroyOnLoad(gameObject);

        channelPrefab = Resources.Load<GameObject>(PATH+"/AudioChannel");
        if (channelPrefab == null)
            throw new Exception("could not get audio prefab");
        
        clips = new Dictionary<string, AudioClip>();
        addNewSoundChannel();

    }
    
    //return the index of the new audio channel
    public int addNewSoundChannel()
    {
        GameObject newChannel = Instantiate(channelPrefab, gameObject.transform);
        if (newChannel == null)
            throw new Exception("could not get audio channel");
        return gameObject.transform.childCount - 1;
    }

    //returns the number of active sound channels 
    public int activeChannels()
    {
        return gameObject.transform.childCount;
    }

    //return audio channel or null if none exists
    public AudioSource getChannel(int index)
    {
        if (index < 0)
        {
            return null;
        }

        int channels = activeChannels() - 1;
        while (channels < index)
        {
            channels = addNewSoundChannel();
        }

        Transform child = gameObject.transform.GetChild(index);
        if (child == null)
            return null;
        
        return child.gameObject.GetComponent<AudioSource>();
    }

    //The sound file must be in the Resources folder
    //if your sound is gun.mp3, use SoundManager.instance.playSound("gun")
    //this assumes that gun.mp3 is in Resources/SoundEffects
    public bool playSound(String name)
    {
        return playSound(name, 0);
    }

    public bool playSound(String name, int channel)
    {
        AudioClip clip;
        if (clips.ContainsKey(name))
        {
           clips.TryGetValue(name, out clip);
        }
        else
        {
            clip = Resources.Load<AudioClip>(PATH+"/"+name);
            clips.Add(name, clip);
        }

        if (clip == null)
        {
            return false;
        }

        return playSound(clip, channel);
    }

    public bool playSound(AudioClip clip, int channel)
    {
        AudioSource source = getChannel(channel);
        if (source == null)
            return false;
        source.clip = clip;
        
        source.Play();
        return true;
    }




}
