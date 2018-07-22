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

    public float animationSpeedMultiplier = 1f;
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
        if (Input.GetKeyDown(KeyCode.Space))
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

            playerRigidBody.constraints = RigidbodyConstraints.None;

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

        var movementChangeInput = Input.GetAxis("Horizontal");
        var movement = movementChangeInput * moveSensitivity;

        var newHorizontalPosition = Mathf.Clamp(transform.position.x + movement, leftMostBound, rightMostBound);
        transform.position = new Vector3(newHorizontalPosition, transform.position.y, transform.position.z);

        expectedPlayerForwardVelocity = playerVelocityOverTime.Evaluate(Time.timeSinceLevelLoad * scaleProgressionThroughVelocityCurveBy) * scaleVelocityCurveOutputBy;

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
