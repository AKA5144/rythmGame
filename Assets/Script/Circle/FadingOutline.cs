using Unity.VisualScripting;
using UnityEngine;

public class FadingOutline : MonoBehaviour
{
    Color temp;
    public float speed;
    public float fadeSpeed;

    public SpriteRenderer circle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      /*   temp = spriteRenderer.color;
        temp.a = 0f;
        spriteRenderer.color = temp;*/
    }

    // Update is called once per frame
    public void animExit()
    {
        Vector3 scale = gameObject.transform.localScale;
        scale.x += Time.deltaTime * speed;
        scale.y += Time.deltaTime * speed;
        gameObject.transform.localScale = scale;

        Color temp = circle.color;
        temp.a -= Time.deltaTime * fadeSpeed; 
        circle.color = temp;

    }
}
