using System;
using System.Collections.Generic;
using System.Linq;
using GameCreator.Runtime.VisualScripting;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ConditionalInteractions
{
    public string conditions;
    public string useItemId;
    public Actions actions;
}


[RequireComponent(typeof(Button))]
public class VN_WorldButton : MonoBehaviour
{
    public string hotspotId;
    public Button mButton;
    private Action<string> onClickCallback;

    private GameConfig gameConfigs => GameConfig.Instance; 
    [SerializeField] private Actions defaultActions;

    private InteractionNode interactionData;
    
    [SerializeField] private List<ConditionalInteractions> interactionsList = new();
    
    private void Awake()
    {
        if (mButton == null) mButton = GetComponent<Button>();
    }

    
    public void SetButton( string interactionId , Action<string> callback)
    {
        if (mButton == null) mButton = GetComponent<Button>();

        gameObject.name = $"VN Button [{interactionId}] - "  ;
        onClickCallback = callback;

        hotspotId = interactionId;
        
        mButton.onClick.RemoveListener(OnButtonClick);
        mButton.onClick.AddListener(OnButtonClick);

        gameObject.SetActive(true);
        // GenerateInteractions();
    }
    
    void GenerateInteractions()
    {
        if (!string.IsNullOrEmpty(hotspotId) && gameConfigs.contentDatabase.CheckInteractionNode(hotspotId))
        {
            interactionData = gameConfigs.contentDatabase.GetInteractionNode(hotspotId);
            gameObject.name = $"Hotspot [{interactionData.Id}] - "  ;
            ClearInteraction();
            for (int i = 0; i < interactionData.interactionRows.Count; i++)
            {
                var currentInteraction = interactionData.interactionRows[i];
                if (currentInteraction.Content_ID.Equals("Default"))
                {
                    ConditionalInteractions defaultInteraction = new ConditionalInteractions();
                    defaultInteraction.conditions = currentInteraction.Conditions;
                    defaultInteraction.actions = defaultActions;
                    interactionsList.Add(defaultInteraction);
                    continue;
                }

                switch (currentInteraction.Row_Type)
                {
                    case "Interaction":
                        // interactionsList.Add(CreateInteraction(currentInteraction));
                        break;
                  default: 
                      Debug.Log($"Cant parse {currentInteraction}");
                      break;
                }
            }
        }
        else
        {
            gameObject.name = $"Hotspot Static [{hotspotId}]";

        }
    }

    // ConditionalInteractions CreateInteraction(InteractionRow currentInteraction)
    // {
    //     return GameContentGenerator.CreateInteraction(currentInteraction, transform, gameConfigs);
    // }

    [Button(ButtonSizes.Large)]
    void ClearInteraction()
    {
        interactionsList = new();
        var tempAllChild = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++) tempAllChild.Add(transform.GetChild(i).gameObject);
        for (int i = 0; i < tempAllChild.Count; i++) DestroyImmediate(tempAllChild[i].gameObject);
    }

    [Button(ButtonSizes.Gigantic)]
    public void InteractWithThisHotspot()
    {
        if (!Application.isPlaying) return;
        Debug.Log($"Interact With  {name}", gameObject);
        // if (interactionsList == null || interactionsList.Count < 1) return;
        var validInteraction = interactionsList.FirstOrDefault(hotspotInteractions =>
            gameConfigs.gameProgression.CheckConditions(hotspotInteractions.conditions));
        if (validInteraction != null) validInteraction.actions.Run();
        // else if (defaultActions != null) defaultActions.Run();
    }
    

    private void OnButtonClick()
    {
        onClickCallback?.Invoke(hotspotId);
    }

    private void OnDestroy()
    {
        if (mButton != null) mButton.onClick.RemoveListener(OnButtonClick);
    }
}
