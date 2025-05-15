using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour {

    public static AudioManagerScript instance;
	public void PlayBatHitAudio()
{
    // Audio disabled - nothing happens
}


    //public GameObject bounceAudioHolder;
    //public GameObject batHitAudioHolder;

   // private AudioSource bounceAudioSource;
   //// private AudioSource batHitAudioSource;

   // void Awake(){
    //    instance = this;
   // }

    //void Start(){
     //   bounceAudioSource = bounceAudioHolder.GetComponent<AudioSource>();
      //  batHitAudioSource = batHitAudioHolder.GetComponent<AudioSource>();

        // Make sure that the audio sources are 3D
      //  bounceAudioSource.spatialize = true;  // Enable 3D sound for bounce
      //  bounceAudioSource.spatialBlend = 1.0f; // Full 3D sound

      //  batHitAudioSource.spatialize = true;  // Enable 3D sound for bat hit
      //  batHitAudioSource.spatialBlend = 1.0f; // Full 3D sound
   // }

    // Play the ball bounce audio
   // public void PlayBounceAudio(){
     //   bounceAudioSource.Play();
   //// }

    // Play the ball hit by bat audio
   // public void PlayBatHitAudio(){
  //      batHitAudioSource.Play();
   // }
}
