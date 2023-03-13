using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CheckPicked : MonoBehaviour
{
    public GameObject currentPlayer;
    public List<GameObject> playerPrefabs;
    public int prefabID = 0;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentPlayer = GameObject.FindGameObjectWithTag("Player");
        Destroy(currentPlayer);
        GameObject newPlayer = Instantiate(playerPrefabs[prefabID], currentPlayer.transform.position, Quaternion.identity);
    }
}
