using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class DelayedEvent : MonoBehaviour
{
    public float delay = 10;
    float timer;

    bool timerIsRunning;


    public UnityEvent OnDelayEnd;


    public void StartCountdown()
    {
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning == true)
        {
            timer = timer + Time.deltaTime;

            if (timer >= delay)
            {
                OnDelayEnd?.Invoke();
                timerIsRunning = false;
                timer = 0;
            }
        }
    }


}
