using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Itens
{
    public class ItemCollectableBase : MonoBehaviour
    {
        public ItemType itemType;

        public string compareTag = "Player";
        public ParticleSystem particleSystem;
        public GameObject graphicItem;
        public float timeToHide = 0.1f;

        public Collider collider;

        [Header("Sounds")]
        public AudioSource audioSource;

        private void OnValidate()
        {
            //collider = GetComponent<Collider>();
        }

        private void Awake()
        {
            OnValidate();
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.transform.CompareTag(compareTag))
            {
                Collect();
            }
        }

        protected virtual void Collect()
        {
            if (collider != null) collider.enabled = false;
            if (graphicItem != null) graphicItem.SetActive(false);
            Invoke(nameof(HideObject), timeToHide);
            OnCollect();
            //Destroy(gameObject, timeToDestroy);
        }

        private void HideObject()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnCollect()
        {
            //VFXManager.Instance.PlayVFXByType(VFXManager.VFXType.COLLECTCOINS, transform.position);
            //if (audioSource != null) audioSource.Play();
            //if (particleSystem != null) particleSystem.Play();

            ItemManager.Instance.AddByType(itemType);
        }
    }

}
