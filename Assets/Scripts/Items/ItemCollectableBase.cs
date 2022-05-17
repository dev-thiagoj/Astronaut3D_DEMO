using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Itens
{
    public class ItemCollectableBase : MonoBehaviour
    {
        public SFXType sfxType;
        public ItemType itemType;

        public string compareTag = "Player";
        //public ParticleSystem particleSystem;
        public GameObject graphicItem;
        public float distToCollect = 0.2f;
        public float timeToHide = 0.1f;
        public float timeToDestroy = 0.3f;

        private RayToTheGround _rayTo;

        /*public Vector3 currPos; //
        public float distToGround; //
        public float spaceToGround = .1f; //
        public Collider collider; //
        public Rigidbody myRb; //*/

        private void Awake()
        {
            //if (myRb == null) myRb = GetComponent<Rigidbody>(); //
            //if (collider != null) distToGround = collider.bounds.extents.y; //

            if (_rayTo != null) _rayTo = GetComponent<RayToTheGround>(); 
        }

        /*private void Update()
        {
            Debug.Log(IsGrounded());
            
            currPos.y = collider.transform.position.y;

            if (IsGrounded())
            {
                Destroy(myRb);
            }
        }*/


        private void OnTriggerEnter(Collider collision)
        {
            if (collision.transform.CompareTag(compareTag))
            {
                Collect();
            }
        }

        /*private bool IsGrounded()
        {
            Debug.DrawRay(transform.position, -Vector2.up, Color.magenta, distToGround + spaceToGround);
            return Physics.Raycast(collider.transform.position, -Vector3.up, distToGround + spaceToGround);
        }*/

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
            //if (particleSystem != null) particleSystem.Play();

            ItemManager.Instance.AddByType(itemType);
        }
    }

}
