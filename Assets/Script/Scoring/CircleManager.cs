using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class CircleManager : MonoBehaviour
{

    private GameObject OutlineCircle;
    private float initPosYAccuracy;
    void Start()
    {

    }

    private void Update()
    {
        DetectClickOnCircle();
    }
    private void DetectClickOnCircle()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.N))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);
            if (hits.Length == 1)
            {
                if (hits[0].collider != null && hits[0].collider.tag == "Circle")
                {
                    Accuracy circleScript = hits[0].collider.gameObject.transform.parent.gameObject.GetComponent<Accuracy>();
                    if (circleScript != null)
                    {
                        circleScript.checkAccuracy();
                    }
                }
                else if (hits[0].collider != null && hits[0].collider.tag == "SliderCircle")
                {
                    BeginSlider circleScript = hits[0].collider.gameObject.transform.parent.gameObject.GetComponent<BeginSlider>();
                    if (circleScript != null)
                    {
                        circleScript.checkAccuracy();
                    }
                }
            }
            else if (hits.Length > 1)
            {
                GameObject circleToDestroy = GetOldestGameObject(hits);
                if (circleToDestroy.tag == "SliderCircle")
                {
                    BeginSlider circleScript = circleToDestroy.GetComponent<BeginSlider>();

                    if (circleScript != null)
                    {
                        circleScript.checkAccuracy();
                    }
                }
                else
                {
                    Accuracy circleScript = circleToDestroy.GetComponent<Accuracy>();

                    if (circleScript != null)
                    {
                        circleScript.checkAccuracy();
                    }
                }

            }

        }
    }
    GameObject GetOldestGameObject(RaycastHit2D[] hits)
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
        Debug.Log(oldestObject);

        return oldestObject;
    }

}
