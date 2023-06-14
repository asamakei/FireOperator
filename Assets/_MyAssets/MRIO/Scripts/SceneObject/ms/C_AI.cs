using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using IceMilkTea.Core;
using System.Linq;
using KanKikuchi.AudioManager;
using System;

public class C_AI : MonoBehaviour
{
    public static float JUDGEINTERVAL = 3;
    private enum CState
    {
        Idle,
        Move,
        DoubleSpeaker
    }
    private ImtStateMachine<C_AI> stateMachine;

    private void Awake()
    {
        // ステートマシンの遷移テーブルを構築
        stateMachine = new ImtStateMachine<C_AI>(this);
        stateMachine.AddTransition<IdleState, MoveState>((int)CState.Move);
        stateMachine.AddTransition<MoveState, IdleState>((int)CState.Idle);
        stateMachine.AddTransition<IdleState, DoubleSpeakerState>((int)CState.DoubleSpeaker);
        stateMachine.AddTransition<DoubleSpeakerState, IdleState>((int)CState.Idle);
        // 開始ステートの設定
        stateMachine.SetStartState<MoveState>();
        // 即ステートを実行したい場合はUpdateを呼ぶ

    }
    private void Update()
    {
        stateMachine.Update();
    }
    private class IdleState : ImtStateMachine<C_AI>.State
    {
        C_AIData aiData;
        protected internal override void Enter()
        {
            if (Context.gameObject.TryGetComponent<C_AIDataManager>(out C_AIDataManager c_AIDataManager))
            {
                aiData = c_AIDataManager.aiData;
                Context.gameObject.transform.DOMoveY(1, JUDGEINTERVAL / 10).SetRelative().SetEase(Ease.InOutSine).SetLink(Context.gameObject).SetLoops(-1, LoopType.Yoyo);
                aiData.spriteRenderer.flipX = PlayerInstance.Instance != null && PlayerInstance.Instance.GetPlayer().transform.position.x > Context.gameObject.transform.position.x;
            }
        }
        float time;
        readonly int MOVE = 70;
        // 状態の更新はこのUpdateで行う
        protected internal override void Update()
        {
            if (aiData == null) return;
            time += Time.deltaTime;
            if (time <= JUDGEINTERVAL) return;
            time = 0;
            Context.gameObject.transform.DOKill();

            float random = UnityEngine.Random.Range(0, 100);
            if (random < MOVE)
            {
                stateMachine.SendEvent((int)CState.Move);
            }
            else if (random >= MOVE)
            {
                stateMachine.SendEvent((int)CState.DoubleSpeaker);
            }

        }

        // 状態から脱出する時の処理はこのExitで行う
        protected internal override void Exit()
        {
            aiData = null;
        }
    }

    private class MoveState : ImtStateMachine<C_AI>.State
    {
        C_AIData aiData;
        Vector3[] paths;
        protected internal override void Enter()
        {
            if (Context.gameObject.TryGetComponent<C_AIDataManager>(out C_AIDataManager c_AIDataManager))
            {

                aiData = c_AIDataManager.aiData;
                Ease randomEase = aiData.randomEase[UnityEngine.Random.Range(0, aiData.randomEase.Length - 1)];


                paths = aiData.movePath.Select(_t => _t.position).ToArray();
                for (int i = 0; i < paths.Length; i++)
                {
                    Vector3 temp = paths[i];
                    int randomIndex = UnityEngine.Random.Range(0, paths.Length - 1);
                    //（説明３）現在の要素に上書き
                    paths[i] = paths[randomIndex];
                    //（説明４）入れ替え元に預けておいた要素を与える
                    paths[randomIndex] = temp;
                }
                Transform playerTransform = null;
                if (PlayerInstance.Instance != null) playerTransform = PlayerInstance.Instance.GetPlayer().transform;

                int index = UnityEngine.Random.Range(0, aiData.movePath.Length - 1);
                int index2 = UnityEngine.Random.Range(0, aiData.moveDuration.Length - 1);
                Context.gameObject.transform.DOPath(paths, aiData.moveDuration[index2], PathType.CatmullRom).SetEase(randomEase).SetLink(Context.gameObject).OnComplete(() => stateMachine.SendEvent((int)CState.Idle)).OnUpdate(() =>
                {
                    if (playerTransform != null && aiData.spriteRenderer != null) aiData.spriteRenderer.flipX = playerTransform.position.x > Context.gameObject.transform.position.x;
                });
            }
        }

    }
    private class DoubleSpeakerState : ImtStateMachine<C_AI>.State
    {
        static string KICK = "Kick";
        static string LIFE = "Life";
        readonly float INTERVAL = 0.4f;
        C_AIData aiData;
        Transform _transform;
        enum DoubleState
        {
            Moving,
            Arrived
        }
        DoubleState doubleState;
        AudioData kickData;
        Transform playerTransform;
        protected internal override void Enter()
        {
            if (Context.gameObject.TryGetComponent<C_AIDataManager>(out C_AIDataManager c_AIDataManager))
            {
                isStateTransitioning = false;
                durationTime = 0;
                aiData = c_AIDataManager.aiData;
                _transform = Context.transform;
                doubleState = DoubleState.Moving;
                Ease randomEase = aiData.randomEase[UnityEngine.Random.Range(0, aiData.randomEase.Length - 1)];
                playerTransform = PlayerInstance.Instance.transform;
                for (int i = 0; i < aiData.splitFaces.Length; i++)
                {
                    int index2 = UnityEngine.Random.Range(0, aiData.moveDuration.Length - 1);
                    aiData.splitFaces[i].gameObject.SetActive(true);
                    aiData.splitFaces[i]._Transform.position = Context.gameObject.transform.position;
                    aiData.splitFaces[i]._Transform.DOMove(aiData.splitMovedTransforms[Mathf.Min(i, aiData.splitMovedTransforms.Length - 1)].position, 2f).SetLink(aiData.splitFaces[i].gameObject).SetEase(randomEase).OnComplete(() =>
                    {
                        doubleState = DoubleState.Arrived;
                        Array.ForEach(aiData.splitFaces, splitFace => { if (playerTransform != null) splitFace.spriteRenderer.flipX = playerTransform.position.x > splitFace._Transform.position.x; });

                    }).SetLink(Context.gameObject);
                }
                kickData = AudioDataManager.Instance.GetAudioData(KICK);
                AudioData life = AudioDataManager.Instance.GetAudioData(LIFE);
                if (aiData.straightFires != null) Array.ForEach(aiData.straightFires, straightFire => straightFire.gameObject.SetActive(false));
                if (life != null) SEManager.Instance.Play(life.audioClip, life.volume);
            }
        }
        float time = 0;
        float durationTime = 0;
        bool isStateTransitioning = false;
        protected internal override void Update()
        {
            if (aiData == null || isStateTransitioning) return;
            if (doubleState != DoubleState.Arrived) return;
            time += Time.deltaTime;
            durationTime += Time.deltaTime;
            if (time < INTERVAL) return;
            time = 0;
            if (durationTime > aiData.splitDuration)
            {
                isStateTransitioning = true;
                for (int i = 0; i < aiData.splitFaces.Length; i++)
                {
                    Ease randomEase = aiData.randomEase[UnityEngine.Random.Range(0, aiData.randomEase.Length - 1)];
                    int index2 = UnityEngine.Random.Range(0, aiData.moveDuration.Length - 1);
                    aiData.splitFaces[i]._Transform.DOMove(Context.transform.position, 2f).SetLink(Context.gameObject).SetEase(randomEase).OnComplete(() =>
                    {
                        if (aiData.straightFires != null) Array.ForEach(aiData.straightFires, straightFire => straightFire.gameObject.SetActive(true));
                        Array.ForEach(aiData.splitFaces, splitFace => splitFace.gameObject.SetActive(false));
                        stateMachine.SendEvent((int)CState.Idle);
                        doubleState = DoubleState.Arrived;
                    }).SetLink(Context.gameObject);
                }
                return;
            }
            for (int l = 0; l < aiData.splitFaces.Length; l++)
            {
                if (kickData != null) SEManager.Instance.Play(kickData.audioClip, kickData.volume);
                aiData.splitFaces[l]._Transform.DOPunchScale(aiData.punchScaleAmount, 0.1f).SetLink(aiData.splitFaces[l].gameObject).SetRelative();
                for (int i = 0; i < aiData.splitFireNum; i++)
                {
                    float angle = Mathf.Lerp(aiData.startAngle, aiData.endAngle, (float)i / (float)aiData.splitFireNum);
                    Vector3 from = aiData.splitFaces[l]._Transform.position;
                    aiData.splitFaces[l].straightFireShooter.Shoot(aiData.splitFaces[l]._Transform.position, (playerTransform != null && aiData.splitFaces[l]._Transform.position.x < playerTransform.position.x) ? from + new Vector3((float)System.Math.Cos(angle), (float)System.Math.Sin(angle), 0) * aiData.distance : from - new Vector3((float)System.Math.Cos(angle), (float)System.Math.Sin(angle), 0) * aiData.distance, aiData.distance / aiData.fireSpeed);
                }
            }


        }
    }
}
