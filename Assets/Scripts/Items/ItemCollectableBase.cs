using UnityEngine;

namespace Itens
{
    public class ItemCollectableBase : MonoBehaviour
    {
        public SFXType sfxType;
        public ItemType itemType;

        public string compareTag = "Player";
        public GameObject graphicItem;
        public float distToCollect = 0.2f;
        public float timeToHide = 0.1f;
        public float timeToDestroy = 0.3f;

        private RayToTheGround _rayTo;

        private void Awake()
        {
            if (_rayTo != null) _rayTo = GetComponent<RayToTheGround>(); 
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.transform.CompareTag(compareTag))
            {
                Collect();
            }
        }

        private void PlaySFX()
        {
            SFXPool.Instance.Play(sfxType);
        }

        protected virtual void Collect()
        {
            PlaySFX();
            if (graphicItem != null) graphicItem.SetActive(false);
            Invoke(nameof(HideObject), timeToHide);
            OnCollect();
            Destroy(gameObject, timeToDestroy);
        }

        private void HideObject()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnCollect()
        {
            ItemManager.Instance.AddByType(itemType);
        }
    }

}
