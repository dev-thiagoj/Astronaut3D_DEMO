using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animation;
using DG.Tweening;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private AnimationBase _animationBase;
    public Animator animator;
    public HealthBase healthBase;

    public CharacterController characterController;
    public float speed = 1f;
    public float turnSpeed = 1f;
    public float gravity = 9.8f;
    public float jumpSpeed = 15f;

    [Header("Run Setup")]
    public KeyCode keyRun = KeyCode.LeftShift;
    public float speedRun = 1.5f;

    private float vSpeed = 0f;

    [Header("Flash")]
    public List<FlashColor> flashColors;

    private void OnValidate()
    {
        _animationBase = GetComponent<AnimationBase>();
        animator = GetComponentInChildren<Animator>();
        healthBase = GetComponent<HealthBase>();
    }

    private void Update()
    {
        Movements();
        Jump();
    }

    #region RUN

    public void Movements()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);

        var inputAxisVertical = Input.GetAxis("Vertical");
        var speedVector = inputAxisVertical * speed * transform.forward;

        var isWalking = inputAxisVertical != 0;

        if (isWalking)
        {
            _animationBase.PlayAnimationByTrigger(AnimationType.RUN);

            if (Input.GetKey(keyRun))
            {
                speedVector *= speedRun;
                animator.speed = speedRun;
            }

            else animator.speed = 1;
        }
        else
            _animationBase.PlayAnimationByTrigger(AnimationType.IDLE);

        vSpeed -= gravity * Time.deltaTime;
        speedVector.y = vSpeed;

        characterController.Move(speedVector * Time.deltaTime);

        /*if(characterController.isGrounded && !isWalking)
        {
            _animationBase.PlayAnimationByTrigger(AnimationType.IDLE);
        }*/
    }

    #endregion

    #region JUMP

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            vSpeed = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                vSpeed = jumpSpeed;
                _animationBase.PlayAnimationByTrigger(AnimationType.JUMP);
            }
        }
    }

    #endregion

    #region DAMAGE
    public void Damage(float damage)
    {
        OnDamage(damage);
    }

    public void Damage(float damage, Vector3 dir)
    {
        OnDamage(damage);
        flashColors.ForEach(i => i.Flash());
        transform.DOMove(transform.position - dir, .1f);
    }

    public void OnDamage(float f)
    {
        //if (particleSystem != null) particleSystem.Emit(intParticles);

        healthBase._currLife -= f;

        if (healthBase._currLife <= 0)
        {
            Death();
        }
    }
    private void Death()
    {
        _animationBase.PlayAnimationByTrigger(AnimationType.DEATH);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            healthBase._currLife = 0;
            Death();
        }
    }

    #endregion
}

