using System.Collections.Generic;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

public class VN_Interactions : MonoBehaviour
{
    public Transform listParent;
    public VN_SelectButton btnChoice;
    public Actions show, hide;
    public List<VN_SelectButton> availableButtons = new List<VN_SelectButton>();

    private VN_World_Manager manager;

    public void Initialize(VN_World_Manager vnWorldManager)
    {
        manager = vnWorldManager;
    }

    public void SetInteractions(List<VN_InteractionData> interactionsInput)
    {
        for (int i = availableButtons.Count; i < interactionsInput.Count; i++)
        {
            VN_SelectButton newButton = Instantiate(btnChoice, listParent);
            availableButtons.Add(newButton);
        }

        for (int i = 0; i < interactionsInput.Count; i++)
        {
            VN_InteractionData interaction = interactionsInput[i];
            VN_SelectButton button = availableButtons[i];
            button.btnId = interaction.targetDialogId;
            button.SetButton(interaction.label, _ => OnInteractionSelected(interaction));
        }

        for (int i = interactionsInput.Count; i < availableButtons.Count; i++)
        {
            availableButtons[i].gameObject.SetActive(false);
        }

        _ = show.Run();
    }

    public async void OnInteractionSelected(VN_InteractionData interactionData)
    {
        await hide.Run();
        manager.OpenDialogue(interactionData.targetDialogId);
    }

    public void Clear()
    {
        for (int i = 0; i < availableButtons.Count; i++)
        {
            availableButtons[i].gameObject.SetActive(false);
        }
    }
}
