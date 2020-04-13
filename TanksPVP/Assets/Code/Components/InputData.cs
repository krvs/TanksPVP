using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[System.Serializable]
public struct InputData : IComponent
{
    public bool Shoot;
    public bool BlockUp;
    public bool BlockDown;
    public bool BlockLeft;
    public bool BlockRight;
    public Direction Direction;
    public Direction IdleDirection;
}