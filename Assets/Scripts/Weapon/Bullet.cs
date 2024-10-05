using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private readonly float sphereLifetime = 3f;

    private PhotonView photonView;

    private int playerID;

    private void Awake() {
        photonView = GetComponent<PhotonView>();
    }

    private void Start() {
        if (photonView.IsMine)
            Invoke(nameof(DestroySphere), sphereLifetime);
    }

    public void SetPlayerWhoShotID(int _playerID) {
        playerID = _playerID;
    }

    private void DestroySphere() {
        if (gameObject != null)
            PhotonNetwork.Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (photonView.IsMine) {
                if (collision.gameObject.GetComponent<PhotonView>().Owner.ActorNumber == playerID)
                    return;
                if (collision.gameObject.GetComponent<Health>().health <= 33.5f) { // Kill
                    RoomManager.instance.kills++;
                    RoomManager.instance.SetHashes();
                    PhotonNetwork.LocalPlayer.AddScore(3);
                }
                else // Hit
                    PhotonNetwork.LocalPlayer.AddScore(1);
                collision.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, 33.5f);
            }
        }
        if (photonView.IsMine)
            PhotonNetwork.Destroy(gameObject);
    }

}
