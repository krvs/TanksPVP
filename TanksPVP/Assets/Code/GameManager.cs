using System;
using System.Collections;
using System.Collections.Generic;
using Morpeh;
using Morpeh.Globals;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TanksConfig _tanksConfig;
    [SerializeField] private GlobalEvent _gameStart;
    [SerializeField] private GlobalEvent _initializeTanks;
    [SerializeField] private List<EmblemProvider> _emblems;

    private PlayerProvider _playerProvider;

    #region Unity

    public override void OnEnable()
    {
        base.OnEnable();
        CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerHasExpired;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerHasExpired;
    }

    private void Start()
    {
        var props = new Hashtable
        {
            {AsteroidsGame.PLAYER_LOADED_LEVEL, true}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        
        SetupPlayer();
    }

    private void OnApplicationQuit()
    {
        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.NetworkClientState != ClientState.Leaving)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    #endregion

    #region PunCallbacks

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(PhotonNetwork.NetworkClientState != ClientState.Leaving)
            PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }
    
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (changedProps.ContainsKey(_tanksConfig.PlayerLoadedLevel))
            {
                if (CheckAllPlayerLoadedLevel())
                {
                    var props = new Hashtable
                    {
                        {CountdownTimer.CountdownStartTime, (float) PhotonNetwork.Time}
                    };
                    PhotonNetwork.CurrentRoom.SetCustomProperties(props);      
                }
            }
        }

        if (CheckAllPlayerLoadedLevel())
            SetPlayerToSide();
    }
    #endregion

    private void OnCountdownTimerHasExpired()
    {
        _gameStart.Publish();
    }
    
    private bool CheckAllPlayerLoadedLevel()
    {
        foreach (var p in PhotonNetwork.PlayerList)
        {
            if (p.CustomProperties.TryGetValue(_tanksConfig.PlayerLoadedLevel, out var playerLoadedLevel))
            {
                if ((bool) playerLoadedLevel)
                {
                    continue;
                }
            }

            return false;
        }

        return true;
    }

    public void SetPlayerToSide()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            _emblems[i].GetData().Owner = PhotonNetwork.PlayerList[i];
        }

        _initializeTanks.Publish();
    }


    private void SetupPlayer()
    {
        var tank = PhotonNetwork.Instantiate("Tank", Vector3.zero, Quaternion.identity);
        _playerProvider = tank.GetComponent<PlayerProvider>();
        _playerProvider.Entity.AddComponent<LockInputMarker>();
        _playerProvider.Entity.AddComponent<InputData>();
    }
}
