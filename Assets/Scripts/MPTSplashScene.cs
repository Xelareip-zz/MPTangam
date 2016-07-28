using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MPTSplashScene : MonoBehaviour
{
    public float delay;
    public string nextScene;
    
    void Start ()
    {
        StartCoroutine(LoadNextInDelay());
	}
	
	IEnumerator LoadNextInDelay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(nextScene);
	}
}
