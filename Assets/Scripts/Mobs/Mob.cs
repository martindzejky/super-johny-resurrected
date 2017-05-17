using UnityEngine;


/// <summary>
/// Defines mob behaviour, physics, and stats. Uses the physics
/// object and enables other components to control the mob.
/// </summary>
public class Mob : MonoBehaviour {

    /// <summary>The team number of the mob.</summary>
    public uint team;

    /// <summary>When lives drop to 0, this mob is destroyed and a body leftover is spawned.</summary>
    public uint lives = 1;

    /// <summary>If true, set the color of this mob's team to this mob's color.</summary>
    public bool setTeamColor = true;

    /// <summary>The world-space position where the eyes should look.</summary>
    public Vector3 eyeTarget;

    /// <summary>Controls the animation of the eyes.</summary>
    public MobEmotion emotion = MobEmotion.Normal;

    private PhysicsObject physicsObject;
    private Collider2D myCollider;
    private SpriteRenderer spriteRenderer;

    private float horizontalInput;
    private bool jumpInput;
    private bool stunned;
    private bool recovering;
    private float stunTimer;
    private float stunRecoveryTimer;
    private float stunTime;
    private MobEmotion previousEmotion = MobEmotion.None;
    private float happyTimer;

    public bool IsStunned() {
        return stunned;
    }

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
        spriteRenderer = GetComponent<SpriteRenderer>();

        var mobTeam = MobTeams.GetTeam(team);
        mobTeam.Mobs.Add(this);
        if (setTeamColor) {
            mobTeam.teamColor = spriteRenderer.color;
        }

        // randomize the color a little
        var newColor = spriteRenderer.color;
        const float variance = .04f;
        newColor.r += Random.Range(-variance, variance);
        newColor.g += Random.Range(-variance, variance);
        newColor.b += Random.Range(-variance, variance);
        spriteRenderer.color = newColor;

        stunTime = Random.Range(Globals.mobStunTimeMin, Globals.mobStunTimeMax);

        eyeTarget = transform.position;

        SetRandomBody();
    }

    public void Update() {
        if (recovering) {
            UpdateRecoveryTimer();
            AdjustVelocityByInput();
        }
        else if (stunned) {
            UpdateStunTimer();
        }
        else {
            AdjustVelocityByInput();
            CheckHeadStomping();
        }

        UpdateHappy();
    }

    public void OnDestroy() {
        MobTeams.GetTeam(team).Mobs.Remove(this);
    }

    public void MakeHappy() {
        happyTimer = Globals.happyTime;
    }

    private float CalculateVelocityForJumpHeight(float height) {
        return Mathf.Sqrt(2f * height * Globals.gravity);
    }

    private void UpdateRecoveryTimer() {
        if (previousEmotion == MobEmotion.None) {
            previousEmotion = emotion;
        }
        emotion = MobEmotion.Shocked;

        stunRecoveryTimer += Time.deltaTime;
        if (stunRecoveryTimer > Globals.mobRecoveryTime) {
            recovering = false;
            stunRecoveryTimer = 0f;

            if (previousEmotion != MobEmotion.None) {
                emotion = previousEmotion;
                previousEmotion = MobEmotion.None;
            }
        }
    }

    private void UpdateStunTimer() {
        stunTimer += Time.deltaTime;
        horizontalInput = 0f;
        jumpInput = false;
        physicsObject.applyGroundFriction = true;
        physicsObject.applyAirFriction = true;

        if (stunTimer > stunTime) {
            stunned = false;
            recovering = true;
            stunTimer = 0f;
        }
    }

    private void UpdateHappy() {
        if (happyTimer > 0f) {
            if (previousEmotion == MobEmotion.None) {
                previousEmotion = emotion;
            }
            emotion = MobEmotion.Happy;

            happyTimer -= Time.deltaTime;

            if (happyTimer <= 0f && previousEmotion != MobEmotion.None) {
                emotion = previousEmotion;
                previousEmotion = MobEmotion.None;
            }
        }
    }

    private void AdjustVelocityByInput() {
        if (!Mathf.Approximately(0f, horizontalInput)) {
            var acceleration = horizontalInput * Globals.mobMaxMoveSpeed / Globals.mobAccelerationTime * Time.deltaTime;

            if ((int) Mathf.Sign(physicsObject.velocity.x) == (int) Mathf.Sign(horizontalInput)) {
                physicsObject.velocity.x += acceleration;
            }
            else {
                physicsObject.velocity.x += acceleration * 2;
            }

            physicsObject.velocity.x = Mathf.Clamp(physicsObject.velocity.x, -Globals.mobMaxMoveSpeed,
                Globals.mobMaxMoveSpeed);
            physicsObject.applyGroundFriction = false;
            physicsObject.applyAirFriction = false;

            // var newScale = transform.localScale;
            // newScale.x = horizontalInput;
            // transform.localScale = newScale;
        }
        else {
            physicsObject.applyGroundFriction = true;
            physicsObject.applyAirFriction = true;
        }

        if (jumpInput && physicsObject.velocity.y < Mathf.Epsilon &&
            (physicsObject.IsGrounded || physicsObject.TimeInAir < .2f)) {
            physicsObject.velocity.y = CalculateVelocityForJumpHeight(Globals.mobJumpHeight);
        }

        jumpInput = false;
        horizontalInput = 0;
    }

    private void CheckHeadStomping() {
        var boxcastPosition = new Vector2(transform.position.x,
            transform.position.y + myCollider.bounds.extents.y / 2f);
        var boxcastSize = physicsObject.size;
        boxcastSize.x -= Globals.skinThickness * 2f;
        boxcastSize.y -= Globals.skinThickness * 2f;

        // TODO: Use only OverlapBox
        var colliders = Physics2D.OverlapBoxAll(boxcastPosition, boxcastSize, 0,
            LayerMask.GetMask(Globals.mobLayerName));
        foreach (var col in colliders) {
            if (col == myCollider) {
                continue;
            }

            var otherMob = col.GetComponent<Mob>();
            if (!otherMob) {
                continue;
            }

            var otherTransform = col.transform;
            var otherPhysicsObject = col.GetComponent<PhysicsObject>();

            // stunned mobs can't stomp
            if (otherMob.IsStunned()) {
                continue;
            }

            if (team == otherMob.team && team != 0) {
                continue;
            }

            if (otherPhysicsObject.velocity.y < 0 &&
                otherTransform.position.y - col.bounds.extents.y > transform.position.y &&
                otherTransform.position.x - col.bounds.extents.x < transform.position.x +
                myCollider.bounds.extents.x - Globals.skinThickness * 2f &&
                otherTransform.position.x + col.bounds.extents.x > transform.position.x -
                myCollider.bounds.extents.x + Globals.skinThickness * 2f) {
                // apply positive velocity to the other mob according to the formula:
                // new Y velocity = maximum(standard jump velocity / 2, mob's current positive velocity * .7 + my velocity * .4)
                otherPhysicsObject.velocity.y = Mathf.Max(CalculateVelocityForJumpHeight(Globals.mobJumpHeight / 2f),
                    -otherPhysicsObject.velocity.y * .7f + physicsObject.velocity.y * .4f);

                lives--;

                if (physicsObject.velocity.y > Mathf.Epsilon) {
                    physicsObject.velocity.x /= 1.5f;
                    physicsObject.velocity.y *= -1;
                }
                else {
                    var xRange = lives == 0 ? 4f : 2f;
                    var yRange = 2f;
                    var yHeight = lives == 0 ? 1.5f : .5f;

                    physicsObject.velocity.x = Random.Range(-xRange, xRange);
                    physicsObject.velocity.y = CalculateVelocityForJumpHeight(yHeight) + Random.Range(-yRange, yRange);
                }

                var prefabRegistry = FindObjectOfType<PrefabRegistry>();

                // add score to the enemy
                MobTeams.GetTeam(otherMob.team).score += 10;
                var scoreText = Instantiate(prefabRegistry.floatingText, transform.position, transform.rotation);
                scoreText.GetComponent<FloatingText>().SetText("+10");

                if (lives == 0) {
                    var body = Instantiate(prefabRegistry.deadBody, transform.position, transform.rotation);
                    body.GetComponent<SpriteRenderer>().color = spriteRenderer.color;
                    body.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
                    body.GetComponent<PhysicsObject>().velocity = physicsObject.velocity;

                    Destroy(gameObject);

                    // add more score to the enemy for killing (10 already added)
                    MobTeams.GetTeam(otherMob.team).score += 10;
                    scoreText.GetComponent<FloatingText>().SetText("+20");
                }
                else {
                    stunned = true;
                    Instantiate(prefabRegistry.lostHeart, new Vector3(transform.position.x,
                        transform.position.y + myCollider.bounds.extents.y,
                        transform.position.z), transform.rotation);
                }

                for (var i = 0; i < Globals.starParticleCount; i++) {
                    Instantiate(prefabRegistry.starParticle, new Vector3(transform.position.x,
                        transform.position.y + myCollider.bounds.extents.y,
                        transform.position.z), transform.rotation);
                }
            }
        }
    }

    private void SetRandomBody() {
        var spriteRegistry = FindObjectOfType<SpriteRegistry>();
        spriteRenderer.sprite = spriteRegistry.mobBodies[Random.Range(0, spriteRegistry.mobBodies.Length)];
    }

}
