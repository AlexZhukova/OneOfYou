using UnityEngine;
using UnityEngine.Events;

public class ActionFromKey : MonoBehaviour
{
    [Tooltip("This keypress is being listened whenever this script is active")]
    public KeyCode interactionKey = KeyCode.E;

    [Space(16)]

    [Tooltip("Activate this even every time key is pressed")]
    public UnityEvent OnKeyPressed;

    [Space(8)]
    [Header("Toggling Event")]
    [Space(8)]

    [Tooltip("Activate this even every time key is pressed. ToggleState will change, and new state will be dynamically applied to any method linked (accepting a (bool) argument)")]
    public UnityEvent<bool> OnKeyToggle;
    public bool toggleState;

    // Update is called once per frame
    void Update()
    {
        // Listen to a key pressed
        if (Input.GetKeyDown(interactionKey))
        {
            // Invoke key pressed event
            OnKeyPressed?.Invoke();

            // Switch toggling state and invoke toggling state with the current state argument
            // You can tie a toggling state event like GameObject/(Dynamic bool) SetActive to this event
            toggleState = !toggleState;
            OnKeyToggle.Invoke(toggleState);
        }
    }
}
