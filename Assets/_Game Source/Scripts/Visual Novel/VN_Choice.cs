using System.Collections.Generic;
using UnityEngine;

public class VN_Choice : MonoBehaviour
{
   public Transform listParent;
   public VN_Button btnChoice;
   public List<VN_Button> availableButtons = new List<VN_Button>();


   public void SetChoices(List<string> choicesInput)
   {
      //TODO btnChoice is the prefab link,
      //setup VN_Button in availableButtons based on choicesInput,
      //if the available button is not enough, instantiate based on btnChoice, and put it to listParent 
      // if available buttons have more than choices input, set active false for remaining list
   }
   
   
   
}
