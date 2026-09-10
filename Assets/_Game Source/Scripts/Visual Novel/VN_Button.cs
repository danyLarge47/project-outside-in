using System;
using TMPro;
using UnityEngine;

public class VN_Button : MonoBehaviour
{

    public string btnId;
    public TextMeshProUGUI txtContent;


    public void SetButton(string msg, Action<string> callback)
    {
        txtContent.text = msg;
        
        callback?.Invoke(btnId);
    }


}
