using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIAmmoUpdater : MonoBehaviour
{
    public Image uiImage;

    [Header("Animation")]
    public float duration = .1f;
    public Ease ease = Ease.Linear;

    private Tween _currTween;

    private void OnValidate()
    {
        if (uiImage == null) uiImage = GetComponent<Image>();
    }

    public void UpdateValue(float f)
    {
        uiImage.fillAmount = f;
    }

    public void UpdateValue(float max, float current)
    {
        if (_currTween != null) _currTween.Kill(); //prevenir possíveis bugs de começar um tween com outro já em andamento
        _currTween = uiImage.DOFillAmount(1 - (current / max), duration).SetEase(ease);
    }
}
