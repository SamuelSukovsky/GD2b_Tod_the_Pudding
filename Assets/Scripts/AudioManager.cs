using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;        // Set variables
    private AudioSource source;

    void Awake()                                // Before first frame
    {
        if(instance == null)                        // if there isn't an instance, create one in Don't destroy
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else                                        // Else destroy self and return
        {
            Destroy(gameObject);
            return; 
        }

        source = GetComponent<AudioSource>();       // Get audiosource on self
    }

    public void PlaySound(AudioClip sound)      // Play sound
    {
        source.PlayOneShot(sound);                  // Plays the provided audio clip
    }
}
