using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animation;
using DG.Tweening;
using Ebac.Core.Singleton;
using Cloth;

//COISAS PARA FAZER:

//fazer anima��o do cannon a cada disparo
//arrumar UI para que a de health n�o atualize com os tiros
//arrumar anima��o de aterrisagem
//implementar VFX e SFX
//melhorar UI design

public class Player : Singleton<Player>
{
    public CharacterController characterController;
    public float speed = 1f;
    public float turnSpeed = 1f;
    public float gravity = 9.8f;
    public float jumpSpeed = 15f;
    public Transform initialPos;

    /*[Header("SFX")]
    public SFXType sfxType;
    private SFXPlayer sfxPlayer;*/

    [Header("Run Setup")]
    public KeyCode keyRun = KeyCode.LeftShift;
    public float speedRun = 1.5f;

    private float _vSpeed = 0f;

    [Header("Flash")]
    public List<FlashColor> flashColors;

    [Header("Life")]
    public HealthBase healthBase;
    public List<Collider> colliders; //character controller tb � interpretado como um collider
    public bool isAlive = true;

    [Header("Bound")]
    public int boundY = 25;

    [Space]
    [SerializeField] private ClothChange clothChange;

    private Animator _animator;
    private bool _jumping = false;


    private void OnValidate()
    {
        if (_animator == null) _animator = GetComponentInChildren<Animator>();
        if (healthBase == null) healthBase = GetComponent<HealthBase>();
        //if (sfxPlayer == null) sfxPlayer = GetComponent<SFXPlayer>();
    }

    protected override void Awake()
    {
        base.Awake();

        OnValidate(); //sempre chamar no awake para garantir que est� sendo validado

        healthBase.OnDamage += Damage;
        healthBase.OnKill += Kill;
    }

    private void Start()
    {   
        Spawn();
    }

    private void Update()
    {
        Movements();
        Jump();
        BoundY();
    }

    private void LoadLifeFromSave()
    {
        healthBase._currLife = SaveManager.Instance.Setup.lifeStatus;
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

            _animator.SetBool("Run", isWalking);
        }
    }

    #endregion

    #region JUMP

    public void Jump()
    {
        if (_jumping)
        {
            _jumping = false;
            _animator.SetTrigger("Land");
        }

        if (characterController.isGrounded && isAlive)
        {
            _vSpeed = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _vSpeed = jumpSpeed;

                if (!_jumping)
                {
                    _jumping = true;
                    _animator.SetTrigger("Jump");
                    transform.DOScaleX(.8f, .5f).SetEase(Ease.OutBack).SetLoops(2, LoopType.Yoyo);
                    transform.DOScaleZ(.8f, .5f).SetEase(Ease.OutBack).SetLoops(2, LoopType.Yoyo);
                    transform.DOScaleY(1.2f, .5f).SetEase(Ease.OutBack).SetLoops(2, LoopType.Yoyo);
                }

            }
        }
    }



    #endregion

    #region LIFE
    public void Damage(HealthBase h)
    {
        flashColors.ForEach(i => i.Flash());
        EffectsManager.Instance.ChangeVignette();
        ShakeCamera.Instance.Shake();

    }

    public void Damage(float damage, Vector3 dir)
    {
        
    }

    private void Kill(HealthBase h)
    {
        if (isAlive) //serve para anima��o tocar apenas uma vez
        {
            SaveManager.Instance.Save();
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

        if (collision.gameObject.CompareTag("Ground"))
        {
            transform.DOScaleX(1.1f, .5f).SetEase(Ease.OutBack).SetLoops(2, LoopType.Yoyo);
            transform.DOScaleZ(1.1f, .5f).SetEase(Ease.OutBack).SetLoops(2, LoopType.Yoyo);
            transform.DOScaleY(0.8f, .5f).SetEase(Ease.OutBack).SetLoops(2, LoopType.Yoyo);
        }
    }

    private void Revive()
    {
        isAlive = true;
        healthBase.ResetLife();
        _animator.SetTrigger("Revive");
        Respawn();
        Invoke(nameof(TurnOnColliders), .1f); //ATEN��O: IMPORTANTE:
                                              //precisa invocar para dar tempo de primeiro respawnar e depois ligar os colliders pois se o charactercontroller ficar ativo
                                              //no mesmo frame do respawn, ele pega a posi��o da morte como refer�ncia ainda e n�o a do checkpoint.  
    }

    private void TurnOnColliders()
    {
        colliders.ForEach(i => i.enabled = true);
    }

    private void BoundY()
    {
        if (transform.position.y < -boundY)
        {
            Kill(healthBase);
            Debug.Log("Kill");
        }   
    }

    #endregion

    #region SPAWN / RESPAWN

    public void Spawn()
    {
        if (CheckpointManager.Instance.lastCheckpointKey > 0)
        {
            transform.position = CheckpointManager.Instance.GetPositionFromLastCheckpoint();
            //healthBase._currLife = SaveManager.Instance.Setup.lifeStatus;
        }
        else transform.position = initialPos.transform.position;
    }

    [NaughtyAttributes.Button]
    public void Respawn()
    {
        if (CheckpointManager.Instance.HasCheckpoint())
        {
            transform.position = CheckpointManager.Instance.GetPositionFromLastCheckpoint();
            LoadLifeFromSave();
        }
        else
        {
            transform.position = initialPos.transform.position;
            LoadLifeFromSave();
        } 
    }

    #endregion

    #region POWERUPS

    public void ChangeSpeed(float speed, float duration)
    {
        StartCoroutine(ChangeSpeedCoroutine(speed, duration));
    }

    IEnumerator ChangeSpeedCoroutine(float localSpeed, float duration)
    {
        var defaultSpeed = speed;
        speed = localSpeed;
        yield return new WaitForSeconds(duration);
        speed = defaultSpeed;
    }

    public void ChangeTexture(ClothSetup setup, float duration)
    {
        StartCoroutine(ChangeTextureCoroutine(setup, duration));
    }

    IEnumerator ChangeTextureCoroutine(ClothSetup setup, float duration)
    {
        clothChange.ChangeTexture(setup);
        yield return new WaitForSeconds(duration);

        clothChange.ResetTexture();
    }

    #endregion
}

