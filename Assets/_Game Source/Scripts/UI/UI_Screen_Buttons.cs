using GameCreator.Runtime.VisualScripting;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UI_Screen_Buttons : MonoBehaviour
{
    public string targetMenu;
    public bool closeAndHideCurrentMenu = true;
    public Button mButton;
    public Actions btnPressFeedback;

    private void Start()
    {
        SetupButton();
    }

    [Button(ButtonSizes.Large)]
      void SetupButton()
    {
        mButton = GetComponent<Button>();
        mButton.onClick.RemoveAllListeners();
        mButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        Debug.Log($"Button {mButton.name} clicked to {targetMenu}");
        // GameEvents.PlaySFX.Invoke(sAudioManager.SFX_BUTTON_SELECT, 1.1f, 1f);
        btnPressFeedback?.Run();
        GameEvents.ChangeScreenTo.Invoke(targetMenu, closeAndHideCurrentMenu, null);
    }
}