using UnityEngine;
using System.Collections.Generic;

public class MPTTuto : MonoBehaviour
{
    public List<GameObject> listOfPages;

    public int currentPage;
    public GameObject pageMarker;
    public List<Vector3> pageMarkerPositions;

    public void OnEnabled()
    {
        currentPage = 0;
        for (int pageId = 0; pageId < listOfPages.Count; ++pageId)
        {
            listOfPages[pageId].SetActive(false);
        }
        pageMarker.transform.position = pageMarkerPositions[currentPage];
    }

    public void PageClicked()
    {
        listOfPages[currentPage].GetComponent<Animator>().Play("ExitTuto");
        ++currentPage;
        pageMarker.transform.position = pageMarkerPositions[currentPage];

        if (currentPage >= listOfPages.Count)
        {
            currentPage = 0;
            gameObject.SetActive(false);
            MPTGameManager.Instance.Resume();
            MPTPlayer.Instance.SetTutoDone(true);
        }
        else
        {
            listOfPages[currentPage].SetActive(true);
        }
    }
}
