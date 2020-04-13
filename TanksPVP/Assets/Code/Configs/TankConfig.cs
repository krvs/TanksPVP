using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TankConfig : ScriptableObject
{
    public List<Tank> TankSprites;
}

[Serializable]
public class Tank
{
    public List<Sprite> Up;
}