using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialAudio : MonoBehaviour {
    private string audioSourceName = "projectileAudio";
    private AudioSource audioSource;

    private void Start() {
        audioSource = gameObject.AddComponent<AudioSource>();

        // General Settings for Audio
        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.volume = 1.0f;

        // Get the audio needed to attach to gameObject
        AudioClip audioClip = Resources.Load<AudioClip>(audioSourceName);
        if (audioClip != null) {
            audioSource.clip = audioClip;
        } else {
            Debug.LogError("Missing audio clip for spatial sound.");
        }
        
        // Settings for spatial audio
        audioSource.spatialize = true;
        audioSource.spatialBlend = 1.0f;
        audioSource.minDistance = 1.0f;
        audioSource.maxDistance = 10.0f;

        audioSource.Play();
    }

    private void Update() {
        // Update position of audio whenever gameObject moves
        audioSource.transform.position = transform.position;
    }
}
