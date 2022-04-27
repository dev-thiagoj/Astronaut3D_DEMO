using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.StateMachine;
using DG.Tweening;


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

    public class BossBase : MonoBehaviour
    {
        private StateMachine<BossAction> stateMachine;

        [Header("Animation")]
        public float startDuration = .5f;
        public Ease ease = Ease.OutBack;

        [Header("Attack")]
        public int attackAmount = 5;
        public float timeBetweenAttacks = 2f;

        public float speed = 5f;
        public List<Transform> waypoints;

        private HealthBase healthBase;

        private void Awake()
        {
            healthBase = GetComponent<HealthBase>();

            Init();

            healthBase.OnKill += OnBossKill;
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

        private void OnBossKill(HealthBase h)
        {
            SwitchState(BossAction.DEATH);
        }

        #region WALK
        
        public void GoToRandomPoint(Action onArrive = null) //Action onArrive = Callback para informar qdo chegou no waypoint para poder começar a atacar, coloco como nulo para não ser um parametro obrigatorio, mas se tiver será usado
        {
            StartCoroutine(GoToPointCoroutine(waypoints[UnityEngine.Random.Range(0, waypoints.Count)], onArrive)); //coloquei UnityEngine.Random pois o Random é usado tanto pelo using system, qto pelo using UnityEngine então eu escolho de qual será usado
            //StartCoroutine(GoToPointCoroutine(waypoints[DevUtills.DevGetRandom(xxx)]));
        }

        IEnumerator GoToPointCoroutine(Transform t, Action onArrive = null)
        {
            while(Vector3.Distance(transform.position, t.position) > 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, t.position, Time.deltaTime * speed);
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

            while(attacks < attackAmount)
            {
                attacks++;
                transform.DOScale(1.1f, .1f).SetLoops(2, LoopType.Yoyo);
                yield return new WaitForSeconds(timeBetweenAttacks);
            }

            endCallback?.Invoke();

        }

        #endregion

        #region ANIMATION

        public void StartInitAnimation()
        {
            transform.DOScale(0, startDuration).SetEase(ease).From();
        }

        #endregion

        #region DEBUG
        [NaughtyAttributes.Button]
        void SwitchInit()
        {
            SwitchState(BossAction.INIT);
        }

        [NaughtyAttributes.Button]
        void SwitchWalk()
        {
            SwitchState(BossAction.WALK);
        }

        [NaughtyAttributes.Button]
        void SwitchAttack()
        {
            SwitchState(BossAction.ATTACK);
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
