using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Animation;


namespace Enemy
{
    public class EnemieBase : MonoBehaviour, IDamageable
    {
        public Rigidbody thisRB;
        public Collider collider;
        public FlashColor flashColor;
        public ParticleSystem particleSystem;
        public Player player;

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

        [Header("Pursuit")]
        public bool canPursuit = false;
        public float speedOfPursuit = 5f;
        private bool _startPursuit = false;
        

        private void OnValidate()
        {
            thisRB = GetComponent<Rigidbody>();
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
            _startPursuit = true;

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
            Pursuit();

            if (Input.GetKeyDown(KeyCode.T))
            {
                OnDamage(5);
            }

        }

        public void Damage(float damage)
        {   
            OnDamage(damage);
        }

        public void Pursuit()
        {
            Vector3 lookDirection = (player.transform.position - thisRB.transform.position).normalized;

            if (canPursuit && _startPursuit)
            {
                thisRB.AddForce(lookDirection * speedOfPursuit, ForceMode.Force);
                PlayAnimationByTrigger(AnimationType.RUN);
            }
        }
    }

}
