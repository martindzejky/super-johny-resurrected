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

    #endregion

    #region Physics

    public static float gravity = 70f;
    public static float skinThickness = .05f;
    public static float groundFriction = 1.3f;
    public static float airFriction = 1.02f;

    #endregion

    #region Mob

    public static float mobMaxMoveSpeed = 12f;
    public static float mobAccelerationTime = .2f;
    public static float mobJumpHeight = 5.5f;
    public static float mobStunTime = 2f;
    public static float mobRecoveryTime = 1f;

    #endregion

}