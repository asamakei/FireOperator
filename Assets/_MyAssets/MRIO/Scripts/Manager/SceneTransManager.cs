using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Linq;
[RequireComponent(typeof(FadeImage))]
public class SceneTransManager : MonoBehaviour
{
    [SerializeField] Texture defaultFadeTexture;
    [SerializeField] Ease easingType;
    static SceneTransManager _i;
    public static SceneTransManager instance
    {
        get { return _i; }
    }
    public static bool IsTransitioning
    {
        get { return isTransitioning; }
    }
    public float transitionDuration { private get; set; } = 1f;
    private FadeImage fadeImage;
    static bool isTransitioning = false;
    Action onSceneLoadEnd;
    void Awake()
    {
        fadeImage = GetComponent<FadeImage>();
        if (_i == null) _i = this;
        else Destroy(gameObject);

    }

    public void SceneTrans(SceneObject[] sceneObjects, bool doesfadein = true, bool doesfadeout = true, Texture fadetexture = default)
    {
        string beforeSceneName = SceneManager.GetActiveScene().name;
        if (fadeImage != null && fadetexture != default) fadeImage.UpdateMaskTexture(fadetexture);

        SceneTransStart(sceneObjects, transitionDuration, doesfadein, doesfadeout);
    }

    private async void SceneTransStart(SceneObject[] sceneObjects, float transDuration, bool doesfadein = true, bool doesfadeout = true)
    {
        isTransitioning = true;
        if (doesfadein) await DOTween.To(() => fadeImage.Range, (range) => fadeImage.Range = range, 1, transDuration).SetEase(easingType).SetLink(gameObject).SetUpdate(UpdateType.Normal, true);
        await SceneManager.LoadSceneAsync(sceneObjects[0], LoadSceneMode.Single);
        for (int i = 1; i < sceneObjects.Length; i++)
        {
            await SceneManager.LoadSceneAsync(sceneObjects[i], LoadSceneMode.Additive);
        }


        GameObject[] objects = SceneManager.GetActiveScene().GetRootGameObjects();
        if (onSceneLoadEnd != null) onSceneLoadEnd();
        if (doesfadeout) await DOTween.To(() => fadeImage.Range, (range) => fadeImage.Range = range, 0, transDuration).SetEase(easingType).SetLink(gameObject).SetUpdate(UpdateType.Normal, true);
        isTransitioning = false;

    }

    public void AddListener(Action onSceneLoadEnd)
    {
        this.onSceneLoadEnd += onSceneLoadEnd;
    }

    public void RemoveListener(Action onSceneLoadEnd)
    {
        this.onSceneLoadEnd -= onSceneLoadEnd;
    }

    public void SetDefaultTexture()
    {
        if (fadeImage != null) fadeImage.UpdateMaskTexture(defaultFadeTexture);
    }
}
