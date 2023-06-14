using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRect : MonoBehaviour{
    // Start is called before the first frame update
    public static Rect[] Rects;

    void Awake() {
        Transform trans = transform;
        Rects = new Rect[trans.childCount];

        for (int i= 0;i < trans.childCount; i++) {
            RectTransform child = transform.GetChild(i).GetComponent<RectTransform>();
            Vector2 position = child.position;
            var bottomLeftPos = Vector2.zero;
            var topRightPos = Vector2.zero;

            bottomLeftPos.x = position.x - child.rect.width * child.lossyScale.x * child.pivot.x;
            bottomLeftPos.y = position.y - child.rect.height * child.lossyScale.y * child.pivot.y;
            topRightPos.x = position.x + child.rect.width * child.lossyScale.x * (1-child.pivot.x);
            topRightPos.y = position.y + child.rect.height * child.lossyScale.y * (1-child.pivot.y);

            Rect rect = new Rect(bottomLeftPos,topRightPos-bottomLeftPos);
            Rects[i] = rect;
        }
    }
}
