using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpriteHitboxData
{
    public Sprite sprite;
    public Vector2 offset;
    public Vector2 size;
}

public class ColliderData : ScriptableObject
{
    [SerializeField]
    private List<SpriteHitboxData> sprites = new List<SpriteHitboxData>();

    public List<SpriteHitboxData> Sprites => sprites;


    public int Count => sprites.Count;

    public SpriteHitboxData this[int index] => sprites[index]; //¿Œµ¶º≠
}
