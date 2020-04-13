using Morpeh;
using Photon.Pun;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using System;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(TankViewSystem))]
public sealed class TankViewSystem : UpdateSystem
{
    private Filter _filter;
    private Filter _invincibleFilter;
    private Filter _respawnFilter;

    public override void OnAwake()
    {
        var commonFilter = World.Filter.With<SpriteView>().With<AnimatorData>();
        _filter = commonFilter.With<PlayerData>().With<InputData>();
        _invincibleFilter = commonFilter.With<InvincibleMarker>();
        _respawnFilter = commonFilter.With<RespawnMarker>();
    }

    public override void OnUpdate(float deltaTime)
    {
        TurnMovingAnimation();
        ShowInvincibleEffect();
        //ShowRespawnEffect();
    }

    private void ShowRespawnEffect()
    {
        foreach (var entity in _respawnFilter)
        {
            ref var animator = ref entity.GetComponent<AnimatorData>();
            animator.Animator.SetBool("IsDead", true);
        }
    }

    private void TurnMovingAnimation()
    {
        foreach (var entity in _filter)
        {
            ref var animator = ref entity.GetComponent<AnimatorData>();
            ref var input = ref entity.GetComponent<InputData>();

            animator.Animator.SetBool("IsMoving", input.Direction != Direction.none);
        }
    }

    private void ShowInvincibleEffect()
    {
        foreach (var entity in _invincibleFilter)
        {
            ref var animator = ref entity.GetComponent<AnimatorData>();
            animator.Animator.SetBool("IsInvincible", true);
        }
    }
}