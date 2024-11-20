using UnityEngine;

public class MoveDebug : MonoBehaviour
{
    // Conversion des osu! pixels en unités Unity
    private const float OsuPixelToUnityX = 0.029296875f;
    private const float OsuPixelToUnityY = 0.0205729167f;

    // Distance maximale à déplacer en osu! pixels
    public int osuPixelDistance = 100;

    // Référence au SpriteRenderer pour afficher un cercle (assurez-vous d'attacher un cercle ici)
    public SpriteRenderer circleRenderer;

    void Start()
    {
        // Si un SpriteRenderer n'est pas attaché, en créer un
        if (circleRenderer == null)
        {
            circleRenderer = gameObject.AddComponent<SpriteRenderer>();
            circleRenderer.sprite = CreateCircleSprite(); // Créer un cercle au démarrage
        }
    }

    void Update()
    {
        // Vérifie si le bouton gauche de la souris est cliqué
        if (Input.GetMouseButtonDown(0))
        {
            // Obtenir la position du clic en unités du monde
            Vector3 mouseWorldPosition = GetMouseWorldPosition();

            // Calculer la direction du déplacement
            Vector3 direction = (mouseWorldPosition - transform.position).normalized;

            // Calculer la distance en unités Unity (équivalent à 100 osu! pixels)
            float moveX = osuPixelDistance * OsuPixelToUnityX;
            float moveY = osuPixelDistance * OsuPixelToUnityY;

            // Créer un vecteur de déplacement en unités Unity
            Vector3 moveDistance = new Vector3(moveX, moveY, 0);

            // Appliquer le déplacement dans la direction du clic
            transform.position += Vector3.Scale(direction, moveDistance);

            // Afficher dans la console pour vérifier
            Debug.Log($"Moved to {transform.position} based on mouse click.");
        }

        // Visualiser le cercle en mouvement
        DrawCircleAtPosition(transform.position);
    }

    // Méthode pour obtenir la position de la souris en unités du monde
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition; // Position en pixels d'écran
        mouseScreenPosition.z = Camera.main.nearClipPlane; // Distance à la caméra
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }

    // Méthode pour dessiner un cercle visuel à une position
    private void DrawCircleAtPosition(Vector3 position)
    {
        // On dessine un cercle à la position de l'objet (si le SpriteRenderer existe)
        if (circleRenderer != null)
        {
            circleRenderer.transform.position = position;
        }
    }

    // Méthode pour créer un sprite de cercle simple (si nécessaire)
    private Sprite CreateCircleSprite()
    {
        Texture2D texture = new Texture2D(50, 50);
        Color[] pixels = texture.GetPixels();

        // Créer un cercle simple sur la texture
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                float distance = Mathf.Sqrt(Mathf.Pow(x - texture.width / 2, 2) + Mathf.Pow(y - texture.height / 2, 2));
                if (distance < texture.width / 2)
                {
                    pixels[y * texture.width + x] = Color.white; // Le cercle sera blanc
                }
                else
                {
                    pixels[y * texture.width + x] = Color.clear; // Le reste est transparent
                }
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
