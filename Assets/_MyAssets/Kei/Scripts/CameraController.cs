using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private CameraSetting _cameraSetting;
    //[SerializeField]
    //private Transform _playerTrans;

    private Transform _trans;
    private Camera _camera;
    private Rect _rect;

    void Start()
    {
        _trans = transform;
        _camera = Camera.main;
        if (Player.Trans == null) return;
        _trans.position = Player.Trans.position + (Vector3)_cameraSetting.Pivot + _trans.position.z * Vector3.forward;
        SetRect(0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Player.Trans == null) return;
        Vector3 destination = Player.Trans.position + (Vector3)_cameraSetting.Pivot;

        float x = Mathf.Lerp(_trans.position.x, destination.x, _cameraSetting.Speed.x);
        float y = Mathf.Lerp(_trans.position.y, destination.y, _cameraSetting.Speed.y);

        if (!_cameraSetting.IsMoveX)
        {
            x = _trans.position.x;
        }
        if (!_cameraSetting.IsMoveY)
        {
            y = _trans.position.y;
        }

        Vector3 newPosition = new Vector3(x, y, _trans.position.z);
        _trans.position = PutInsideRect(newPosition);
    }

    void SetRect(int index)
    {
        _rect = StageRect.Rects[index];
    }
    Vector3 PutInsideRect(Vector3 newPosition)
    {
        Vector3 bottomLeft = _camera.ScreenToWorldPoint(Vector3.zero);
        Vector3 topRight = _camera.ScreenToWorldPoint(new Vector3(Screen.width - Screen.width * _camera.rect.x, Screen.height - Screen.height * _camera.rect.y, 0.0f));
        Vector2 screenHalfSize = (topRight - bottomLeft) / 2;

        bottomLeft += newPosition - _trans.position;
        topRight += newPosition - _trans.position;

        float left = _rect.x + screenHalfSize.x;
        float right = _rect.x + _rect.width - screenHalfSize.x;

        float bottom = _rect.y + screenHalfSize.y;
        float top = _rect.y + _rect.height - screenHalfSize.y;

        if (left > right)
        {
            newPosition.x = (left + right) / 2;
        }
        else
        {
            newPosition.x = Mathf.Clamp(newPosition.x, left, right);
        }

        if (bottom > top)
        {
            newPosition.y = (bottom + top) / 2;
        }
        else
        {
            newPosition.y = Mathf.Clamp(newPosition.y, bottom, top);
        }
        return newPosition;

    }
}
