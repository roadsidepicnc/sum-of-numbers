using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public static class TransformExtensions
{
    public static async UniTask DoHitScale(this Transform targetTransform, Action onCompleteCallback, float duration = 0.2f)
    {
        targetTransform.localScale = Vector3.one * 0.9f;
        await targetTransform.DOScale(1f, duration).SetEase(Ease.OutCirc, 3f).From(0f);
        onCompleteCallback?.Invoke();
    }
}