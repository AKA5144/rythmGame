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

    public GameObject ObjectParent;

    public CheckHierarchie hierarchie;

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
        // checkAccuracy();
        DetectClickOnCircle();
        AccuracyAnimation();
        if (OutlineCircle.transform.localScale.x < 0.4 && !isClicked)
        {
            InitiateDestroy(0);
        }
    }
    private void DetectClickOnCircle()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("hit");
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);
            if (hits[0].collider != null && hits[0].collider.gameObject == circle)
            {
                checkAccuracy();
            }
            /* if (hits.Length == 1)
             {
                 if (hits[0].collider != null && hits[0].collider.gameObject == circle)
                 {
                     checkAccuracy();
                 }
             }
             else if (hits.Length > 1)
             {
                 if (IsOldestGameObject(hits))
                 {
                     checkAccuracy();
                 }

             }*/

        }
    }
    bool IsOldestGameObject(RaycastHit2D[] hits)
    {
        GameObject oldestObject = null;
        int oldestIndex = int.MaxValue;

        foreach (RaycastHit2D hit in hits)
        {
            GameObject obj = hit.collider.gameObject.transform.parent.gameObject;
            int siblingIndex = obj.transform.GetSiblingIndex();

            if (siblingIndex < oldestIndex)
            {
                oldestIndex = siblingIndex;
                oldestObject = obj;
            }
        }
        if (oldestObject == gameObject)
        {
            return true;
        }
        return false;
    }
    private void checkAccuracy()
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
