using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace Enemy
{
    public class EnemieBase : MonoBehaviour
    {
        public float startLife = 10f;

        [SerializeField] private float _currentLife;

        [Header("Start Animation")]
        public float startDuration = .2f;
        public Ease ease = Ease.OutBack;
        public bool startWithBornAnimation = true;

        private void Awake()
        {
            Init();
        }

        protected void ResetLife()
        {
            _currentLife = startLife;
        }

        protected virtual void Init()
        {
            ResetLife();
            BornAnimation();
        }

        protected virtual void Kill()
        {
            OnKill();
        }

        protected virtual void OnKill()
        {
            Destroy(gameObject);
        }

        public void OnDamage(float f)
        {
            _currentLife -= f;

            if (_currentLife <= 0)
            {
                Kill();
            }
        }

        #region Animation

        private void BornAnimation()
        {
            if (startWithBornAnimation)
                transform.DOScale(0, startDuration).SetEase(ease).From();
        }

        #endregion

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                OnDamage(5);
            }
        }
    }

}
