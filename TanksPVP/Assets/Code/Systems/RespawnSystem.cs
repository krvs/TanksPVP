using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(RespawnSystem))]
public sealed class RespawnSystem : UpdateSystem
{
    public TanksConfig Config;
    private Filter _respawnFilter;
    private Filter _emblemFilter;

    public override void OnAwake()
    {
        _respawnFilter = World.Filter.With<RespawnMarker>().With<PlayerData>().With<TransformData>();
        _emblemFilter = World.Filter.With<Emblem>();
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (var entity in _respawnFilter)
        {
            ref var respawnMarker = ref entity.GetComponent<RespawnMarker>();
            if (respawnMarker.RespawnTime > 0)
            {
                respawnMarker.RespawnTime -= deltaTime;
                return;
            }

            ref var transform = ref entity.GetComponent<TransformData>();
            ref var player = ref entity.GetComponent<PlayerData>();

            foreach (var emblemEntity in _emblemFilter)
            {
                ref var emblem = ref emblemEntity.GetComponent<Emblem>();
                if (emblem.Owner == player.PhotonView.Owner)
                    transform.transform.position = emblem.SpawnTransform.position;
            }

            entity.RemoveComponent<RespawnMarker>();
            entity.RemoveComponent<LockInputMarker>();

            //ref var animator = ref entity.GetComponent<AnimatorData>();
            //animator.Animator.SetBool("IsDead", false);

            var invincible = new InvincibleMarker { Duration = Config.InvincibleDuration };
            entity.SetComponent(invincible);
        }
    }
}