using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPicked : MonoBehaviour
{
    private GameObject currentPlayer;
    public GameObject playerPrefab;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("veikia");
        player = GameObject.FindGameObjectWithTag("Player");
        SwitchToPicked();
    }

    private void SwitchToPicked()
    {
        if(LastDisabledObject.currentObject != null)
        {
            Debug.Log("Yra");
            // spawn the new player prefab at the current position of the old player
            GameObject newPlayer = Instantiate(playerPrefab, currentPlayer.transform.position, Quaternion.identity);

            // destroy the old player
            Destroy(player);
        }
    }
}
