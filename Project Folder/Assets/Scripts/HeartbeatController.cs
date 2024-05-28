using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartbeatController : MonoBehaviour
{
    public AudioSource heartBeatAudioSource;
    public Transform player; // The AR Camera or the player's position in AR
    public float maxVolumeDistance = 2.0f; // Distance at which the volume is maximum
    public float minVolumeDistance = 10.0f; // Distance at which the volume is minimum

    void Start()
    {
        if (heartBeatAudioSource != null)
        {
            heartBeatAudioSource.loop = true; // Ensure the sound loops
            heartBeatAudioSource.Play();
        }
    }

    void Update()
    {
        if (heartBeatAudioSource != null && player != null)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            float volume = Mathf.Clamp01((minVolumeDistance - distance) / (minVolumeDistance - maxVolumeDistance));
            heartBeatAudioSource.volume = volume;
        }
    }
}

