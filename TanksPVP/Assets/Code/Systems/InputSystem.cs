using Morpeh;
using Morpeh.Globals;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(InputSystem))]
public sealed class InputSystem : UpdateSystem
{
    private Filter _filter;

    public override void OnAwake()
    {
        _filter = World.Filter.With<PlayerData>().With<InputData>().Without<LockInputMarker>();
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (var playerEntity in _filter)
        {
            var player = playerEntity.GetComponent<PlayerData>();

            ref var input = ref playerEntity.GetComponent<InputData>();
                
            if(Input.GetKey(KeyCode.W))
                input.Direction = input.IdleDirection = Direction.up;
            else if (Input.GetKey(KeyCode.S))
                input.Direction = input.IdleDirection = Direction.down;
            else if (Input.GetKey(KeyCode.D))
                input.Direction = input.IdleDirection = Direction.right;
            else if (Input.GetKey(KeyCode.A))
                input.Direction = input.IdleDirection = Direction.left;
            else
                input.Direction = Direction.none;
                
            input.Shoot = Input.GetKey(KeyCode.Space);
        }
    }
}