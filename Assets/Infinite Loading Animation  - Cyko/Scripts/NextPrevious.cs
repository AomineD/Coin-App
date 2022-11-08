 using UnityEngine;
 
 public class NextPrevious : MonoBehaviour
{

    public GameObject[] prefabs;
    public Transform parent;
    int i = 0;

    public void BtnNext()
    {
        if (i + 1 < prefabs.Length)
        {
            Destroy(parent.GetChild(0).gameObject);
            i++;
            GameObject g = Instantiate(prefabs[i], prefabs[i].transform.position, prefabs[i].transform.rotation);
            g.transform.SetParent(parent, false);
            g.transform.SetAsFirstSibling();
        }
    }

    public void BtnPrev()
    {
        if (i - 1 > 0)
        {
            Destroy(parent.GetChild(0).gameObject);
            i--;
            GameObject g = Instantiate(prefabs[i], prefabs[i].transform.position, prefabs[i].transform.rotation);
            g.transform.SetParent(parent, false);
            g.transform.SetAsFirstSibling();
        }
    }
}