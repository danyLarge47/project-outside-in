using System.Collections.Generic;
using UnityEngine;

public class VN_World_Manager : MonoBehaviour
{
    public enum VN_World_State
    {
        World_Map,
        Character_Map,
        Character_Interactions,
        Dialogue,
        Dialogue_Choices
    }

    public VN_WorldMap worldMap;
    public VN_Map_Characters characterMap;

    public VN_Interactions characterInteractions;
    public VN_Dialogue_Delivery dialogueDelivery;
    public VN_Dialogue_Choice dialogueChoice;

    public List<VN_CharacterData> characters = new List<VN_CharacterData>();
    public List<VN_DialogueData> dialogues = new List<VN_DialogueData>();

    public void Initialize()
    {
        worldMap.Initialize(this);
        characterMap.Initialize(this);

        characterInteractions.Initialize(this);
        dialogueDelivery.Initialize(this);
        dialogueChoice.Initialize(this);
    }

    public void OpenInteraction(string characterId)
    {
        VN_CharacterData character = characters.Find(c => c.characterId == characterId);
        if (character == null)
        {
            Debug.LogWarning($"VN_World_Manager: no character with id '{characterId}'");
            return;
        }

        characterInteractions.SetInteractions(character.interactions);
    }

    public void OpenDialogue(string dialogueId)
    {
        VN_DialogueData dialogue = dialogues.Find(d => d.dialogueId == dialogueId);
        if (dialogue == null)
        {
            Debug.LogWarning($"VN_World_Manager: no dialogue with id '{dialogueId}'");
            return;
        }

        _ = dialogueDelivery.DoDialogue(
            dialogue.speakerName,
            dialogue.text,
            dialogue.expression,
            () => OnDialogueComplete(dialogue));
    }

    private void OnDialogueComplete(VN_DialogueData dialogue)
    {
        if (dialogue.choices != null && dialogue.choices.Count > 0)
        {
            dialogueChoice.SetChoices(dialogue.choices);
        }
    }
}
