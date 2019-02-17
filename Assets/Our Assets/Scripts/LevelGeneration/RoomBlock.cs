using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBlock : MonoBehaviour {

    public enum Direction { n, e, s, w, NULL};

    public Node northNode, eastNode, southNode, westNode;

    public struct CoordDimentions {
        public int n, e, s, w;
        public CoordDimentions (int _n, int _e, int _s, int _w) { n = _n; e = _e; s = _s; w = _w; }
    }

    public CoordDimentions Dimentions {
        //TODO: work through connecting room blocks in a more complex way to determine size
        //will currently only work for rooms that have the largest dimetions directly 
        //coming out of the centre, eg plus shapes and squares
        get {
            CoordDimentions dimentions = new CoordDimentions(0, 0, 0, 0);

            //determine the y dimentions of any set of room blocks
            if (southBlock != null) {
                if (southBlock.southBlock == null) dimentions.s = 1;
                else dimentions.s = 2;
            }
            if (northBlock != null) {
                if (northBlock.northBlock == null) dimentions.n = 1;
                else dimentions.n = 2;
            }

            //determine the x dimetions of any set of room blocks
            if (eastBlock != null) {
                if (eastBlock.eastBlock == null) dimentions.e = 1;
                else dimentions.e = 2;
            }
            if (westBlock != null) {
                if (westBlock.westBlock == null) dimentions.w = 1;
                else dimentions.w = 2;
            }

            return dimentions;
        }
    }

    private int NumEnemies
    {
        get
        {
            int i = 0;
            //foreach (Enemy enemy in GetComponentInChildren<Enemy>()) {
            //    i++;
            //}}
            return i;
        }
    }

    private bool cleared = false;

    public int getNumDoors() {
        int retVal = 0;
        if ((int)northNode.myType == 1) retVal++; if ((int)eastNode.myType == 1) retVal++;
        if ((int)southNode.myType == 1) retVal++; if ((int)westNode.myType == 1) retVal++; 
        return retVal;
    }

    [HideInInspector]
    public int distFromCore;

    #region The things to be within this room block

    //obstacle spots
    public List<Transform> obstacleSpots;
    //obstacles
    public List<GameObject> obstacles;

    //decoration spots
    public List<Transform> decorationSpots;
    //avalible decorations
    public List<GameObject> decorations;

    //enemy spawn spots
    public List<Transform> enemySpawnSpots;
    //avalible enemies
    public List<GameObject> enemies;

    //avalible rewards
    public List<GameObject> rewards;

    #endregion

    [Tooltip("These are the room blocks that will form a larger room," +
        "Only fill out what will be there")]
    public RoomBlock northBlock, eastBlock, southBlock, westBlock;

    [HideInInspector]
    public RoomBlock parentRoom;

    [HideInInspector]
    public Direction createdFrom;

    public RoomBlock ConstructRoom(ref int numDoors) {
        //--------------------------
        //disables nodes over the room which created this one
        //--------------------------
        switch ((int)createdFrom) {
            case    1: northNode = null; break;
            case    2: eastNode  = null; break;
            case    3: southNode = null; break;
            case    4: westNode  = null; break;
            default  : break;
        }
        RoomBlock outPut = new RoomBlock();

        numDoors += getNumDoors();
        //--------------------------
        //do room construction 
        //--------------------------

        //Need to do collision checks on nodes before spawning rooms

        #region roomConstruction
        if (northNode.myType    == Node.Type.roomNode) {
            RoomBlock _northBlock =                                                                     //Create the next room block for this room
                Instantiate(northBlock, northNode.transform.position, northNode.transform.rotation);    //tell it its distance from the core room
            _northBlock.createdFrom = Direction.s;                                                      //tell it where it was created from so it
            _northBlock.distFromCore = distFromCore;                                                    //ignores that direction in it's creation
            outPut.parentRoom = this;
            _northBlock.ConstructRoom(ref numDoors);
            northBlock = _northBlock;
        }
        if (eastNode.myType     == Node.Type.roomNode) {
            RoomBlock _eastBlock = 
                Instantiate(eastBlock, eastNode.transform.position, eastNode.transform.rotation);
            _eastBlock.createdFrom = Direction.w;
            _eastBlock.distFromCore = distFromCore;
            outPut.parentRoom = this;
            outPut = _eastBlock.ConstructRoom(ref numDoors);
        }
        if (southNode.myType    == Node.Type.roomNode) {
            RoomBlock _southBlock = 
                Instantiate(southBlock, southNode.transform.position, southNode.transform.rotation);
            _southBlock.createdFrom = Direction.n;
            _southBlock.distFromCore = distFromCore;
            outPut.parentRoom = this;
            outPut = _southBlock.ConstructRoom(ref numDoors);
        }
        if (westNode.myType     == Node.Type.roomNode) {
            RoomBlock _westBlock = 
                Instantiate(westBlock, westNode.transform.position, westNode.transform.rotation);
            _westBlock.createdFrom = Direction.e;
            _westBlock.distFromCore = distFromCore; 
            outPut.parentRoom = this;
            outPut = _westBlock.ConstructRoom(ref numDoors);
        }
#endregion

        //--------------------------
        //generate walls
        //--------------------------
        if (northNode.myType == Node.Type.secretDoorNode || northNode.myType == Node.Type.wallNode) {
            /*create a wall room block*/
        }
        if (eastNode.myType  == Node.Type.secretDoorNode || northNode.myType == Node.Type.wallNode) {
            /*create a wall room block*/
        }
        if (southNode.myType == Node.Type.secretDoorNode || northNode.myType == Node.Type.wallNode) {
            /*create a wall room block*/
        }
        if (westNode.myType  == Node.Type.secretDoorNode || northNode.myType == Node.Type.wallNode) {
            /*create a wall room block*/
        }

        //--------------------------
        //fill the room
        //--------------------------
        FillOutRoom();


        return outPut;
    }

    public bool HasDoorHere(Direction d) {
        switch ((int)d) {
            case 1: if (northNode.myType == Node.Type.doorNode) return true; break;
            case 2: if (eastNode.myType == Node.Type.doorNode) return true; break;
            case 3: if (southNode.myType == Node.Type.doorNode) return true; break;
            case 4: if (westNode.myType == Node.Type.doorNode) return true; break;
            default: break;
        }
        return false;
    }


    #region build within room
    public void FillOutRoom() {
        SpawnObstacles();
        DecorateRoom();
    }

    private void SpawnObstacles()
    {
        
    }

    private void DecorateRoom() {
        /*spawn in random decorative things in the room at the decorate spots*/
    }

    private void CheckCleared() {
        if(NumEnemies == 0)
        {
            SpawnRewards();
            OpenDoors();
        }
    }
    #endregion

    #region player interactions with room
    private void OnEnterRoom()
    {
        //check if room is cleared
        if (cleared)
            return;
        //if not close all the doors and
        //wake the enemies in the room
        if(WakeEnemies())
            CloseDoors();
        //when the room get's cleared
        //StartCoroutine(CheckCleared());

        //spawn rewards in the closest tile to the centre
        //open the doors
        //do nothing if the room is cleared
    }

    private void CloseDoors()
    {
        //close the doors in the room
    }


    private bool WakeEnemies() {
        
        if (NumEnemies == 0) return false;
        return true;
    }

    private void SpawnRewards() {
        float rng = Random.Range(0, 1);
        float rsc = 0.2f /*rewardSpawnChance*/;
        while (rsc > 1)
        {
            rsc -= 1;
            int reward = Random.Range(0, rewards.Count);
            Instantiate(rewards[reward]);
        }
        if(rng <= rsc)
        {
            int reward = Random.Range(0, rewards.Count);
            Instantiate(rewards[reward]);
        }
    }

    private void OpenDoors()
    {
        //do the opposite of close doors
    }
    #endregion
}
