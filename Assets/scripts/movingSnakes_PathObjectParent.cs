using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingSnakes_PathObjectParent : MonoBehaviour
{
    public movingSnakes_PathPoint[] commonPathPoint;

    [Header("Scale and Position Difference")]
    public float[] scales;
    public float[] positionDifference;
}
