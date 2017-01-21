/// <summary>
/// Contains all global and tweakable constants for the game.
/// </summary>
public class Globals {

    #region Units

    public static uint pixelsPerUnit = 32;

    #endregion

    #region Layers

    public static string mobLayerName = "Mob";
    public static string solidLayerName = "Solid";
    public static string navigationLayerName = "Navigation";

    #endregion

    #region Physics

    public static float gravity = 70f;
    public static float skinThickness = .05f;
    public static float groundFriction = 1.3f;
    public static float airFriction = 1.05f;

    #endregion

    #region Mob

    public static float mobMaxMoveSpeed = 12f;
    public static float mobAccelerationTime = .2f;
    public static float mobJumpHeight = 5.5f;
    public static float mobStunTime = 5f;
    public static float mobRecoveryTime = 1f;
    public static uint playerTeam = 1;

    #endregion

    #region AI

    public static float aiRetargetTimer = 2f;
    public static float aiThinkTimer = 1f;
    public static float aiClosestTargetTimer = 1f;
    public static float aiWanderTimer = 3f;
    public static float aiWanderMovementTime = .3f;
    public static float aiPathingTimer = 1f;
    public static float aiAttackRadius = 6f;
    public static float aiCaptureRadius = 3.5f;
    public static float enemyGoalImportanceRatio = 1.5f;
    public static float aiStunnedEnemyPenalty = 10f;

    #endregion

    #region Navigation

    public static float maxNodeDistance = 6.5f;
    public static float minNodeContactDistance = .8f;

    #endregion

    #region Goals

    public static float goalCaptureAmountPerMob = .03f;
    public static float respawnTime = 10f;

    #endregion

    #region UI

    public static float floatingTextTime = 3f;

    #endregion

}
