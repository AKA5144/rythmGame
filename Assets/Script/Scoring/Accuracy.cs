using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System;

public class Accuracy : MonoBehaviour
{
    [SerializeField] private List<Sprite> AccSprite;

    public GameObject AccuracyGo;

    public GameObject circle;

    public GameObject OutlineCircle;

    public FadingOutline fading;

    public bool isClicked;

    public AudioSource audioSource;

    private SpriteRenderer accuracyRenderer;

    private bool SoundPlayed = false;

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
      //  AutoPlay();
    }

    public void AutoPlay()
    {

        if (OutlineCircle.transform.localScale.x < 1 && !SoundPlayed)
        {
            Debug.Log(Time.time - 2);
            audioSource.Play();
            InitiateDestroy(0);   
            SoundPlayed = true;
            accuracyRenderer.sprite = AccSprite[3];
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
            updateAccuracy(index);
            updateScore(index);
            circle.GetComponent<CircleCollider2D>().enabled = false;
            isClicked = true;
            accuracyRenderer.sprite = AccSprite[index];
        }
    }
    private void updateAccuracy(int index)
    {
        ScoringManager.accuracy = (300 * ScoringManager.Perfect + 100 * ScoringManager.Good + 50 * ScoringManager.Bad);
        ScoringManager.accuracy = ScoringManager.accuracy / (300 * (ScoringManager.Perfect + ScoringManager.Good + ScoringManager.Bad + ScoringManager.miss));
        ScoringManager.accuracy = ScoringManager.accuracy /0.01f;
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