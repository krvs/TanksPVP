using Morpeh;
using Morpeh.Globals;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.UI;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(ShootingSystem))]
public sealed class ShootingSystem : UpdateSystem
{
    public GlobalEvent Event;
    private Filter _filter;
    private float _shootingTimer;
    
    public override void OnAwake() {
        _filter = World.Filter.With<PlayerData>().With<TransformData>().With<InputData>().With<ColliderData>();
    }

    public override void OnUpdate(float deltaTime)
    {
        Shoot();
    }

    private void Shoot()
    {
        foreach (var entity in _filter)
        {
            ref var input = ref entity.GetComponent<InputData>();
            ref var player = ref entity.GetComponent<PlayerData>();
            ref var transform = ref entity.GetComponent<TransformData>();

            if (input.Shoot && _shootingTimer <= 0.0)
            {
                _shootingTimer = 0.2f;

                var origin = transform.transform.position;
                if (input.IdleDirection == Direction.up)
                    origin += Vector3.up * 0.2f;
                if(input.IdleDirection == Direction.down)
                    origin += Vector3.down * 0.2f;
                if(input.IdleDirection == Direction.left)
                    origin += Vector3.left * 0.2f;
                if(input.IdleDirection == Direction.right)
                    origin += Vector3.right * 0.2f;

                player.PhotonView.RPC("Shoot", RpcTarget.AllViaServer, origin, transform.transform.rotation, player.PhotonView.Owner);
            }

            if (_shootingTimer > 0.0f)
            {
                _shootingTimer -= Time.deltaTime;
            }
        }
    }
}