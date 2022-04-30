using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animation;
using DG.Tweening;

//COISAS PARA FAZER:

//usar o _alive como check ao inves do healthbase._currlife para definir qdo os inimigos atiram/perseguem 
//fazer animação do cannon a cada disparo
//arrumar UI para que a de health não atualize com os tiros
//arrumar animação de pulo e sua física
//implementar VFX e SFX
//melhorar UI design

public class Player : MonoBehaviour//, IDamageable
{
    public CharacterController characterController;
    public float speed = 1f;
    public float turnSpeed = 1f;
    public float gravity = 9.8f;
    public float jumpSpeed = 15f;

    [Header("Run Setup")]
    public KeyCode keyRun = KeyCode.LeftShift;
    public float speedRun = 1.5f;

    private float _vSpeed = 0f;

    [Header("Flash")]
    public List<FlashColor> flashColors;

    [Header("Life")]
    public HealthBase healthBase;
    public List<Collider> colliders; //character controller tb é interpretado como um collider
    public bool isAlive = true;

    private Animator _animator;


    private void OnValidate()
    {
        //if (_animationBase == null) _animationBase = GetComponent<AnimationBase>();
        if (_animator == null) _animator = GetComponentInChildren<Animator>();
        if (healthBase == null) healthBase = GetComponent<HealthBase>();
    }

    private void Awake()
    {
        OnValidate(); //sempre chamar no awake para garantir que está sendo validado

        healthBase.OnDamage += Damage;
        healthBase.OnKill += Kill;
    }

    private void Update()
    {
        Movements();
        Jump();
    }

    #region RUN

    public void Movements()
    {
        if (isAlive)
        {
            transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);

            var inputAxisVertical = Input.GetAxis("Vertical");
            var speedVector = inputAxisVertical * speed * transform.forward;

            var isWalking = inputAxisVertical != 0;

            if (isWalking)
            {
                if (Input.GetKey(keyRun))
                {
                    speedVector *= speedRun;
                    _animator.speed = speedRun;
                }

                else _animator.speed = 1;
            }

            _vSpeed -= gravity * Time.deltaTime;
            speedVector.y = _vSpeed;

            characterController.Move(speedVector * Time.deltaTime);

            _animator.SetBool("RunBool", isWalking);

        }
    }

    #endregion

    #region JUMP

    public void Jump()
    {
        if (characterController.isGrounded && isAlive)
        {
            _vSpeed = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _vSpeed = jumpSpeed;
                _animator.SetBool("JumpBool", !characterController.isGrounded);
            }
        }
    }

    #endregion

    #region LIFE
    public void Damage(HealthBase h)
    {
        flashColors.ForEach(i => i.Flash());
    }

    public void Damage(float damage, Vector3 dir)
    {
        //OnDamage(damage);
        //transform.DOMove(transform.position - dir, .1f);
    }
    private void Kill(HealthBase h)
    {
        if (isAlive) //serve para animação tocar apenas uma vez
        {
            isAlive = false;
            _animator.SetTrigger("Death");
            colliders.ForEach(i => i.enabled = false);

            Invoke(nameof(Revive), 5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {   
            Kill(healthBase);
        }
    }

    private void Revive()
    {
        isAlive = true;
        healthBase.ResetLife();
        _animator.SetTrigger("Revive");
        Respawn();
        Invoke(nameof(TurnOnColliders), .1f); //ATENÇÃO: IMPORTANTE:
                                              //precisa invocar para dar tempo de primeiro respawnar e depois ligar os colliders pois se o charactercontroller ficar ativo
                                              //no mesmo frame do respawn, ele pega a posição da morte como referência ainda e não a do checkpoint.  
    }

    private void TurnOnColliders()
    {
        colliders.ForEach(i => i.enabled = true);
    }

    #endregion

    #region RESPAWN

    [NaughtyAttributes.Button]
    public void Respawn()
    {
        if (CheckpointManager.Instance.HasCheckpoint())
        {
            transform.position = CheckpointManager.Instance.GetPositionFromLastCheckpoint();
        }
    }

    #endregion
}

