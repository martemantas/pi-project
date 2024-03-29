using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class RoomInfo
{
    public string name;

    public int X;
    public int Y;
}

public class RoomController : MonoBehaviour
{
    public static RoomController instance;
    string currentWorldName = "Outside";

    RoomInfo currentLoadRoomData;
    Room currRoom;
    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();
    public List<Room> loadedRooms = new List<Room>();
    bool isLoadingRoom = false;
    bool spawnedBossRoom = false;
    bool updatedRooms = false;
    //public Tile chest;
    //public GameObject openChest;
    public GameObject screenChanger;


    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        UpdateRoomQueue();
        nuke();
    }

    void UpdateRoomQueue()
    {
        if (isLoadingRoom)
        {
            return;
        }

        if (loadRoomQueue.Count == 0)
        {
            if (!spawnedBossRoom)
            {
                StartCoroutine(SpawnBossRoom());
            }
            else if (spawnedBossRoom && !updatedRooms)
            {
                foreach (Room room in loadedRooms)
                {
                    room.RemoveUnconnectedDoors();
                }
                UpdateRooms();
                updatedRooms = true;
            }
            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;

        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    IEnumerator SpawnBossRoom()
    {
        spawnedBossRoom = true;
        yield return new WaitForSeconds(0.5f);
        if (loadRoomQueue.Count == 0)
        {
            Room bossRoom = loadedRooms[loadedRooms.Count - 1];
            Room tempRoom = new Room(bossRoom.X, bossRoom.Y);
            Destroy(bossRoom.gameObject);
            var roomToRemove = loadedRooms.Single(r => r.X == tempRoom.X && r.Y == tempRoom.Y);
            loadedRooms.Remove(roomToRemove);
            LoadRoom("End", tempRoom.X, tempRoom.Y);
        }
    }

    public void LoadRoom(string name, int x, int y)
    {
        if (DoesRoomExist(x, y))
        {
            return;
        }

        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.X = x;
        newRoomData.Y = y;

        loadRoomQueue.Enqueue(newRoomData);
    }

    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        string roomName = currentWorldName + info.name;

        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while (loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    public void RegisterRoom(Room room)
    {
        if (!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Y))
        {
            room.transform.position = new Vector3(
                currentLoadRoomData.X * room.Width,
                currentLoadRoomData.Y * room.Height,
                0
            );

            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.X + ", " + room.Y;
            room.transform.parent = transform;

            isLoadingRoom = false;

            if (loadedRooms.Count == 0)
            {
                CameraController.instance.currRoom = room;
            }

            loadedRooms.Add(room);
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }

    public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y) != null;
    }

    public Room FindRoom(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y);
    }

    public string GetRandomRoomName()
    {
        string[] possibleRooms = new string[]
        {
            "Basic1",
            "Basic2",
            "Basic3",
            "Basic4",
            "Basic5"
        };

        return possibleRooms[Random.Range(0, possibleRooms.Length)];
    }

    public void OnPlayerEnterRoom(Room room)
    {
		currRoom = room;
        Vector3 center = currRoom.GetRoomCenter();

        CameraController.instance.UpdateCameraData(room);

        Transform player = GameObject.FindWithTag("Player").transform;
        Vector3 diff = center - player.position;
        diff.Normalize();
        player.position = new Vector3(player.position.x + diff.x * 0.2f, player.position.y + diff.y * 0.2f, player.position.z);

        StartCoroutine(RoomCoroutine());
        if (!currRoom.GetComponentInChildren<Visit>().visited)
            currRoom.GetComponentInChildren<Visit>().VisitRoom();
        //SceneChanger[] sceneChanger = room.GetComponentsInChildren<SceneChanger>();
        screenChanger.SetActive(false);

        // not working
        GameObject boss = GameObject.Find("Boss");
        //BossController[] boss = room.GetComponentsInChildren<BossController>();


        if (boss.GetComponent<BossHealthController>().health <= 1)
        {
            EnablePortal();
        }
    }
    public void EnablePortal()
    {
        if (screenChanger.activeInHierarchy == false)
        {
            screenChanger.SetActive(true);
        }
    }

    public IEnumerator RoomCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        UpdateRooms();
    }

    public void nuke()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            foreach (Room room in loadedRooms)
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if (enemies.Length > 0)
                {
                    foreach (EnemyController enemy in enemies)
                    {
                        if (enemy.inRoom)
                        {
                            enemy.GetComponentInParent<HealthController>().Damage(100);
                        }
                    }

                }
            }
        }

    }

    public void UpdateRooms()
    {
        foreach (Room room in loadedRooms)
        {
            if (currRoom != room)
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                BossController[] boss = room.GetComponentsInChildren<BossController>();
                if (enemies != null)
                {
                    foreach (EnemyController enemy in enemies)
                    {
                        enemy.inRoom = false;
                    }

                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(false);
                    }
                }
                else if(boss != null)
                {
                    foreach (BossController b in boss)
                    {
                        b.inRoom = false;
                    }

                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(false);
                    }
                }
                else
                {
                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(false);
                    }
                }
                /*if (boss != null)
                {
                    foreach (BossController b in boss)
                    {
                        b.inRoom = false;
                    }

                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(false);
                    }
                }
                else
                {
                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(false);
                    }
                }*/
            }
            else
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                BossController[] boss = room.GetComponentsInChildren<BossController>();
                if (enemies.Length > 0)
                {
                    foreach (EnemyController enemy in enemies)
                    {
                        enemy.inRoom = true;
                    }

                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(true);
                    }
                }
                else if(boss.Length > 0)
                {
                    foreach (BossController b in boss)
                    {
                        b.inRoom = true;
                    }

                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(true);
                    }
                }
                else
                {
                    if (!room.IsCleared)
                    {
                        room.IsCleared = true;
                        MoneyManager.MoneyGainOnRun(200);

                        foreach (Door door in room.GetComponentsInChildren<Door>())
                        {
                            door.doorCollider.SetActive(false);
                            if (door != null)
                            {
                                SpriteRenderer doorSprite = door.doorSprite.GetComponent<SpriteRenderer>();
                                doorSprite.color = Color.black;
                            }
                        }
						FindObjectOfType<AudioManager>().Play("doorOpen");
					}
                    else
                    {
                        foreach (Door door in room.GetComponentsInChildren<Door>())
                        {
                            door.doorCollider.SetActive(false);
                            if (door != null)
                            {
                                SpriteRenderer doorSprite = door.doorSprite.GetComponent<SpriteRenderer>();
                                doorSprite.color = Color.black;
                            }
                        }
						
					}
                }
                /*if (boss.Length > 0)
                {
                    foreach (BossController b in boss)
                    {
                        b.inRoom = true;
                    }

                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(true);
                    }
                }
                else
                {
                    if (!room.IsCleared)
                    {
                        room.IsCleared = true;
                        MoneyManager.MoneyGainOnRun(200);

                        foreach (Door door in room.GetComponentsInChildren<Door>())
                        {
                            door.doorCollider.SetActive(false);
                            if (door != null)
                            {
                                SpriteRenderer doorSprite = door.doorSprite.GetComponent<SpriteRenderer>();
                                doorSprite.color = Color.black;
                            }
                        }
                    }
                    else
                    {
                        foreach (Door door in room.GetComponentsInChildren<Door>())
                        {
                            door.doorCollider.SetActive(false);
                            if (door != null)
                            {
                                SpriteRenderer doorSprite = door.doorSprite.GetComponent<SpriteRenderer>();
                                doorSprite.color = Color.black;
                            }
                        }
                    }
                }*/
            }
        }
    }
}
