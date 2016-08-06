using UnityEngine;
using System.Collections.Generic;

public class MPTTuto : MonoBehaviour
{
    public List<GameObject> listOfPages;

    public int currentPage;

    public void OnEnabled()
    {
        currentPage = 0;
        for (int pageId = 0; pageId < listOfPages.Count; ++pageId)
        {
            listOfPages[pageId].SetActive(false);
        }
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
            listOfPages[currentPage].SetActive(true);
        }
    }
}
