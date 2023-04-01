using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private GameObject loaderCanvas;
    [SerializeField] private Image progressBar;
    private float target;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void LoadScene(int sceneId)
    {
        target = 0;
        progressBar.fillAmount = 0;

        var scene = SceneManager.LoadSceneAsync(sceneId);
        scene.allowSceneActivation = false;

        loaderCanvas.SetActive(true);

        do
        {
            target = scene.progress;
        } while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        await Task.Delay(800);
        loaderCanvas.SetActive(false);
    }

    void Update()
    {
        progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, target, 3 * Time.deltaTime)+0.1f;
    }
}
