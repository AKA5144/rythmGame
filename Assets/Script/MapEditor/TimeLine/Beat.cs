using UnityEngine;

public class Beat : MonoBehaviour
{
    public float speed = 400f; // Vitesse du beat (ajuste selon besoin)
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        if (rectTransform == null)
        {
            Debug.LogError("❌ Le Beat n'a pas de RectTransform !");
        }
    }

    private void Update()
    {
        if (PlayerSongManager.Instance.IsPlaying()) // Vérifie si la musique joue
        {
            float moveAmount = speed * Time.deltaTime;
            rectTransform.anchoredPosition -= new Vector2(moveAmount, 0);
        }
    }
}
