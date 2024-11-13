using UnityEngine;

public class DebugOnCLicObjext : MonoBehaviour
{

    public GameObject parentObject;
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.N))
        {
            DetectClickedObject();
        }
    }

    void DetectClickedObject()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Utiliser RaycastAll pour d�tecter tous les objets sous le rayon.
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);

        // V�rifier si des objets ont �t� d�tect�s.
        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    // R�cup�rer l'objet cliqu� (le parent de l'objet sur lequel on clique)
                    GameObject clickedObject = hit.collider.gameObject.transform.parent.gameObject;

                    Debug.Log("GameObject cliqu� : " + clickedObject.name);

                    Transform mostRecentTransform = CheckHierarchie();

                    // V�rifier si l'objet est le plus vieux
                    if (clickedObject.transform == mostRecentTransform)
                    {
                        Destroy(mostRecentTransform.gameObject);
                        Debug.Log("L'objet cliqu� est le plus vieux");
                    }
                    else
                    {
                        Debug.Log("NOPE");
                    }
                }
            }
        }
    }

    Transform CheckHierarchie()
    {
        Transform[] children = new Transform[parentObject.transform.childCount];

        for (int i = 0; i < parentObject.transform.childCount; i++)
        {
            children[i] = parentObject.transform.GetChild(i);
        }

        if (children.Length > 0)
        {
            return children[0];
        }

        return null;
    }
}
