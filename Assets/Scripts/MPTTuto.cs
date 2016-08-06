using UnityEngine;
using System.Collections.Generic;

public class MPTTuto : MonoBehaviour
{
    public List<GameObject> listOfPages;

    public int currentPage;
    public GameObject pageMarker;
    public List<Transform> pageMarkerPositions;

    public void OnEnable()
    {
        currentPage = 0;
        listOfPages[0].SetActive(true);
        listOfPages[currentPage].GetComponent<Animator>().Play("StayTuto");
        for (int pageId = 1; pageId < listOfPages.Count; ++pageId)
        {
            listOfPages[pageId].SetActive(false);
        }
        pageMarker.transform.position = pageMarkerPositions[currentPage].position + Vector3.back;
    }

    public void PageClicked()
    {
        listOfPages[currentPage].GetComponent<Animator>().Play("ExitTuto");
        ++currentPage;

        if (currentPage >= listOfPages.Count)
        {
            currentPage = 0;
            gameObject.SetActive(false);
            MPTGameManager.Instance.Resume();
            MPTPlayer.Instance.SetTutoDone(true);
        }
        else
        {
            pageMarker.transform.position = pageMarkerPositions[currentPage].position + Vector3.back;
            listOfPages[currentPage].SetActive(true);
            listOfPages[currentPage].GetComponent<Animator>().Play("EnterTuto");
        }
    }
}
