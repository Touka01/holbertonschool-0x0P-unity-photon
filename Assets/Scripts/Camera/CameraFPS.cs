using UnityEngine;

public class CameraFPS : MonoBehaviour {

    [SerializeField] private Transform player;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private bool lockCursor = false;
    private float cameraVerticalRotation = 0f;

    void Start() {
        if (lockCursor)
            LockCursor();
    }

    public void LockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {

        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (inputX != 0 || inputY != 0) {
            cameraVerticalRotation -= inputY;
            cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
            transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

            player.Rotate(Vector3.up * inputX);
        }
        
    }
}
