using UnityEngine;

public class TileNode : MonoBehaviour
{
    public TileNode up, down, left, right;
    public int gridX, gridY;
    public bool isConnectedRight = false;
    public bool isConnectedDown = false;
}