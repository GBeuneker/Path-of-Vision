using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class RoomDrawing : MonoBehaviour
{
    List<GameObject> currentRoomObjects;
    List<GameObject> currentRoomEnemies;
    Dictionary<Vector2, GameObject> mapOfRooms = new Dictionary<Vector2, GameObject>();
    Room currentRoom;
    public float tileWidth, tileHeight;

    // Use this for initialization
    public void Initialize()
    {
        currentRoomObjects = new List<GameObject>();
        currentRoomEnemies = new List<GameObject>();
        GameObject tile = Resources.Load<GameObject>("Background Prefabs/Base Tile");
        tileWidth = tile.GetComponent<SpriteRenderer>().bounds.size.x;
        tileHeight = tile.GetComponent<SpriteRenderer>().bounds.size.y;
        MasterScript.tileWidth = this.tileWidth;
        MasterScript.tileHeight = this.tileHeight;
    }

    public void DrawRoom(Room room)
    {
        DestroyPreviousRoom();
        ShowRoomOnMap(room);

        room.RoomMesh.SetActive(true);

        currentRoom = room;
        currentRoomObjects = new List<GameObject>();
        currentRoomEnemies = new List<GameObject>();
        for (int y = 0; y < room.Height; ++y)
            for (int x = 0; x < room.Width; ++x)
            {
                if (room.roomLayout[y].Length != room.Width)
                    throw new Exception("Line: " + y + " is not the same length as the others!");

                float xPos = x * this.tileWidth;
                float yPos = y * this.tileHeight;
                switch (room.GetTile(x, y))
                {
                    case '+': //Door position
                        SpawnObject(xPos, yPos, "Background Prefabs/Closed Door");
                        break;
                    case '=': //Open door position
                        SpawnObject(xPos, yPos, "Background Prefabs/Open Door");
                        break;
                    case '*': // Key position
                        SpawnKey(xPos, yPos, room);
                        break;
                    case '!': // Chest position
                        SpawnChest(xPos, yPos, (ChestRoom)room);
                        break;
                    default:
                        break;
                }
            }
        foreach (Enemy enmy in room.enemyList)
            SpawnEnemy(enmy);
        foreach (GameObject loot in room.lootList)
            loot.SetActive(true);
    }

    public void ShowRoomOnMap(Room room)
    {
        GameObject map = GameObject.FindWithTag("Map");

        //See if the room already exists in our dictionary
        GameObject currentRoomMapObject;
        mapOfRooms.TryGetValue(room.RoomLocation, out currentRoomMapObject);
        if (currentRoomMapObject != null)
        {
            currentRoomMapObject.GetComponent<Image>().color = Color.white;
            ShowRoomMapNeighbours(room, map);
            map.transform.localPosition = -currentRoomMapObject.transform.localPosition;
        }

        //Do not add a room again to the map if it's already been explored
        if (room.Explored)
            return;

        //Add a new room to our dictionary
        GameObject roomMapObject = AddNewRoomToMap(room, map);
        map.transform.localPosition = -roomMapObject.transform.localPosition;
        roomMapObject.GetComponent<Image>().color = Color.white;
        ShowRoomMapNeighbours(room, map);

    }

    public GameObject AddNewRoomToMap(Room room, GameObject map)
    {
        room.Explored = true;
        GameObject roomMapObject = Resources.Load<GameObject>("Room Prefabs/RoomMap");
        roomMapObject = (GameObject)Instantiate(roomMapObject, Vector3.zero, roomMapObject.transform.rotation);
        roomMapObject.transform.SetParent(map.transform);
        roomMapObject.transform.localScale = Vector3.one;
        Vector2 roomMapSize = roomMapObject.GetComponent<RectTransform>().sizeDelta;
        roomMapObject.transform.localPosition = new Vector3(roomMapSize.x * room.RoomLocation.x, roomMapSize.y * room.RoomLocation.y, roomMapObject.transform.position.z);
        mapOfRooms[room.RoomLocation] = roomMapObject;
        roomMapObject.GetComponent<Image>().color = Color.gray;

        return roomMapObject;
    }

    public void ShowRoomMapNeighbours(Room room, GameObject map)
    {
        //Show the neighbours of current room as well
        if (room.UpNeighbour != null)
        {
            if (!mapOfRooms.ContainsKey(room.UpNeighbour.RoomLocation))
                AddNewRoomToMap(room.UpNeighbour, map);
        }
        if (room.DownNeighbour != null)
        {
            if (!mapOfRooms.ContainsKey(room.DownNeighbour.RoomLocation))
                AddNewRoomToMap(room.DownNeighbour, map);
        }
        if (room.LeftNeighbour != null)
        {
            if (!mapOfRooms.ContainsKey(room.LeftNeighbour.RoomLocation))
                AddNewRoomToMap(room.LeftNeighbour, map);
        }
        if (room.RightNeighbour != null)
        {
            if (!mapOfRooms.ContainsKey(room.RightNeighbour.RoomLocation))
                AddNewRoomToMap(room.RightNeighbour, map);
        }
    }

    public void DestroyPreviousRoom()
    {
        //If we came out of a chest room, set chest back to inactive
        if (currentRoom is ChestRoom)
        {
            ChestRoom chestRoom = (ChestRoom)currentRoom;
            chestRoom.Chest.SetActive(false);
        }
        foreach (GameObject gam in currentRoomObjects)
        {
            Destroy(gam);
        }
        for (int i = 0; i < currentRoomEnemies.Count; ++i)
        {
            currentRoom.enemyList[i].LocalPosition = currentRoomEnemies[i].GetComponent<EnemyMovement>().localPosition;
            Destroy(currentRoomEnemies[i]);
        }

        if (currentRoom != null)
        {
            currentRoom.RoomMesh.SetActive(false);

            //Remove killed enemies
            currentRoom.enemyList.RemoveRange(currentRoomEnemies.Count, currentRoom.enemyList.Count - currentRoomEnemies.Count);
            mapOfRooms[currentRoom.RoomLocation].GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);

            //Show the loot that was in that room
            foreach (GameObject loot in currentRoom.lootList)
            {
                loot.SetActive(false);
            }
        }

    }

    void SpawnChest(float xPos, float yPos, ChestRoom room)
    {
        room.Chest.transform.position = new Vector3(xPos, yPos, 0);
        room.Chest.SetActive(true);
    }

    void SpawnKey(float xPos, float yPos, Room currentRoom)
    {
        GameObject key = Resources.Load<GameObject>("Room Prefabs/KeyObject");
        key = (GameObject)Instantiate(key, new Vector3(xPos, yPos, 0), key.transform.rotation);
        currentRoom.Key = key;
        currentRoomObjects.Add(key);
    }

    void SpawnObject(float xPos, float yPos, string path)
    {
        GameObject roomObject = Resources.Load<GameObject>(path);
        roomObject = (GameObject)Instantiate(roomObject, new Vector3(xPos, yPos, 0), roomObject.transform.rotation);
        currentRoomObjects.Add(roomObject);
    }

    void SpawnEnemy(Enemy enemy)
    {
        GameObject newEnemy = (GameObject)Instantiate(Resources.Load<GameObject>(enemy.Path), MasterScript.GetRealPosition(enemy.LocalPosition), transform.rotation);
        EnemyMovement newEnemyMovement = newEnemy.GetComponent<EnemyMovement>();
        newEnemyMovement.Room = enemy.Room;
        newEnemyMovement.localPosition = enemy.LocalPosition;
        currentRoomEnemies.Add(newEnemy);
    }

    public void KillEnemy(GameObject enemy)
    {
        DropLoot(enemy);

        currentRoomEnemies.Remove(enemy);
        Destroy(enemy);
    }

    private void DropLoot(GameObject enemy)
    {
        if (UnityEngine.Random.Range(0, 8) > 0) //1 in 8 chance to drop loot
            return;
        GameObject loot = Resources.Load<GameObject>("Room Prefabs/Loot");
        Vector3 lootPosition = currentRoom.FindFreeLootSpot(enemy.GetComponent<EnemyMovement>().localPosition);
        if (lootPosition == Vector3.zero)
            return;
        loot = (GameObject)Instantiate(loot, lootPosition, enemy.transform.rotation);
        loot.transform.SetParent(GameObject.Find("_Wastebin").transform);
        currentRoom.SaveLoot(loot);
    }

    public GameObject FindEnemy(Vector2 localPosition)
    {
        foreach (GameObject enmy in currentRoomEnemies)
        {
            if (enmy.GetComponent<EnemyMovement>().localPosition == localPosition)
                return enmy;
        }
        return null;
    }
}
