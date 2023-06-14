using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(GridLayoutGroup))]
public class LevelView : MonoBehaviour
{
    [SerializeField] GameObject levelStarPrefab;
    [SerializeField] int defaultLevelStarsNum;
    [SerializeField] int[] levelThresholds;
    [SerializeField] Vector2[] cellSizes;
    [SerializeField] Vector2[] spaces;
    GridLayoutGroup gridLayoutGroup;
    List<GameObject> levelStars;
    private void Awake()
    {
        levelStars = new List<GameObject>();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        for (int i = 0; i < defaultLevelStarsNum; i++)
        {
            GameObject obj = Instantiate(levelStarPrefab, transform);
            levelStars.Add(obj);
            obj.SetActive(false);
        }
    }

    public void ChangeLevel(int level)
    {
        int index = 0;
        for (int i = 0; i < levelThresholds.Length - 1; i++)
        {
            if (level >= levelThresholds[i] && level < levelThresholds[i + 1]) index = i;
        }
        index = (level > levelThresholds[levelThresholds.Length - 1]) ? levelThresholds.Length - 1 : index;
        gridLayoutGroup.cellSize = cellSizes[Mathf.Clamp(index, 0, cellSizes.Length - 1)];
        gridLayoutGroup.spacing = spaces[Mathf.Clamp(index, 0, spaces.Length - 1)];
        for (int i = 0; i < levelStars.Count; i++)
        {
            levelStars[i].SetActive(i < level);
        }
    }
}
