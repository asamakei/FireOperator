using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "MyGame/NumberImageDBSO", fileName = "NumberImage")]

public class NumberImageDBSO : ScriptableObject
{
    [SerializeField] Sprite[] numberSprite;

    public Sprite[] GetNumberSprites(int number)
    {
        List<Sprite> sprites = new List<Sprite>();
        int tmpnumber = number;
        for (int i = Mathf.FloorToInt(Mathf.Log10(number)) + 1; i > 1; i--)
        {
            int f = Mathf.FloorToInt(Mathf.Pow(10, i-1));
            int tmp = Mathf.FloorToInt(tmpnumber / f);
            if (tmp < numberSprite.Length) sprites.Add(numberSprite[tmp]);
            tmpnumber = tmpnumber % f;
        }
        return sprites.ToArray();
    }

}
