using System.Collections;
using UnityEngine;

partial class PlayerControl : MonoBehaviour
{
    Dungeon currentDungeon;
    Room currentRoom;
    Vector2 localPosition;
    RoomDrawing roomDrawing;
    public float speed;
    public Sprite[] playerDirections;

    // Use this for initialization
    void Start()
    {
        //Initiate the first floor on the masterscript
        MasterScript.NextFloor();
        Reset();
    }

    void Reset()
    {
        currentDungeon = MasterScript.CurrentDungeon;
        currentRoom = currentDungeon.StartRoom;

        this.roomDrawing = GetComponent<RoomDrawing>();
        roomDrawing.Initialize();
        roomDrawing.DrawRoom(currentRoom);

        localPosition = new Vector2((int)1, (int)1);
        transform.position = MasterScript.GetRealPosition(localPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (MasterScript.stopMovement)
        {
            transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            return;
        }

        transform.GetComponent<Rigidbody2D>().velocity = speed * Time.deltaTime * (MasterScript.GetRealPosition(localPosition) - transform.position);

        if (transform.GetComponent<Rigidbody2D>().velocity.magnitude < 0.2f)
            HandleInput();

    }

    public void HandleInput()
    {
        if (Input.GetKey(KeyCode.A) || MobileInput.swipeDirection == MobileInput.Direction.Left)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = playerDirections[0];
            Move(new Vector2(-1, 0));
        }
        else if (Input.GetKey(KeyCode.S) || MobileInput.swipeDirection == MobileInput.Direction.Down)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = playerDirections[1];
            Move(new Vector2(0, -1));
        }
        else if (Input.GetKey(KeyCode.D) || MobileInput.swipeDirection == MobileInput.Direction.Right)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = playerDirections[2];
            Move(new Vector2(1, 0));
        }
        else if (Input.GetKey(KeyCode.W) || MobileInput.swipeDirection == MobileInput.Direction.Up)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = playerDirections[3];
            Move(new Vector2(0, 1));
        }
    }

    private void MoveToRoom(Room newRoom)
    {
        currentRoom = newRoom;
        roomDrawing.DrawRoom(currentRoom);
    }

    void Move(Vector2 direction)
    {
        if (transform.GetComponent<Rigidbody2D>().velocity.magnitude > 0.2f)
            return;

        Vector2 newPosition = localPosition + direction;

        if (newPosition.x < 0)
        {
            MoveToRoom(currentRoom.LeftNeighbour);
            transform.position = MasterScript.GetRealPosition(currentRoom.rightSpawn);
            localPosition = currentRoom.rightSpawn;
            return;
        }
        else if (newPosition.x >= currentRoom.Width)
        {
            MoveToRoom(currentRoom.RightNeighbour);
            transform.position = MasterScript.GetRealPosition(currentRoom.leftSpawn);
            localPosition = currentRoom.leftSpawn;
            return;
        }
        else if (newPosition.y < 0)
        {
            MoveToRoom(currentRoom.DownNeighbour);
            transform.position = MasterScript.GetRealPosition(currentRoom.upSpawn);
            localPosition = currentRoom.upSpawn;
            return;
        }
        else if (newPosition.y >= currentRoom.Height)
        {
            MoveToRoom(currentRoom.UpNeighbour);
            transform.position = MasterScript.GetRealPosition(currentRoom.downSpawn);
            localPosition = currentRoom.downSpawn;
            return;
        }

        switch (currentRoom.GetTile((int)newPosition.x, (int)newPosition.y))
        {
            //Tiles you cannot walk through
            case '#':
            case '|':
                break;
            //Tiles with a special function
            case '*':
                currentRoom.RemoveKey();
                currentDungeon.bossRoom.OpenHatch();
                localPosition += direction;
                GetComponent<Player>().hasKey = true;
                break;
            case '=':
                localPosition += direction;
                this.NextLevel();
                break;
            //Default background tiles
            default:
                localPosition += direction;
                break;

        }
    }

    private void NextLevel()
    {
        roomDrawing.DestroyPreviousRoom();
        MasterScript.NextFloor();
        this.Reset();
    }

    public Vector2 LocalPosition
    {
        get { return localPosition; }
    }

    public Room CurrentRoom
    {
        get { return currentRoom; }
    }
}
