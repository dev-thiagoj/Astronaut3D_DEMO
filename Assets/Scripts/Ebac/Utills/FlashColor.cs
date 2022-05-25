using UnityEngine;
using DG.Tweening;

public class FlashColor : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public SkinnedMeshRenderer skinnedMeshRenderer;

    [Header("Setup")]
    public string colorParameter = "_EmissionColor";
    public Color color = Color.red;
    public float duration = .1f;
    
    private Tween _currTween;

    private void OnValidate()
    {
        if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null) skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    [NaughtyAttributes.Button]
    public void Flash()
    {
        if (meshRenderer != null && !_currTween.IsActive())
            _currTween = meshRenderer.material.DOColor(color, colorParameter, duration).SetLoops(2, LoopType.Yoyo);

        if (skinnedMeshRenderer != null && !_currTween.IsActive())
            _currTween = skinnedMeshRenderer.material.DOColor(color, colorParameter, duration).SetLoops(2, LoopType.Yoyo);
    }
}
