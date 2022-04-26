using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animation;

public class Player : MonoBehaviour, IDamageable
{
    public Animator animator;

    public CharacterController characterController;
    public float speed = 1f;
    public float turnSpeed = 1f;
    public float gravity = 9.8f;
    public float jumpSpeed = 15f;

    //[Header("Animation")]
    //[SerializeField] private AnimationBase _animationBase;

    [Header("Run Setup")]
    public KeyCode keyRun = KeyCode.LeftShift;
    public float speedRun = 1.5f;

    private float vSpeed = 0f;

    [Header("Flash")]
    public List<FlashColor> flashColors;

    private void Update()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);

        var inputAxisVertical = Input.GetAxis("Vertical");
        var speedVector = inputAxisVertical * speed * transform.forward;

        if (characterController.isGrounded)
        {
            vSpeed = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                vSpeed = jumpSpeed;
                animator.SetBool("Jump", true);
            }

            Invoke("EndJumpAnimation", .1f); 

        }

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

        //if (inputAxisVertical != 0) animator.SetBool("Run", true);
        //else animator.SetBool("Run", false);
        //por ser um if/else bem simples, podemos fazer da seguinte forma com o mesmo resultado

        animator.SetBool("Run", isWalking);

    }

    public void EndJumpAnimation()
    {
        if (characterController.isGrounded)
            animator.SetBool("Jump", false);
            
    }

    private void Death()
    {
        animator.SetBool("Death", true);
    }

    /*public void PlayAnimationByTrigger(AnimationType animationType)
    {
        _animationBase.PlayAnimationByTrigger(animationType);
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Death();
        }
    }

    #region LIFE
    public void Damage(float damage)
    {
        flashColors.ForEach(i => i.Flash());
    }

    public void Damage(float damage, Vector3 dir)
    {
        Damage(damage);
    }
    #endregion
}
