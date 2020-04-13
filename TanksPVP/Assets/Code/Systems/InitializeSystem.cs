using System;
using Morpeh;
using Morpeh.Globals;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(InitializeSystem))]
public sealed class InitializeSystem : UpdateSystem
{
    public TanksConfig Config;
    public GlobalEvent StartGameEvent;
    public GlobalEvent InitializeTanks;
    private Filter _lockInputFilter;
    private Filter _bulletFilter;
    private Filter _playerFilter;
    private Filter _emblemFilter;
    
    public override void OnAwake()
    {
        _lockInputFilter = World.Filter.With<LockInputMarker>();
        _bulletFilter = World.Filter.With<Bullet>().With<TransformData>().Without<InitializedMarker>();
        _playerFilter = World.Filter.With<PlayerData>().With<TransformData>().Without<InitializedMarker>();
        _emblemFilter = World.Filter.With<Emblem>();
    }

    public override void OnUpdate(float deltaTime)
    {
        OnStartGame();
        InitializeTank();
        InitializeBullet();
    }

    private void InitializeTank()
    {
        if (InitializeTanks.IsPublished)
        {
            foreach (var entity in _playerFilter)
            {
                ref var transform = ref entity.GetComponent<TransformData>();
                ref var player = ref entity.GetComponent<PlayerData>();
                ref var health = ref entity.GetComponent<Health>();
                ref var marker = ref entity.GetComponent<MarkerSpriteView>();

                player.Speed = Config.Speed;
                health.Value = Config.Lives;
                if (player.PhotonView.IsMine)
                    marker.SpriteRenderer.sprite = Config.AllyMarker;
                else
                    marker.SpriteRenderer.sprite = Config.EnemyMarker;

                foreach (var emblemEntity in _emblemFilter)
                {
                    ref var emblem = ref emblemEntity.GetComponent<Emblem>();
                    ref var emblemMarker = ref emblemEntity.GetComponent<MarkerSpriteView>();
                    if (emblem.Owner == player.PhotonView.Owner && player.PhotonView.IsMine)
                    {
                        transform.transform.position = emblem.SpawnTransform.position;
                        transform.transform.rotation = emblem.SpawnTransform.rotation;

                        emblemMarker.SpriteRenderer.sprite = Config.AllyMarker;
                    }
                    else if(emblem.Owner == player.PhotonView.Owner && !player.PhotonView.IsMine)
                    {
                        emblemMarker.SpriteRenderer.sprite = Config.EnemyMarker;
                    }
                }

                entity.AddComponent<InitializedMarker>();
            }
        }
    }

    private void OnStartGame()
    {
        if (StartGameEvent.IsPublished)
        {
            foreach (var entity in _lockInputFilter)
            {
                entity.RemoveComponent<LockInputMarker>();
            }
        }
    }

    private void InitializeBullet()
    {
        foreach (var entity in _bulletFilter)
        {
            ref var bullet = ref entity.GetComponent<Bullet>();
            
            entity.AddComponent<InitializedMarker>();
        }
    }
}