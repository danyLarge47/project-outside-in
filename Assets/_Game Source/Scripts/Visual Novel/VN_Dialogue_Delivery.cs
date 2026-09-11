using System;
using System.Threading.Tasks;
using GameCreator.Runtime.VisualScripting;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class VN_Dialogue_Delivery : MonoBehaviour
{
    public UI_Screen vnScreen;
    public Image characterPortrait;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterDialogue;

	[SerializeField] InputActionReference nextButton;

    private TaskCompletionSource<bool> nextInput;
    private VN_World_Manager manager;

    public Actions goToActionScene;
        
 
    void OnEnable()
    {
        nextButton.action.performed += OnNextPressed; 

        nextButton.action.Enable(); 
    }

    void OnDisable()
    {
        nextButton.action.performed -= OnNextPressed; 

        nextButton.action.Disable(); 
    }
    

    public void Initialize(VN_World_Manager vnWorldManager)
    {
        manager = vnWorldManager;

    }

    public async Task RunDialogueContent(VN_DialogueContent contentNode)
    {
        if (contentNode == null || contentNode.dialogues == null) return;

        await ShowScreen();

        Debug.Log($"VN_Dialogue_Delivery.RunDialogueContent()");
        for (int i = 0; i < contentNode.dialogues.Count; i++)
        {
            var line = contentNode.dialogues[i];
            if (line == null) continue;
            await DoDialogue(line.speakerName, line.text, line.expression, null);
        }

        await CloseDialogue();

        goToActionScene?.Run();
        
    }

    public async Task DoDialogue(string speakerName, string dialogue, Sprite expression, Action dialogueComplete)
    {
        await ShowScreen();

        // if (characterPortrait != null)
        // {
        //     characterPortrait.sprite = expression;
        //     characterPortrait.enabled = expression != null;
        // }

        if (characterName != null) characterName.text = speakerName;
        if (characterDialogue != null) characterDialogue.text = dialogue;

        await WaitForNext();

        
        dialogueComplete?.Invoke();

    }

    public Task CloseDialogue()
    {
        return HideScreen();
    }

    private Task ShowScreen()
    {
        if (vnScreen == null || vnScreen.isOpen) return Task.CompletedTask;

        var tcs = new TaskCompletionSource<bool>();
        vnScreen.Open(() => tcs.TrySetResult(true));
        return tcs.Task;
    }

    private Task HideScreen()
    {
        if (vnScreen == null || !vnScreen.isOpen) return Task.CompletedTask;

        var tcs = new TaskCompletionSource<bool>();
        vnScreen.Close(() => tcs.TrySetResult(true));
        return tcs.Task;
    }

    private Task WaitForNext()
    {
        nextInput = new TaskCompletionSource<bool>();
        return nextInput.Task;
    }

    private void OnNextPressed(InputAction.CallbackContext context)
    {
        nextInput?.TrySetResult(true);
    }
}
