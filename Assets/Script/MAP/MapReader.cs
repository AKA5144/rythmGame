using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

static public class MapReader
{
    // culture info used for maps
    static public CultureInfo cultureInfo = CultureInfo.GetCultureInfo("fr-FR");

    public enum ObjectType
    {
        Circle,
        Slider
    }
    public struct PositionData
    {
        public float timeCode;
        public Vector3 position;
        public ObjectType objectType;
        public List<Vector3> sliderPoints;
        public PositionData(float timeCode, Vector3 position, ObjectType objectType, List<Vector3> sliderPoints = null)
        {
            this.timeCode = timeCode;
            this.position = position;
            this.objectType = objectType;
            this.sliderPoints = sliderPoints ?? new List<Vector3>();
        }
    }

    static public Vector3 ParseCoords(string[] coords)
    {
        return new Vector2(float.Parse(coords[0].Trim(), cultureInfo), float.Parse(coords[1].Trim(), cultureInfo));
    }

    static public PositionData CreateCircle(string[] datas)
    {
        Vector3 position = ParseCoords(datas);
        float timeCode = float.Parse(datas[2].Trim(), cultureInfo);

        return new PositionData(timeCode * 0.001f, position, ObjectType.Circle, null);
    }

    static public PositionData CreateSlider(string[] datas)
    {
        Vector3 position = ParseCoords(datas);
        float timeCode = float.Parse(datas[2].Trim(), cultureInfo);

        List<Vector3> sliderPoints = new List<Vector3>();
        string pointsString = datas[3].Substring(2);
        string[] pointPairs = pointsString.Split('|');

        sliderPoints.Add(position);
        foreach (var pointPair in pointPairs)
        {
            string[] coords = pointPair.Split(':');

            if (coords.Length != 2)
            {
                Debug.LogWarning("Point mal formaté : " + pointPair);
                continue;
            }

            try
            {
                sliderPoints.Add(ParseCoords(coords));

                Debug.Log($"Point ajouté : ({sliderPoints.Last().x}, {sliderPoints.Last().y})");
            }
            catch (FormatException _)
            {
                Debug.LogError("Erreur lors du parsing du point " + pointPair);
            }
        }

        return new PositionData(timeCode * 0.001f, position, ObjectType.Slider, sliderPoints);
    }

    static public PositionData ParseStringValues(string[] datas)
    {
        ObjectType objectType = (datas.Length >= 4 && datas[3].Trim().StartsWith("Sl")) ? ObjectType.Slider : ObjectType.Circle;

        PositionData positionData;
        switch (objectType)
        {
            case ObjectType.Circle:
                positionData = CreateCircle(datas);
                break;
            case ObjectType.Slider:
                positionData = CreateSlider(datas);
                break;
            default:
                throw new Exception("Can't find type for object");
        }

        return positionData;
    }

    static public List<PositionData> ReadDataFromFile(string filePath)
    {
        List<PositionData> positionDatas = new List<PositionData>();

        if (!File.Exists(filePath))
        {
            throw new Exception("Fichier introuvable : \"" + filePath);
        }

        try
        {
            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                positionDatas.Add(ParseStringValues(lines[i].Split(';')));
            }

        }
        catch (Exception ex)
        {
            Debug.LogError("Erreur lors de la lecture du fichier : " + filePath + " | " + ex.Message);
        }

        return positionDatas;
    }
}
