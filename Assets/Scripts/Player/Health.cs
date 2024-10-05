using UnityEngine;
using Photon.Pun;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float health = 100f;
    public bool isLocalPlayer;
    private bool isInvincible = false;

    [Header("UI")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Animator hitAnim;

    [Header("VFX")]
    [SerializeField] private GameObject deathVFX;

    [PunRPC]
    public void TakeDamage(float damage) {
        if (isInvincible == false) {
            isInvincible = true;
            hitAnim.SetTrigger("hitTrigger");
            health -= damage;
            healthBar.fillAmount = health / 100f;

            if (health <= 0) {
                PhotonNetwork.Instantiate(deathVFX.name, transform.position, Quaternion.identity);                
                if (isLocalPlayer) {
                    RoomManager.instance.deaths++;
                    RoomManager.instance.SetHashes();
                    RoomManager.instance.SpawnPlayer();
                    PhotonNetwork.Destroy(gameObject);
                }
                    
            } else
                StartCoroutine(Invincibility());
        }
    }

    IEnumerator Invincibility() {
        yield return new WaitForSeconds(0.2f);
        isInvincible = false;
    }
}
