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
        private Player _player;

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


        private void OnValidate()
        {
            thisRB = GetComponent<Rigidbody>();
            collider = GetComponentInChildren<Collider>();
            flashColor = GetComponentInChildren<FlashColor>();
            particleSystem = GetComponentInChildren<ParticleSystem>();
            _animationBase = GetComponentInChildren<AnimationBase>();
        }

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            _player = GameObject.FindObjectOfType<Player>();
        }

        public virtual void Update()
        {
            if (lookAtPlayer)
            {
                transform.LookAt(_player.transform.position);
            }

            PlayerKilled();
            Pursuit();
        }

        public void PlayerKilled()
        {
            if (_player.healthBase._currLife <= 0)
            {
                _startPursuit = false;
            }
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
            canPursuit = false;

            lookAtPlayer = false;

            OnKill();
        }

        protected virtual void OnKill()
        {
            PlayAnimationByTrigger(AnimationType.DEATH);
            Destroy(gameObject, 10f);
        }

        public void OnDamage(float f)
        {
            if (flashColor != null) flashColor.Flash();
            if (particleSystem != null) particleSystem.Emit(intParticles);

            transform.position -= transform.forward; //serve para dar um "tranco" no inimigo qdo ele leva o tiro

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


        public void Damage(float damage)
        {
            OnDamage(damage);
        }

        public void Damage(float damage, Vector3 dir)
        {
            OnDamage(damage);
            transform.DOMove(transform.position - dir, .1f);
        }

        public void Pursuit()
        {
            Vector3 lookDirection = (_player.transform.position - thisRB.transform.position).normalized;
            lookAtPlayer = true;

            if (canPursuit && _startPursuit)
            {
                thisRB.AddForce(lookDirection * speedOfPursuit, ForceMode.Force);
                PlayAnimationByTrigger(AnimationType.RUN);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
                _player.Damage(1);
        }
    }

}
