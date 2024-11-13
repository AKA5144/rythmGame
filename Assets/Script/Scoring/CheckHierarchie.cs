using UnityEngine;

public class CheckHierarchie : MonoBehaviour
{
    GameObject parent;
    void Start()
    {
        parent = gameObject.transform.parent.gameObject;
    }

    public bool Check(RaycastHit2D hit)
    {
        if (hit.collider != null)
        {

            // Récupérer l'objet cliqué (le parent de l'objet sur lequel on clique)
            GameObject clickedObject = hit.collider.gameObject.transform.parent.gameObject;
            Debug.Log(clickedObject);
            // Debug.Log("GameObject cliqué : " + clickedObject);
            if (clickedObject == gameObject)
            {
                // Vérifier si l'objet est le plus vieux
                if (CheckIsOldest(gameObject))
                {
                    return true;
                }
                return false;
            }
        }
        return false;
    }

    public bool CheckIsOldest(GameObject go)
    {

        Transform[] children = new Transform[parent.transform.childCount];

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            children[i] = parent.transform.GetChild(i);
        }
        if (children.Length > 0)
        {
            if (children[0] == go.transform)
            {
                SetLastsibling();
                return true;
            }
        }
        return false;
    }

    public void SetLastsibling()
    {
        gameObject.transform.SetAsLastSibling();
    }
}
