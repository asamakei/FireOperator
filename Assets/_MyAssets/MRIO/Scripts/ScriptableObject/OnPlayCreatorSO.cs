using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "MyGame/OnPlayCreatorSO", fileName = "OnPlayCreator")]

public class OnPlayCreatorSO : ScriptableObject
{
    [SerializeField] GameObject[] prefabs;
	void OnEnable()
	{
#if UNITY_EDITOR
		if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode == false) { return; }
#endif

		System.Array.ForEach(prefabs, prefab =>
		{
			GameObject obj = Instantiate(prefab);
		});
	}
}
