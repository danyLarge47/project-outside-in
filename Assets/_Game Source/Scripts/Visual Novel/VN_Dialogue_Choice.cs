using System.Collections.Generic;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

public class VN_Dialogue_Choice : MonoBehaviour
{
   public Transform listParent;
   public VN_SelectButton btnChoice;
   public Actions showChoices;
   public Actions hideChoices;
   public List<VN_SelectButton> availableButtons = new List<VN_SelectButton>();

   private VN_World_Manager manager;

   public void Initialize(VN_World_Manager vnWorldManager)
   {
      manager = vnWorldManager;
   }

   public void SetChoices(List<VN_ChoiceData> choicesInput)
   {
      for (int i = availableButtons.Count; i < choicesInput.Count; i++)
      {
         VN_SelectButton newButton = Instantiate(btnChoice, listParent);
         availableButtons.Add(newButton);
      }

      for (int i = 0; i < choicesInput.Count; i++)
      {
         VN_ChoiceData choice = choicesInput[i];
         VN_SelectButton button = availableButtons[i];
         button.btnId = choice.dialogueId;
         button.SetButton(choice.label, _ => OnChoiceSelected(choice));
      }

      for (int i = choicesInput.Count; i < availableButtons.Count; i++)
      {
         availableButtons[i].gameObject.SetActive(false);
      }

      _ = showChoices.Run();
   }

   public async void OnChoiceSelected(VN_ChoiceData choiceData)
   {
      await hideChoices.Run();
      manager.OpenDialogue(choiceData.dialogueId);
   }

   public void Clear()
   {
      for (int i = 0; i < availableButtons.Count; i++)
      {
         availableButtons[i].gameObject.SetActive(false);
      }
   }
}
