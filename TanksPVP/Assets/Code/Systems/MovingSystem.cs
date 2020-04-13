using Morpeh;
using Morpeh.Globals;
using Photon.Pun;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(MovingSystem))]
public sealed class MovingSystem : UpdateSystem
{
    private Filter _filter;
    
    private float speed = 1f;
    
    public override void OnAwake()
    {
        _filter = World.Filter.With<PlayerData>().With<TransformData>().With<InputData>();
    }

    public override void OnUpdate(float deltaTime) {
        foreach (var entity in _filter)
        {
            ref var player = ref entity.GetComponent<PlayerData>();

            if (player.PhotonView.IsMine)
            {
                ref var transform = ref entity.GetComponent<TransformData>();
                ref var input = ref entity.GetComponent<InputData>();

                var direction = Vector3.zero;
                if (input.Direction == Direction.up && !input.BlockUp)
                    direction.y = 1;
                else if (input.Direction == Direction.down && !input.BlockDown)
                    direction.y = -1;
                else if (input.Direction == Direction.left && !input.BlockLeft)
                    direction.x = -1;
                else if (input.Direction == Direction.right && !input.BlockRight)
                    direction.x = 1;

                if (input.Direction == Direction.up)
                    transform.transform.rotation = Quaternion.Euler(new Vector3(0,0, 0));
                else if (input.Direction == Direction.down)
                    transform.transform.rotation = Quaternion.Euler(new Vector3(0,0, 180));
                else if (input.Direction == Direction.left)
                    transform.transform.rotation = Quaternion.Euler(new Vector3(0,0, 90));
                else if (input.Direction == Direction.right)
                    transform.transform.rotation = Quaternion.Euler(new Vector3(0,0, 270));

                transform.transform.position += direction * speed * deltaTime;
            }
        }
    }                                   
}