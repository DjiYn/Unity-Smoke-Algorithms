using UnityEditor;

[CustomEditor(typeof(Interactible), true)]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Interactible interactible = target as Interactible;

        if(target.GetType() == typeof(EventOnlyInteractable))
        {
            interactible.PromptMessage = EditorGUILayout.TextField("Prompt Message.", interactible.PromptMessage);

            EditorGUILayout.HelpBox("EventOnlyInteract can ONLY use UnityEvents", MessageType.Info);

            if (interactible.GetComponent<InteractionEvent>() == null)
            {
                interactible.UseEvents = true;
                interactible.gameObject.AddComponent<InteractionEvent>();
            }
        } 
        else
        {
            base.OnInspectorGUI();

            if (interactible.UseEvents)
            {
                if (interactible.GetComponent<InteractionEvent>() == null)
                {
                    interactible.gameObject.AddComponent<InteractionEvent>();
                }
            }
            else
            {
                if (interactible.GetComponent<InteractionEvent>() != null)
                {
                    DestroyImmediate(interactible.GetComponent<InteractionEvent>());
                }
            }
        }
    }
}
