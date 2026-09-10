using System;
using System.Text;
using System.Threading.Tasks;
using GameCreator.Runtime.VisualScripting;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class UI_Screen : MonoBehaviour
{
    [FoldoutGroup("UI Screen"), SerializeField]
    public int screenLayer;

    [FoldoutGroup("UI Screen"), SerializeField]
    public string screenName;

    [FoldoutGroup("UI Screen"), SerializeField]
    public CanvasGroup screenCanvasGroup;

    [FoldoutGroup("UI Screen"), SerializeField]
    private Actions show;

    [FoldoutGroup("UI Screen"), SerializeField]
    private Actions hide;

    [FoldoutGroup("UI Screen"), SerializeField]
    public bool isOpen;

    [FoldoutGroup("UI Screen"), SerializeField]
    private bool isAnimating;

    [FoldoutGroup("UI Screen"), SerializeField]
    private UnityEvent onWindowOpen;

    [FoldoutGroup("UI Screen"), SerializeField]
    private UnityEvent onWindowClosed;

    private Action<UI_Screen> backButtonCallbackE;

    public void SetupScreen()
    {
        screenCanvasGroup = GetComponentInChildren<CanvasGroup>(true);
        Debug.Log($"Setup Screen {screenName}", gameObject);
        screenCanvasGroup.name = $"Panel : {screenName}";
        StringBuilder dashCount = new StringBuilder();
        for (int i = 1; i < screenLayer; i++)
        {
            dashCount.Append("─");
        }

        // gameObject.name = $"└{dashCount} Layer {screenLayer} , Screen : {screenName}";      
        gameObject.name = $"└{dashCount} {screenName}";

        // TextMeshProUGUI tmp = panel.gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
        // if (tmp)
        // {
        //     tmp.gameObject.name = $"txt_menuTitle_{screenName}";
        //     tmp.text = $"Menu :: {screenName}";
        // }
    }

    [Button(ButtonSizes.Large)]
    public void ForceOpen()
    {
        if (isAnimating) return;
        isAnimating = true;
        _ = AnimateOpen();
    }

    [Button(ButtonSizes.Large)]
    public void ForceClose()
    {
        // Debug.Log($"ForceClose Called by {name}", gameObject);
        if (isAnimating) return;
        isAnimating = true;
        _ = AnimateClose();
    }

    [Button(ButtonSizes.Large)]
    public void DebugToggleView()
    {
        if (screenCanvasGroup == null) return;
        if (isOpen)
        {
            screenCanvasGroup.alpha = 0f;
            screenCanvasGroup.blocksRaycasts = false;
            screenCanvasGroup.interactable = false;
            screenCanvasGroup.gameObject.SetActive(false);
        }
        else
        {
            screenCanvasGroup.alpha = 1f;
            screenCanvasGroup.blocksRaycasts = true;
            screenCanvasGroup.interactable = true;
            screenCanvasGroup.gameObject.SetActive(true);
        }

        isOpen = !isOpen;
    }

    public void Open(Action callback = null)
    {
        // Debug.Log($"{name} Open Called", gameObject);
        if (isOpen) return;
        if (isAnimating) return;
        isAnimating = true;
        _ = AnimateOpen(callback);
    }

    public void Close(Action callback = null)
    {
        // Debug.Log($"{name} Close Called", gameObject);
        if (!isOpen) return;
        if (isAnimating) return;
        // Debug.Log($"{name} Executing" + $" Close");
        isAnimating = true;
        _ = AnimateClose(callback);
    }

    async Task AnimateOpen(Action callback = null)
    {
        await show.Run();
        isAnimating = false;
        isOpen = true;
        callback?.Invoke();
        onWindowOpen.Invoke();
    }

    async Task AnimateClose(Action callback = null)
    {
        await hide.Run();
        isAnimating = false;
        isOpen = false;
        callback?.Invoke();
        onWindowClosed.Invoke();
    }
}