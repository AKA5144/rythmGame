using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
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
    public int totalTravel;

    private int currentPointIndex = 0;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isClicked = false;
    private SpriteRenderer accuracyRenderer;
    private float initPosYAccuracy;
    private bool isAtEnd = false;
    private int currentTravel;
    public AudioSource audioSource;
    private bool SoundPlayed = false;
    private bool SoundExitPlayed = false;
    private int currentSegment = 0;
    private float progress = 0f;

    private int direction;


    void Start()
    {
        currentTravel = 1;
        if (currentTravel <= totalTravel)
        {
            lineRenderer.material.color = Color.yellow;
            if (currentTravel == totalTravel)
                lineRenderer.material.color = Color.green;
        }
        direction = 1;
        AccuracyGo.SetActive(false);
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
            if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.V) || Input.GetKey(KeyCode.N))
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
            Vector3 endPosition = lineRenderer.GetPosition(currentSegment + direction);

            progress += speed * Time.deltaTime / Vector3.Distance(startPosition, endPosition);

            CircleToMove.transform.position = Vector3.Lerp(startPosition, endPosition, progress);

            if (progress >= 1f)
            {
                progress = 0f;
                if (currentTravel <= totalTravel)
                {
                    lineRenderer.material.color = Color.yellow;
                    if (currentTravel == totalTravel)
                        lineRenderer.material.color = Color.green;
                    if (direction > 0)
                    {
                        currentSegment++;
                        if (currentSegment >= lineRenderer.positionCount - 1 && currentSegment < lineRenderer.positionCount)
                        {
                            direction = -1;
                            currentTravel++;
                        }
                    }
                    else if (direction < 0)
                    {
                        currentSegment--;
                        if (currentSegment <= 0 && currentSegment < lineRenderer.positionCount)
                        {
                            direction = 1;
                            currentTravel++;
                        }
                    }
                }
                else
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
            AccuracyGo.SetActive(true);
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
        switch (index)
        {
            case 0:
                ScoringManager.miss++;
                break;
            case 1:
                ScoringManager.Bad++;
                break;
            case 2:
                ScoringManager.Good++;
                break;
            case 3:
                ScoringManager.Perfect++;
                break;
        }
        if (index != 0 && !SoundPlayed)
        {
            audioSource.Play();
            SoundPlayed = true;
        }
        updateAccuracy(index);
        updateScore(index);
        isMoving = true;
        isClicked = true;
        accuracyRenderer.sprite = AccSprite[index];
    }
    private void updateAccuracy(int index)
    {
        ScoringManager.accuracy = (300 * ScoringManager.Perfect + 100 * ScoringManager.Good + 50 * ScoringManager.Bad);
        ScoringManager.accuracy = ScoringManager.accuracy / (300 * (ScoringManager.Perfect + ScoringManager.Good + ScoringManager.Bad + ScoringManager.miss));
        ScoringManager.accuracy = ScoringManager.accuracy / 0.01f;
        ScoringManager.accuracy = (float)Math.Round(ScoringManager.accuracy, 2);
    }
    private void updateScore(int index)
    {
        ScoringManager.score = (300 * ScoringManager.Perfect + 100 * ScoringManager.Good + 50 * ScoringManager.Bad);
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
