using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onInteract;

    public UnityEvent OnInteract {  get { return onInteract; } set { onInteract = value; } }
}
