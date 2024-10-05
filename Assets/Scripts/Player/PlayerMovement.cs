using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpPower = 5f;
    private Vector2 inputVector;
    private Vector3 moveDir;
    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void Update() {
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }


    void FixedUpdate() {
        moveDir = new(inputVector.x, 0f, inputVector.y);
        if (moveDir != Vector3.zero) {
            // Change la direction du vecteur vers la où regarde le player
            moveDir = transform.TransformDirection(moveDir);

            if (IsGrounded())
                transform.position += moveSpeed * Time.deltaTime * moveDir;
            else
                transform.position += (moveSpeed * 0.7f) * Time.deltaTime * moveDir;
        }
    }

    public void Jump() {
        if (IsGrounded()) {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    private bool IsGrounded() {
        return Physics.Raycast(transform.position, -transform.up, 1.25f);
    }
}
