using System;
using System.Threading.Tasks;
using DG.Tweening;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using Tween = DG.Tweening.Tween;


[Category("_Custom Actions/UI/Canvas Group")]
[Keywords("UI", "Canvas", "CanvasGroup", "Fade")]
[Serializable]
public class Action_CanvasGroup : Instruction
{
    public CanvasGroup canvasGroup;
    [Range(0.0f, 1.0f)] public float alpha;
    public float duration = 0.3f;
    public bool interactable;
    public bool blockRaycasts;
    public bool waitComplete;
    public bool hideAfter = true;
    public Ease ease = Ease.Linear;
    private bool isWaiting;
    private Tween mTween;
    

    protected override async Task Run(Args args)
    {
        if (mTween != null && mTween.IsActive()) mTween.Kill();
        this.canvasGroup.interactable = interactable;
        this.canvasGroup.blocksRaycasts = this.blockRaycasts;
        canvasGroup.gameObject.SetActive(true);
        isWaiting = waitComplete;
        if (canvasGroup.alpha != alpha)
        {
            // if (alpha > 0) canvasGroup.alpha = 0f;
            mTween = canvasGroup.DOFade(alpha, duration).SetEase(ease).OnComplete(delegate
            {
                if (hideAfter && alpha <= 0.1f) canvasGroup.gameObject.SetActive(false);
                isWaiting = false;
            }).SetUpdate(true);
            if (waitComplete)
                while (isWaiting) await Task.Yield();

        }

    }
}


 