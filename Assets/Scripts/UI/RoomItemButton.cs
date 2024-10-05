using UnityEngine;

public class RoomItemButton : MonoBehaviour
{
    public string roomName;

    public void OnButtonPressed() {
        RoomList.instance.JoinRoomByName(roomName);
    }
}
