using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(InvincibleSystem))]
public sealed class InvincibleSystem : UpdateSystem {
    private Filter _invincible;
    public override void OnAwake() {
        _invincible = World.Filter.With<InvincibleMarker>();
    }

    public override void OnUpdate(float deltaTime) {
        foreach (var entity in _invincible)
        {
            ref var invincible = ref entity.GetComponent<InvincibleMarker>();
            if(invincible.Duration > 0)
            {
                invincible.Duration -= deltaTime;
                return;
            }

            entity.RemoveComponent<InvincibleMarker>();
            ref var animator = ref entity.GetComponent<AnimatorData>();
            animator.Animator.SetBool("IsInvincible", false);
        }
    }
}