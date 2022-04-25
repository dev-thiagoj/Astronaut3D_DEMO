using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Animation;


namespace Enemy
{
    public class EnemieBase : MonoBehaviour, IDamageable
    {
        public Collider collider;
        public FlashColor flashColor;
        public ParticleSystem particleSystem;

        public float startLife = 10f;

        [SerializeField] private float _currentLife;
        
        [Header("Animation")]
        [SerializeField] private AnimationBase _animationBase;

        [Header("Start Animation")]
        public float startDuration = .2f;
        public Ease ease = Ease.OutBack;
        public bool startWithBornAnimation = true;

        [Header("Particles")]
        public int intParticles = 15;

        private void OnValidate()
        {
            collider = GetComponentInChildren<Collider>();
            flashColor = GetComponentInChildren<FlashColor>();
            particleSystem = GetComponentInChildren<ParticleSystem>();
        }

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
            if (collider != null) collider.enabled = false;
            Destroy(gameObject, 10f);
            PlayAnimationByTrigger(AnimationType.DEATH);
        }

        public void OnDamage(float f)
        {
            if (flashColor != null) flashColor.Flash();
            if (particleSystem != null) particleSystem.Emit(intParticles);
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

        public void PlayAnimationByTrigger(AnimationType animationType)
        {
            _animationBase.PlayAnimationByTrigger(animationType);
        }

        #endregion

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                OnDamage(5);
            }
        }

        public void Damage(float damage)
        {
            Debug.Log("Damage");
            OnDamage(damage);
        }
    }

}
