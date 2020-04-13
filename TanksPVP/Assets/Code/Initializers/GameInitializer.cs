using Morpeh;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Initializers/" + nameof(GameInitializer))]
public sealed class GameInitializer : Initializer
{
    public ObjectPool BulletPull;
    
    public override void OnAwake()
    {
        BulletPull.Clear();
        var pool = PhotonNetwork.PrefabPool as DefaultPool;
        PhotonNetwork.PrefabPool = BulletPull;
    }

    public override void Dispose() {
    }
}