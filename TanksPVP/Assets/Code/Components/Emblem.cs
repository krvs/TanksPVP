using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Photon.Realtime;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[System.Serializable]
public struct Emblem : IComponent {
    public Player Owner;
    public Transform SpawnTransform;
}