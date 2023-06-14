using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UIElements;
#endif

public class Touchable : MonoBehaviour {

    public static float HeatingSpeedPerSec = 40f;
    public static float CoolingSpeedPerSec = 1f;

    [SerializeField]
    public bool _autoUpdateInEditor = false;
    [SerializeField]
    protected float _thermalEnergy = 0f;
    [SerializeField]
    protected bool _isTouchable = true;
    public float HeatingRate = 1f;
    public float CoolingRate = 1f;
    public float MinEnergy = -10f;
    public float MaxEnergy = -10f;

    private bool _isTouched = false;

    public void OnTouchedEnter() {
        if (_isTouchable) {
            _isTouched = true;
            TouchEnterEvent();
        }
    }

    public void OnTouchedExit() {
        _isTouched = false;
        if (_isTouchable) {
            TouchExitEvent();
        }
    }

    protected virtual void FixedUpdate() {
        ThermalUpdate();
    }

    protected void ThermalUpdate() {
        float preThermal = _thermalEnergy;
        ChangeThermal();
        ThermalEvent(_thermalEnergy - preThermal);
        SubThermalEvent(_thermalEnergy - preThermal);
    }

    private void ChangeThermal() {
        if (_isTouched) {
            AddThermal(Touchable.HeatingSpeedPerSec);
        }
        if (_thermalEnergy > 0) {
            AddThermal(-Touchable.CoolingSpeedPerSec);
        }
        _thermalEnergy = Mathf.Clamp(_thermalEnergy, MinEnergy, MaxEnergy);
    }

    public void AddThermal(float heatPerSec) {
        float changeRate = 0;
        if (heatPerSec > 0) {
            changeRate = HeatingRate;
        } else {
            changeRate = CoolingRate;
        }
        _thermalEnergy += Time.deltaTime * heatPerSec * changeRate;
    }
    public float GetThermal() {
        return _thermalEnergy;
    }

    protected virtual void ThermalEvent(float diff) { }
    protected virtual void SubThermalEvent(float diff) { }
    protected virtual void TouchEnterEvent() { }
    protected virtual void TouchExitEvent() { }

    public virtual void AutoMake() {}

#if UNITY_EDITOR
    [CustomEditor(typeof(Touchable), true)]
    [CanEditMultipleObjects]
    public class TouchableEditor : Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            Touchable touchable = (Touchable)target;
            if (touchable._autoUpdateInEditor) {
                touchable.AutoMake();
                EditorUtility.SetDirty(target);
            } else {
                if (GUILayout.Button("Update")) {
                    touchable.AutoMake();
                    EditorUtility.SetDirty(target);
                }
            }
        }
    }
#endif

}
