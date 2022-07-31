using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Drops/New Drop", fileName = "DropObject", order = 0)]
public class ScriptableDrops : ScriptableObject
{
    [Header("Drop Information")]
    [SerializeField] string _name;
    public string Name { get { return _name; } }

    [SerializeField] DropType _type;
    public DropType Type { get { return _type; } }

    [SerializeField] Material _mat;
    public Material Mat { get { return _mat; } }

    [SerializeField] Sprite _sprite;
    public Sprite Sprite { get { return _sprite; } }

}

public enum DropType
{
    Default = 0,

    Slime1 = 10,
    Slime2 = 11,
}
