using UnityEngine;

public class Audiotest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.Play();
    }

 
}
