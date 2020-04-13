using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Code
{
    public class Lobby : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TextMeshProUGUI _text;
        private const string _roomName = "BattleCity";
        private readonly RoomOptions _options = new RoomOptions {MaxPlayers = 2};

        #region UNITY

        public void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LocalPlayer.NickName = "Player " + Random.Range(1000, 10000);
            PhotonNetwork.ConnectUsingSettings();
        }

        #endregion

        #region PUN CALLBACKS

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinOrCreateRoom(_roomName, _options, null);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            PhotonNetwork.JoinRoom(_roomName);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(_roomName, _options, null);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(_roomName, _options, null);
        }

        public override void OnJoinedRoom()
        {
            _text.text = "Waiting for opponent";
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            _text.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;

                PhotonNetwork.LoadLevel("Main");
            }
        }

        #endregion
    }
}