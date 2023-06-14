//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Cysharp.Threading.Tasks;
//using System;
//using DG.Tweening;
//using UnityEngine.SceneManagement;
//using System.Linq;
//[RequireComponent(typeof(FadeImage))]
//public class SceneTransManager : SingletonMonoBehaviour<SceneTransManager>
//{
//    [SerializeField] Texture defaultFadeTexture;
//    [SerializeField] Ease easingType;
//    public float transitionDuration { private get; set; } = 1.5f;
//    private FadeImage fadeImage;
//    private Camera mainCameraBuf;
//    void Awake()
//    {
//        fadeImage = GetComponent<FadeImage>();
//    }

//    public void SceneTrans(string scenename, bool doesfadein = true, bool doesfadeout = true, Texture fadetexture = default)
//    {
//        string beforeSceneName = SceneManager.GetActiveScene().name;
//        SaveManager.Instance.StoreData(beforeSceneName);
//        if (fadeImage != null && fadetexture != default) fadeImage.UpdateMaskTexture(fadetexture);
//        if (SceneManager.GetSceneByName(scenename) != null) SceneTransStart(scenename,transitionDuration, doesfadein, doesfadeout);
//    }
//    public void SceneTrans(string scenename,float transDuration, bool doesfadein = true, bool doesfadeout = true, Texture fadetexture = default)
//    {
//        float buf = transitionDuration;
//        transitionDuration = transDuration;
//        SceneTrans(scenename, doesfadein, doesfadeout, fadetexture);
//        transitionDuration = buf;
//    }

//    private async void SceneTransStart(string scenename,float transDuration,bool doesfadein = true, bool doesfadeout = true)
//    {
//        if (doesfadein) await DOTween.To(() => fadeImage.Range, (range) => fadeImage.Range = range, 1, transDuration).SetEase(easingType);

//        await SceneManager.LoadSceneAsync(scenename);

//        GameObject[] objects = SceneManager.GetActiveScene().GetRootGameObjects();
//        GameObject sdmobj = (objects != null) ? Array.Find(objects, obj => obj.CompareTag(Strings.SCENEDATAMANAGER)) : null;
//        mainCameraBuf = (objects != null && objects.ToList().TryFind(obj => obj.CompareTag(Strings.MAINCAMERA), out GameObject cameraObj)) ? cameraObj.GetComponent<Camera>() : null;
//        if (sdmobj != null && sdmobj.TryGetComponent<SceneDataManager>(out SceneDataManager sceneDataManager))
//        {
//            sceneDataManager.Load(SaveManager.Instance.saveData);
//        }

//        if (doesfadeout) await DOTween.To(() => fadeImage.Range, (range) => fadeImage.Range = range, 0, transDuration).SetEase(easingType);
//    }

//    public Camera GetNowSceneMainCamera()
//    {
//        return mainCameraBuf;
//    }
//}
