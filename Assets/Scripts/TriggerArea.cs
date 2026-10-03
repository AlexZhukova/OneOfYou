using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerArea : MonoBehaviour
{
    [Tooltip("if empty everything is whitelisted")]
    public List<string> tagWhitelist;
    public UnityEvent triggerEnterEvent;
    public UnityEvent triggerExitEvent;


    private void OnTriggerEnter(Collider other)
    {
        if (tagWhitelist.Count == 0 || tagWhitelist.Contains(other.gameObject.tag))
        {
            triggerEnterEvent?.Invoke();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (tagWhitelist.Count == 0 || tagWhitelist.Contains(other.gameObject.tag))
        {
            triggerExitEvent?.Invoke();
        }
    }
}
