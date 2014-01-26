using UnityEngine;
using System.Collections;
using System.IO;

public class GenerateDungeon : MonoBehaviour {
    // Define Dungeon Data Constants
    const uint NOTHING = 0x00000000;

    const uint BLOCKED = 0x00000001;
    const uint ROOM = 0x00000002;
    const uint CORRIDOR = 0x00000004;
    // this is below       0x00000008;
    const uint PERIMETER = 0x00000010;
    const uint ENTRANCE = 0x00000020;
    const uint ROOM_ID = 0x0000FFC0;

    const uint ARCH = 0x00010000;
    const uint DOOR = 0x00020000;
    const uint LOCKED = 0x00040000;
    const uint TRAPPED = 0x00080000;
    const uint SECRET = 0x00100000;
    const uint PORTC = 0x00200000;
    const uint STAIR_DN = 0x00400000;
    const uint STAIR_UP = 0x00800000;

    const uint CONNECTED = 0x00000008;

    const uint LABEL = 0xFF000000;

    const uint OPENSPACE = ROOM | CORRIDOR;
    const uint DOORSPACE = ARCH | DOOR | LOCKED | TRAPPED | SECRET | PORTC;
    const uint ESPACE = ENTRANCE | DOORSPACE | 0xFF000000;
    const uint STAIRS = STAIR_DN | STAIR_UP;

    const uint BLOCK_ROOM = BLOCKED | ROOM;
    const uint BLOCK_CORR = BLOCKED | PERIMETER | CORRIDOR;
    const uint BLOCK_DOOR = BLOCKED | DOORSPACE;

    // Define Other Constants
    int[] sizeTable = new int[10]{ 20, 40, 80, 120, 160, 240, 320, 400, 480, 720 };
    const float SIZE_VARIATION = 0.2F;
    int CROSS_SHAPE_FACTOR = 5;
    int[] roomSizeTable = new int[10]{ 4, 6, 8, 10, 14, 18, 24, 30, 40, 50 };
    const int PERCENT_SPACE_ROOMS = 3;
    const float DOOR_CHANCE = 0.11F;

	// Use this for initialization
	void Start () {
        MakeDungeon(2, 0, 0);
    }

    // errors to check for:
    // size > 9
    // shape out of range
    // roomSize > 5
    void MakeDungeon(int size, int shape, int roomSize) {	
        // Get Customization Values
        int height = sizeTable[size] + (int)((Random.value - 0.5F) * ((float)sizeTable[size]) * SIZE_VARIATION);
        int width = sizeTable[size] + (int)((Random.value - 0.5F) * ((float)sizeTable[size]) * SIZE_VARIATION);

        // Initiallize Dungeon Variables
        uint[,] theDungeonData = new uint[height,width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                theDungeonData[i,j] = NOTHING;
            }
        }
        MaskShape(shape, height, width, ref theDungeonData);
        PlaceRooms(roomSizeTable[roomSize], height, width, ref theDungeonData);
        PlaceStairs(height, width, ref theDungeonData);
        SetPerimeters(height, width, ref theDungeonData);
        PlaceDoors(height, width, ref theDungeonData);
        PathToEntrance(height, width, ref theDungeonData);
        MakeCorridors(size, height, width, ref theDungeonData);

        //char[] debugString = new char[width];
        Texture2D debugTexture = new Texture2D(width, height);
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                //debugString[j] = (char)(theDungeonData[i, j] + 0x30);
                if (theDungeonData[i, j] == NOTHING)
                {
                    debugTexture.SetPixel(j, i, Color.black);
                }
                else if ((theDungeonData[i, j] & ROOM) == ROOM)
                {
                    debugTexture.SetPixel(j, i, Color.blue);
                }
                else if ((theDungeonData[i, j] & DOOR) == DOOR)
                {
                    debugTexture.SetPixel(j, i, Color.green);
                }
                else if ((theDungeonData[i, j] & PERIMETER) == PERIMETER)
                {
                    debugTexture.SetPixel(j, i, Color.red);
                }
                else if ((theDungeonData[i, j] & STAIR_UP) == STAIR_UP)
                {
                    debugTexture.SetPixel(j, i, Color.magenta);
                }
            }
            //Debug.Log(new string(debugString));
        }
        debugTexture.Apply();
        File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", debugTexture.EncodeToPNG());

        return;
	}

    private void PathToEntrance(int height, int width, ref uint[,] theDungeonData)
    {
        int i = 0;
        int j = 0;

        for (int ii = 0; ii < height; ii++)
        {
            for (int jj = 0; jj < width; jj++)
            {
                if ((theDungeonData[ii, jj] & ENTRANCE) == ENTRANCE)
                {
                    i = ii;
                    j = jj;
                }
            }
        }

        FindPathToCorridor(i - 1, j, height, width, theDungeonData);
    }

    private void FindPathToCorridor(int i, int j, int height, int width, uint[,] theDungeonData)
    {
        int ii = i;
        while (((theDungeonData[ii, j] & BLOCKED) != BLOCKED) && (ii < height))
        {
            theDungeonData[ii, j] = theDungeonData[ii, j] | CORRIDOR;
            ii--;
        }
    }

    private void MakeCorridors(int size, int height, int width, ref uint[,] theDungeonData)
    {
        //while (DoorsNotConnected(height, width, ref theDungeonData))
        for (int k = 0; k < (sizeTable[size] / 2); k++)
        {
            for (int i = (int)(Random.value * height); i < height; i++)
            {
                for (int j = (int)(Random.value * height); j < width; j++)
                {
                    if (((theDungeonData[i, j] & DOOR) == DOOR) && ((theDungeonData[i, j] & CONNECTED) != CONNECTED))
                    {
                        PlaceCorridor(i, j, height, width, ref theDungeonData);
                    }
                }
            }
        }
    }

    private void PlaceCorridor(int i, int j, int height, int width, ref uint[,] theDungeonData)
    {
        if ((theDungeonData[i + 1, j] & ROOM) == ROOM)
        {
            SearchHorizontal(i - 1, j, width, ref theDungeonData);
        }
        else if ((theDungeonData[i - 1, j] & ROOM) == ROOM)
        {
            SearchHorizontal(i + 1, j, width, ref theDungeonData);
        }
        else if ((theDungeonData[i, j + 1] & ROOM) == ROOM)
        {
            SearchVertical(i, j - 1, height, ref theDungeonData);
        }
        else if ((theDungeonData[i, j - 1] & ROOM) == ROOM)
        {
            SearchVertical(i, j + 1, height, ref theDungeonData);
        }
    }

    private void SearchVertical(int i, int j, int height, ref uint[,] theDungeonData)
    {
        bool up = false;
        bool down = false;
        for (int ii = 0; ii < i; ii++)
        {
            if (((theDungeonData[ii, j] & DOOR) == DOOR) || ((theDungeonData[ii, j] & CORRIDOR) == CORRIDOR))
            {
                down = true;
            }
            else if ((theDungeonData[ii, j] & PERIMETER) == PERIMETER)
            {
                down = false;
            }
        }
        for (int ii = height - 1; ii > i; ii--)
        {
            if (((theDungeonData[ii, j] & DOOR) == DOOR) || ((theDungeonData[ii, j] & CORRIDOR) == CORRIDOR))
            {
                up = true;
            }
            else if ((theDungeonData[ii, j] & PERIMETER) == PERIMETER)
            {
                up = false;
            }
        }
        if (down)
        {
            int ii = i;
            while (!((theDungeonData[ii, j] & DOOR) == DOOR) && !((theDungeonData[ii, j] & CORRIDOR) == CORRIDOR))
            {
                theDungeonData[ii, j] = theDungeonData[ii, j] | CORRIDOR;
                ii--;
            }
        }
        if (up)
        {
            int ii = i;
            while (!((theDungeonData[ii, j] & DOOR) == DOOR) && !((theDungeonData[ii, j] & CORRIDOR) == CORRIDOR))
            {
                theDungeonData[ii, j] = theDungeonData[ii, j] | CORRIDOR;
                ii++;
            }
        }
    }

    private void SearchHorizontal(int i, int j, int width, ref uint[,] theDungeonData)
    {
        bool left = false;
        bool right = false;
        for (int jj = 0; jj < j; jj++)
        {
            if (((theDungeonData[i, jj] & DOOR) == DOOR)  || ((theDungeonData[i, jj] & CORRIDOR) == CORRIDOR))
            {
                left = true;
            }
            else if ((theDungeonData[i, jj] & PERIMETER) == PERIMETER)
            {
                left = false;
            }
        }
        for (int jj = width - 1; jj > j; jj--)
        {
            if (((theDungeonData[i, jj] & DOOR) == DOOR) || ((theDungeonData[i, jj] & CORRIDOR) == CORRIDOR))
            {
                right = true;
            }
            else if ((theDungeonData[i, jj] & PERIMETER) == PERIMETER)
            {
                right = false;
            }
        }
        if (left)
        {
            int jj = j;
            while (!((theDungeonData[i, jj] & DOOR) == DOOR) && !((theDungeonData[i, jj] & CORRIDOR) == CORRIDOR))
            {
                theDungeonData[i, jj] = theDungeonData[i, jj] | CORRIDOR;
                jj--;
            }
        }
        if (right)
        {
            int jj = j;
            while (!((theDungeonData[i, jj] & DOOR) == DOOR) && !((theDungeonData[i, jj] & CORRIDOR) == CORRIDOR))
            {
                theDungeonData[i, jj] = theDungeonData[i, jj] | CORRIDOR;
                jj++;
            }
        }
    }

    private bool DoorsNotConnected(int height, int width, ref uint[,] theDungeonData)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (((theDungeonData[i, j] & DOOR) == DOOR) && ((theDungeonData[i, j] & CONNECTED) != CONNECTED))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void PlaceDoors(int height, int width, ref uint[,] theDungeonData)
    {
        for (int i = 1; i < height - 1; i++) //for all non-border cells
        {
            for (int j = 1; j < width - 1; j++)
            {
                if ((theDungeonData[i, j] & PERIMETER) == PERIMETER) // if it is a preimeter
                {
                    if (((theDungeonData[i + 1, j] & PERIMETER) == PERIMETER) && ((theDungeonData[i - 1, j] & PERIMETER) == PERIMETER)) // if above and below are perimeter
                    {
                        if (((theDungeonData[i, j + 1] & ROOM) == ROOM) && ((theDungeonData[i, j - 1] == NOTHING) || ((theDungeonData[i, j - 1] & ROOM) == ROOM))) // sides must be room/room or room/nil
                        {
                            ChancePutDoor(i, j, ref theDungeonData);
                        }
                        else if (((theDungeonData[i, j - 1] & ROOM) == ROOM) && ((theDungeonData[i, j + 1] == NOTHING) || ((theDungeonData[i, j + 1] & ROOM) == ROOM)))
                        {
                            ChancePutDoor(i, j, ref theDungeonData);
                        }
                    }
                    else if (((theDungeonData[i, j + 1] & PERIMETER) == PERIMETER) && ((theDungeonData[i, j - 1] & PERIMETER) == PERIMETER)) // if both sides are perimeter
                    {
                        if (((theDungeonData[i + 1, j] & ROOM) == ROOM) && ((theDungeonData[i - 1, j] == NOTHING) || ((theDungeonData[i - 1, j] & ROOM) == ROOM))) // sides must be room/room or room/nil
                        {
                            ChancePutDoor(i, j, ref theDungeonData);
                        }
                        else if (((theDungeonData[i - 1, j] & ROOM) == ROOM) && ((theDungeonData[i + 1, j] == NOTHING) || ((theDungeonData[i + 1, j] & ROOM) == ROOM)))
                        {
                            ChancePutDoor(i, j, ref theDungeonData);
                        }
                    }
                }
            }
        }
    }

    private void ChancePutDoor(int i, int j, ref uint[,] theDungeonData)
    {
        if (Random.value < DOOR_CHANCE)
        {
            theDungeonData[i, j] = BLOCKED | DOOR;
        }
    }

    private void PlaceStairs(int height, int width, ref uint[,] theDungeonData)
    {
        bool worked = false;
        int y1 = 0;
        int x1 = 0;

        while (worked == false)
        {
            worked = true;
            y1 = ((int)(((Random.value * 0.5F) + 0.5F) * (float)height));
            x1 = ((int)(((Random.value * 0.5F) + 0.5F) * (float)width));
            for (int i = (y1 - 2); i < (y1 + 5); i++)
            {
                for (int j = (x1 - 1); j < (x1 + 2); j++)
                {
                    if ((i >= height) || (j >= width) || (i < 0) || (j < 0))
                    {
                        worked = false;
                    }
                    else if ((theDungeonData[i, j] & BLOCKED) == BLOCKED)
                    {
                        worked = false;
                    }
                }
            }
        }
        theDungeonData[y1 + 1, x1] = theDungeonData[y1 + 1, x1] | STAIR_UP | BLOCKED;
        theDungeonData[y1 + 2, x1] = theDungeonData[y1 + 2, x1] | STAIR_UP | BLOCKED;
        theDungeonData[y1, x1] = theDungeonData[y1, x1] | DOOR | ENTRANCE | BLOCKED;
        Debug.Log("Stairs Placed");
    }

    private void SetPerimeters(int height, int width, ref uint[,] theDungeonData)
    {
        for (int i = 0; i < height; i++) //for all cells
        {
            for (int j = 0; j < width; j++)
            {
                if ((theDungeonData[i, j] & BLOCKED) != BLOCKED) // if blocked ignore
                {
                    for (int ii = 0; ii < 3; ii++) // check each neighbor
                    {
                        for (int jj = 0; jj < 3; jj++)
                        {
                            if (((i - 1 + ii) >= 0) && ((i - 1 + ii) < height) && ((j - 1 + jj) >= 0) && ((j - 1 + jj) < width)) //must stay within bounds
                            {
                                if ((theDungeonData[(i - 1 + ii), (j - 1 + jj)] & ROOM) == ROOM)
                                {
                                    theDungeonData[i, j] = theDungeonData[i, j] | PERIMETER;
                                    //if ((ii == 1) ^ (jj == 1))
                                    //{
                                    //    theDungeonData[i, j] = theDungeonData[i, j] | DOOR;
                                    //}
                                }
                                if ((theDungeonData[(i - 1 + ii), (j - 1 + jj)] & STAIR_UP) == STAIR_UP)
                                {
                                    theDungeonData[i, j] = theDungeonData[i, j] | PERIMETER;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void PlaceRooms(int roomSize, int height, int width, ref uint[,] theDungeonData)
    {
        int numOfRooms = (((height * width) / PERCENT_SPACE_ROOMS) / (roomSize * roomSize));
        bool retry = false;
        for (int i = 0; i < numOfRooms; i++)
        {
            retry = PlaceOneRoom(roomSize, height, width, ref theDungeonData);
            if (retry == true)
            {
                i--;
                retry = false;
            }
        }
    }

    private bool PlaceOneRoom(int roomSize, int height, int width, ref uint[,] theDungeonData)
    {
        int yCenter = ((int)(Random.value * (float)(height - (roomSize / 2)))) + (roomSize / 2);
        int xCenter = ((int)(Random.value * (float)(width - (roomSize / 2)))) + (roomSize / 2);
        int yRoomSize = (int)((Random.value - 0.5F) * ((float)sizeTable[roomSize]) * SIZE_VARIATION);
        int xRoomSize = (int)((Random.value - 0.5F) * ((float)sizeTable[roomSize]) * SIZE_VARIATION);

        for (int i = (yCenter - (yRoomSize / 2) - 2); i < (yCenter + (yRoomSize / 2) + 2); i++)
        {
            for (int j = (xCenter - (xRoomSize / 2) - 1); j < (xCenter + (xRoomSize / 2) + 1); j++)
            {
                if ((i >= height) || (j >= width) || (i < 0) || (j < 0))
                {
                   return true;
                }
                else if (((theDungeonData[i, j] & BLOCKED) == BLOCKED) && ((theDungeonData[i, j] & ROOM) != ROOM))
                {
                    return true;
                }
            }
        }

        for (int i = (yCenter - (yRoomSize / 2)); i < (yCenter + (yRoomSize / 2)); i++)
        {
            for (int j = (xCenter - (xRoomSize / 2)); j < (xCenter + (xRoomSize / 2)); j++)
            {
                theDungeonData[i, j] = BLOCK_ROOM;
            }
        }

        return false;
    }

    // shape values: 0 = rectangle, 1 = cross
    private void MaskShape(int shape, int height, int width, ref uint[,] theDungeonData)
    {
        if(shape == 0) {
            return;
        } else if(shape == 1) {
            int hSection = height / CROSS_SHAPE_FACTOR;
            int wSection = width / CROSS_SHAPE_FACTOR;

            for(int i = 0; i < height; i++) {
                for(int j = 0; j < width; j++) {
                    if(((i < hSection) || (i > (height - hSection))) && ((j < wSection) || (j > (width - wSection)))) {
                        theDungeonData[i, j] = theDungeonData[i, j] | BLOCKED;
                    }
                }
            }
        } else {
            // iz error, iz not good
        }
    }

    // Update is called once per frame
    void Update() {
	
	}
}
