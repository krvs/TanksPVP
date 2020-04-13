using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(CheckDeathSystem))]
public sealed class CheckDeathSystem : UpdateSystem
{
    public TanksConfig Config;
    private Filter _healthFilter;
    private Filter _player;
    
    public override void OnAwake()
    {
        _healthFilter = World.Filter.With<Health>();
        _player = World.Filter.With<PlayerData>();
    }

    public override void OnUpdate(float deltaTime) {
        foreach (var entity in _healthFilter)
        {
            ref var health = ref entity.GetComponent<Health>();
            if (health.Value == 0)
            {
                if (entity.Has<PlayerData>())
                {
                    var respawnMarker = new RespawnMarker() {RespawnTime = 3f};
                    entity.SetComponent(respawnMarker);                
                    entity.AddComponent<LockInputMarker>();
                    health.Value = Config.Lives;

                    ref var transform = ref entity.GetComponent<TransformData>();
                    transform.transform.position = new Vector2(100, 100);
                } 
                else if(entity.Has<Emblem>())
                {
                    health.Value = 1;
                    ref var emblem = ref entity.GetComponent<Emblem>();
                    ref var spriteView = ref entity.GetComponent<SpriteView>();
                    spriteView.SpriteRenderer.sprite = Config.EmblemDead;
                    var endGameEntity = World.CreateEntity();
                    var endGame = new EndGameMarker { Duration = 3f };
                    endGameEntity.SetComponent(endGame);

                    foreach (var playerEntity in _player)
                    {
                        ref var player = ref playerEntity.GetComponent<PlayerData>();
                        if(emblem.Owner == player.PhotonView.Owner)
                            TextSingleton.Instance.Text.text = "You won";
                        else
                            TextSingleton.Instance.Text.text = "You lost";
                    }                              
                }
                else
                {
                    ref var transform = ref entity.GetComponent<TransformData>();
                    Destroy(transform.transform.gameObject);
                }   
            }
        }
    }
}