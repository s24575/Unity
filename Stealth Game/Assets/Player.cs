using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 7;
    public float smoothOverTime = .1f;
    public float turnSpeed = 8;

    float angle;
    float smoothInputMagnitude;
    float smoothMoveVelocity;
    Vector3 velocity;

    Rigidbody myRigidbody;
    bool disabled;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        Guard.OnGuardHasSpottedPlayer += Disable;
    }
    void Update()
    {
        if (!disabled) {
            Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            float inputMagnitude = inputDirection.magnitude;
            smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothOverTime);

            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);

            velocity = moveSpeed * smoothInputMagnitude * transform.forward;
        }
    }

    void FixedUpdate()
    {
        if (!disabled) {
            myRigidbody.MoveRotation(Quaternion.Euler(Vector3.up * angle));
            myRigidbody.MovePosition(myRigidbody.position + velocity * Time.deltaTime);
        }
    }

    void Disable() {
        disabled = true;
    }

    void OnDestroy() {
        Guard.OnGuardHasSpottedPlayer -= Disable;
    }
}