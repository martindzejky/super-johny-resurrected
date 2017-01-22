﻿/// <summary>
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
    public static float destroyLevel = -1000f;

    #endregion

    #region Mob

    public static float mobMaxMoveSpeed = 12f;
    public static float mobAccelerationTime = .2f;
    public static float mobJumpHeight = 5.5f;
    public static float mobStunTimeMax = 2f;
    public static float mobStunTimeMin = 4f;
    public static float mobRecoveryTime = 1f;
    public static uint playerTeam = 1;
    public static float eyeFollowSpeed = 10f;
    public static float maxEyeXOffset = .3f;
    public static float maxEyeYOffset = .1f;
    public static float happyTime = 1.5f;

    #endregion

    #region AI

    public static float aiRetargetTimer = 2f;
    public static float aiThinkTimer = .5f;
    public static float aiClosestTargetTimer = .5f;
    public static float aiWanderTimer = 3f;
    public static float aiWanderMovementTime = .8f;
    public static float aiPathingTimer = 1f;

    #endregion

    #region Navigation

    public static float maxNodeDistance = 6.5f;
    public static float minNodeContactDistance = 1.5f;

    #endregion

    #region Goals

    public static float goalCaptureAmountPerMob = .03f;
    public static float respawnTime = 5f;

    #endregion

    #region UI

    public static float floatingTextTime = 3f;

    #endregion

}
