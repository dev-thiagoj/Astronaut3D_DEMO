using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Ebac.StateMachine;
using DG.Tweening;
using Animation;


namespace Boss
{
    public enum BossAction
    {
        INIT,
        IDLE,
        ANIM,
        WALK,
        ATTACK,
        DEATH
    }

    public class BossBase : MonoBehaviour, IDamageable
    {
        public StateMachine<BossAction> stateMachine;

        public bool _isAlive = true;

        [Header("Animation")]
        [SerializeField] private AnimationBase _animationBase;
        public float startDuration = 2f;
        public Ease ease = Ease.OutBack;
        [SerializeField] private HealthBase healthBase;
        [SerializeField] private FlashColor flashColor;
        private Vector3 _normalScale = Vector3.one;

        [Header("Attack")]
        public EnemyGun enemyGun;
        public ProjectileBase projectileBase;
        public bool lookAtPlayer = true;
        private Player _player;
        public int attackAmount = 5;
        public float timeBetweenAttacks = 2f;

        [Header("SFX")]
        public SFXType sfxDeath;
        public SFXType sfxWakeup;

        [Header("Waypoints")]
        public float speed = 5f;
        public List<Transform> waypoints;

        [Header("Events")]
        public UnityEvent OnKillEvent;

        private void OnValidate()
        {
            if (healthBase == null) healthBase = GetComponent<HealthBase>();
            if (_animationBase == null) _animationBase = GetComponentInChildren<AnimationBase>();
            if (_player == null) _player = FindObjectOfType<Player>();
            if (flashColor == null) flashColor = GetComponentInChildren<FlashColor>();
        }

        private void Awake()
        {
            Init();
            OnValidate();
            
            if(healthBase != null) healthBase.OnKill += OnBossKill;
        }

        private void Init()
        {
            stateMachine = new StateMachine<BossAction>();
            stateMachine.Init();
            
            stateMachine.RegisterStates(BossAction.INIT, new BossStateInit());
            stateMachine.RegisterStates(BossAction.ANIM, new BossStateAnim());
            stateMachine.RegisterStates(BossAction.WALK, new BossStateWalk());
            stateMachine.RegisterStates(BossAction.ATTACK, new BossStateAttack());
            stateMachine.RegisterStates(BossAction.DEATH, new BossStateDeath());
        }

        private void Start()
        {
            enemyGun = GetComponentInChildren<EnemyGun>();
            transform.localScale = _normalScale / 2;
        }

        private void Update()
        {
            if (lookAtPlayer)
            {
                transform.LookAt(Player.Instance.transform.position);
            }

            if (!_player.isAlive)
            {
                enemyGun.StopShoot();
                StopAllCoroutines();
            }
        }

        [NaughtyAttributes.Button]
        public void StartBoss()
        {
            SwitchState(BossAction.ANIM);
        }

        #region DAMAGE/KILL

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

            transform.position -= transform.forward;

            healthBase._currLife -= f;

            if (healthBase._currLife <= 30)
            {
                enemyGun.amountShoots = 3;
                enemyGun.angle = 5;
            }

            if(healthBase._currLife <= 15)
            {
                enemyGun.angle = 3;
                projectileBase.speed = 40;
            }

            if (healthBase._currLife <= 0)
            {
                Kill();
            }
        }

        private void PlayDeathSFX()
        {
            SFXPool.Instance.Play(sfxDeath);
        }
        
        private void PlayWakeupSFX()
        {
            SFXPool.Instance.Play(sfxWakeup);
        }

        protected virtual void Kill()
        {
            SwitchState(BossAction.DEATH);

            _isAlive = false;

            lookAtPlayer = false;

            enemyGun.StopShoot();

            PlayDeathSFX();

            _animationBase.PlayAnimationByTrigger(AnimationType.DEATH);
            transform.localScale = Vector3.one;

            OnKill();
        }

        protected virtual void OnKill()
        {
            OnKillEvent?.Invoke();
            Destroy(gameObject, 10f);
        }

        public void OnBossKill(HealthBase h)
        {

        }

        #endregion

        #region WALK

        public void GoToRandomPoint(Action onArrive = null)
        {
            StartCoroutine(GoToPointCoroutine(waypoints[UnityEngine.Random.Range(0, waypoints.Count)], onArrive));
        }

        IEnumerator GoToPointCoroutine(Transform t, Action onArrive = null)
        {
            while (Vector3.Distance(transform.position, t.position) > 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, t.position, Time.deltaTime * speed);
                transform.LookAt(Player.Instance.transform.position);
                _animationBase.PlayAnimationByTrigger(AnimationType.RUN);
                yield return new WaitForEndOfFrame();
            }

            onArrive?.Invoke();
        }

        #endregion

        #region ATTACK

        public void StartAttack(Action endCallback = null)
        {
            StartCoroutine(StartAttackCoroutine(endCallback));
        }

        IEnumerator StartAttackCoroutine(Action endCallback = null)
        {
            int attacks = 0;

            while (attacks < attackAmount)
            {
                attacks++;
                _animationBase.PlayAnimationByTrigger(AnimationType.ATTACK);
                enemyGun.StartShoot();
                yield return new WaitForSeconds(timeBetweenAttacks);
            }

            endCallback?.Invoke();

        }

        public void StopShoot()
        {
            enemyGun.StopShoot();
        }

        #endregion

        #region ANIMATION

        public void StartInitAnimation()
        {
            PlayWakeupSFX();
            transform.DOScale(_normalScale, startDuration).SetEase(ease);

            Invoke(nameof(StartAction), 5);
        }

        public void StartAction()
        {
            SwitchState(BossAction.WALK);
        }

        #endregion

        #region STATE MACHINE

        public void SwitchState(BossAction state)
        {
            stateMachine.SwitchState(state, this);
        }

        #endregion
    }

}
