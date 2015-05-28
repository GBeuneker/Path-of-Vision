using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;


public class Dungeon
{
    List<Room> rooms;
    Room startRoom;
    Room[,] roomGrid;
    public BossRoom bossRoom;

    public Dungeon(int floor)
    {
        rooms = new List<Room>();
        roomGrid = new Room[1, 1];
        //Make a starting room
        startRoom = new Room(this.LoadRoomLayout(0, "NormalRoom"));
        rooms.Add(startRoom);

        int roomAmount = CalculateRoomAmount(floor);
        GenerateDungeon(roomAmount, startRoom);
    }

    private int CalculateRoomAmount(int floor)
    {
        int answer = 3 + 2 * Mathf.CeilToInt(floor / 2f);

        for (int i = 0; i <= floor; ++i)
        {
            //1/3 chance to add another room
            if (RandomGenerator.RandomInt(1, 4) <= 1)
                answer += 1;
        }

        return answer;
    }

    private void GenerateDungeon(int roomsToSpawn, Room startRoom)
    {
        //Determine the size of the grid
        roomGrid = new Room[roomsToSpawn, roomsToSpawn];
        int randomX = RandomGenerator.RandomInt(0, roomsToSpawn);
        int randomY = RandomGenerator.RandomInt(0, roomsToSpawn);
        startRoom.RoomLocation = new Vector2(randomX, randomY);
        roomGrid[randomX, randomY] = startRoom;

        //Save the open positions
        List<Vector2> openSpots = new List<Vector2>();
        openSpots.AddRange(GetAdjPositions(randomX, randomY, roomsToSpawn, roomsToSpawn));
        //Save the closed positions
        List<Vector2> closedSpots = new List<Vector2>();
        closedSpots.Add(new Vector2(randomX, randomY));

        while (roomsToSpawn > 0)
        {
            if (openSpots.Count <= 0)
                throw new Exception("Dungeon Generator has no open spots left to fill! (Error 001)");

            int randomSpotIndex = RandomGenerator.RandomInt(0, openSpots.Count);
            Vector2 randomSpotPosition = openSpots[randomSpotIndex];
            closedSpots.Add(randomSpotPosition);
            //Get the new open spots
            List<Vector2> adjacentSpots = GetAdjPositions((int)randomSpotPosition.x, (int)randomSpotPosition.y, roomGrid.GetLength(0), roomGrid.GetLength(1));
            openSpots.AddRange(adjacentSpots);
            //Leave out the taken spots
            openSpots = openSpots.Except(closedSpots).ToList();

            if (roomGrid[(int)randomSpotPosition.x, (int)randomSpotPosition.y] != null)
                throw new Exception("Dungeon Generator tried to fill a taken spot! (Error 002)");

            Room newRoom = RandomRoom();

            if (roomsToSpawn == 3)
            {
                newRoom = RandomRoom("ChestRoom");
            }
            else if (roomsToSpawn == 2)
            {
                newRoom.PlaceKey();
            }
            else if (roomsToSpawn == 1)
            {
                newRoom = RandomRoom("BossRoom");
                this.bossRoom = (BossRoom)newRoom;
                this.bossRoom.SpawnBoss();
            }

            if (RandomGenerator.RandomInt(0, 16) <= 14 && roomsToSpawn > 1)
            {
                newRoom.SpawnEnemies();
            }

            //Connect the room to any adjacent rooms
            newRoom = MakeConnections(newRoom, randomSpotPosition, roomGrid.GetLength(0), roomGrid.GetLength(1));
            newRoom.RoomLocation = randomSpotPosition;
            roomGrid[(int)randomSpotPosition.x, (int)randomSpotPosition.y] = newRoom;
            rooms.Add(newRoom);
            roomsToSpawn--;
        }
    }

    private List<Vector2> GetAdjPositions(int xPos, int yPos, int maxX, int maxY)
    {
        List<Vector2> answer = new List<Vector2>();

        if ((xPos - 1) >= 0)
            answer.Add(new Vector2(xPos - 1, yPos));
        if ((yPos - 1) >= 0)
            answer.Add(new Vector2(xPos, yPos - 1));
        if ((xPos + 1) < maxX)
            answer.Add(new Vector2(xPos + 1, yPos));
        if ((yPos + 1) < maxY)
            answer.Add(new Vector2(xPos, yPos + 1));

        return answer;
    }

    private Room MakeConnections(Room room, Vector2 pos, int maxX, int maxY)
    {
        int posX = (int)pos.x;
        int posY = (int)pos.y;

        if (posX - 1 >= 0)
        {
            room.LeftNeighbour = roomGrid[posX - 1, posY];
            if (roomGrid[posX - 1, posY] != null)
                roomGrid[posX - 1, posY].RightNeighbour = room;
        }
        if (posX + 1 < maxX)
        {
            room.RightNeighbour = roomGrid[posX + 1, posY];
            if (roomGrid[posX + 1, posY] != null)
                roomGrid[posX + 1, posY].LeftNeighbour = room;
        }
        if (posY - 1 >= 0)
        {
            room.DownNeighbour = roomGrid[posX, posY - 1];
            if (roomGrid[posX, posY - 1] != null)
                roomGrid[posX, posY - 1].UpNeighbour = room;
        }
        if (posY + 1 < maxY)
        {
            room.UpNeighbour = roomGrid[posX, posY + 1];
            if (roomGrid[posX, posY + 1] != null)
                roomGrid[posX, posY + 1].DownNeighbour = room;
        }

        return room;
    }

    private Room RandomRoom(string roomType = "NormalRoom")
    {
        switch (roomType)
        {
            case "NormalRoom":
                return new Room(LoadRoomLayout(RandomGenerator.RandomInt(1, 5), roomType)); //Amount of different rooms we have
            case "BossRoom":
                return new BossRoom(LoadRoomLayout(RandomGenerator.RandomInt(1, 2), roomType)); //Amount of different Boss rooms we have
            case "ChestRoom":
                return new ChestRoom(LoadRoomLayout(RandomGenerator.RandomInt(1, 2), roomType)); ;
            default:
                return null;
        }
    }

    public List<string> LoadRoomLayout(int roomIndex, string roomType)
    {
        List<string> textLines = new List<string>();
        TextAsset file = Resources.Load<TextAsset>("Room Layouts/" + roomType + " " + roomIndex);

        string text = file.text;
        textLines = text.Split('\n').ToList();

        return textLines;
    }

    public Room StartRoom
    {
        get { return startRoom; }
    }

    public List<Room> Rooms
    {
        get { return rooms; }
    }
}