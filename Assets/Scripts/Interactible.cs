using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactible : MonoBehaviour
{
    [SerializeField]
    private string promptMessage;
    public string PromptMessage { get { return promptMessage; } set { promptMessage = value; } }
    [SerializeField]
    private bool useEvents;
    public bool UseEvents { get { return useEvents; } set { useEvents = value; } }

    public void BaseInteract()
    {
        if (useEvents)
        {
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        }

        Interact();
    }


    protected virtual void Interact()
    {
        
    }
}
