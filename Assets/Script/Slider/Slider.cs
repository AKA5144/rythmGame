using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slider : MonoBehaviour
{
    public Transform[] points; 
    public int segmentCount = 20; 
    private LineRenderer lineRenderer;
    public BeginSlider begin;
    
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = (points.Length - 1) * segmentCount + 1;
        if (points.Length >= 2)
        {
            Vector3[] curvePoints = GenerateCatmullRomCurve(points, segmentCount);
            lineRenderer.positionCount = curvePoints.Length;
            lineRenderer.SetPositions(curvePoints);
            begin.StartSlider();
        }
        else
        {
            Debug.LogWarning("Il faut au moins deux points pour créer une courbe.");
        }
    }

    void Update()
    {

    }

    Vector3[] GenerateCatmullRomCurve(Transform[] points, int segments)
    {
        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < points.Length - 1; i++)
        {
            //cheplus comment sa marche mais sa marche
            Vector3 p0 = (i == 0) ? points[i].position : points[i - 1].position;//pevious point
            Vector3 p1 = points[i].position;//current point
            Vector3 p2 = points[i + 1].position;//next point
            Vector3 p3 = (i + 2 < points.Length) ? points[i + 2].position : points[i + 1].position;

            //generate segment beetween point
            for (int j = 0; j <= segments; j++)
            {
                float t = j / (float)segments;
                positions.Add(CatmullRom(p0, p1, p2, p3, t));
            }
        }

        return positions.ToArray();
    }

    Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        //catmull roll calcul
        return 0.5f * ((2 * p1) +//current point
                       (-p0 + p2) * t +//next point
                       (2 * p0 - 5 * p1 + 4 * p2 - p3) * t2 +//squared
                       (-p0 + 3 * p1 - 3 * p2 + p3) * t3);//cubed
    }
}
