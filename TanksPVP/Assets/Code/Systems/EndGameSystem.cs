using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Photon.Pun;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EndGameSystem))]
public sealed class EndGameSystem : UpdateSystem {
    private Filter _endGame;
    public override void OnAwake() {
        _endGame = World.Filter.With<EndGameMarker>();
    }

    public override void OnUpdate(float deltaTime) {
        foreach (var entity in _endGame)
        {
            ref var endGame = ref entity.GetComponent<EndGameMarker>();
            if (endGame.Duration > 0)
            {
                endGame.Duration -= deltaTime;
                return;
            }

            PhotonNetwork.LeaveRoom();
            entity.RemoveComponent<EndGameMarker>();
        }
    }
}