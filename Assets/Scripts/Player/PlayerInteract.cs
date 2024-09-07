using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    private new Camera camera;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    private InputManager inputManager;
    void Start()
    {
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
        
    }

    void Update()
    {
        playerUI.UpdateText(string.Empty);

        Ray ray = new Ray(camera.transform.position, camera.transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * distance);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactible>() != null)
            {
                Interactible interactible = hitInfo.collider.GetComponent<Interactible>();

                playerUI.UpdateText(interactible.PromptMessage);

                if (inputManager.OnFoot.Interact.triggered)
                {
                    interactible.BaseInteract();
                }
            }
        }
    }
}
