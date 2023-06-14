using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
public enum StageClearEventType
{
    General = 0,
    DirectLoad = 1,
    DirectLoad2 = 2,
    DirectLoad3 = 3,
    FinalLoad
}

[System.Serializable]
public enum StageLoadRequirement
{
    PreviousClear,
    VideoWatch,
    Keycode
}
/// <summary>
/// 何かステージデータに必要なものがあれば追加可能
/// </summary>
[System.Serializable]
public class StageData
{
    public SceneObject[] loadingScenes;
    public Vector3 defaultPlayerPosition;
    public int level = 1;
    public string bgmIdentifier;
    public string stageName;
    public bool shouldUseStageName = false;
    public string stageButtonNumber;
    public Sprite stagePicture;
    public StageLoadRequirement requirement;
    public StageClearEventType stageClearEventType = StageClearEventType.General;
    public StageVariableDataSO directLoadData;
    public Color backColor = Color.black;
    public Sprite backGroundPicture;
    public int rendererNumber = 0;
    [TextArea(0, 3)] public string stageText;
    public VideoClip tutorialClip;
    [TextArea(0, 3)] public string tutorialText;
    public VideoClip endClip;
    [TextArea(0, 3)] public string endText;

}
