using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PointerSO", menuName = "SciptableObject", order = 0)]
[System.Serializable]
public class PointerSO : ScriptableObject
{
    public PointerType Type;
    public Vector2 Hotspot;
    public Texture2D Texture;
}
