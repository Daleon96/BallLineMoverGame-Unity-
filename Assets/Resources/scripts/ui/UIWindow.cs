using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class UIWindow : MonoBehaviour
{
    public virtual void OpenWindow()
    {
        Transform targetObject = gameObject.transform.GetChild(0);
        targetObject.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        gameObject.SetActive(true);
        DOTween.Sequence()
            .Append(targetObject.DOScale(Vector3.one, 1f)
                .SetEase(Ease.OutBack)) // Збільшення з 50% до 100% за 1 сек
            .OnComplete(() => Debug.Log("Анімація завершена"));
    }

    public virtual void CloseWindow()
    {
        Transform targetObject = this.gameObject.transform.GetChild(0);
        targetObject.localScale = new Vector3(1f, 1f, 1f);
        DOTween.Sequence()
            .Append(targetObject.DOScale(Vector3.zero, 0.25f).SetEase(Ease.Linear))  // Збільшення з 50% до 100% за 1 сек
            .OnComplete(() =>
            {
                this.gameObject.SetActive(false);
            });
    }
}
