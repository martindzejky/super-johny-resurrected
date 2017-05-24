/// <summary>
/// Contains all global and tweakable constants for the game.
/// </summary>
public class Globals {

    #region Debug

    public static bool debugging = false;

    #endregion

    #region Camera

    public static float cameraAfterDeathTimeout = 3f;

    #endregion

    #region Units

    public static uint pixelsPerUnit = 16;
    public static float cameraAdjust = .5f;

    #endregion

    #region Layers

    public static string mobLayerName = "Mob";
    public static string solidLayerName = "Solid";
    public static string navigationLayerName = "Navigation";

    #endregion

    #region Physics

    public static float gravity = 50f;
    public static float skinThickness = .05f;
    public static float groundFriction = 1.3f;
    public static float airFriction = 1.06f;
    public static float destroyLevel = -1000f;

    #endregion

    #region Effects

    public static uint starParticleCount = 12;

    #endregion

    #region Mob

    public static float mobMaxMoveSpeed = 10f;
    public static float mobAccelerationTime = .15f;
    public static float mobJumpHeight = 5.4f;
    public static float mobStunTimeMax = 2f;
    public static float mobStunTimeMin = 4f;
    public static float mobRecoveryTime = 1f;
    public static float eyeFollowSpeed = 5f;
    public static float maxEyeXOffset = .3f;
    public static float maxEyeYOffset = .1f;
    public static float happyTime = 1.5f;

    #endregion

    #region Team

    public static float respawnTimePerAliveMob = 1.2f;
    public static float respawnTimePerCapturedFlag = -.3f;
    public static float minimumRespawnTime = 1.5f;
    public static float startingRespawnTime = 4f;
    public static uint respawnsPerTeam = 100;

    #endregion

    #region Player

    public static uint playerTeam = 1;
    public static float playerIdleEyesTime = 2f;
    public static float playerRespawnTime = 6f;

    #endregion

    #region AI

    public static float aiRetargetTimer = 2f;
    public static float aiThinkTimer = .5f;
    public static float aiClosestTargetTimer = .5f;
    public static float aiWanderTimer = 8f;
    public static float aiWanderLookTimer = 4f;
    public static float aiPathingTimer = 1f;

    #endregion

    #region Navigation

    public static float maxNodeDistance = 6.5f;
    public static float minNodeContactDistance = 1.5f;

    #endregion

    #region Goals

    public static float goalCaptureAmountPerMob = .05f;

    #endregion

    #region UI

    public static float floatingTextTime = 3f;

    #endregion

}
