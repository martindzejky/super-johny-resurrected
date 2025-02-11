﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>
/// Handles the logic of a leftover. This can be for example a dead body or a lost heart.
/// Waits a little and then desaturates and burries the leftover. Or just destroys it on contact.
/// </summary>
public class Leftover : MonoBehaviour {

    public enum State {

        Falling,
        Burrying,
        Burried

    }


    /// <summary>If true, initialize the object to a random starting velocity.</summary>
    public bool initVelocity = true;

    /// <summary>If true, rotate the leftover while it is in the air.</summary>
    public bool rotate = true;

    /// <summary>If true, bury the leftover.</summary>
    public bool bury;

    private float rotation;
    private State state = State.Falling;
    private PhysicsObject physicsObject;
    private SpriteRenderer sprite;

    public void Awake() {
        physicsObject = GetComponent<PhysicsObject>();
        sprite = GetComponent<SpriteRenderer>();

        // initialize rotation and velocity
        rotation = rotate ? Random.Range(2f, 10f) * (Random.value < .5f ? 1 : -1) : 0f;
        if (initVelocity) {
            physicsObject.velocity = new Vector2(Random.Range(-6f, 6f), Random.Range(10f, 18f));
        }
    }

    public void Update() {
        switch (state) {
            case State.Falling:
                RotateAndDetectCollision();
                break;

            case State.Burrying:
                Bury();
                break;

            case State.Burried:
                break;
        }
    }

    private void RotateAndDetectCollision() {
        if (physicsObject && physicsObject.IsGrounded) {
            if (bury) {
                state = State.Burrying;
                Destroy(physicsObject);
                physicsObject = null;
            }
            else {
                Destroy(gameObject);
            }
        }
        else {
            transform.Rotate(Vector3.forward, rotation);
        }
    }

    private void Bury() {
        const float delta = .002f;
        transform.Translate(Vector2.down * .15f * Time.deltaTime, Space.World);
        sprite.color = new Color(sprite.color.r - delta, sprite.color.g - delta, sprite.color.b - delta);

        if (sprite.color.maxColorComponent < .5f) {
            state = State.Burried;
        }
    }

}
