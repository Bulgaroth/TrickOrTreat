using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager _instance;
    private static readonly int Start = Animator.StringToHash("Start");
    private static readonly int End = Animator.StringToHash("End");

    [SerializeField] private float transitionTime = 1.5f;
    [SerializeField] private Animator animator;

    private bool _isTransitioning;


    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator LoadScene(int scene)
    {
        if (_isTransitioning) yield break;
        _isTransitioning = true;
        Time.timeScale = 1;

        animator.SetTrigger(Start);

        yield return new WaitForSeconds(transitionTime);

        var loadingScene = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        while (!loadingScene.isDone) yield return null;

        yield return new WaitForEndOfFrame(); // fix for the no frame when loading a scene
        yield return new WaitForEndOfFrame();

        animator.SetTrigger(End);
        _isTransitioning = false;
    }

    private IEnumerator LoadScene(string scene)
    {
        if (_isTransitioning) yield break;
        _isTransitioning = true;
        Time.timeScale = 1;

        animator.SetTrigger(Start);

        yield return new WaitForSeconds(transitionTime);

        var loadingScene = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single) ??
                            SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);

        while (!loadingScene.isDone) yield return null;

        yield return new WaitForEndOfFrame(); // fix for the no frame when loading a scene
        yield return new WaitForEndOfFrame();

        animator.SetTrigger(End);
        _isTransitioning = false;
    }

    public static void LoadSceneWithTransition(int scene)
    {
        _instance.StartCoroutine(_instance.LoadScene(scene));
    }

    public static void LoadSceneWithTransition(string scene)
    {
        _instance.StartCoroutine(_instance.LoadScene(scene));
    }
       
}
