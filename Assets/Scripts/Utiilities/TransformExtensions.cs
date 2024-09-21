using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public static class TransformExtensions
{
    public static async UniTask DOHitScale(this Transform targetTransform, Action onCompleteCallback, float duration = 0.2f)
    {
        targetTransform.localScale = Vector3.one * 0.9f;
        targetTransform.DOScale(1f, duration).SetEase(Ease.OutBack, 3f);
        await UniTask.Delay((int)(duration * 1000));
        onCompleteCallback?.Invoke();
    }
}