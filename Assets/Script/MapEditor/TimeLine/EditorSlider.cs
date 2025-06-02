using UnityEngine;

public class EditorSlider : MonoBehaviour
{
    public GameObject beginPos;
    public GameObject endPos;
    [SerializeField] RectTransform lineImage;

    public float timeCode;
    private void Update()
    {
        Vector3 start = beginPos.transform.localPosition;
        Vector3 end = endPos.transform.localPosition;

        Vector3 dir = (end - start).normalized;
        float distance = Vector3.Distance(start, end);

        // Position de la ligne au centre
        lineImage.localPosition = start + (end - start) * 0.5f;

        // Angle entre les deux points
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        lineImage.rotation = Quaternion.Euler(0, 0, angle);

        // Longueur de la ligne
        lineImage.sizeDelta = new Vector2(distance, lineImage.sizeDelta.y);
    }
}
