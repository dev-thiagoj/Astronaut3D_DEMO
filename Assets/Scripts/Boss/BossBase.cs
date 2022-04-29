using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.StateMachine;
using DG.Tweening;
using Animation;

//COISAS PARA FAZER:

//arrumar o healthbase do bossbase
//implementar particle system do boss
//implementar SFX do boss
//arrumar posiçao da gun para nao ser afetada pela animaçao de attack
//add callback para sincronizar animaçao de attack com o tiro


namespace Boss
{
    public enum BossAction
    {
        INIT,
        IDLE,
        WALK,
        ATTACK,
        DEATH
    }

    public class BossBase : MonoBehaviour, IDamageable
    {
        public StateMachine<BossAction> stateMachine;

        [Header("Animation")]
        [SerializeField] private AnimationBase _animationBase;
        public float startDuration = .5f;
        public Ease ease = Ease.OutBack;

        [Header("Attack")]
        public GunBase gunBase;
        public bool lookAtPlayer = false;
        private Player _player;
        public int attackAmount = 5;
        public float timeBetweenAttacks = 2f;

        public float speed = 5f;
        public List<Transform> waypoints;
        public GameObject bossTrigger;

        [SerializeField] private HealthBase healthBase;
        [SerializeField] private FlashColor flashColor;
        //private Vector3 _currScale = Vector3.one; //

        private void OnValidate()
        {
            if (healthBase == null) healthBase = GetComponent<HealthBase>();
            if (_animationBase == null) _animationBase = GetComponentInChildren<AnimationBase>();
            if (_player == null) _player = GameObject.FindObjectOfType<Player>();
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
            stateMachine.RegisterStates(BossAction.WALK, new BossStateWalk());
            stateMachine.RegisterStates(BossAction.ATTACK, new BossStateAttack());
            stateMachine.RegisterStates(BossAction.DEATH, new BossStateDeath());
        }

        private void Start()
        {
            gunBase = GetComponentInChildren<GunBase>();
            //transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); //
        }

        private void Update()
        {
            if (lookAtPlayer)
            {
                transform.LookAt(_player.transform.position);
            }

            if (_player.healthBase._currLife <= 0)
            {
                gunBase.StopShoot();
                StopAllCoroutines();
            }
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
            //if (particleSystem != null) particleSystem.Emit(intParticles);

            transform.position -= transform.forward; //serve para dar um "tranco" no inimigo qdo ele leva o tiro

            healthBase._currLife -= f;

            if (healthBase._currLife <= 0)
            {
                Kill();
            }
        }

        protected virtual void Kill()
        {
            SwitchState(BossAction.DEATH);

            lookAtPlayer = false;

            _animationBase.PlayAnimationByTrigger(AnimationType.DEATH);
            transform.localScale = Vector3.one;

            OnKill();
        }

        protected virtual void OnKill()
        {
            Destroy(gameObject, 10f);
        }

        public void OnBossKill(HealthBase h)
        {

        }

        #endregion

        #region WALK

        public void GoToRandomPoint(Action onArrive = null) //Action onArrive = Callback para informar qdo chegou no waypoint para poder começar a atacar, coloco como nulo para não ser um parametro obrigatorio, mas se tiver será usado
        {
            Debug.Log("Enter Walk Random Point");
            StartCoroutine(GoToPointCoroutine(waypoints[UnityEngine.Random.Range(0, waypoints.Count)], onArrive)); //coloquei UnityEngine.Random pois o Random é usado tanto pelo using system, qto pelo using UnityEngine então eu escolho de qual será usado
        }

        IEnumerator GoToPointCoroutine(Transform t, Action onArrive = null)
        {
            while (Vector3.Distance(transform.position, t.position) > 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, t.position, Time.deltaTime * speed);
                _animationBase.PlayAnimationByTrigger(AnimationType.RUN);
                yield return new WaitForEndOfFrame();
            }

            onArrive?.Invoke(); //é o mesmo que if (onArrive != null) onArrive.Invoke();

        }

        #endregion

        #region ATTACK

        public void StartAttack(Action endCallback = null) //Action endCallback = callback para informar qdo acabou a rodada de ataque
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
                gunBase.StartShoot();
                yield return new WaitForSeconds(timeBetweenAttacks);
            }

            endCallback?.Invoke();

        }

        #endregion

        #region ANIMATION

        public void StartInitAnimation()
        {
            Debug.Log("Start Animation Enter");
            transform.DOScale(0, startDuration).SetEase(ease).From(); //(0, startDuration)
            //SwitchState(BossAction.WALK);
        }

        #endregion

        #region DEBUG
        [NaughtyAttributes.Button]
        public void SwitchInit()
        {
            SwitchState(BossAction.INIT);
        }

        [NaughtyAttributes.Button]
        public void SwitchWalk()
        {
            SwitchState(BossAction.WALK);
        }

        [NaughtyAttributes.Button]
        void SwitchAttack()
        {
            SwitchState(BossAction.ATTACK);
        }

        [NaughtyAttributes.Button]
        public void DebugDamage()
        {
            Damage(5);
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
