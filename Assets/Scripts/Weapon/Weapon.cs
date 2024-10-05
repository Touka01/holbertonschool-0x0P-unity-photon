using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private GameObject bullet;

    public float fireRate = 1f;
    public float bulletSpeed = 20f;
    private float nextFire = 0f;

    private int ammo = 5;
    [SerializeField] private Slider reloadSLider;
    private float fillTime = 0f;
    private float fillValue = 0.33f;

    private void Update()
    {
        if (nextFire > 0f)
            nextFire -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && nextFire <= 0f && ammo > 0) {
            nextFire = fireRate;
            Fire();
            reloadSLider.value = 0f;
            fillTime = 0f;
            ammo--;
        }

        if (reloadSLider.value != 1f) {
            reloadSLider.value = Mathf.Lerp(reloadSLider.minValue, reloadSLider.maxValue, fillTime);
            fillTime += fillValue * Time.deltaTime;
        } else if (ammo != 5)
            ammo = 5;
    }

    private void Fire() {
        Vector3 camForward = camera.transform.forward;
        GameObject newBullet = PhotonNetwork.Instantiate(bullet.name, camera.transform.position + camForward, Quaternion.identity);
        newBullet.GetComponent<Bullet>().SetPlayerWhoShotID(PhotonNetwork.LocalPlayer.ActorNumber);

        if (newBullet.TryGetComponent<Rigidbody>(out var bulletRigidbody))
            bulletRigidbody.velocity = camForward * bulletSpeed;
    }
}
