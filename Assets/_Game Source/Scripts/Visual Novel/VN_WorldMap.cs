using System.Collections.Generic;
using UnityEngine;

public class VN_WorldMap : MonoBehaviour
{
    public List<VN_WorldButton> WorldButtons = new();

    private VN_World_Manager manager;

    public void Initialize(VN_World_Manager vnWorldManager)
    {
        manager = vnWorldManager;
        for (int i = 0; i < WorldButtons.Count; i++)
        {
            VN_WorldButton button = WorldButtons[i];
            button.SetButton(OnButtonSelected);
        }
    }

    public void OnButtonSelected(string buttonId)
    {
        Debug.Log($"WorldMap.OnButtonSelected: {buttonId}");
        manager.OpenInteraction(buttonId);
    }
}
