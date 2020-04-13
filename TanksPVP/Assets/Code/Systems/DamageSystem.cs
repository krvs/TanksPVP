using Morpeh;
using Photon.Pun;
using Sirenix.Serialization;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(DamageSystem))]
public sealed class DamageSystem : UpdateSystem
{
    private Filter _bulletFilter;
    public override void OnAwake()
    {
        _bulletFilter = World.Filter.With<Bullet>().With<TransformData>().With<CollisionData>();
    }

    public override void OnUpdate(float deltaTime) {
        foreach (var entity in _bulletFilter)
        {
            ref var bullet = ref entity.GetComponent<Bullet>();
            ref var transform = ref entity.GetComponent<TransformData>();
            ref var collisionData = ref entity.GetComponent<CollisionData>();
            if(collisionData.Entity != null)
            {
                ref var health = ref collisionData.Entity.GetComponent<Health>();
                if(health.Value > 0 && !collisionData.Entity.Has<InvincibleMarker>())
                    health.Value -= bullet.Damage;
            }
            
            entity.RemoveComponent<CollisionData>();
            PhotonNetwork.PrefabPool.Destroy(transform.transform.gameObject);
        }
    }
}