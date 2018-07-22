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

    public Transform collisionForceApplyLocation;
    public float collisionForceMultiplier = 1f;
    private bool youEfdUp;
    private float expectedPlayerForwardVelocity;

    Animator animator;
    int walkAnimationHash = Animator.StringToHash("Run");

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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
            // idk if I like this, let's experiment a little
            // var contact = collision.contacts[0];
            // player hits a wall and gets thrown back at the contact point, looks weird they hit the wall flat, then fall forward not backward
            // playerRigidBody.AddForceAtPosition((contact.normal) * collisionForceMultiplier, contact.point, ForceMode.Impulse);
            // this doesn't do much of anything, I thought maybe it would amplify the collision, gonna try bouncy physics materials instead
            // playerRigidBody.AddForceAtPosition((contact.normal * -1) * collisionForceMultiplier, contact.point, ForceMode.Impulse);

            // this at least makes the trip over the low barriers instead of simply falling backward unintuitively
            playerRigidBody.AddForceAtPosition(playerRigidBody.transform.forward * collisionForceMultiplier * expectedPlayerForwardVelocity, 
                                               collisionForceApplyLocation.position, ForceMode.Impulse);

            animator.enabled = false;
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

        expectedPlayerForwardVelocity = playerVelocityOverTime.Evaluate(Time.time * scaleProgressionThroughVelocityCurveBy) * scaleVelocityCurveOutputBy;

        var upwardVelocity = playerRigidBody.velocity.y;
        if (jumpRequested)
        {
            // TODO grounded check
            upwardVelocity = jumpUpVelocity;
            jumpRequested = false;
        }

        playerRigidBody.velocity = new Vector3(playerRigidBody.velocity.x, upwardVelocity, expectedPlayerForwardVelocity);

        animator.speed = expectedPlayerForwardVelocity;
    }
}
