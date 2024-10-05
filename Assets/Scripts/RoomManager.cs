using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;
using Photon.Realtime;
using System.Collections.Generic;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [HideInInspector] public static RoomManager instance;
    public GameObject player;

    [Space] public Transform[] spawnPoints;
    [Space] public GameObject roomCam;
    [Space] public GameObject connUI;
    [Space] public GameObject LobbyUI;

    [SerializeField] private TextMeshProUGUI nameText;

    public new string name = "";
    [HideInInspector] public int kills = 0;
    [HideInInspector] public int deaths = 0;

    [HideInInspector] public string roomNameToJoin;

    private void Awake() {
        instance = this;
    }

    public void ChangeName(string _name) {
        name = _name;
        nameText.text = name;
    }

    public void CreateRoomButtonPressed() {
        connUI.SetActive(true);

        PhotonNetwork.CreateRoom(roomNameToJoin + "_" + UnityEngine.Random.Range(1000, 9999), new RoomOptions { MaxPlayers = 4 }, null);
    }

    public void JoinRoomButtonPressed() {
        connUI.SetActive(true);

        PhotonNetwork.JoinRoom(roomNameToJoin);
    }

    public override void OnJoinedRoom() {
        base.OnJoinedRoom();

        roomCam.SetActive(false);

        SpawnPlayer();
    }

    public override void OnJoinRoomFailed(short returnCode, string message) {
        connUI.SetActive(false);
        LobbyUI.SetActive(true);
        gameObject.SetActive(false);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        if (otherPlayer.IsLocal) {
            PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        }
    }

    public void SpawnPlayer() {
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);

        _player.GetComponent<PlayerSetup>().IsLocalPlayer();
        _player.GetComponent<Health>().isLocalPlayer = true;
        _player.GetComponent<PhotonView>().RPC("SetName", RpcTarget.AllBuffered, name);
        _player.GetComponent<PhotonView>().RPC("SetMaterial", RpcTarget.AllBuffered, _player.GetComponent<PhotonView>().Owner.ActorNumber - 1);
        PhotonNetwork.LocalPlayer.NickName = name;
    }

    public void SetHashes() {
        try {
            Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;

            hash["kills"] = kills;
            hash["deaths"] = deaths;

            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        } catch { }
    }
}
