using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Footsteps : MonoBehaviour
{

    public List<AudioClip> audioFootsteps;
    public AudioSource footstepSource;
    private Vector3 prevPosition;
    private Vector3 currentPosition;
    public float minDistance = 1;
    public Transform objectTransform;

    void Start()
    {
        prevPosition = objectTransform.position;
    }

    void Update()
    {
        currentPosition = objectTransform.position;
        if (Vector3.Distance(currentPosition, prevPosition) > minDistance)
        {
            footstepSource.Play();
            prevPosition = objectTransform.position;
        }
        if (Vector3.Distance(currentPosition, prevPosition) == 0)
        {
            footstepSource.Stop();
        }
    }
}
