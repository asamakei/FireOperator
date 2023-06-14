using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using KanKikuchi.AudioManager;

public class KeiMachine : MonoBehaviour{
    [SerializeField] Transform _body;
    [SerializeField] Transform _arm_LF;
    [SerializeField] Transform _arm_RF;
    [SerializeField] Transform _arm_LB;
    [SerializeField] Transform _arm_RB;
    [SerializeField] Transform _leg_LF;
    [SerializeField] Transform _leg_RF;
    [SerializeField] Transform _leg_LB;
    [SerializeField] Transform _leg_RB;
    [SerializeField] Transform _gun_arm_L;
    [SerializeField] Transform _gun_L;
    [SerializeField] Transform _gun_arm_R;
    [SerializeField] Transform _gun_R;
    [SerializeField] StraightFireShooter _shooter;
    [SerializeField] Transform _shooter_L;
    [SerializeField] Transform _shooter_R;
    [SerializeField] Transform _scope_L;
    [SerializeField] Transform _scope_R;
    [SerializeField] RectTransform _rect;
    [SerializeField] Transform _iceball;
    [SerializeField] Transform _fragments;
    [SerializeField] BoxCollider2D _goal;

    Rigidbody2D[] rigid2Ds;
    Transform[] transforms;
    Transform _trans;
    float right, left, top, bottom;
    bool isDeath = false;
    bool isCrack = false;

    const string STEP = "MachineStep";
    const string JUMP = "MachineJump";
    const string LAND = "MachineLanding";
    const string CRACK = "MachineCrack";
    const string BREAK = "MachineBreak";
    const string BARBEAM = "MachineBar";
    const string ARCBEAM = "MachineArc";
    const string BOOST = "MachineBoost";

    void Start(){
        _trans = transform;
        StartCoroutine(mainMove());
        right = _rect.position.x + _rect.rect.width / 2;
        left = _rect.position.x - _rect.rect.width / 2;
        top = _rect.position.y + _rect.rect.height / 2;
        bottom = _rect.position.y - _rect.rect.height / 2;
        rigid2Ds = _fragments.GetComponentsInChildren<Rigidbody2D>(true);
        transforms = _fragments.GetComponentsInChildren<Transform>(true);
    }
    private void FixedUpdate() {
        deathCheck();
    }
    private IEnumerator mainMove() {
        bool isRight;
        bool isMachineRight;
        int rand;
        yield return new WaitForSeconds(2);
        while (true) {
            isRight = isPlayerRight();
            isMachineRight = (right + left) / 2 <= _trans.position.x;
            if(isMachineRight == isRight) {
                if (Random.value < 0.5f) {
                    StartCoroutine(firebar(isRight, 5, 1, 0.5f, 0.04f, 0));
                }
                yield return step(!isRight, 0.3f, Random.Range(4, 8));
            }
            if (isDeath) break;
            rand = Random.Range(0, 4);
            if(rand == 0) {
                yield return step(Random.value<0.5f, 0.3f, Random.Range(1, 6));
            } else if(rand == 1) {
                if (Random.value < 0.5f) {
                    StartCoroutine(step(Random.value < 0.5f, 0.3f, 2));
                }
                yield return firebar(isRight, 5, 5, 0.5f, 0.04f, 0);
            } else if(rand == 2) {
                if (Random.value < 0.5f) {
                    StartCoroutine(step(Random.value < 0.5f, 0.3f, 2));
                }
                yield return firearc(isRight, 5, 5, 0.2f, -30, 30, 0.1f, 0);
            } else if(rand == 3) {
                yield return jump(6, 1, 2, 2, 0.1f);
            }
            if (isCrack) break;
        }
        isDeath = true;
        yield return death();
    }
    private void deathCheck() {
        if ((!isCrack) && (!isDeath) && _iceball.localScale.x < 0.6f) {
            isCrack = true;
            StartCoroutine(crack());
        }
    }
    private IEnumerator crack() {
        _fragments.localScale = _iceball.localScale;
        _iceball.gameObject.SetActive(false);
        _fragments.gameObject.SetActive(true);
        PlaySE(CRACK);
        yield return null;
    }
    private IEnumerator death() {
        yield return new WaitForSeconds(1);
        foreach(Rigidbody2D rb in rigid2Ds) {
            rb.isKinematic = false;
            rb.velocity = ((new Vector2(Random.Range(-1f,1f),Random.Range(0f,1f))).normalized * 3);
            rb.AddTorque(Random.Range(-1f,1f)*1000);
        }
        PlaySE(BREAK);
        yield return new WaitForSeconds(2);
        StartCoroutine(jump_leg(_arm_LF, _leg_LF, 0.3f, false));
        StartCoroutine(jump_leg(_arm_LB, _leg_LB, 0.3f, false));
        StartCoroutine(jump_leg(_arm_RF, _leg_RF, 0.3f, false));
        StartCoroutine(jump_leg(_arm_RB, _leg_RB, 0.3f, false));
        _trans.DOMoveY(-0.55f, 0.3f).SetRelative(true).SetEase(Ease.InQuad).SetLink(gameObject);
        yield return new WaitForSeconds(1);
        _goal.enabled = true;
        yield return null;
    }
    private bool isPlayerRight() {
        return _trans.position.x <= Player.Trans.position.x;
    }
    private IEnumerator firebar(bool isRight, int fireCount, int barCount,float speed, float fireSleep,float barSleep) {
        yield return lockOn(false, isRight, 0.03f, 0.04f);
        yield return new WaitForSeconds(0.2f);

        PlaySE(BARBEAM);
        for (int i = 0; i < fireCount; i++) {
            yield return shoot(isRight, 1/speed);
            yield return new WaitForSeconds(fireSleep);
        }

        yield return new WaitForSeconds(barSleep);

        for (int i = 0; i < barCount-1; i++) {
            yield return lockOn(false, isRight, 0f, 0.04f);
            yield return new WaitForSeconds(0.2f);
            PlaySE(BARBEAM);
            for (int j = 0; j < fireCount; j++) {
                yield return shoot(isRight, 1/speed);
                yield return new WaitForSeconds(fireSleep);
            }
            yield return new WaitForSeconds(barSleep);
        }
        yield return lockOn(true, isRight, 0.2f);
    }
    private IEnumerator jump(float height, float time, int hoverTime, int hoverCount, float fireSleep) {
        float destination;
        bool isRight = _trans.position.x >= (left + right) / 2;
        if (isRight) {
            destination = left + (right - left) / 4 * Random.value;
        } else {
            destination = right - (right - left) / 4 * Random.value;
        }
        yield return jump_begin(height,time);
        StartCoroutine(firebar(isPlayerRight(), 1, 10, 0.5f, 0, 0));
        _trans.DOBlendableLocalMoveBy(Vector3.right * (destination - _trans.position.x), hoverTime * hoverCount).SetEase(Ease.InOutQuad).SetLink(gameObject);
        yield return hover(1,hoverTime,hoverCount,1,fireSleep);
        yield return jump_end(height,time);
    }
    private IEnumerator jump_begin(float height, float time) {
        StartCoroutine(jump_leg(_arm_LF, _leg_LF, time * 0.3f, false));
        StartCoroutine(jump_leg(_arm_LB, _leg_LB, time * 0.3f, false));
        StartCoroutine(jump_leg(_arm_RF, _leg_RF, time * 0.3f, false));
        StartCoroutine(jump_leg(_arm_RB, _leg_RB, time * 0.3f, false));
        _trans.DOMoveY(-0.55f, time * 0.3f).SetRelative(true).SetEase(Ease.InQuad).SetLink(gameObject);
        yield return new WaitForSeconds(time * 0.5f);
        PlaySE(JUMP);
        StartCoroutine(jump_leg(_arm_LF, _leg_LF, time * 0.1f, true));
        StartCoroutine(jump_leg(_arm_LB, _leg_LB, time * 0.1f, true));
        StartCoroutine(jump_leg(_arm_RF, _leg_RF, time * 0.1f, true));
        StartCoroutine(jump_leg(_arm_RB, _leg_RB, time * 0.1f, true));
        _trans.DOMoveY(0.55f + height, time * 0.5f).SetRelative(true).SetEase(Ease.OutQuad).SetLink(gameObject);
        yield return new WaitForSeconds(time * 0.5f);


    }
    private IEnumerator hover(float shake, float time, int shakeCount, int fireSpeed, float fireSleep) {
        Sequence move = DOTween.Sequence();
        for(int i = 0; i < shakeCount; i++) {
            move.Append(_trans.DOBlendableLocalMoveBy(-2 * shake * Vector3.up, time * 0.5f).SetRelative(true).SetEase(Ease.InOutQuad))
                .Append(_trans.DOBlendableLocalMoveBy(2 * shake * Vector3.up, time * 0.5f).SetRelative(true).SetEase(Ease.InOutQuad)).SetLink(gameObject);
        }
        float firetime = time * shakeCount;
        PlaySE(BOOST);
        PlaySE(BOOST,firetime*2/3);
        StartCoroutine(hover_fire(Vector3.up * 0.5f, firetime/3, 10, fireSleep,0));
        StartCoroutine(hover_fire(Vector3.up * 0.5f + Vector3.right * 1f, firetime/3, 10, fireSleep,0));
        StartCoroutine(hover_fire(Vector3.up * 0.5f - Vector3.right * 1f, firetime/3, 10, fireSleep,0));
        StartCoroutine(hover_fire(Vector3.up * 0.5f, firetime / 3, 10, fireSleep, firetime * 2 / 3));
        StartCoroutine(hover_fire(Vector3.up * 0.5f + Vector3.right * 1f, firetime / 3, 10, fireSleep, firetime * 2 / 3));
        StartCoroutine(hover_fire(Vector3.up * 0.5f - Vector3.right * 1f, firetime / 3, 10, fireSleep, firetime * 2 / 3));
        yield return new WaitForSeconds(move.Duration());
    }
    private IEnumerator jump_end(float height,float time) {
        StartCoroutine(jump_leg(_arm_LF, _leg_LF, time * 0.1f, false));
        StartCoroutine(jump_leg(_arm_LB, _leg_LB, time * 0.1f, false));
        StartCoroutine(jump_leg(_arm_RF, _leg_RF, time * 0.1f, false));
        StartCoroutine(jump_leg(_arm_RB, _leg_RB, time * 0.1f, false));
        _trans.DOMoveY(-0.55f - height, time * 0.5f).SetRelative(true).SetEase(Ease.InQuad).SetLink(gameObject);
        yield return new WaitForSeconds(time * 0.5f);
        PlaySE(LAND);
        yield return new WaitForSeconds(time * 0.2f);
        StartCoroutine(jump_leg(_arm_LF, _leg_LF, time * 0.3f, true));
        StartCoroutine(jump_leg(_arm_LB, _leg_LB, time * 0.3f, true));
        StartCoroutine(jump_leg(_arm_RF, _leg_RF, time * 0.3f, true));
        StartCoroutine(jump_leg(_arm_RB, _leg_RB, time * 0.3f, true));
        _trans.DOMoveY(0.55f, time * 0.3f).SetRelative(true).SetEase(Ease.OutQuad).SetLink(gameObject);
        yield return new WaitForSeconds(time * 0.3f);

    }
    private IEnumerator hover_fire(Vector3 from, float time, float speed, float sleep, float delay) {
        yield return new WaitForSeconds(delay);
        int count = (int)Mathf.Ceil(time/sleep);
        Vector3 anchor;
        for (int i = 0; i < count; i++) {
            anchor = _trans.position;
            _shooter.Shoot(anchor + from, anchor + from + Vector3.down * 8, 8/speed);
            yield return new WaitForSeconds(sleep);
        }
    }
    private IEnumerator jump_leg(Transform arm, Transform leg, float time,bool isReset) {
        Sequence move = DOTween.Sequence();
        if (isReset) {
            int dir = (int)Mathf.Sign(arm.localScale.x);
            move.Append(arm.DOLocalRotate(Vector3.forward * 0 * dir, time).SetEase(Ease.Linear))
                .Join(leg.DOLocalRotate(Vector3.forward * 0, time).SetEase(Ease.Linear)).SetLink(gameObject);
        } else {
            int dir = (int)Mathf.Sign(arm.localScale.x);
            move.Append(arm.DOLocalRotate(Vector3.forward * 10 * dir, time).SetEase(Ease.Linear))
                .Join(leg.DOLocalRotate(Vector3.forward * (-50), time).SetEase(Ease.Linear)).SetLink(gameObject);
        }
        yield return new WaitForSeconds(move.Duration());
    }
    private IEnumerator firearc(bool isRight, int fireCount,int arcCount,float speed, float begin, float end, float fireSleep, float barSleep) {
        float degree = begin;
        yield return lockOn(false, isRight, 0.03f, 0.04f, degree);
        yield return new WaitForSeconds(0.2f);
        yield return lockOn(false, isRight, 0f, fireSleep, degree);
        for (int i = 0; i < arcCount; i++) {
            PlaySE(ARCBEAM);
            for (int j = 0; j < fireCount; j++) {
                degree = begin + (end - begin) * j / (fireCount-1);
                yield return lockOn(false, isRight, 0f, fireSleep, degree);
                yield return shoot(isRight, 1 / speed);
                yield return null;
            }
            yield return new WaitForSeconds(barSleep);
        }
        yield return lockOn(true, isRight, 0.2f);
    }
    private IEnumerator step(bool isRight, float time, int count) {
        (Transform, Transform) lf;
        (Transform, Transform) lb;
        (Transform, Transform) rf;
        (Transform, Transform) rb;
        int dir;

        int steppable;

        if (isRight) {
            steppable = (int)Mathf.Floor((right - _trans.position.x)/2);
            lf = (_arm_RF, _leg_RF);
            lb = (_arm_RB, _leg_RB);
            rf = (_arm_LF, _leg_LF);
            rb = (_arm_LB, _leg_LB);
            dir = -1;
        } else {
            steppable = (int)Mathf.Floor((_trans.position.x - left)/2);
            lf = (_arm_LF, _leg_LF);
            lb = (_arm_LB, _leg_LB);
            rf = (_arm_RF, _leg_RF);
            rb = (_arm_RB, _leg_RB);
            dir = 1;
        }

        count = Mathf.Min(count,steppable);
        if (count <= 0) {
            yield return null;
            yield break;
        }
        StartCoroutine(step_body(time * 0.5f, 0.1f));

        _trans.DOLocalMoveX(-1*dir, time * 0.5f).SetEase(Ease.Linear).SetRelative(true).SetLink(gameObject);
        StartCoroutine(step_forward(lf, time * 0.5f, true));
        StartCoroutine(step_back(lb, time * 0.5f, false));
        yield return new WaitForSeconds(time * 0.15f);
        PlaySE(STEP);
        StartCoroutine(step_forward(rf, time * 0.5f, false));
        StartCoroutine(step_back(rb, time * 0.5f, true));
        yield return new WaitForSeconds(time * 0.35f);

        for (int i = 0; i < count - 1; i++) {
            StartCoroutine(step_body(time * 0.5f, 0.1f));
            _trans.DOLocalMoveX(-1*dir, time * 0.5f).SetEase(Ease.Linear).SetRelative(true).SetLink(gameObject);
            StartCoroutine(step_forward(lb, time * 0.5f, true));
            StartCoroutine(step_back(lf, time * 0.5f, false));
            yield return new WaitForSeconds(time * 0.15f);
            PlaySE(STEP);
            StartCoroutine(step_forward(rb, time * 0.5f, false));
            StartCoroutine(step_back(rf, time * 0.5f, true));
            yield return new WaitForSeconds(time * 0.35f);
            StartCoroutine(step_body(time * 0.5f, 0.1f));
            _trans.DOLocalMoveX(-1*dir, time * 0.5f).SetEase(Ease.Linear).SetRelative(true).SetLink(gameObject);
            StartCoroutine(step_forward(lf, time * 0.5f, true));
            StartCoroutine(step_back(lb, time * 0.5f, false));
            yield return new WaitForSeconds(time * 0.15f);
            PlaySE(STEP);
            StartCoroutine(step_forward(rf, time * 0.5f, false));
            StartCoroutine(step_back(rb, time * 0.5f, true));
            yield return new WaitForSeconds(time * 0.35f);
        }
        StartCoroutine(step_body(time * 0.5f, 0.1f));
        _trans.DOLocalMoveX(-1*dir, time * 0.5f).SetEase(Ease.Linear).SetRelative(true).SetLink(gameObject);
        StartCoroutine(step_default(lb, time * 0.5f, true));
        StartCoroutine(step_default(lf, time * 0.5f, false));
        yield return new WaitForSeconds(time * 0.15f);
        PlaySE(STEP);
        StartCoroutine(step_default(rf, time * 0.5f, true));
        StartCoroutine(step_default(rb, time * 0.5f, false));
        yield return new WaitForSeconds(time * 0.35f);
    }

    private IEnumerator step_body(float time, float length) {
        Sequence move = DOTween.Sequence();
        move.Append(_body.DOMoveY(length, time/2).SetRelative(true).SetEase(Ease.InQuad))
            .Append(_body.DOMoveY(-length, time/2).SetRelative(true).SetEase(Ease.OutQuad)).SetLink(gameObject);
        yield return new WaitForSeconds(move.Duration());
    }
    private IEnumerator step_move((Transform, Transform) parts, float time, bool isUp,Vector2 rotate) {
        Transform arm = parts.Item1;
        Transform leg = parts.Item2;
        Sequence move = DOTween.Sequence();
        int dir = (int)Mathf.Sign(arm.localScale.x);

        if (isUp) {
            time /= 2;
            move.Append(arm.DOLocalRotate(Vector3.forward * (-15) * dir, time).SetEase(Ease.Linear))
                .Join(leg.DOLocalRotate(Vector3.forward * 15, time).SetEase(Ease.Linear)).SetLink(gameObject);
        }
        move.Append(arm.DOLocalRotate(Vector3.forward * rotate.x * dir, time).SetEase(Ease.Linear))
            .Join(leg.DOLocalRotate(Vector3.forward * rotate.y, time).SetEase(Ease.Linear)).SetLink(gameObject);

        yield return new WaitForSeconds(move.Duration());

    }
    private IEnumerator step_forward((Transform,Transform) parts, float time, bool isUp) {
        yield return step_move(parts,time,isUp,new Vector2(14,-37));
    }
    private IEnumerator step_default((Transform, Transform) parts, float time, bool isUp) {
        yield return step_move(parts, time, isUp, new Vector2(0, 0));
    }
    private IEnumerator step_back((Transform,Transform) parts, float time, bool isUp) {
        yield return step_move(parts, time, isUp, new Vector2(0, 27));
    }
    private IEnumerator lockOn(bool isReset,bool isRight,float time1,float time2 = 0,float degree = 0) {
        Vector2 target = Player.Trans.position;
        if (isReset) {
            yield return lockOn_reset(_gun_arm_R, _gun_R, time1);
            yield return lockOn_reset(_gun_arm_L, _gun_L, time1);
        } else {
            if(isRight) {
                yield return lockOn_move(target, _gun_arm_R, _gun_R, time1, time2, degree);
            } else {
                yield return lockOn_move(target, _gun_arm_L, _gun_L, time1, time2, degree);
            }
        }
    }
    private IEnumerator lockOn_move(Vector2 target,Transform arm, Transform gun,float time1, float time2,float degree) {
        Tween move_arm;
        Tween move_gun;
        int dir = (int)Mathf.Sign(arm.localScale.x);

        move_arm = arm.DOLocalRotate(Vector3.forward * (-25) * dir, time1).SetEase(Ease.Linear).SetLink(gameObject);
        yield return new WaitForSeconds(move_arm.Duration());

        Vector2 v = target - (Vector2)gun.position;
        float theta = Mathf.Atan2(v.y,v.x * dir) /Mathf.PI * 180 - arm.eulerAngles.z * dir + 180 + degree;

        theta = ((theta + 180)%360+360)%360-180;
        move_gun = gun.DOLocalRotate(Vector3.forward * theta, time2).SetEase(Ease.Linear).SetLink(gameObject);
        yield return new WaitForSeconds(move_gun.Duration());
    }
    private IEnumerator lockOn_reset(Transform arm, Transform gun, float time) {
        Sequence move = DOTween.Sequence();
        int dir = (int)Mathf.Sign(arm.localScale.x);

        move.Append(arm.DOLocalRotate(Vector3.forward * 0 * dir, time).SetEase(Ease.Linear))
            .Join(gun.DOLocalRotate(Vector3.forward * 45, time).SetEase(Ease.Linear)).SetLink(gameObject);
        yield return new WaitForSeconds(move.Duration());
    }
    private IEnumerator shoot(bool isRight,float duration,float delay=0) {
        yield return new WaitForSeconds(delay);
        if (isRight) {
            _shooter.Shoot(_shooter_R.position,_scope_R.position,duration);
        } else {
            _shooter.Shoot(_shooter_L.position, _scope_L.position, duration);
        }
    }
    private void PlaySE(string path,float delay = 0) {
        StartCoroutine(play(path,delay));
    }
    private IEnumerator play(string path, float delay = 0) {
        yield return new WaitForSeconds(delay);
        AudioData data = AudioDBManager.Instance.audioDataDBSO.GetAudioData(path);
        if (data != null) SEManager.Instance.Play(data.audioClip, data.volume);
    }
}
