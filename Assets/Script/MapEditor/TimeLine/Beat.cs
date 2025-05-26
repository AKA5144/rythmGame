using UnityEngine;

public class Beat : MonoBehaviour
{
    public float speed = 5f;
    public Transform indicator;            

    private Vector3 startLocalPos;

    void Start()
    {
        startLocalPos = transform.localPosition;
    }

    void Update()
    {
        // Scroll à droite
        transform.localPosition -= Vector3.right * speed * Time.deltaTime;

        // Si dépasse l’indicateur + marge
        if (transform.localPosition.x < indicator.localPosition.x)
        {
            transform.localPosition = startLocalPos;
        }
    }
}
