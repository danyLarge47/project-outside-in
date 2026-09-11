// using GameCreator.Runtime.VisualScripting;
// using UnityEngine;
//
// public static class GameContentGenerator
//
// {
//      public static ConditionalInteractions CreateInteraction(InteractionRow currentInteraction, Transform transform,
//         GameConfig gameConfigs)
//     {
//         var newInteraction = new GameObject(currentInteraction.Type + " -> " + currentInteraction.Conditions);
//         newInteraction.transform.SetParent(transform);
//         var newAction = newInteraction.AddComponent<Actions>();
//         Instruction lastInstruction = null;
//         newAction.AddInstructions(new Action_GameState
//         {
//             gameStateManager = gameConfigs.gameStateManager, newGameState = gameConfigs.gameStateManager.cutscene
//         });
//         if (gameConfigs.contentDatabase.CheckContent(currentInteraction.Content_ID))
//         {
//             var targetContent = gameConfigs.contentDatabase.GetContentNode(currentInteraction.Content_ID);
//             for (int j = 0; j < targetContent.contentRows.Count; j++)
//             {
//                 var newInstruction = CreateContentInstructions(targetContent.contentRows[j], gameConfigs,
//                     newInteraction.transform);
//                 if (newInstruction != null)
//                 {
//                     if (lastInstruction != null && newInstruction.GetType() != typeof(Action_DoDialogue) &&
//                         lastInstruction.GetType() == typeof(Action_DoDialogue))
//                     {
//                         var lastDialogue = lastInstruction as Action_DoDialogue;
//                         lastDialogue.data.hideAfter = true;
//                     }
//
//                     if (newInstruction.GetType() == typeof(Action_DoDialogue)) lastInstruction = newInstruction;
//                     newAction.AddInstructions(newInstruction);
//                 }
//             }
//         }
//         else
//         {
//             Debug.LogWarning($"Content {currentInteraction.Content_ID} Not Found", transform);
//         }
//
//         if (lastInstruction != null)
//         {
//             var lastDialogue = lastInstruction as Action_DoDialogue;
//             lastDialogue.data.hideAfter = true;
//         }
//
//         newAction.AddInstructions(new Action_GameState
//         {
//             gameStateManager = gameConfigs.gameStateManager, newGameState = gameConfigs.gameStateManager.gameplay
//         });
//         ConditionalInteractions newInteractionData = new ConditionalInteractions
//         {
//             conditions = currentInteraction.Conditions,
//             useItemId = currentInteraction.Using_Item_ID,
//             actions = newAction
//         };
//         return newInteractionData;
//     }
//
//     public static Actions CreateSubInteraction(string Content_ID, GameConfig gameConfigs, Transform parent)
//     {
//         var newInteraction = new GameObject(" -> " + Content_ID);
//         newInteraction.transform.SetParent(parent);
//         var newAction = newInteraction.AddComponent<Actions>();
//         newAction.AddInstructions(new Action_GameState
//         {
//             gameStateManager = gameConfigs.gameStateManager, newGameState = gameConfigs.gameStateManager.cutscene
//         });
//         if (gameConfigs.contentDatabase.CheckContent(Content_ID))
//         {
//             var targetContent = gameConfigs.contentDatabase.GetContentNode(Content_ID);
//             Instruction lastDialogueInstructions = null;
//             for (int j = 0; j < targetContent.contentRows.Count; j++)
//             {
//                 var newInstruction = CreateContentInstructions(targetContent.contentRows[j], gameConfigs,
//                     newInteraction.transform);
//                 if (newInstruction != null)
//                 {
//                     if (newInstruction.GetType() == typeof(Action_DoDialogue))
//                         lastDialogueInstructions = newInstruction;
//                     newAction.AddInstructions(newInstruction);
//                 }
//             }
//
//             if (lastDialogueInstructions != null)
//             {
//                 var lastDialogue = lastDialogueInstructions as Action_DoDialogue;
//                 lastDialogue.data.hideAfter = false;
//             }
//         }
//         else
//         {
//             Debug.LogWarning($"Content {Content_ID} Not Found");
//         }
//
//         newAction.AddInstructions(new Action_CloseDialogue());
//         newAction.AddInstructions(new Action_GameState
//         {
//             gameStateManager = gameConfigs.gameStateManager, newGameState = gameConfigs.gameStateManager.gameplay
//         });
//         return newAction;
//     }
//      public static Instruction CreateContentInstructions(ContentRow rowData, GameConfig gameConfigs, Transform parent)
//     {
//         switch (rowData.Type)
//         {
//             case "System": return Create_System_Instructions(rowData, gameConfigs, parent);
//             case "Cutscene": return Create_Cutscene_Instructions(rowData, gameConfigs, parent);
//             case "Character": return Create_Character_Instructions(rowData, gameConfigs, parent);
//             case "Audio": return Create_Audio_Instructions(rowData, gameConfigs);
//         }
//
//         if (rowData.Type != "NodeStart" && rowData.Type != "NodeEnd")
//             Debug.LogWarning($"Fail to CreateContentInstructions {rowData.Type}");
//         return null;
//     }
//
//     public static Instruction Create_System_Instructions(ContentRow rowData, GameConfig gameConfigs, Transform parent)
//     {
//         switch (rowData.ActionDetails)
//         {
//             case "Wait":
//                 float.TryParse(rowData.Var_A, out var waitDuration);
//                 return new Action_Wait { duration = waitDuration };
//             case "Fade":
//                 bool fadeOut = string.Equals(rowData.Var_A, "Out");
//                 float.TryParse(rowData.Var_B, out var fadeDuration);
//                 bool waitComplete = rowData.WaitComplete;
//                 return new Action_FadeCamera
//                 {
//                     fadeOut = fadeOut, duration = fadeDuration, waitComplete = waitComplete
//                 };
//             case "Variables":
//                 return new Action_SetVar
//                 {
//                     gameProgression = gameConfigs.gameProgression, variableOperations = rowData.Var_A
//                 };
//             case "Obtain_Info": return new Action_Obtain_Item() { itemId = rowData.Var_A, isObtain = true };
//             case "Remove_Info": return new Action_Obtain_Item() { itemId = rowData.Var_A, isObtain = false };
//             case "Update_Info": return new Action_Update_Item() { itemId = rowData.Var_A };
//             case "Load_Scene":
//                 return new Action_LoadScene() { targetSceneName = rowData.Var_A, sceneTransitionType = rowData.Var_B };
//             case "InsertContent": return Create_Insert_Content_Instruction(rowData, gameConfigs, parent);
//             case "JumpToContent": return Create_Jump_To_Content_Instruction(rowData, gameConfigs, parent);
//         }
//
//         Debug.LogWarning($"Fail to Create_System_Instructions {rowData.ActionDetails}");
//         return null;
//     }
//
//     public static Instruction Create_Insert_Content_Instruction(ContentRow rowData, GameConfig gameConfigs,
//         Transform parent)
//     {
//         return null;
//     }
//
//     public static Instruction Create_Jump_To_Content_Instruction(ContentRow rowData, GameConfig gameConfigs,
//         Transform parent)
//     {
//         var createdContent = CreateSubInteraction(rowData.Var_A, gameConfigs, parent);
//         return new Action_JumpToContent()
//         {
//             conditions = rowData.Var_B,
//             parentAction = parent.GetComponent<Actions>(),
//             createdContent = createdContent,
//         };
//     }
//
//     public static Instruction Create_Cutscene_Instructions(ContentRow rowData, GameConfig gameConfigs,
//         Transform parent)
//     {
//         switch (rowData.ActionDetails)
//         {
//             case DialogueData.Dialogue_Bubble:
//                 return Create_Instructions_Dialogue(rowData, gameConfigs, parent, DialogueData.Dialogue_Bubble);
//             case DialogueData.DialogueType_VN:
//                 return Create_Instructions_Dialogue(rowData, gameConfigs, parent, DialogueData.DialogueType_VN);
//             case DialogueData.Dialogue_Narrator:
//                 return Create_Instructions_Dialogue(rowData, gameConfigs, parent, DialogueData.Dialogue_Narrator);
//             case "CustomActions":
//                 Actions targetActions = null;
//                 var targetObject = GameObject.Find(rowData.Var_A);
//                 if (targetObject) targetActions = targetObject.GetComponent<Actions>();
//                 return new Action_CustomActions
//                 {
//                     actionName = rowData.Var_A, actionDetails = rowData.Var_B, targetActions = targetActions
//                 };
//             case "CloseDialogue": return new Action_CloseDialogue { waitComplete = true };
//         }
//
//         Debug.LogWarning($"Fail to Create_Cutscene_Instructions {rowData.ActionDetails}");
//         return null;
//     }
//
//     public static Instruction Create_Character_Instructions(ContentRow rowData, GameConfig gameConfigs,
//         Transform parent)
//     {
//         switch (rowData.ActionDetails)
//         {
//             case "Move":
//                 Debug.LogWarning("TODO Fix Move To Content Generator");
//                 return new Action_Character_MoveTo() { };
//             case "FaceTo":
//                 Debug.LogWarning("TODO Fix Face To Content Generator");
//                 return new Action_Character_FaceTo { };
//         }
//
//         Debug.LogWarning($"Fail to Create_Character_Instructions {rowData.ActionDetails}");
//         return null;
//     }
//
//     static Instruction Create_Instructions_Dialogue(ContentRow rowData, GameConfig gameConfigs, Transform parent,
//         string dialogueUiType)
//     {
//         var dialogueData = new DialogueData()
//         {
//             dialogueId = rowData.Id,
//             uiType = dialogueUiType,
//             characterName = rowData.Var_A,
//             characterLines = rowData.Var_B,
//             portraitId = rowData.Var_C,
//             hideAfter = rowData.WaitComplete
//         };
//         return new Action_DoDialogue { data = dialogueData };
//     }
//
//     public static Instruction Create_Audio_Instructions(ContentRow rowData, GameConfig gameConfigs)
//     {
//         AudioClip targetClip = null;
//         switch (rowData.ActionDetails)
//         {
//             case "StopAllBgm":
//                 float.TryParse(rowData.Var_A, out var defaultDuration);
//                 if (defaultDuration <= 0) defaultDuration = 1f;
//                 return new InstructionCommonAudioMusicStopAll(){transitionOut = defaultDuration , m_WaitToComplete = rowData.WaitComplete};
//         
//                 
//             case "Bgm":
//                 targetClip = gameConfigs.audioManager.GetBGM(rowData.Var_A);
//                 return new InstructionCommonAudioMusicPlay(){m_AudioClip = targetClip};
//             case "Sfx":
//                 targetClip = gameConfigs.audioManager.GetSFX(rowData.Var_A);
//                 return new InstructionCommonAudioSFXPlay(){m_AudioClip = targetClip , m_WaitToComplete = rowData.WaitComplete};
//             case "Ambiance": 
//                 targetClip = gameConfigs.audioManager.GetAMB(rowData.Var_A);
//                 return new InstructionCommonAudioAmbientPlay(){m_AudioClip = targetClip};
//             default:
//                 Debug.LogWarning($"Cant Parse Audio {rowData.ActionDetails}");
//                 break;
//         }
//
//         return null;
//     }
//     
//     
//     
//     
// }
