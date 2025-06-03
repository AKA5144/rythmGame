using UnityEngine;

public class ShrinkingEditorCIrcle : MonoBehaviour
{
    [SerializeField] private Transform outlineTransform;
    public EditorCircle circle;

    public void SetOutlineScale(float scale)
    {
        if (outlineTransform == null)
        {
            Debug.LogError("Outline Transform not assigned!");
            return;
        }

        outlineTransform.localScale = new Vector3(scale, scale, scale);
    }
}
