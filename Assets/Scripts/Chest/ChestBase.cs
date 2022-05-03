using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChestBase : MonoBehaviour
{
    public Animator animator;
    public string triggerOpen = "Open";
    
    [Header("Notification")]
    public GameObject notification;
    public float startScale;
    public float duration = .2f;
    public Ease ease = Ease.OutBack;
    

    // Start is called before the first frame update
    void Start()
    {
        startScale = notification.transform.localScale.x;
        HideNotification();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [NaughtyAttributes.Button]
    private void OpenChest()
    {
        animator.SetTrigger(triggerOpen);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            OpenChest();
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
