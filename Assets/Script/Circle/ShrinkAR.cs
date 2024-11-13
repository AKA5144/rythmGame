using UnityEngine;

public class ShrinkAR : MonoBehaviour
{
    [SerializeField] Vector3 BeginSize;
    [SerializeField] Transform Outline;
    [SerializeField] GameObject Circle;
    [SerializeField] SpriteRenderer CircleRenderer;
    [SerializeField] float AR;
    private Vector3 shrink;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Outline.position = Circle.transform.position;
        Outline.localScale = BeginSize;
        BeginSize.x = transform.localScale.x * 2.5f;
        BeginSize.y = transform.localScale.y * 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        Outline.position = Circle.transform.position;
        if (Outline != null)
        {
            shrink = Outline.localScale;
        }
        if (shrink.x + shrink.y <= 0)
        {

            //Destroy(Outline.gameObject);
           // FadeOutCircle();
        }
        else
        {

            shrink.x -= AR * Time.deltaTime * 0.5f;
            shrink.y -= AR * Time.deltaTime * 0.5f;
            shrink.z -= 0;
            Outline.localScale = shrink;
        }

    }

    void FadeOutCircle()
    {

        Color temp = CircleRenderer.color;
        if (temp.a > 0)
        {

            temp.a = CircleRenderer.color.a - Time.deltaTime * AR * 1.5f;
            CircleRenderer.color = temp;
        }
        else
        {
         //    Destroy(gameObject);
        }
    }
}
