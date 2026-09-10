using System;
using System.Threading.Tasks;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VN_Dialogue_Delivery : MonoBehaviour
{
    private bool isDialogWindowOpen;
    public Image characterPortrait;
    public Text characterName;
    public Text characterDialogue;

    [Tooltip("Full-panel/advance button the player clicks to move to the next line.")]
    public Button nextButton;

    public Actions closeDialogueUI;
    public Actions showDialogueUI;

    private TaskCompletionSource<bool> nextInput;

    private void Awake()
    {
        if (nextButton != null) nextButton.onClick.AddListener(OnNextPressed);
    }

    private void OnDestroy()
    {
        if (nextButton != null) nextButton.onClick.RemoveListener(OnNextPressed);
    }

    private VN_World_Manager manager;

    public void Initialize(VN_World_Manager vnWorldManager)
    {
        manager = vnWorldManager;

    }

    public async Task DoDialogue(string speakerName, string dialogue, Sprite expression, Action dialogueComplete)
    {
        if (!isDialogWindowOpen)
        {
            await showDialogueUI.Run();
            isDialogWindowOpen = true;
        }

        if (characterPortrait != null)
        {
            characterPortrait.sprite = expression;
            characterPortrait.enabled = expression != null;
        }

        if (characterName != null) characterName.text = speakerName;
        if (characterDialogue != null) characterDialogue.text = dialogue;

        await WaitForNext();

        dialogueComplete?.Invoke();
    }

    public async Task CloseDialogue()
    {
        if (!isDialogWindowOpen) return;

        await closeDialogueUI.Run();
        isDialogWindowOpen = false;
    }

    private Task WaitForNext()
    {
        nextInput = new TaskCompletionSource<bool>();
        return nextInput.Task;
    }

    private void OnNextPressed()
    {
        nextInput?.TrySetResult(true);
    }
}
