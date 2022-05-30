using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Animation;
using System.Collections;

namespace Enemy
{
    public class EnemyBase : MonoBehaviour, IDamageable
    {
        public Rigidbody thisRB;
        public Collider collider;
        public FlashColor flashColor;

        public float startLife = 10f;

        [Header("Look At")]
        public bool lookAtPlayer = false;
        public Transform target;

        [SerializeField] private float _currentLife;
        private Vector3 _currPos;
        private Quaternion _currRotation;

        [Header("Animation")]
        [SerializeField] private AnimationBase _animationBase;

        [Header("Start Animation")]
        public float startDuration = .2f;
        public Ease ease = Ease.OutBack;
        public bool startWithBornAnimation = true;

        [Header("Particles")]
        public ParticleSystem particleSystem;
        public int intParticles = 15;

        [Header("Pursuit")]
        public bool canPursuit = false;
        public float speedOfPursuit = 25f;
        public bool _startPursuit = false;

        [Header("Events")]
        public UnityEvent OnKillEvent;

        [Header("SFX")]
        public SFXPlayerHelper sFXPlayerHelper;

        private void OnValidate()
        {
            if (thisRB == null) thisRB = GetComponent<Rigidbody>();
            if (collider == null) collider = GetComponentInChildren<Collider>();
            if (flashColor == null) flashColor = GetComponentInChildren<FlashColor>();
            if (particleSystem == null) particleSystem = GetComponentInChildren<ParticleSystem>();
            if (_animationBase == null) _animationBase = GetComponentInChildren<AnimationBase>();
            if (sFXPlayerHelper == null) sFXPlayerHelper = GetComponentInChildren<SFXPlayerHelper>();
        }

        private void Awake()
        {   
            Init();
        }

        private void Start()
        {
            //target = Player.Instance.transform;
        }

        public virtual void Update()
        {
            if (lookAtPlayer)
            {
                var lookPos = Player.Instance.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
                
                //transform.LookAt(Player.Instance.transform.position);
            }

            _currPos = collider.transform.position;
            _currRotation = collider.transform.rotation;

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
            if (!Player.Instance.isAlive)
            {
                canPursuit = false;
                _startPursuit = false;
            }
            else return;
        }

        protected void ResetLife()
        {
            _currentLife = startLife;
        }

        #region ========== LIFE ==========

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
            transform.position = new Vector3(_currPos.x, _currPos.y, _currPos.z);
            transform.rotation = new Quaternion(_currRotation.x, _currRotation.y, _currRotation.z, _currRotation.w);
            lookAtPlayer = false;
            canPursuit = false;

            thisRB.useGravity = false;
            if (thisRB != null) thisRB.Sleep();
            if (collider != null) collider.enabled = false;


            OnKill();
        }

        protected virtual void OnKill()
        {
            Destroy(gameObject, 10f);
            PlayAnimationByTrigger(AnimationType.DEATH);
            sFXPlayerHelper.PlayEnemyDead();
            OnKillEvent?.Invoke();
        }
        #endregion

        #region ========== ANIMATION ==========

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

        #region ========== PURSUIT ATTACK ==========

        public void Pursuit()
        {
            lookAtPlayer = true;
            Vector3 lookDirection = (Player.Instance.transform.position - thisRB.transform.position).normalized;

            if (canPursuit && _startPursuit)
            {
                thisRB.AddForce(lookDirection.normalized * speedOfPursuit, ForceMode.Force);
                if (thisRB.velocity.sqrMagnitude > 1f) thisRB.velocity *= 0.99f;
                PlayAnimationByTrigger(AnimationType.RUN);
            }
            else return;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
                Player.Instance.Kill(Player.Instance.healthBase);
        }
        #endregion
    }

}
