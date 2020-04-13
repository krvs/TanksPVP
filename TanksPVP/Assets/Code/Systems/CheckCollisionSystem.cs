using System;
using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(CheckCollisionSystem))]
public sealed class CheckCollisionSystem : FixedUpdateSystem
{
    private Filter _playerFilter;
    private Filter _bulletFilter;

    private float _distance = 0.02f;
    
    public override void OnAwake()
    {
        _playerFilter = World.Filter.With<PlayerData>().With<TransformData>().With<InputData>().With<ColliderData>();
        _bulletFilter = World.Filter.With<Bullet>().With<TransformData>().With<ColliderData>();
    }

    public override void OnUpdate(float deltaTime)
    {
        CheckCollisionsWithPlayer();
        CheckCollisionWithBullet();
    }

    private void CheckCollisionsWithPlayer()
    {
        foreach (var entity in _playerFilter)
        {
            ref var transform = ref entity.GetComponent<TransformData>();
            ref var input = ref entity.GetComponent<InputData>();
            ref var collider = ref entity.GetComponent<ColliderData>();

            var hitUpLeft =
                Physics2D.Raycast(new Vector2(collider.Collider.bounds.min.x, collider.Collider.bounds.max.y + _distance),
                    Vector2.up, _distance);
            var hitUpMiddle =
                Physics2D.Raycast(new Vector2(transform.transform.position.x, collider.Collider.bounds.max.y + _distance),
                    Vector2.up, _distance);
            var hitUpRight =
                Physics2D.Raycast(new Vector2(collider.Collider.bounds.max.x, collider.Collider.bounds.max.y + _distance),
                    Vector2.up, _distance);

            var hitDownLeft =
                Physics2D.Raycast(new Vector2(collider.Collider.bounds.min.x, collider.Collider.bounds.min.y - _distance),
                    Vector2.down, _distance);
            var hitDownMiddle =
                Physics2D.Raycast(new Vector2(transform.transform.position.x, collider.Collider.bounds.min.y - _distance),
                    Vector2.down, _distance);
            var hitDownRight =
                Physics2D.Raycast(new Vector2(collider.Collider.bounds.max.x, collider.Collider.bounds.min.y - _distance),
                    Vector2.down, _distance);

            var hitLeftTop =
                Physics2D.Raycast(new Vector2(collider.Collider.bounds.min.x - _distance, collider.Collider.bounds.max.y),
                    Vector2.left, _distance);
            var hitLeftMiddle =
                Physics2D.Raycast(new Vector2(collider.Collider.bounds.min.x - _distance, transform.transform.position.y),
                    Vector2.left, _distance);
            var hitLeftBottom =
                Physics2D.Raycast(new Vector2(collider.Collider.bounds.min.x - _distance, collider.Collider.bounds.min.y),
                    Vector2.left, _distance);

            var hitRightTop =
                Physics2D.Raycast(new Vector2(collider.Collider.bounds.max.x + _distance, collider.Collider.bounds.max.y),
                    Vector2.right, _distance);
            var hitRightMiddle =
                Physics2D.Raycast(new Vector2(collider.Collider.bounds.max.x + _distance, transform.transform.position.y),
                    Vector2.right, _distance);
            var hitRightBottom =
                Physics2D.Raycast(new Vector2(collider.Collider.bounds.max.x + _distance, collider.Collider.bounds.min.y),
                    Vector2.right, _distance);

            input.BlockUp = hitUpLeft.collider != null || hitUpMiddle.collider != null || hitUpRight.collider != null;
            input.BlockDown = hitDownLeft.collider != null || hitDownMiddle.collider != null ||
                              hitDownRight.collider != null;
            input.BlockLeft = hitLeftTop.collider != null || hitLeftMiddle.collider != null ||
                              hitLeftBottom.collider != null;
            input.BlockRight = hitRightTop.collider != null || hitRightMiddle.collider != null ||
                               hitRightBottom.collider != null;
        }
    }

    private void CheckCollisionWithBullet()
    {
        foreach (var entity in _bulletFilter)
        {
            ref var transform = ref entity.GetComponent<TransformData>();

            var hit = Physics2D.Raycast(transform.transform.position + transform.transform.up * 0.08f, Vector2.up, _distance);

            if (hit.collider != null)
            {
                var collisionData = new CollisionData
                {
                    Entity = hit.collider.TryGetComponent(out HealthProvider healthProvider)
                        ? healthProvider.Entity
                        : null
                };
                entity.SetComponent(collisionData);
            }
        }
    }
}