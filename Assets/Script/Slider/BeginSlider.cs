using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeginSlider : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float speed = 5f;
    public GameObject CircleToMove;
    public GameObject Outline;
    public GameObject OutlineCircle;
    public FadingOutline fading;
    [SerializeField] private List<Sprite> AccSprite;
    public GameObject AccuracyGo;

    private int currentPointIndex = 0;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isClicked = false;
    private SpriteRenderer accuracyRenderer;
    private float initPosYAccuracy;
    private bool isAtEnd = false;

    private int currentSegment = 0;
    private float progress = 0f;


    void Start()
    {
        accuracyRenderer = AccuracyGo.GetComponent<SpriteRenderer>();
        initPosYAccuracy = AccuracyGo.transform.position.y;
    }

    public void StartSlider()
    {
        CircleToMove.transform.position = lineRenderer.GetPosition(0);
        currentPointIndex = 1;
        targetPosition = lineRenderer.GetPosition(currentPointIndex);

    }

    void Update()
    {
        if (isMoving && isClicked)
        {
            if (Input.GetMouseButton(0))
            {
                //  Debug.Log("good");
            }
            else
            {
                accuracyRenderer.sprite = AccSprite[0];
            }
        }
        if (lineRenderer != null && lineRenderer.positionCount > 1 && CircleToMove != null && !isAtEnd && isClicked)
        {
            Vector3 startPosition = lineRenderer.GetPosition(currentSegment);
            Vector3 endPosition = lineRenderer.GetPosition(currentSegment + 1);

            progress += speed * Time.deltaTime / Vector3.Distance(startPosition, endPosition);

            CircleToMove.transform.position = Vector3.Lerp(startPosition, endPosition, progress);

            if (progress >= 1f)
            {
                progress = 0f;
                currentSegment++;

                if (currentSegment >= lineRenderer.positionCount - 1)
                {
                    initPosYAccuracy = CircleToMove.transform.position.y;
                    Vector3 temp = CircleToMove.transform.position;
                    temp.y = temp.y + CircleToMove.GetComponent<SpriteRenderer>().bounds.size.y / 2;
                    AccuracyGo.transform.position = temp;
                    isAtEnd = true;
                }
            }
        }

        if (isAtEnd)
        {
            isMoving = false;
            AccuracyAnimation();
        }
        if (Outline.transform.localScale.x < 0.4 && !isMoving && !isClicked)
        {
            InitiateDestroy(0);
        }
    }

    public void checkAccuracy()
    {
        if (!isClicked)
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
    }
    private void InitiateDestroy(int index)
    {
        isMoving = true;
        isClicked = true;
        accuracyRenderer.sprite = AccSprite[index];
    }
    private void AccuracyAnimation()
    {
        if (isClicked)
        {
            fading.animExit();
            CircleToMove.GetComponent<CircleCollider2D>().enabled = false;
            Vector3 pos = AccuracyGo.transform.position;
            pos.y += Time.deltaTime * 1f;
            AccuracyGo.transform.position = pos;
            if (pos.y > initPosYAccuracy + 1.5f)
            {
                Destroy(gameObject);
            }
        }
    }
}
