using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Animation;

//COISAS PARA FAZER:

// - implementar o pursuit no EnemyWalk ao inv�s daqui

namespace Enemy
{
    public class EnemyBase : MonoBehaviour, IDamageable
    {
        public Rigidbody thisRB;
        public Collider collider;
        public FlashColor flashColor;
        public ParticleSystem particleSystem;
        public Player player;

        public float startLife = 10f;
        public bool lookAtPlayer = false;

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
        public float speedOfPursuit = 25f;
        public bool _startPursuit = false;

        [Header("Events")]
        public UnityEvent OnKillEvent;

        private void OnValidate()
        {
            if (thisRB == null) thisRB = GetComponent<Rigidbody>();
            if (collider == null) collider = GetComponentInChildren<Collider>();
            if (flashColor == null) flashColor = GetComponentInChildren<FlashColor>();
            if (particleSystem == null) particleSystem = GetComponentInChildren<ParticleSystem>();
            if (_animationBase == null) _animationBase = GetComponentInChildren<AnimationBase>();
        }

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            player = GameObject.FindObjectOfType<Player>();
        }

        public virtual void Update()
        {
            if (lookAtPlayer)
            {
                transform.LookAt(player.transform.position);
            }

            PlayerKilled();
            Pursuit();
        }

        protected virtual void Init()
        {
            ResetLife();
            BornAnimation();
        }

        protected virtual void PlayerKilled()
        {
            if (!player.isAlive)
            {
                _startPursuit = false;
            }
        }

        protected void ResetLife()
        {
            _currentLife = startLife;
        }

        #region LIFE
        public void Damage(float damage)
        {
            OnDamage(damage);
        }

        public void Damage(float damage, Vector3 dir)
        {
            OnDamage(damage);
            transform.DOMove(transform.position - dir, .1f);
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

        protected virtual void Kill()
        {
            canPursuit = false;

            lookAtPlayer = false;

            OnKill();
        }

        protected virtual void OnKill()
        {
            //if (collider != null) collider.enabled = false;
            Destroy(gameObject, 10f);
            PlayAnimationByTrigger(AnimationType.DEATH);
            OnKillEvent?.Invoke();
        }
        #endregion

        #region ANIMATION

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

        #region PURSUIT ATTACK

        public void Pursuit()
        {
            lookAtPlayer = true;
            Vector3 lookDirection = (player.transform.position - thisRB.transform.position).normalized;

            if (canPursuit && _startPursuit)
            {
                thisRB.AddForce(lookDirection * speedOfPursuit, ForceMode.Force);
                PlayAnimationByTrigger(AnimationType.RUN);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
                player.healthBase.Damage(1);
        }
        #endregion
    }

}
