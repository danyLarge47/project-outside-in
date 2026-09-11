using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class VN_Map_Characters : MonoBehaviour
{
    
    public List<string> interactionList;
    
    public List<VN_WorldButton> WorldButtons = new();

    private VN_World_Manager manager;

    public void Initialize(VN_World_Manager vnWorldManager)
    {
        manager = vnWorldManager;
        SetupButtons();
    }

    public void OnButtonSelected(string buttonId)
    {
        Debug.Log($"CharacterMap.OnButtonSelected: {buttonId}");
        manager.OpenInteraction(buttonId);
    }

    [Button(ButtonSizes.Large)]
    void SetupButtons()
    {
        for (int i = 0; i < WorldButtons.Count; i++)
        {
            VN_WorldButton button = WorldButtons[i];
            button.SetButton(interactionList[i],OnButtonSelected);

        }
    }
}
