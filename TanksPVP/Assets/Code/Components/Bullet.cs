using Morpeh;
using Photon.Realtime;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[System.Serializable]
public struct Bullet : IComponent
{
    public Player Owner;
    public int Damage;
    public float Speed;
}