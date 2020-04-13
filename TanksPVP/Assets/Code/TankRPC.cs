using System;
using System.Collections;
using System.Collections.Generic;
using Morpeh.Globals;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class TankRPC : MonoBehaviour
{
    [PunRPC]
    public void Shoot(Vector3 origin, Quaternion originAngle, Player owner, PhotonMessageInfo info)
    {
        var bulletGameObject = PhotonNetwork.PrefabPool.Instantiate("Bullet", origin, originAngle);
        bulletGameObject.SetActive(true);
        var bulletProvider = bulletGameObject.GetComponent<BulletProvider>();
        ref var bullet = ref bulletProvider.GetData();
        bullet.Owner = owner;
    }

    [PunRPC]
    public void Respawn()
    {
    }
}
