using UnityEngine;
using DG.Tweening;

public class ChestBase : MonoBehaviour
{
    [Header("Animation")]
    public Animator animator;
    public string triggerOpen = "Open";
    public KeyCode keyCode = KeyCode.Z;

    [Header("Notification")]
    public GameObject notification;
    public float startScale;
    public float duration = .2f;
    public Ease ease = Ease.OutBack;

    [Space]
    public ChestItemBase chestItemBase;

    private bool _chestOpened = false;


    void Start()
    {
        startScale = notification.transform.localScale.x;
        HideNotification();
    }

    void Update()
    {
        if (Input.GetKeyDown(keyCode) && notification.activeSelf)
        {
            OpenChest();
        }
    }

    [NaughtyAttributes.Button]
    private void OpenChest()
    {
        if (_chestOpened) return;

        animator.SetTrigger(triggerOpen);
        _chestOpened = true;
        HideNotification();
        Invoke(nameof(ShowItem), .3f);
    }

    private void ShowItem()
    {
        chestItemBase.ShowItem();
        Invoke(nameof(CollectItem), 1f);
    }

    private void CollectItem()
    {
        chestItemBase.Collect();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && !_chestOpened)
        {
            ShowNotification();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            HideNotification();
        }
    }

    private void ShowNotification()
    {
        notification.SetActive(true);
        notification.transform.localScale = Vector3.zero;
        notification.transform.DOScale(startScale, duration).SetEase(ease);
    }

    private void HideNotification()
    {
        notification.SetActive(false);
    }
}
