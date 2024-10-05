using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using static UnityEngine.UI.Image;

public class RoomList : MonoBehaviourPunCallbacks
{
    [HideInInspector] public static RoomList instance;

    public GameObject roomManagerGameObject;
    public RoomManager roomManager;

    [Header("UI")]
    public Transform roomListParent;
    public GameObject roomListItemPrefab;

    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    private void Awake() {
        instance = this;
    }

    IEnumerator Start()
    {
        if (PhotonNetwork.InRoom) {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }

        yield return new WaitUntil(() => !PhotonNetwork.IsConnected);


        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
    }

    public void ChangeRoomName(string _roomName) {
        roomManager.roomNameToJoin = _roomName;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        if (cachedRoomList.Count <= 0)
            cachedRoomList = roomList;
        else
            foreach (var room in roomList) {
                for (int i = 0; i < cachedRoomList.Count; i++) {
                    if (cachedRoomList[i].Name == room.Name) {
                        List<RoomInfo> newList = cachedRoomList;

                        if (room.RemovedFromList)
                            newList.Remove(newList[i]);
                        else
                            newList[i] = room;

                        cachedRoomList = newList;
                    }
                }
            }

        UpdateUI();
    }

    void UpdateUI() {
        foreach (Transform roomitem in roomListParent) {
            Destroy(roomitem.gameObject);
        }

        foreach (var room in cachedRoomList) {
            GameObject roomItem = Instantiate(roomListItemPrefab, roomListParent);

            roomItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Name.Substring(0, room.Name.Length - 5); ;
            roomItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = room.PlayerCount + "/4";

            roomItem.GetComponent<RoomItemButton>().roomName = room.Name;
        }
    }

    public void JoinRoomByName(string _name) {
        roomManager.roomNameToJoin = _name;
        roomManagerGameObject.SetActive(true);
        RoomManager.instance.JoinRoomButtonPressed();
        gameObject.SetActive(false);
    }
}
