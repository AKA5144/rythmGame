using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Accuracy : MonoBehaviour
{
    [SerializeField] private List<Sprite> AccSprite;

    public GameObject AccuracyGo;

    public GameObject circle;

    public GameObject OutlineCircle;

    public FadingOutline fading;

    public bool isClicked;

    private SpriteRenderer accuracyRenderer;


    private float initPosYAccuracy;
    void Start()
    {
        accuracyRenderer = AccuracyGo.GetComponent<SpriteRenderer>();
        initPosYAccuracy = AccuracyGo.transform.position.y;
        //  LoadImageAndApplyToSprite(imagePath);
        //accuracyRenderer.sprite = AccSprite[3];
        isClicked = false;
    }

    private void Update()
    {
        AccuracyAnimation();
        if (OutlineCircle.transform.localScale.x < 0.4 && !isClicked)
        {
            InitiateDestroy(0);
        }
        if (OutlineCircle.transform.localScale.x < 1.3 && OutlineCircle.transform.localScale.x > 1)
        {
            Debug.Log(Time.time);
        }
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
        else if (OutlineCircle.transform.localScale.x < 1.5 && OutlineCircle.transform.localScale.x > 1.3)
        {
            InitiateDestroy(2);
        }
        else if (OutlineCircle.transform.localScale.x < 1.3 && OutlineCircle.transform.localScale.x > 1)
        {
            InitiateDestroy(3);
        }
        else if (OutlineCircle.transform.localScale.x < 1 && OutlineCircle.transform.localScale.x > 0.8)
        {
            InitiateDestroy(2);
        }
        else if (OutlineCircle.transform.localScale.x < 0.8 && OutlineCircle.transform.localScale.x > 0.7)
        {
            InitiateDestroy(1);
        }
        else if (OutlineCircle.transform.localScale.x < 0.7)
        {
            InitiateDestroy(0);
        }
    }
    private void InitiateDestroy(int index)
    {
        if (!isClicked)
        {
            circle.GetComponent<CircleCollider2D>().enabled = false;
            isClicked = true;
            accuracyRenderer.sprite = AccSprite[index];
        }
    }
    private void AccuracyAnimation()
    {
        if (isClicked)
        {
            fading.animExit();
            Vector3 pos = AccuracyGo.transform.position;
            pos.y += Time.deltaTime * 0.5f;
            AccuracyGo.transform.position = pos;
            if (pos.y > initPosYAccuracy + 0.5f)
            {
                DestroyGameObject();
            }
        }
    }
    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}