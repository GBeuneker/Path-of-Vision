using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    public float moveFrequency;
    public float triggerRange;
    float moveTimer;
    public Vector2 localPosition;
    RoomDrawing roomDrawing;
    GameObject player;
    Room currentRoom;

    // Use this for initialization
    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player");
        this.roomDrawing = player.GetComponent<RoomDrawing>();
        this.moveTimer = moveFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (MasterScript.stopMovement)
        {
            this.moveTimer = moveFrequency;
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            return;
        }

        //Check if the enemy should fight with the player
        if (collider2D.bounds.Contains(player.transform.position))
        {
            if (GameObject.FindWithTag("CombatOverlay") == null)
            {
                GameObject combatOverlay = (GameObject)Instantiate(Resources.Load<GameObject>("Combat Prefabs/CombatOverlay"));
                combatOverlay.GetComponent<CombatManager>().Setup(player, gameObject);
            }
            return;
        }

        if (moveFrequency > 0)
        {

            moveTimer -= Time.deltaTime;
            if (moveTimer <= 0 && transform.GetComponent<Rigidbody2D>().velocity.magnitude < 0.2f)
            {
                moveTimer = moveFrequency;
                List<Vector2> moveList = AstarSearch(player.GetComponent<PlayerControl>().LocalPosition);
                Vector2 move;
                if (moveList.Count <= 0) //AstarSearch couldn't find a route
                {
                    move = RandomMove();
                    moveTimer = moveFrequency * 3.5f;
                }
                else
                    move = moveList[0];
                MoveTo(move);


            }
        }
        transform.GetComponent<Rigidbody2D>().velocity = speed * Time.deltaTime * (MasterScript.GetRealPosition(localPosition) - transform.position);
    }

    void MoveTo(Vector2 destination)
    {
        if (IsValidPosition(destination))
        {
            localPosition = destination;
        }
    }

    List<Vector2> AstarSearch(Vector2 localDestination)
    {
        HashSet<Vector2> closedList = new HashSet<Vector2>();
        Node localPos = new Node(localPosition);
        Func<Node, int> compare = ManHattanDistance;
        PriorityQueue<Node, int> openList = new PriorityQueue<Node, int>(ManHattanDistance);
        openList.Enqueue(localPos);
        closedList.Add(localPos.localPosition);

        while (openList.Count > 0)
        {
            Node element = openList.Dequeue();

            if (element.route.Count > 15)   //Do not look further than X tiles ahead
            {
                break;
            }

            //We have found the destination
            if (element.localPosition == localDestination)
            {
                return element.route;
            }


            List<Node> nbrs = GetSuccesors(element);
            foreach (Node nbr in nbrs)
            {
                if (!closedList.Contains(nbr.localPosition))
                {
                    openList.Enqueue(nbr);
                    closedList.Add(nbr.localPosition);
                }

            }
        }

        //No route was found
        return new List<Vector2>();
    }

    int ManHattanDistance(Node inputNode)
    {
        Vector2 localDestination = player.GetComponent<PlayerControl>().LocalPosition;
        int xDistance = (int)Mathf.Abs(inputNode.localPosition.x - localDestination.x);
        int yDistance = (int)Mathf.Abs(inputNode.localPosition.y - localDestination.y);

        return xDistance + yDistance;
    }

    List<Node> GetSuccesors(Node inputNode)
    {
        List<Node> answer = new List<Node>();

        Vector2 up = inputNode.localPosition + new Vector2(0, 1);
        if (IsValidPosition(up))
        {
            Node upNode = new Node(up, inputNode.route);
            answer.Add(upNode);
        }
        Vector2 down = inputNode.localPosition + new Vector2(0, -1);
        if (IsValidPosition(down))
        {
            Node downNode = new Node(down, inputNode.route);
            answer.Add(downNode);
        }
        Vector2 right = inputNode.localPosition + new Vector2(1, 0);
        if (IsValidPosition(right))
        {
            Node rightNode = new Node(right, inputNode.route);
            answer.Add(rightNode);
        }
        Vector2 left = inputNode.localPosition + new Vector2(-1, 0);
        if (IsValidPosition(left))
        {
            Node leftNode = new Node(left, inputNode.route);
            answer.Add(leftNode);
        }

        return answer;
    }

    Vector2 RandomMove()
    {
        Vector2 direction = Vector2.zero;
        int choice = UnityEngine.Random.Range(0, 4);
        switch (choice)
        {
            case 0:
                direction = new Vector2(1, 0); // Move Right
                break;
            case 1:
                direction = new Vector2(-1, 0); // Move Left
                break;
            case 2:
                direction = new Vector2(0, 1); // Move Up
                break;
            case 3:
                direction = new Vector2(0, -1); // Move Down
                break;
            default:
                break;
        }

        return localPosition + direction;
    }

    bool IsValidPosition(Vector2 position)
    {
        if (position.x < 0 || position.x >= currentRoom.Width ||
            position.y < 0 || position.y >= currentRoom.Height)
            return false;

        switch (currentRoom.GetTile((int)position.x, (int)position.y))
        {
            //Tiles you cannot walk through
            case '#':
            case '|':
                return false;
            //Default background tiles
            default:
                return true;
        }
    }

    public Room Room
    {
        get { return currentRoom; }
        set
        {
            currentRoom = value;
        }
    }

    public int DistanceFromPlayer
    {
        get
        {
            //Manhattan distance to the player
            return (int)Mathf.Abs(this.localPosition.x - player.GetComponent<PlayerControl>().LocalPosition.x) +
                   (int)Mathf.Abs(this.localPosition.y - player.GetComponent<PlayerControl>().LocalPosition.y);
        }
    }
}

public class Node
{
    public Vector2 localPosition;
    public List<Vector2> route;

    public Node(Vector2 localPosition, List<Vector2> previousRoute = null)
    {
        route = new List<Vector2>();
        this.localPosition = localPosition;
        if (previousRoute != null)
        {
            route.AddRange(previousRoute);
            route.Add(localPosition);
        }
    }
}

public class PriorityQueue<TElement, TKey>
{
    private SortedDictionary<TKey, Queue<TElement>> dictionary = new SortedDictionary<TKey, Queue<TElement>>();
    private Func<TElement, TKey> selector;

    public PriorityQueue(Func<TElement, TKey> selector)
    {
        this.selector = selector;
    }

    public void Enqueue(TElement item)
    {
        TKey key = selector(item);
        Queue<TElement> queue;
        if (!dictionary.TryGetValue(key, out queue))
        {
            queue = new Queue<TElement>();
            dictionary.Add(key, queue);
        }

        queue.Enqueue(item);
    }

    public TElement Dequeue()
    {
        if (dictionary.Count == 0)
            throw new Exception("No items to Dequeue:");
        var key = dictionary.Keys.First();

        var queue = dictionary[key];
        var output = queue.Dequeue();
        if (queue.Count == 0)
            dictionary.Remove(key);

        return output;
    }

    public int Count
    {
        get { return dictionary.Count; }
    }
}
