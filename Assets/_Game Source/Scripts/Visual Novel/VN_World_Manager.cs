using System;
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

    // public VN_Interactions characterInteractions;
    // public VN_Dialogue_Choice dialogueChoice;
    public VN_Dialogue_Delivery dialogueDelivery;

    public List<VN_CharacterData> characters = new List<VN_CharacterData>();
    public List<VN_DialogueData> dialogues = new List<VN_DialogueData>();

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        worldMap.Initialize(this);
        characterMap.Initialize(this);

        // characterInteractions.Initialize(this);
        dialogueDelivery.Initialize(this);
        // dialogueChoice.Initialize(this);

        var check = GameConfig.Instance;
    }

    public void OpenInteraction(string dialogueId)
    {
        OpenDialogue(dialogueId);
    }

    public void OpenDialogue(string dialogueId)
    {
        Debug.Log($" Calling dialogue {dialogueId}");


        var interactionData = GameConfig.Instance.contentDatabase.GetInteractionNode(dialogueId);
        if (interactionData == null)
        {
            Debug.LogWarning($"VN_World_Manager: no interaction with id '{dialogueId}'");
            return;
        }

        // Pick the first interaction row whose conditions currently pass.
        var progression = GameConfig.Instance.gameProgression;
        string targetContentId = null;
        for (int i = 0; i < interactionData.interactionRows.Count; i++)
        {
            var row = interactionData.interactionRows[i];
            if (string.IsNullOrEmpty(row.Content_ID)) continue;
            if (progression.CheckConditions(row.Conditions))
            {
                targetContentId = row.Content_ID;
                break;
            }
        }

        if (string.IsNullOrEmpty(targetContentId))
        {
            Debug.LogWarning($"VN_World_Manager: no interaction row matched conditions for '{dialogueId}'");
            return;
        }

        var contentData = GameConfig.Instance.contentDatabase.GetContentData(targetContentId);
        if (contentData == null)
        {
            Debug.LogWarning($"VN_World_Manager: no content with id '{targetContentId}'");
            return;
        }

        VN_DialogueContent contentNode = BuildDialogueContent(contentData);
        _ = dialogueDelivery.RunDialogueContent(contentNode);
    }

    private VN_DialogueContent BuildDialogueContent(ContentNode node)
    {
        var content = new VN_DialogueContent { contentId = node.Id };

        for (int i = 0; i < node.contentRows.Count; i++)
        {
            var row = node.contentRows[i];
            if (string.IsNullOrEmpty(row.Row_Type)) continue;
            if (!row.Row_Type.Equals("Dialogue")) continue;


            switch (row.Row_Type)
            {
                case "Dialogue" :
                    content.dialogues.Add(new VN_DialogueData
                    {
                        dialogueId = row.Row_Id,
                        speakerName = row.Args_1,
                        text = row.Args_2,
                    });
                    break;
                
                case "DialogueEnd" :
                    
                    break;
                
                default: Debug.Log($"Cant handle {row.Row_Type}");
                    break;
                
                
            }
          
        }

        return content;
    }

    private void OnDialogueComplete(VN_DialogueData dialogue)
    {
        // if (dialogue.choices != null && dialogue.choices.Count > 0)
        // {
        //     dialogueChoice.SetChoices(dialogue.choices);
        // }
    }
}
