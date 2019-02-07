using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public enum Type {
        doorNode =1,
        roomNode,
        secretDoorNode,
        wallNode,
        deadNode
    };

    public Type myType;

}
