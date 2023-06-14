using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using KanKikuchi.AudioManager;
public class Player : Touchable
{
    static string JUMP = "Jump";
    static string DIVE = "Dive";
    static string ICEMELT = "Melt";
    static float EFFECTINTERVAL = 0.1f;
    public static Transform Trans;
    static float EPSILON = 0.5f;

    [SerializeField] private PlayerSetting _playerSetting;
    [SerializeField] private float _meltingSpeed = 0.01f;
    [SerializeField] private ParticleSystem _particle;

    private bool _isLeftMove = false;
    private bool _isRightMove = false;
    private bool _isJumpButton = false;

    private bool _isLeftButtonDownUI = false;
    private bool _isRightButtonDownUI = false;
    private bool _isJumpButtonDownUI = false;

    private bool _isOnFloor = false;
    private bool _isInsideWater = false;
    private bool _isOnIce = false;
    //private bool _isMelting = false;
    private bool _isParticlePlaying;

    private bool _isGround = false;
    private bool _isJumpOn = false;
    private bool _isMovable = true;

    private int _countFloor = 0;
    private int _countWater = 0;
    private int _countIce = 0;

    private float _defaultmass;
    private float _defaultScale;

    private Transform _trans;
    private Rigidbody2D _rigid;
    private Animator _anim;
    private SpriteRenderer _render;

    private Rigidbody2D _parentRigid = null;

    private int _hashWalk = Animator.StringToHash("isWalk");
    private int _hashFall = Animator.StringToHash("isFall");
    private int _hashForcedFall = Animator.StringToHash("isForcedFall");
    private int _hashJump = Animator.StringToHash("Jump");
    AudioData meltData;
    AudioData diveData;
    AudioData jumpData;
    float time;
    private Vector2 _velocity = Vector2.zero;
    public void ChangeMovable(bool _isMovable)
    {
        this._isMovable = _isMovable;
        if (!_isMovable) _rigid.velocity = Vector3.zero;
    }
    private void Awake()
    {
        jumpData = AudioDataManager.Instance.GetAudioData(JUMP);
        diveData = AudioDataManager.Instance.GetAudioData(DIVE);
        meltData = AudioDataManager.Instance.GetAudioData(ICEMELT);
        Player.Trans = transform;
    }
    void Start()
    {
        _trans = transform;
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _render = GetComponent<SpriteRenderer>();
        _particle.Stop();
        _defaultmass = _rigid.mass;
        _defaultScale = _trans.localScale.x;

    }

    void Update()
    {
        SetKeyFlags();
    }

    void FixedUpdate()
    {

        MovePlayer();
        ThermalUpdate();
    }

    public void SetLeftButtonDown(bool isDown)
    {
        _isLeftButtonDownUI = isDown;
    }
    public void SetRightButtonDown(bool isDown)
    {
        _isRightButtonDownUI = isDown;
    }
    public void SetJumpButtonDown(bool isDown)
    {
        //if (jumpData != null && (_isGround | _isOnIce | _isOnFloor)) SEManager.Instance.Play(jumpData.audioClip, jumpData.volume); 
        _isJumpButtonDownUI = isDown;
    }

    private void SetKeyFlags()
    {
        if (StageManager.IsStageStart)
        {
            _isLeftMove = _isLeftButtonDownUI || Input.GetKey(KeyCode.LeftArrow);
            _isRightMove = _isRightButtonDownUI || Input.GetKey(KeyCode.RightArrow);
            _isJumpButton = _isJumpButtonDownUI || Input.GetKeyDown(KeyCode.UpArrow);
            _isJumpButtonDownUI = false;
            if (_isJumpButton && (_isGround || _isInsideWater))
            {
                _isJumpOn = true;
            }
        }
        else
        {
            _isLeftMove = false;
            _isRightMove = false;
            _isJumpButton = false;
            _isJumpOn = false;
        }

    }

    private void MovePlayer()
    {
        Vector2 velocity;
        var vx = 0f;
        var vy = 0f;
        var inertia = 0f;

        if (_parentRigid != null)
        {
            velocity = _rigid.velocity - _parentRigid.velocity;
        }
        else
        {
            velocity = _rigid.velocity;
        }

        if (_isGround)
        {
            if (_isOnIce)
            {
                inertia = _playerSetting.InertiaScaleIce;
            }
            else
            {
                inertia = _playerSetting.InertiaScaleGround;
            }

        }
        else
        {
            inertia = _playerSetting.InertiaScaleAir;
        }

        if (_isLeftMove)
        {
            vx = -_playerSetting.WalkSpeed;
        }
        if (_isRightMove)
        {
            vx = _playerSetting.WalkSpeed;
        }

        if (_isJumpOn)
        {
            _isJumpOn = false;
            vy = _playerSetting.JumpForce;
            _anim.SetTrigger(_hashJump);
        }
        else
        {
            vy = velocity.y - _playerSetting.GravityScale * Time.deltaTime;
        }

        vx = Mathf.Lerp(vx, velocity.x, inertia);
        vy = Mathf.Max(vy, -_playerSetting.TerminalVelocity);
        if (!_isMovable) vx = 0;

        if (_parentRigid != null)
        {
            _rigid.velocity = new Vector2(vx, vy) + _parentRigid.velocity;
        }
        else
        {
            _rigid.velocity = new Vector2(vx, vy);
        }

        bool isWalk = _isRightMove || _isLeftMove;
        bool isFall = (vy < 0) && (!_isGround);
        bool isForcedFall = vy < 0;
        _anim.SetBool(_hashWalk, isWalk);
        _anim.SetBool(_hashFall, isFall);
        _anim.SetBool(_hashForcedFall, isForcedFall);
        if (_isRightMove && (vx > 0))
        {
            _render.flipX = false;
        }
        else if (_isLeftMove && (vx < 0))
        {
            _render.flipX = true;
        }
    }

    protected override void ThermalEvent(float diff)
    {
        if (_thermalEnergy >= MaxEnergy)
        {
            _trans.localScale -= Vector3.one * _meltingSpeed * Time.deltaTime;
            _rigid.mass = _defaultmass * _trans.localScale.x / _defaultScale;
            if (_particle == null) return;
            _isParticlePlaying = _particle.isEmitting; 
            time += Time.deltaTime;
            if (time > EFFECTINTERVAL && _isTouchable && !_isParticlePlaying)
            {
                _particle.Play();
            }
            if (time > EFFECTINTERVAL && meltData != null)
            {
                time = 0;
                SEManager.Instance.Play(meltData.audioClip, meltData.volume);
            }
            
           
        }
        else if (_isParticlePlaying)
        {
            if (_particle == null) return;
            _particle.Stop();
        }
        if (_trans.localScale.x <= EPSILON)
        {
            gameObject.SetActive(false);
            if (!SceneTransManager.IsTransitioning) DOVirtual.DelayedCall(1f, () =>
              {
                  if (UIManager.uIState == UIState.Game)
                  {
                      UIManager.uIState = UIState.Fail;
                      Variables.failState = FailState.Melt;
                  }

              });

        }
    }

    /*protected override void TouchEnterEvent() {
        _isMelting = true;
    }
    protected override void TouchExitEvent() {
        _isMelting = false;
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (collision.isTrigger && !obj.CompareTag("Water")) return;
        if (obj.CompareTag("Water") && diveData != null && SEManager.Instance.GetCurrentAudioNames().Count < ((float)SEManager.Instance.AudioPlayerNum / 2.0f)) SEManager.Instance.Play(diveData.audioClip, diveData.volume); 
        JudgeOnGround(obj, 1);

        if (obj.CompareTag("Floor") || obj.CompareTag("Ice"))
        {
            if (_parentRigid == null)
            {
                obj.TryGetComponent(out _parentRigid);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (collision.isTrigger && !obj.CompareTag("Water")) return;
        JudgeOnGround(obj, -1);
        if (obj.CompareTag("Floor") || obj.CompareTag("Ice"))
        {
            _parentRigid = null;
        }
    }
    private void JudgeOnGround(GameObject obj, int diff)
    {
        CountTrigger(obj, "Floor", ref _countFloor, ref _isOnFloor, diff);
        CountTrigger(obj, "Ice", ref _countIce, ref _isOnIce, diff);
        CountTrigger(obj, "Water", ref _countWater, ref _isInsideWater, diff);
        _isGround = _isOnFloor || _isOnIce;

    }
    private void CountTrigger(GameObject obj, string tag, ref int counter, ref bool flag, int diff)
    {
        if (obj.CompareTag(tag))
        {
            counter += diff;
            flag = counter > 0;
        }
    }
}
