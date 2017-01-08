﻿using UnityEngine;


/// <summary>
/// Defines mob behaviour, physics, and stats. Uses the physics
/// object and enables other components to control the mob.
/// </summary>
public class Mob : MonoBehaviour {

    public uint team = 0;
    public GameObject deadBodyPrefab;

    private PhysicsObject physicsObject;
    private Collider2D myCollider;
    private float horizontalInput = 0f;
    private bool jumpInput = false;

    public void Move(float direction) {
        horizontalInput = direction;
    }

    public void Stop() {
        horizontalInput = 0f;
    }

    public void Jump() {
        jumpInput = true;
    }

    public void Awake() {
        physicsObject = GetComponent<PhysicsObject>();
        myCollider = GetComponent<Collider2D>();
    }

    public void Update() {
        AdjustVelocityByInput();
        CheckHeadStomping();
    }

    private float CalculateVelocityForJumpHeight(float height) {
        return Mathf.Sqrt(2f * height * Globals.gravity);
    }

    private void AdjustVelocityByInput() {
        if (!Mathf.Approximately(0f, horizontalInput)) {
            var acceleration = horizontalInput * Globals.mobMaxMoveSpeed / Globals.mobAccelerationTime * Time.deltaTime;

            if (Mathf.Sign(physicsObject.velocity.x) == Mathf.Sign(horizontalInput)) {
                physicsObject.velocity.x += acceleration;
            }
            else {
                physicsObject.velocity.x += acceleration * 2;
            }

            physicsObject.velocity.x = Mathf.Clamp(physicsObject.velocity.x, -Globals.mobMaxMoveSpeed, Globals.mobMaxMoveSpeed);
            physicsObject.applyGroundFriction = false;
            physicsObject.applyAirFriction = false;

            var newScale = transform.localScale;
            newScale.x = horizontalInput;
            transform.localScale = newScale;
        }
        else {
            physicsObject.applyGroundFriction = true;
            physicsObject.applyAirFriction = true;
        }

        if (jumpInput && (physicsObject.isGrounded || physicsObject.timeInAir < .2f)) {
            physicsObject.velocity.y = CalculateVelocityForJumpHeight(Globals.mobJumpHeight);
        }
        jumpInput = false;
    }

    private void CheckHeadStomping() {
        var boxcastPosition = new Vector2(transform.position.x, transform.position.y + myCollider.bounds.extents.y / 2f);
        var boxcastSize = physicsObject.size;
        boxcastSize.x -= Globals.skinThickness * 2f;
        boxcastSize.y -= Globals.skinThickness * 2f;

        var colliders = Physics2D.OverlapBoxAll(boxcastPosition, boxcastSize, 0, LayerMask.GetMask(Globals.mobLayerName));
        foreach (var collider in colliders) {
            if (collider == myCollider) {
                continue;
            }

            var otherMob = collider.GetComponent<Mob>();
            if (!otherMob) {
                continue;
            }

            var otherTransform = collider.transform;
            var otherPhysicsObject = collider.GetComponent<PhysicsObject>();

            if (team == otherMob.team && team != 0) {
                continue;
            }

            if (otherPhysicsObject.velocity.y < 0 &&
                otherTransform.position.y - collider.bounds.extents.y > transform.position.y &&
                otherTransform.position.x - collider.bounds.extents.x < transform.position.x + myCollider.bounds.extents.x - Globals.skinThickness * 2f &&
                otherTransform.position.x + collider.bounds.extents.x > transform.position.x - myCollider.bounds.extents.x + Globals.skinThickness * 2f) {
                // apply positive velocity to the other mob according to the formula:
                // new Y velocity = maximum(standard jump velocity / 2, mob's current positive velocity * .7 + my velocity * .4)
                otherPhysicsObject.velocity.y = Mathf.Max(CalculateVelocityForJumpHeight(Globals.mobJumpHeight / 2f),
                    -otherPhysicsObject.velocity.y * .7f + physicsObject.velocity.y * .4f);

                var body = Instantiate(deadBodyPrefab, transform.position, transform.rotation);
                body.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;

                var bodyVelocity = physicsObject.velocity;
                if (bodyVelocity.y > Mathf.Epsilon) {
                    bodyVelocity.x /= 1.5f;
                    bodyVelocity.y *= -1;
                }
                else {
                    bodyVelocity.x = Random.Range(-4f, 4f);
                    bodyVelocity.y = CalculateVelocityForJumpHeight(1.5f) + Random.Range(-2f, 2f);
                }
                body.GetComponent<PhysicsObject>().velocity = bodyVelocity;

                Destroy(gameObject);
            }
        }
    }

}
