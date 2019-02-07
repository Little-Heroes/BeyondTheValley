using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public List<RoomBlock> cores = new List<RoomBlock>();
    public List<RoomBlock> rooms = new List<RoomBlock>();
    public List<RoomBlock> secretRooms = new List<RoomBlock>();
    public List<RoomBlock> bossRooms = new List<RoomBlock>();

    public RoomBlock wall;

    public int bossRoomDistFromCore;
    bool placedBossRoom = false;
    int numDoors = 0;
        
    RoomBlock coreRoom = new RoomBlock();
    RoomBlock currentRoom = new RoomBlock();
    RoomBlock lastRoom = new RoomBlock();
    List<RoomBlock> placedRooms;
    private void GenerateLevel() {

        PlaceCore();
        //start coroutine that generates the level
        StartCoroutine(GenrateLevel());
        //start a thing to show loading level ui
        
    }
    //place a core room
    private void PlaceCore() {
        coreRoom = Instantiate(cores[Random.Range(0, cores.Count)], Vector3.zero, Quaternion.identity);
        coreRoom.createdFrom = RoomBlock.Direction.NULL;
        coreRoom.ConstructRoom(ref numDoors);
        currentRoom = coreRoom;
        currentRoom.distFromCore = 0;
        placedRooms.Add(coreRoom);
    }

    //place treasure in empty room
    private void PlaceTreasure() {
        int rng = Random.Range(0, rooms.Count);
        RoomBlock tRoom = rooms[rng];
        int n = 0;

        //Quadratic probing if the selected room is inadiquet to avoid clumping (selecting the same set of rooms)
        while (tRoom.enemySpawnSpots.Count > 0) { n++; int newRNG = rng + (n * n); while (newRNG > rooms.Count) { newRNG -= rooms.Count; } tRoom = rooms[newRNG]; }
    }

    //place new room
    private bool PlaceRoom() {
        int northSpace = 1;
        int eastSpace = 1;
        int southSpace = 1;
        int westSpace = 1;
        RoomBlock tmpRm = lastRoom;
        lastRoom = currentRoom;

        //need to randomise which door is chosen
        //Place a room at the next door in clockwise order
        if (currentRoom.northNode != null) { if (currentRoom.northNode.myType == Node.Type.doorNode) {
                //detect collisions in a 3x3 room block size

                RoomBlock newRoom = GetRandomRoom(northSpace, eastSpace, southSpace, westSpace, RoomBlock.Direction.s);
                newRoom.distFromCore = currentRoom.distFromCore++;
                currentRoom = newRoom;
                return true;
            }
        }

        if (currentRoom.eastNode != null) { if (currentRoom.eastNode.myType == Node.Type.doorNode) {
                RoomBlock newRoom = GetRandomRoom(northSpace, eastSpace, southSpace, westSpace, RoomBlock.Direction.w);
                newRoom.distFromCore = currentRoom.distFromCore++;
                currentRoom = newRoom;
                return true;
            }
        }

        if (currentRoom.southNode != null) { if (currentRoom.southNode.myType == Node.Type.doorNode) {
                RoomBlock newRoom = GetRandomRoom(northSpace, eastSpace, southSpace, westSpace, RoomBlock.Direction.n);
                newRoom.distFromCore = currentRoom.distFromCore++;
                currentRoom = newRoom;
                return true;
            }
        }

         if (currentRoom.westNode != null) { if (currentRoom.westNode.myType == Node.Type.doorNode) {
                RoomBlock newRoom = GetRandomRoom(northSpace, eastSpace, southSpace, westSpace, RoomBlock.Direction.e);
                newRoom.distFromCore = currentRoom.distFromCore++;
                currentRoom = newRoom;
                return true;
            }
        }
        lastRoom = tmpRm;
        return false;
    }

    private RoomBlock GetRandomRoom(int northSpace, int eastSpace, int southSpace, int westSpace, RoomBlock.Direction d) {
        RoomBlock roomBlock = new RoomBlock();
        int rng;
        bool boss = false;
        //need a way to say stop generating along this chain when it gets too long
        //also need a way to check if you are generating next to a room 
        //so the rooms can generate with appropriate doors and also tell
        //the adjacent room that this one is now next to it
        if (!placedBossRoom && currentRoom.distFromCore == bossRoomDistFromCore - 1) {
            rng = Random.Range(0, bossRooms.Count); roomBlock = bossRooms[rng]; boss = true;
        }
        //randomly chooses a room from this areas list of rooms
        else { rng = Random.Range(0, rooms.Count); roomBlock = rooms[rng]; }
        //Quadratically probes the list of rooms if the one selected does not meet the requirments for the space
        int n = 0;
        while ((roomBlock.Dimentions.n > northSpace || roomBlock.Dimentions.e > eastSpace   //checks if the room can fit  
             || roomBlock.Dimentions.s > southSpace || roomBlock.Dimentions.w > westSpace)  //within the avalible space and 
            && roomBlock.HasDoorHere(d)) {                                                  //it has a door that can connect
            n++;
            if (boss) {
                int nRNG = rng + (n * n);                                   //if this is checking for boss rooms
                while (nRNG > bossRooms.Count) { nRNG -= bossRooms.Count; } //it will loop through the boss rooms quadratically
                roomBlock = bossRooms[nRNG];                                //looking for a suitable room
                placedBossRoom = true;
            }
            else {
                int newRNG = rng + (n * n);
                while (newRNG > rooms.Count) { newRNG -= rooms.Count; }
                roomBlock = rooms[newRNG];
            }
        }

        return roomBlock;
    }

    private IEnumerator GenrateLevel() {

        //Need a way to control number of rooms

        //currently will only go along a single chain of doors,
        //need a way to go back along the chain until there is a free door
        while (numDoors > 0) {
            if(lastRoom != currentRoom) currentRoom.parentRoom = lastRoom;
            currentRoom.ConstructRoom(ref numDoors);
            if (PlaceRoom()) { numDoors--; placedRooms.Add(currentRoom); }
            else { currentRoom = currentRoom.parentRoom; }
            yield return new WaitForSecondsRealtime(0.1f);
        }
        for (int i = placedRooms.Count; i > 0; i++) {
            placedRooms[i].FillOutRoom();
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
