using System.Collections;
using System.Collections.Generic;
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


    // Start is called before the first frame update
    void Start()
    {
        startScale = notification.transform.localScale.x;
        HideNotification();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyCode) && notification.activeSelf) //activeSelf retorna uma booleana, para false usar !notification.activeSelf
        {
            OpenChest();
        }
    }

    [NaughtyAttributes.Button]
    private void OpenChest()
    {
        if (_chestOpened) return; //se o bau ja estiver sido aberto, retorna

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

    [NaughtyAttributes.Button]

    private void ShowNotification()
    {
        notification.SetActive(true);
        notification.transform.localScale = Vector3.zero;
        notification.transform.DOScale(startScale, duration).SetEase(ease);
    }

    [NaughtyAttributes.Button]

    private void HideNotification()
    {
        notification.SetActive(false);
    }
}
