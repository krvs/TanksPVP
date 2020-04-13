using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TanksConfig : ScriptableObject
{
    public List<TankConfig> Tanks;
    public Sprite AllyMarker;
    public Sprite EnemyMarker;
    public Sprite EmblemDead;
    public float Speed;
    public int Lives;
    public float RespawnTime;
    public string PlayerLoadedLevel = "PlayerLoadedLevel";
    public float InvincibleDuration;
}