using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public bool displayGridGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float avoidanceRadius;
    Vector3 worldBottomLeft;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;


    void Awake() {
        nodeDiameter = nodeRadius * 2;
        avoidanceRadius = 0.8f;
    }

    // Used by Heap class.
    public int MaxSize {
        get {
            return gridSizeX * gridSizeY;
        }
    }

    // Sets gridWorldSize from input floats x and y.
    public void setGridWorld(float x, float y) {
        gridWorldSize.x = x;
        gridWorldSize.y = y;
    }

    // Creates grid based on Vector2 gridWorldSize and float nodeRadius. Checks are made for collisions with objects in the "unwalkable" layer for each node. 
    // Nodes that are within avoidanceRadius of an unwalkable object are given a high movement penalty.
    public void CreateGrid() {
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        grid = new Node[gridSizeX, gridSizeY];
        worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));
                int movementPenalty = (Physics2D.OverlapCircle(worldPoint, avoidanceRadius, unwalkableMask)) ? 100 : 0;
                grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenalty);
            }
        }
    }

    // Returns node in grid based on world coordinates.
    public Node NodeFromWorldPoint(Vector3 worldPosition) {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		
        return grid[x, y];
    }

    // Returns neighbours of input node in the grid.
    public List<Node> GetNeighbours(Node node) {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }


    // Updates walkability and weights on nodes within an area defined by a center, width and height of a square.
    public void UpdateArea(Vector3 center, float xsize, float ysize) {
        Vector3 bottomLeft = center - Vector3.right*(xsize/2 + avoidanceRadius)- Vector3.up*(ysize/2 + avoidanceRadius);
        Vector3 topRight = center + Vector3.right*(xsize/2 + avoidanceRadius) + Vector3.up*(xsize/2 + avoidanceRadius);

        Node nBL = NodeFromWorldPoint(bottomLeft);
        Node nTR = NodeFromWorldPoint(topRight);

        int xStart = nBL.gridX;
        int yStart = nBL.gridY;
        int xEnd = nTR.gridX;
        int yEnd = nTR.gridY;

        for (int x = xStart; x <= xEnd; x++) {
            for (int y = yStart; y <= yEnd; y++) {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));
                int movementPenalty = (Physics2D.OverlapCircle(worldPoint, avoidanceRadius, unwalkableMask)) ? 100 : 0;

                grid[x,y].walkable = walkable;
                grid[x,y].movementPenalty = movementPenalty;
                
            }
        }

    }

    // Draws grid with unwalkable nodes in red, walkable nodes with penalties in green and walkable nodes without penalty in white.
    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

        Color red = new Color(1, 0, 0, 0.5f);
        Color green = new Color(0, 1, 0, 0.5f);
        Color white = new Color(1, 1, 1, 0.5f);

        if (grid != null && displayGridGizmos) {
            foreach (Node n in grid) {
                Gizmos.color = (n.walkable) ? ((n.movementPenalty == 0) ? white : green) : red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }

}
