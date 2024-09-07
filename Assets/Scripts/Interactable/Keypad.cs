using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactible
{
    [SerializeField]
    private GameObject door;
    private bool isdoorOpen;

    protected override void Interact()
    {
        isdoorOpen = !isdoorOpen;

        door.GetComponent<Animator>().SetBool("isOpen", isdoorOpen);
    }

}
