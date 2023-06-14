using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using IceMilkTea.Core;
public class A_AI : MonoBehaviour
{
    public static float JUDGEINTERVAL = 5;
    public static Vector3 eyeDefaultPos;
    private enum AState
    {
        Idle,
        Move,
        WholeEyeMove,
        EyeMove,
        Laser
    }
    private ImtStateMachine<A_AI> stateMachine;
    private void Awake()
    {
        if (TryGetComponent<A_AIDataManager>(out A_AIDataManager a_AIDataManager))
        {
            eyeDefaultPos = a_AIDataManager.aiData.wholeEye.transform.localPosition;
        }
        // ステートマシンの遷移テーブルを構築
        stateMachine = new ImtStateMachine<A_AI>(this);
        stateMachine.AddTransition<IdleState, MoveState>((int)AState.Move);
        stateMachine.AddTransition<IdleState, WholeEyeMoveState>((int)AState.WholeEyeMove);
        stateMachine.AddTransition<WholeEyeMoveState, EyeMoveState>((int)AState.EyeMove);
        stateMachine.AddTransition<IdleState, LaserState>((int)AState.Laser);
        stateMachine.AddTransition<LaserState, IdleState>((int)AState.Idle);
        stateMachine.AddTransition<EyeMoveState, IdleState>((int)AState.Idle);
        stateMachine.AddTransition<WholeEyeMoveState, IdleState>((int)AState.Idle);
        stateMachine.AddTransition<MoveState, IdleState>((int)AState.Idle);
        // 開始ステートの設定
        stateMachine.SetStartState<IdleState>();
        // 即ステートを実行したい場合はUpdateを呼ぶ

    }
    private void Update()
    {
        stateMachine.Update();
    }
    private class IdleState : ImtStateMachine<A_AI>.State
    {
        A_AIData aiData;
        protected internal override void Enter()
        {
            if (Context.gameObject.TryGetComponent<A_AIDataManager>(out A_AIDataManager a_AIDataManager))
            {
                aiData = a_AIDataManager.aiData;
                if (PlayerInstance.Instance == null) return;
                Player player = PlayerInstance.Instance.GetPlayer();
                Context.transform.DOPunchScale(aiData.punchScaleAmount, 0.4f).SetLink(Context.gameObject).SetRelative();
                aiData.face.ChangeFlip(player != null && player.transform.position.x > Context.gameObject.transform.position.x);
            }
        }
        float time;
        readonly int LASER = 5;
        readonly int MOVE = 75;
        readonly int WEMOVE = 85;
        readonly int EMOVE = 100;
        // 状態の更新はこのUpdateで行う
        protected internal override void Update()
        {
            if (aiData == null) return;
            time += Time.deltaTime;
            if (time <= JUDGEINTERVAL) return;
            time = 0;
            float random = Random.Range(0, 100);
            if (random < LASER)
            {
                stateMachine.SendEvent((int)AState.Laser);
            }
            else if (random >= LASER && random < MOVE)
            {
                stateMachine.SendEvent((int)AState.Move);
            }
            else if (random >= MOVE && random < WEMOVE)
            {
                stateMachine.SendEvent((int)AState.WholeEyeMove);
            }
            else if (random >= WEMOVE && random <= EMOVE)
            {
                stateMachine.SendEvent((int)AState.EyeMove);
            }
        }

        // 状態から脱出する時の処理はこのExitで行う
        protected internal override void Exit()
        {
            aiData = null;
        }
    }
    private class MoveState : ImtStateMachine<A_AI>.State
    {
        A_AIData aiData;
        protected internal override void Enter()
        {
            if (Context.gameObject.TryGetComponent<A_AIDataManager>(out A_AIDataManager a_AIDataManager))
            {
                aiData = a_AIDataManager.aiData;

                int index = Random.Range(0, aiData.movePath.Length - 1);
                int index2 = Random.Range(0, aiData.moveDuration.Length - 1);
                Context.gameObject.transform.DOMove(aiData.movePath[index].position, aiData.moveDuration[index2]).SetEase(Ease.Linear).SetLink(Context.gameObject).OnComplete(() => stateMachine.SendEvent((int)AState.Idle));
            }
        }

    }

    private class WholeEyeMoveState : ImtStateMachine<A_AI>.State
    {
        A_AIData aiData;
        protected internal override void Enter()
        {
            if (Context.gameObject.TryGetComponent<A_AIDataManager>(out A_AIDataManager a_AIDataManager))
            {
                aiData = a_AIDataManager.aiData;
                Player player = PlayerInstance.Instance.GetPlayer();
                aiData.wholeEye.DOKill();
                Vector3 moveAmount = (player == null) ? new Vector3(Random.Range(0, aiData.eyeMoveAmount), Random.Range(0, aiData.eyeMoveAmount), Random.Range(0, aiData.eyeMoveAmount)) : (player.transform.position - aiData.wholeEye.transform.position).normalized * aiData.eyeMoveAmount;
                DOTween.Sequence().Append(aiData.wholeEye.transform.DOLocalMove(moveAmount, aiData.eyeMoveDuration).SetRelative().SetEase(Ease.OutQuad))
                                  .AppendInterval(1f)
                                  .Append(aiData.wholeEye.transform.DOLocalMove(-moveAmount, aiData.eyeMoveDuration / 2).SetRelative().SetEase(Ease.OutQuad))
                                    .SetLink(Context.gameObject).OnComplete(() =>
                                    {
                                        int random = Random.Range(0, 1);
                                        if (random == 1) aiData.wholeEye.transform.DOLocalMove(eyeDefaultPos, 0.5f).SetLink(Context.gameObject);
                                        stateMachine.SendEvent((int)AState.Idle);
                                    });
            }
        }
    }
    private class EyeMoveState : ImtStateMachine<A_AI>.State
    {
        A_AIData aiData;
        protected internal override void Enter()
        {
            if (Context.gameObject.TryGetComponent<A_AIDataManager>(out A_AIDataManager a_AIDataManager))
            {
                aiData = a_AIDataManager.aiData;
                aiData.wholeEye.Eye.transform.DOMoveX(1f, 1).SetLoops(1, LoopType.Yoyo).SetRelative().SetEase(Ease.OutQuad).OnComplete(() => stateMachine.SendEvent((int)AState.Idle));
            }
        }
    }

    private class LaserState : ImtStateMachine<A_AI>.State
    {
    }

}
