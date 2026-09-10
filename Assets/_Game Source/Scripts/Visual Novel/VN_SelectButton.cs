using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class VN_SelectButton : MonoBehaviour
{
    public string btnId;
    public TextMeshProUGUI txtContent;
    public Button mButton;

    private Action<string> onClickCallback;

    private void Awake()
    {
        if (mButton == null) mButton = GetComponent<Button>();
    }

  
    public void SetButton(string msg, Action<string> callback)
    {
        if (mButton == null) mButton = GetComponent<Button>();

        txtContent.text = msg;
        onClickCallback = callback;

        mButton.onClick.RemoveListener(OnButtonClick);
        mButton.onClick.AddListener(OnButtonClick);

        gameObject.SetActive(true);
    }

    private void OnButtonClick()
    {
        onClickCallback?.Invoke(btnId);
    }

    private void OnDestroy()
    {
        if (mButton != null) mButton.onClick.RemoveListener(OnButtonClick);
    }
}
