using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using IceMilkTea.Core;
using System.Linq;
public class BC_AI : MonoBehaviour
{
    public static float JUDGEINTERVAL = 3;
    private enum BCState
    {
        Idle,
        Move,
        IceBorn,
        SuperFire
    }
    private ImtStateMachine<BC_AI> stateMachine;

    private void Awake()
    {
        // ステートマシンの遷移テーブルを構築
        stateMachine = new ImtStateMachine<BC_AI>(this);
        stateMachine.AddTransition<IdleState, MoveState>((int)BCState.Move);
        stateMachine.AddTransition<MoveState, IdleState>((int)BCState.Idle);
        stateMachine.AddTransition<IdleState, IceBornState>((int)BCState.IceBorn);
        stateMachine.AddTransition<IceBornState, IdleState>((int)BCState.Idle);
        stateMachine.AddTransition<IdleState, SuperFireState>((int)BCState.SuperFire);
        stateMachine.AddTransition<SuperFireState, IdleState>((int)BCState.Idle);
        // 開始ステートの設定
        stateMachine.SetStartState<MoveState>();
        // 即ステートを実行したい場合はUpdateを呼ぶ

    }
    private void Update()
    {
        stateMachine.Update();
    }
    private class IdleState : ImtStateMachine<BC_AI>.State
    {
        BC_AIData aiData;
        Vector3 defaultScale;
        protected internal override void Enter()
        {
            defaultScale = Context.transform.localScale;
            if (Context.gameObject.TryGetComponent<BC_AIDataManager>(out BC_AIDataManager bc_AIDataManager))
            {
                aiData = bc_AIDataManager.aiData;
                if (aiData.isShake) { Context.gameObject.transform.DOPunchScale(aiData.hitScaleAmount, 0.1f).SetRelative().SetLoops(-1).SetLink(Context.gameObject).OnComplete(() => Context.transform.localScale = defaultScale); }
                else { Context.gameObject.transform.DOMoveY(1, JUDGEINTERVAL / 10).SetRelative().SetEase(Ease.InOutSine).SetLink(Context.gameObject).SetLoops(-1, LoopType.Yoyo); }
                aiData.spriteRenderer.flipX = PlayerInstance.Instance != null && PlayerInstance.Instance.GetPlayer().transform.position.x > Context.gameObject.transform.position.x;
            }
        }
        float time;
        readonly int MOVE = 60;
        readonly int FIRE = 80;
        // 状態の更新はこのUpdateで行う
        protected internal override void Update()
        {
            if (aiData == null) return;
            time += Time.deltaTime;
            if (time <= JUDGEINTERVAL) return;
            time = 0;
            Context.gameObject.transform.DOKill();
            if (!aiData.isIceBorn)
            {
                stateMachine.SendEvent((int)BCState.Move);
                return;
            }

            float random = Random.Range(0, 100);
            if (random < MOVE)
            {
                stateMachine.SendEvent((int)BCState.Move);
            }
            else if (random >= MOVE && random < FIRE)
            {
                stateMachine.SendEvent((int)BCState.SuperFire);
            }
            else
            {
                stateMachine.SendEvent((int)BCState.IceBorn);
            }

        }

        // 状態から脱出する時の処理はこのExitで行う
        protected internal override void Exit()
        {
            aiData = null;
        }
    }

    private class MoveState : ImtStateMachine<BC_AI>.State
    {
        BC_AIData aiData;
        Vector3[] paths;
        protected internal override void Enter()
        {
            if (Context.gameObject.TryGetComponent<BC_AIDataManager>(out BC_AIDataManager bc_AIDataManager))
            {

                aiData = bc_AIDataManager.aiData;
                Ease randomEase = aiData.randomEase[Random.Range(0, aiData.randomEase.Length - 1)];


                paths = aiData.movePath.Select(_t => _t.position).ToArray();
                for (int i = 0; i < paths.Length; i++)
                {
                    Vector3 temp = paths[i];
                    int randomIndex = Random.Range(0, paths.Length - 1);
                    //（説明３）現在の要素に上書き
                    paths[i] = paths[randomIndex];
                    //（説明４）入れ替え元に預けておいた要素を与える
                    paths[randomIndex] = temp;
                }
                Transform playerTransform = null;
                if (PlayerInstance.Instance != null) playerTransform = PlayerInstance.Instance.GetPlayer().transform;

                int index = Random.Range(0, aiData.movePath.Length - 1);
                int index2 = Random.Range(0, aiData.moveDuration.Length - 1);
                Context.gameObject.transform.DOPath(paths, aiData.moveDuration[index2], PathType.CatmullRom).SetEase(randomEase).SetLink(Context.gameObject).OnComplete(() => stateMachine.SendEvent((int)BCState.Idle)).OnUpdate(() =>
                {
                    if (playerTransform != null && aiData.spriteRenderer != null) aiData.spriteRenderer.flipX = playerTransform.position.x > Context.gameObject.transform.position.x;
                });
            }
        }

    }
    private class IceBornState : ImtStateMachine<BC_AI>.State
    {
        int num;
        static string NAGA = "Naga";
        readonly float INTERVAL = 0.1f;
        BC_AIData aiData;
        Transform _transform;

        protected internal override void Enter()
        {
            if (Context.gameObject.TryGetComponent<BC_AIDataManager>(out BC_AIDataManager bc_AIDataManager))
            {
                aiData = bc_AIDataManager.aiData;
                num = 15;
                _transform = Context.transform;
                AudioData tmp = AudioDataManager.Instance.GetAudioData(NAGA);
                if (tmp != null) KanKikuchi.AudioManager.SEManager.Instance.Play(tmp.audioClip, tmp.volume);
            }
        }
        float time = 0;
        protected internal override void Update()
        {
            time += Time.deltaTime;
            if (time < INTERVAL) return;
            time = 0;
            aiData.iceSpawner.Spawn(_transform.position);
            num--;
            if (num <= 0) stateMachine.SendEvent((int)BCState.Idle);
        }
    }
    private class SuperFireState : ImtStateMachine<BC_AI>.State
    {
        BC_AIData aiData;
        float defaultShootSpeed;
        float defaultShootInterval;
        readonly float INTERVAL = 4f;
        protected internal override void Enter()
        {
            if (Context.gameObject.TryGetComponent<BC_AIDataManager>(out BC_AIDataManager bc_AIDataManager))
            {
                aiData = bc_AIDataManager.aiData;
                defaultShootSpeed = aiData.toPlayerShoot.ShootSpeed;
                defaultShootInterval = aiData.toPlayerShoot.ShootInterval;
                aiData.toPlayerShoot.ShootInterval = aiData.superShootInterval;
                aiData.toPlayerShoot.ShootSpeed = aiData.superShootSpeed;
            }
        }
        float time = 0;
        protected internal override void Update()
        {
            time += Time.deltaTime;
            if (time < INTERVAL) return;
            time = 0;
            stateMachine.SendEvent((int)BCState.Idle);
        }
        protected internal override void Exit()
        {
            aiData.toPlayerShoot.ShootInterval = defaultShootInterval;
            aiData.toPlayerShoot.ShootSpeed = defaultShootSpeed;
        }
    }
}
