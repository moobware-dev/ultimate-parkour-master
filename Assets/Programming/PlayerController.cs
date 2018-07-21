using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSensitivity = 1f;
    public float leftMostBound = -1.5f;
    public float rightMostBound = 1.5f;

    public AnimationCurve playerVelocityOverTime;
    public float scaleProgressionThroughVelocityCurveBy = 0.1f;
    public float scaleVelocityCurveOutputBy = 2f;

    public float jumpUpVelocity = 3f;
    bool jumpRequested;

    Rigidbody playerRigidBody;

    public float collisionForceMultiplier = 1f;
    private bool youEfdUp;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (youEfdUp) {
            return;
        }

        // right mouseclick
        if (Input.GetMouseButtonDown(1))
        {
            jumpRequested = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (youEfdUp) {
            return;
        }

        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }

        if (collision.gameObject.CompareTag("Obstacle")) {
            Debug.Log("Player hit an obstacle!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1");

            playerRigidBody.constraints = RigidbodyConstraints.None;
            var contact = collision.contacts[0];
            playerRigidBody.AddForceAtPosition(contact.normal * collisionForceMultiplier, contact.point, ForceMode.Impulse);
            youEfdUp = true;
        }
    }

    void FixedUpdate()
    {
        if (youEfdUp) {
            return;
        }

        var movementChangeInput = Input.GetAxis("Mouse ScrollWheel");
        var movement = movementChangeInput * moveSensitivity;

        var newHorizontalPosition = Mathf.Clamp(transform.position.x + movement, leftMostBound, rightMostBound);
        transform.position = new Vector3(newHorizontalPosition, transform.position.y, transform.position.z);

        var forwardVelocity = playerVelocityOverTime.Evaluate(Time.time * scaleProgressionThroughVelocityCurveBy) * scaleVelocityCurveOutputBy;
        var upwardVelocity = playerRigidBody.velocity.y;
        if (jumpRequested)
        {
            // TODO grounded check
            upwardVelocity = jumpUpVelocity;
            jumpRequested = false;
        }

        playerRigidBody.velocity = new Vector3(playerRigidBody.velocity.x, upwardVelocity, forwardVelocity);
    }
}
