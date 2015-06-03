using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TileBuilder : MonoBehaviour
{
    int sizeX, sizeY;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuildTextures(Room room)
    {
        Sprite ground = Resources.LoadAll<Sprite>("Background Sprites/Tileset 1")[8];
        Sprite wall = Resources.LoadAll<Sprite>("Background Sprites/Tileset 2")[3];

        int texWidth = (room.Width + 1) * (int)ground.textureRect.width;
        int texHeight = (room.Height + 1) * (int)ground.textureRect.height;
        int tileWidth = (int)ground.textureRect.width;
        int tileHeight = (int)ground.textureRect.height;

        Texture2D texture = new Texture2D(texWidth, texHeight);

        for (int y = 0; y < room.Height; ++y)
            for (int x = 0; x < room.Width; ++x)
            {
                switch (room.GetTile(x, y))
                {
                    case '#': //Wall position
                    case '|': //Door position
                        texture.SetPixels(x * (int)ground.textureRect.width, y * (int)ground.textureRect.height, (int)wall.textureRect.width, (int)wall.textureRect.height, GetPixelsFromSprite(wall));
                        break;
                    case '-': //Ground position
                        texture.SetPixels(x * tileWidth, y * tileHeight, tileWidth, tileHeight, GetPixelsFromSprite(ground));
                        break;
                    case '*': // Key position
                        texture.SetPixels(x * tileWidth, y * tileHeight, tileWidth, tileHeight, GetPixelsFromSprite(ground));
                        break;
                    case '!': // Chest position
                        texture.SetPixels(x * tileWidth, y * tileHeight, tileWidth, tileHeight, GetPixelsFromSprite(ground));
                        break;
                    case '+': // Hatch position
                        //texture.SetPixels(x * tileWidth, y * tileHeight, tileWidth, tileHeight, Color.black);
                        break;
                    case '=': // Open Hatch position
                        //texture.SetPixels(x * tileWidth, y * tileHeight, tileWidth, tileHeight, GetPixelsFromSprite(ground));
                        break;
                    default:
                        break;
                }
            }

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.materials[0].mainTexture = texture;
    }

    Color[] GetPixelsFromSprite(Sprite sprite)
    {
        return sprite.texture.GetPixels((int)sprite.textureRect.x, (int)sprite.textureRect.y, (int)sprite.textureRect.width, (int)sprite.textureRect.height);
    }

    public void BuildMesh(int sizeX, int sizeY, Vector2 tileSize)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;

        int vSizeX = sizeX + 1;
        int vSizeY = sizeY + 1;
        int numVertices = vSizeX * vSizeY;
        int numTiles = sizeX * sizeY;
        int numTriangles = numTiles * 2;

        //Generate mesh data
        Vector3[] vertices = new Vector3[numVertices];
        Vector3[] normals = new Vector3[numVertices];
        Vector2[] uv = new Vector2[numVertices];

        int[] triangles = new int[numTriangles * 3];

        for (int y = 0; y < sizeY; ++y)
            for (int x = 0; x < sizeX; ++x)
            {
                vertices[y * vSizeX + x] = new Vector3(x * tileSize.x, y * tileSize.y, 0);
                normals[y * vSizeX + x] = Vector3.up;
                uv[y * vSizeX + x] = new Vector2((float)x / sizeX, (float)y / sizeY);
            }

        for (int y = 0; y < sizeY; y++)
            for (int x = 0; x < sizeX; x++)
            {
                int squareIndex = y * sizeX + x;
                int triOffset = squareIndex * 6;
                triangles[triOffset + 0] = y * vSizeX + x + 0;
                triangles[triOffset + 1] = y * vSizeX + x + vSizeX + 0;
                triangles[triOffset + 2] = y * vSizeX + x + vSizeX + 1;

                triangles[triOffset + 3] = y * vSizeX + x + 0;
                triangles[triOffset + 4] = y * vSizeX + x + vSizeX + 1;
                triangles[triOffset + 5] = y * vSizeX + x + 1;
            }


        //Create new mesh and populate with data
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        //Assign mesh to our meshFilter, meshRenderer and meshCollider
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();

        meshFilter.mesh = mesh;
    }
}
