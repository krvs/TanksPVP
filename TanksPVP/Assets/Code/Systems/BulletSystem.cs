using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(BulletSystem))]
public sealed class BulletSystem : UpdateSystem
{
    private Filter _bullets;

    public override void OnAwake()
    {
        _bullets = World.Filter.With<Bullet>().With<TransformData>();
    }

    public override void OnUpdate(float deltaTime)
    {
        MoveBullet(deltaTime);
    }

    private void MoveBullet(float deltaTime)
    {
        foreach (var entity in _bullets)
        {
            ref var bullet = ref entity.GetComponent<Bullet>();
            ref var transform = ref entity.GetComponent<TransformData>();

            transform.transform.position += transform.transform.up * bullet.Speed * deltaTime;
        }
    }
}