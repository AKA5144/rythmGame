using UnityEngine;

public class EditorCircle : MonoBehaviour
{
    public CircleData circleData;

    public bool circleSpawned = false;

    public float timeCode
    {
        get => circleData.timeCode;
        set => circleData.timeCode = value;
    }

    public Vector2 position
    {
        get => circleData.position;
        set => circleData.position = value;
    }
}
