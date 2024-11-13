using System.Collections.Generic;
using UnityEngine;

public class SliderAccuracy : MonoBehaviour
{
    [SerializeField] private List<Sprite> AccSprite;

    public GameObject AccuracyGo;

    public GameObject circle;

    public GameObject OutlineCircle;

    public GameObject ObjectParent;

    private SpriteRenderer accuracyRenderer;


    private float initPosYAccuracy;
    void Start()
    {
        accuracyRenderer = AccuracyGo.GetComponent<SpriteRenderer>();
        initPosYAccuracy = AccuracyGo.transform.position.y;
        //  LoadImageAndApplyToSprite(imagePath);
        accuracyRenderer.sprite = AccSprite[3];
    }
    private void Update()
    {
    }
    public void checkAccuracy()
    {
        if (OutlineCircle.transform.localScale.x > 2)
        {
        }
        else if (OutlineCircle.transform.localScale.x < 2 && OutlineCircle.transform.localScale.x > 1.7)
        {
            InitiateDestroy(0);
        }
        else if (OutlineCircle.transform.localScale.x < 1.7 && OutlineCircle.transform.localScale.x > 1.5)
        {
            InitiateDestroy(1);
        }
        else if (OutlineCircle.transform.localScale.x < 1.5 && OutlineCircle.transform.localScale.x > 1.2)
        {
            InitiateDestroy(2);
        }
        else if (OutlineCircle.transform.localScale.x < 1.2 && OutlineCircle.transform.localScale.x > 0.9)
        {
            InitiateDestroy(3);
        }
        else if (OutlineCircle.transform.localScale.x < 0.9 && OutlineCircle.transform.localScale.x > 0.6)
        {
            InitiateDestroy(2);
        }
        else if (OutlineCircle.transform.localScale.x < 0.6 && OutlineCircle.transform.localScale.x > 0.4)
        {
            InitiateDestroy(1);
        }
        else if (OutlineCircle.transform.localScale.x < 0.4)
        {
            InitiateDestroy(0);
        }
    }
    private void InitiateDestroy(int index)
    {
            accuracyRenderer.sprite = AccSprite[index];
    }
 /*   private void AccuracyAnimation()
    {
        if (isClicked)
        {
            Vector3 pos = AccuracyGo.transform.position;
            pos.y += Time.deltaTime * 0.5f;
            AccuracyGo.transform.position = pos;
            if (pos.y > initPosYAccuracy + 0.5f)
            {
                DestroyGameObject();
            }
        }
    }*/

}
