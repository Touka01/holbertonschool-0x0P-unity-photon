using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private new GameObject camera;
    [SerializeField] private new string name;
    [SerializeField] private TextMeshPro nameText;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Material[] materials;

    public void IsLocalPlayer() {
        movement.enabled = true;
        canvas.SetActive(true);
        camera.SetActive(true);
        nameText.enabled = false;
    }

    [PunRPC]
    public void SetName(string _name) {
        name = (_name != "" ? name : "?");
        nameText.text = name;
    }

    [PunRPC]
    public void SetMaterial(int index) {
        Renderer renderer = GetComponent<Renderer>();
        index = Mathf.Clamp(index, 0, materials.Length - 1);
        renderer.material = materials[index];
    }

}
  