using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    private GameObject channelPrefab;
    public static SoundManager instance;

    private const string PATH = "SoundEffects";
    
    private void Awake()
    {

        instance = this;
        DontDestroyOnLoad(gameObject);

        channelPrefab = Resources.Load<GameObject>(PATH+"/AudioChannel");
        if (channelPrefab == null)
            throw new Exception("could not get audio prefab");

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
        AudioClip clip = Resources.Load<AudioClip>(PATH+"/"+name);
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
