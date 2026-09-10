using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class UI_ScreenManager : SerializedMonoBehaviour
{
    [SerializeField] private Transform uiGroupParent;
    [SerializeField] private UI_Screen screen_MainMenu;
    [SerializeField] private UI_Screen currentScreen;
    [SerializeField] private List<UI_Screen> UiScreens;
    [SerializeField] private Dictionary<string, UI_Screen> dictionaryUiScreens;

    private void OnEnable()
    {
        GameEvents.ChangeScreenTo.AddListener(ChangeScreenFromId);
        // GameEvents.Universal_BackButton.AddListener(UniversalBackButtonHandler);
    }

    private void OnDisable()
    {
        GameEvents.ChangeScreenTo.RemoveListener(ChangeScreenFromId);
        // GameEvents.Universal_BackButton.RemoveListener(UniversalBackButtonHandler);
    }

    private void Start()
    {
        ResetDictionary();
        currentScreen = screen_MainMenu;
        currentScreen.Open();
    }

    [Button(ButtonSizes.Large)]
    private void FindUI()
    {
        UiScreens = uiGroupParent.GetComponentsInChildren<UI_Screen>(includeInactive: true).ToList();
        ResetDictionary();
    }

    private void ResetDictionary()
    {
        dictionaryUiScreens = new Dictionary<string, UI_Screen>();
        foreach (var screen in UiScreens)
        {
            if (dictionaryUiScreens.ContainsKey(screen.screenName))
            {
                Debug.Log($"{screen.screenName} exist", screen.gameObject);
                continue;
            }

            dictionaryUiScreens.Add(screen.screenName, screen);
        }
    }

    [Button(ButtonSizes.Large)]
    private void SetupScreensNames()
    {
        foreach (var screen in UiScreens) screen.SetupScreen();
    }

    private void ChangeScreenFromId(string targetMenu, bool closePrevScreen, Action callback)
    {
        var screen = dictionaryUiScreens[currentScreen.screenName];
          currentScreen = dictionaryUiScreens[targetMenu];

        PageTransition(screen, currentScreen, closePrevScreen, callback);
    }

    public void PageTransition(UI_Screen prev, UI_Screen next, bool closePrevScreen, Action onComplete = null)
    {
        Debug.Log($"{name} Page Transition from {prev.screenName} to {next.screenName}", gameObject);
        currentScreen = next;
        if (closePrevScreen)
        {
            prev.Close(delegate { currentScreen.Open(delegate { onComplete?.Invoke(); }); });
        }
    }
}