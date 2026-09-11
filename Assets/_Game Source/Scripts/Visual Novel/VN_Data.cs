using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class VN_ChoiceData
{
    public string label;
    public string dialogueId;
}

[Serializable]
public class VN_InteractionData
{
    public string label;
    public string targetDialogId;
}

[Serializable]
public class VN_DialogueData
{
    public string dialogueId;
    public string speakerName;
    [TextArea] public string text;
    public Sprite expression;
    public List<VN_ChoiceData> choices = new List<VN_ChoiceData>();
}


[Serializable]
public class VN_DialogueContent
{
    public string contentId; 
    public List<VN_DialogueData> dialogues = new () ;
}



[Serializable]
public class VN_CharacterData
{
    public string characterId;
    public string displayName;
    public List<VN_InteractionData> interactions = new List<VN_InteractionData>();
}

[Serializable]
public class ContentRow
{
    public string Row_Type;
    public string Row_Notes;
    public string Row_Id;
    public string Args_1;
    public string Args_2;
    public string Args_3;
    public string Args_4;
    public string Args_5;

    public ContentRow GetDuplicate()
    {
        return new ContentRow
        {
            Row_Type = Row_Type,
            Row_Notes = Row_Notes,
            Row_Id = Row_Id,
            Args_1 = Args_1,
            Args_2 = Args_2,
            Args_3 = Args_3,
            Args_4 = Args_4,
            Args_5 = Args_5
        };
    }
}

[Serializable]
public class ContentNode
{
    public string Id;
    [TableList] public List<ContentRow> contentRows = new();
}

[Serializable]
public class InteractionRow
{
    public string Row_Type;
    public string Interaction_Id;
    public string Conditions;
    public string Content_ID;
    
    public InteractionRow GetDuplicate()
    {
        return new InteractionRow
        {
            Row_Type = Row_Type,
            Interaction_Id = Interaction_Id,
            Conditions = Conditions,
            Content_ID = Content_ID, 
        };
    }
}

[Serializable]
public class InteractionNode
{
    public string Id;
    [TableList] public List<InteractionRow> interactionRows = new();
}

