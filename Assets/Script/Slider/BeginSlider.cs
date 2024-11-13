using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeginSlider : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float speed = 5f;

    private int currentPointIndex = 0;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isClicked = false;
    public GameObject Outline;
    public GameObject OutlineCircle;
    [SerializeField] private List<Sprite> AccSprite;
    private SpriteRenderer accuracyRenderer;
    public GameObject AccuracyGo;
    private float initPosYAccuracy;
    void Start()
    {
        accuracyRenderer = AccuracyGo.GetComponent<SpriteRenderer>();
        initPosYAccuracy = AccuracyGo.transform.position.y;
    }

    public void StartSlider()
    {
        if (!isMoving && !isClicked)
        {
            transform.position = lineRenderer.GetPosition(0);
            currentPointIndex = 1;
            targetPosition = lineRenderer.GetPosition(currentPointIndex);

        }

    }

    void Update()
    {
        DetectClickOnCircle();
        if (isMoving && isClicked)
        {
            if (Input.GetMouseButton(0))
            {
                Debug.Log("good");
            }
            else
            {
                accuracyRenderer.sprite = AccSprite[0];
            }
        }
        if (isMoving && lineRenderer != null && lineRenderer.positionCount > 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                currentPointIndex++;
                if (currentPointIndex >= lineRenderer.positionCount)
                {
                    initPosYAccuracy = AccuracyGo.transform.position.y;
                    isMoving = false;
                }
                else
                {
                    targetPosition = lineRenderer.GetPosition(currentPointIndex);
                }
            }
        }
        if (Outline.transform.localScale.x < 0.4 && !isMoving && !isClicked)
        {
            InitiateDestroy(0);
        }

        if (!isMoving && isClicked)
        {

            AccuracyAnimation();
        }
    }


    private void DetectClickOnCircle()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.N))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject && !isClicked)
            {
                checkAccuracy();
            }
        }
        if (isMoving)
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {

            }
            else
            {

            }
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
        isMoving = true;
        isClicked = true;
        accuracyRenderer.sprite = AccSprite[index];
    }
    private void AccuracyAnimation()
    {
        if (isClicked)
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            Vector3 pos = AccuracyGo.transform.position;
            pos.y += Time.deltaTime * 0.5f;
            AccuracyGo.transform.position = pos;
            if (pos.y > initPosYAccuracy + 0.5f)
            {
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
    }
}
