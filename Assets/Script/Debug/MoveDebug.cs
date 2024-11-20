using UnityEngine;

public class MoveDebug : MonoBehaviour
{
    // Conversion des osu! pixels en unit�s Unity
    private const float OsuPixelToUnityX = 0.029296875f;
    private const float OsuPixelToUnityY = 0.0205729167f;

    // Distance maximale � d�placer en osu! pixels
    public int osuPixelDistance = 100;

    // R�f�rence au SpriteRenderer pour afficher un cercle (assurez-vous d'attacher un cercle ici)
    public SpriteRenderer circleRenderer;

    void Start()
    {
        // Si un SpriteRenderer n'est pas attach�, en cr�er un
        if (circleRenderer == null)
        {
            circleRenderer = gameObject.AddComponent<SpriteRenderer>();
            circleRenderer.sprite = CreateCircleSprite(); // Cr�er un cercle au d�marrage
        }
    }

    void Update()
    {
        // V�rifie si le bouton gauche de la souris est cliqu�
        if (Input.GetMouseButtonDown(0))
        {
            // Obtenir la position du clic en unit�s du monde
            Vector3 mouseWorldPosition = GetMouseWorldPosition();

            // Calculer la direction du d�placement
            Vector3 direction = (mouseWorldPosition - transform.position).normalized;

            // Calculer la distance en unit�s Unity (�quivalent � 100 osu! pixels)
            float moveX = osuPixelDistance * OsuPixelToUnityX;
            float moveY = osuPixelDistance * OsuPixelToUnityY;

            // Cr�er un vecteur de d�placement en unit�s Unity
            Vector3 moveDistance = new Vector3(moveX, moveY, 0);

            // Appliquer le d�placement dans la direction du clic
            transform.position += Vector3.Scale(direction, moveDistance);

            // Afficher dans la console pour v�rifier
            Debug.Log($"Moved to {transform.position} based on mouse click.");
        }

        // Visualiser le cercle en mouvement
        DrawCircleAtPosition(transform.position);
    }

    // M�thode pour obtenir la position de la souris en unit�s du monde
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition; // Position en pixels d'�cran
        mouseScreenPosition.z = Camera.main.nearClipPlane; // Distance � la cam�ra
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }

    // M�thode pour dessiner un cercle visuel � une position
    private void DrawCircleAtPosition(Vector3 position)
    {
        // On dessine un cercle � la position de l'objet (si le SpriteRenderer existe)
        if (circleRenderer != null)
        {
            circleRenderer.transform.position = position;
        }
    }

    // M�thode pour cr�er un sprite de cercle simple (si n�cessaire)
    private Sprite CreateCircleSprite()
    {
        Texture2D texture = new Texture2D(50, 50);
        Color[] pixels = texture.GetPixels();

        // Cr�er un cercle simple sur la texture
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
