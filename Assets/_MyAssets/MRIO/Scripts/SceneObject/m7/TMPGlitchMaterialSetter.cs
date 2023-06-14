using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
public class TMPGlitchMaterialSetter : MonoBehaviour
{
    [SerializeField] Material toMaterial;
    private void Awake()
    {
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        mesh.material = toMaterial;
    }
}
