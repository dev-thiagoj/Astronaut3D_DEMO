using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animation;
using DG.Tweening;

public class Player : MonoBehaviour//, IDamageable
{
    //[SerializeField] private AnimationBase _animationBase;
    private Animator animator;
    public HealthBase healthBase;
    public List<Collider> colliders;

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

    private bool _alive = true;

    private void OnValidate()
    {
        //if (_animationBase == null) _animationBase = GetComponent<AnimationBase>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (healthBase == null) healthBase = GetComponent<HealthBase>();
    }

    private void Awake()
    {
        OnValidate(); //sempre chamar no awake para garantir que está sendo validado

        healthBase.OnDamage += Damage;
        healthBase.OnDamage += OnKill;
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
            if (Input.GetKey(keyRun))
            {
                speedVector *= speedRun;
                animator.speed = speedRun;
            }

            else animator.speed = 1;
        }

        vSpeed -= gravity * Time.deltaTime;
        speedVector.y = vSpeed;

        characterController.Move(speedVector * Time.deltaTime);

        animator.SetBool("RunBool", isWalking);
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
                animator.SetBool("JumpBool", !characterController.isGrounded);
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
    private void OnKill(HealthBase h)
    {
        if (_alive) //serve para animação tocar apenas uma vez
        {
            _alive = false;
            animator.SetTrigger("Death");
            colliders.ForEach(i => i.enabled = false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            healthBase._currLife = 0;
        }
    }

    #endregion
}

