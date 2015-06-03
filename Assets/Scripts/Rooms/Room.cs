using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text;

public class Room
{
    public List<string> roomLayout;
    public List<Enemy> enemyList;
    public List<GameObject> lootList;
    protected Room right, left, up, down;
    public Vector2 rightSpawn, leftSpawn, upSpawn, downSpawn;

    GameObject key, roomMesh;
    Vector2 keyPosition;

    public Room(List<string> roomLayout)
    {
        this.roomLayout = roomLayout;
        enemyList = new List<Enemy>();
        lootList = new List<GameObject>();
    }

    public void MakeRoomMesh()
    {
        GameObject tilePrefab = Resources.Load<GameObject>("Background Prefabs/Base Tile");
        float tileWidth = tilePrefab.renderer.bounds.size.x;
        float tileHeight = tilePrefab.renderer.bounds.size.y;

        GameObject roomBackground = Resources.Load<GameObject>("Background Prefabs/TileMap");
        Vector3 meshPosition = new Vector3(-tileWidth / 2, -tileHeight / 2, 0);
        roomBackground = (GameObject)MonoBehaviour.Instantiate(roomBackground, meshPosition, Quaternion.identity);
        roomBackground.name = Name;
        TileBuilder tileBuilder = roomBackground.GetComponent<TileBuilder>();
        tileBuilder.BuildMesh(Width + 1, Height + 1, new Vector2(tileWidth, tileHeight));
        tileBuilder.BuildTextures(this);


        GameObject wasteBin = GameObject.Find("_Wastebin");
        roomBackground.transform.SetParent(wasteBin.transform);
        roomBackground.transform.localScale = Vector3.one;
        roomBackground.SetActive(false);
        this.roomMesh = roomBackground;
    }

    public char GetTile(int x, int y)
    {
        return this.roomLayout[Height - 1 - y][x];
    }

    public void SetTile(int x, int y, char newCharacter)
    {
        roomLayout[Height - 1 - y] = ReplaceAt(roomLayout[Height - 1 - y], x, newCharacter);
    }

    private string ReplaceAt(string input, int index, char replace)
    {
        StringBuilder output = new StringBuilder(input);
        output[index] = replace;
        return output.ToString();
    }

    public void RemoveKey()
    {
        MonoBehaviour.Destroy(this.key);
        SetTile((int)keyPosition.x, (int)keyPosition.y, '-');
    }

    public void PlaceKey()
    {
        int xIndex = RandomGenerator.RandomInt(2, this.Width - 2);
        int yIndex = RandomGenerator.RandomInt(2, this.Height - 2);

        while (GetTile(xIndex, yIndex) != '-')
        {
            xIndex = RandomGenerator.RandomInt(2, this.Width - 2);
            yIndex = RandomGenerator.RandomInt(2, this.Height - 2);
        }

        keyPosition = new Vector2(xIndex, yIndex);
        SetTile(xIndex, yIndex, '*');
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < RandomGenerator.RandomInt(1, 4); ++i)
        {
            Enemy randomEnemy = new Enemy(RandomEnemyPath(), this);
            randomEnemy.LocalPosition = RandomFreePosition();

            if (randomEnemy.LocalPosition != Vector2.zero)
                enemyList.Add(randomEnemy);
        }
    }

    public Vector3 FindFreeLootSpot(Vector2 localPosition)
    {
        Vector2 localAnswer = localPosition;
        List<Vector2> neighbours = new List<Vector2>();
        neighbours.Add(new Vector2(0, 1));
        neighbours.Add(new Vector2(1, 0));
        neighbours.Add(new Vector2(0, -1));
        neighbours.Add(new Vector2(-1, 0));
        neighbours.Add(new Vector2(-1, 1));
        neighbours.Add(new Vector2(-1, -1));
        neighbours.Add(new Vector2(1, 1));
        neighbours.Add(new Vector2(1, -1));
        neighbours.Add(new Vector2(0, 0)); // Dummy value

        for (int i = 0; i < neighbours.Count; ++i)
        {
            bool free = true;
            foreach (GameObject loot in lootList)
            {
                if ((loot.transform.position - MasterScript.GetRealPosition(localAnswer)).magnitude <= 0.1f ||
                    GetTile((int)localAnswer.x, (int)localAnswer.y) != '-')
                {
                    if (WithinRange(localAnswer + neighbours[i]))
                        localAnswer = localPosition + neighbours[i];
                    free = false;
                    //If we exploited all possible actions and there is still no free spot
                    if (i == neighbours.Count - 1)
                        return Vector3.zero;
                }
            }
            if (free)
                break;
        }

        return MasterScript.GetRealPosition(localAnswer);
    }

    public void SaveLoot(GameObject obj)
    {
        lootList.Add(obj);
    }

    private string RandomEnemyPath()
    {
        return "Enemy Prefabs/Enemy " + RandomGenerator.RandomInt(1, 2); // Amount of enemies we have
    }

    private GameObject RandomEnemy()
    {
        return Resources.Load<GameObject>("Enemy Prefabs/Enemy " + RandomGenerator.RandomInt(1, 2));
    }

    private Vector2 RandomFreePosition()
    {
        bool validAnswer = false;
        Vector2 freePosition = Vector2.zero;
        int attempts = 0;

        while (!validAnswer)
        {
            freePosition = new Vector2(RandomGenerator.RandomInt(2, this.Width - 2), RandomGenerator.RandomInt(2, this.Height - 2));
            validAnswer = true;

            //Prevent infinite loops
            attempts++;
            if (attempts >= 100)
                return Vector2.zero;

            //See if the position we picked is an empty tile
            if (GetTile((int)freePosition.x, (int)freePosition.y) != '-')
            {
                validAnswer = false;
                continue;
            }
            foreach (Enemy enemy in enemyList)
            {
                if (enemy.LocalPosition == freePosition)
                {
                    validAnswer = false;
                    continue;
                }
            }
        }

        return freePosition;
    }

    private bool WithinRange(Vector2 localPostion)
    {
        if (localPostion.x < 0)
            return false;
        else if (localPostion.x > Width - 1)
            return false;
        else if (localPostion.y < 0)
            return false;
        else if (localPostion.y > Height - 1)
            return false;
        else
            return true;
    }

    #region Properties

    public GameObject Key
    {
        get { return key; }
        set { key = value; }
    }

    public bool Explored
    {
        get;
        set;
    }

    public Vector2 RoomLocation
    {
        get;
        set;
    }

    public Room RightNeighbour
    {
        get { return right; }
        set
        {
            right = value;
            if (right != null)
            {
                for (int y = 0; y < this.Height; ++y)
                {
                    if (GetTile(Width - 1, y) == '|')
                    {
                        SetTile(Width - 1, y, '-');
                        rightSpawn = new Vector2(Width - 1, y);
                    }
                }
            }
        }
    }

    public Room LeftNeighbour
    {
        get { return left; }
        set
        {
            left = value;
            if (left != null)
            {
                for (int y = 0; y < this.Height; ++y)
                {
                    if (GetTile(0, y) == '|')
                    {
                        SetTile(0, y, '-');
                        leftSpawn = new Vector2(0, y);
                    }
                }
            }
        }
    }

    public Room UpNeighbour
    {
        get { return up; }
        set
        {
            up = value;
            if (up != null)
            {
                for (int x = 0; x < this.Width; ++x)
                {
                    if (GetTile(x, this.Height - 1) == '|')
                    {
                        SetTile(x, this.Height - 1, '-');
                        upSpawn = new Vector2(x, Height - 1);
                    }
                }
            }
        }
    }

    public Room DownNeighbour
    {
        get { return down; }
        set
        {
            down = value;
            if (down != null)
            {
                for (int x = 0; x < this.Width; ++x)
                {
                    if (GetTile(x, 0) == '|')
                    {
                        SetTile(x, 0, '-');
                        downSpawn = new Vector2(x, 0);
                    }
                }
            }
        }
    }

    public int Width
    {
        get { return this.roomLayout[0].Length; }
    }

    public int Height
    {
        get { return this.roomLayout.Count; }
    }

    public string Name
    {
        get
        {
            return "Room: " + RoomLocation.x + ", " + RoomLocation.y;
        }
    }

    public GameObject RoomMesh
    {
        get { return roomMesh; }
    }
    #endregion
}
