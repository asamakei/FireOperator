using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム内で使う変数
/// UIに表示するときはUniRxで値を監視するのがおすすめ
/// ・Unityで学ぶMVPパターン ~ UniRxを使って体力Barを作成する ~
/// https://qiita.com/Nakashima_Hibari/items/5e0c36c3b0df78110d32
/// </summary>
public enum GameState
{
    Game = 0,
    Tutorial = 1
}
public enum FailState
{
    Melt = 0,
    Fall = 1
}
public static class Variables
{
    public static int currentStageIndex = 0;
    public static GameState gameState = GameState.Game;
    public static FailState failState = FailState.Melt;
}
