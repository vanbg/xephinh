using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public static Action<bool> GameOver;
    public static Action<int> AddScores;// tinh diem 

    public static Action CheckIfShapeCanBePlaced; // kiem tra hanh dong cua hinh
    public static Action MoveShapeToStartPosition;// hinh dang di chuyen ve vi tri bat dau
    public static Action RequestNewShapes; // yeu cau hinh dang moi
    public static Action SetShapeInactive;// dat hinh dang ko hoat dong

    public static Action<int, int> UpdateBestScoreBar;

}
