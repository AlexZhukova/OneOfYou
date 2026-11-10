using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Footsteps : MonoBehaviour
{
    [Header("Audio")]
    public List<AudioClip> audioFootsteps;
    public AudioSource footstepSource;

    [Header("Settings")]
    public Transform objectTransform;
    public float stepInterval = 0.45f;
    public float minMovementSpeed = 0.1f;

    private Vector3 previousPosition;
    private float stepTimer;


    private void Start()
    {
        if (objectTransform == null)
            objectTransform = transform;

        previousPosition = objectTransform.position;
    }

    private void Update()
    {
        Vector3 currentPosition = objectTransform.position;

        Vector3 movement = currentPosition - previousPosition;
        movement.y = 0f;

        float speed = movement.magnitude / Mathf.Max(Time.deltaTime, 0.0001f);

        previousPosition = currentPosition;

        if (speed < minMovementSpeed)
        {
            stepTimer = 0f;
            return;
        }
        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0f)
        {
            PlayFootstep();
            stepTimer = stepInterval;
        }
    }

    private void PlayFootstep()
    {
        if (footstepSource == null || audioFootsteps.Count == 0)
            return;
        AudioClip clip = audioFootsteps[Random.Range(0, audioFootsteps.Count)];

        if (clip != null)
            footstepSource.PlayOneShot(clip);
    }

}
