using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    public const int HEIGHT = 64;
    public const int WIDTH = 128;
    public const float SIZE_SCALE = 4f;

    public const float PLAYER_SPEED = 30f;
    public const float BIGCAT_SPEED = 30f;

    public const float SMALLCAT_SPEED_MIN = 24f;
    public const float SMALLCAT_SPEED_MAX = 30.5f;

    public const float RESTART_TIME = 3f; // for debug. this will not be used in the finished game

    public const int PLAYER_LAYER = 1;
    public const int CAMERA_LAYER = -10;

    public const float SCAT_LAYER = 2f;

    public const int MAIN_SCENE = 0;
    public const int MENU_SCENE = 1;
}
