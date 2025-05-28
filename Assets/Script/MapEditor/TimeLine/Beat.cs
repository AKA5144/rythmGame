using UnityEngine;

public class Beat : MonoBehaviour
{
    public float speed = 5f;

    public Vector3 startPos;


    public void Move()
    {
        transform.localPosition -= Vector3.right * speed * Time.deltaTime;
    }
    public bool BeatComplete(Transform indicator)
    {
        return transform.localPosition.x <= indicator.localPosition.x;
    }
}
