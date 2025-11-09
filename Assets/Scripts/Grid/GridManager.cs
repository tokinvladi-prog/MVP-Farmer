using System.Xml.Serialization;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private Vector2Int _gridSize = new(10, 10);
    [SerializeField]
    private GameObject _tilePrefab;

    private FarmTile[,] grid;

    public static GridManager Instance;

    private void OnDrawGizmosSelected()
    {
        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(new Vector3(x, 0, y), new Vector3(1, 0.1f, 1));
            }
        }
    }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else Instance = this;

        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new FarmTile[_gridSize.x, _gridSize.y];

        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                Vector3 position = new(x, 0, y);
                GameObject tile = Instantiate(_tilePrefab, position, Quaternion.identity);

                grid[x, y] = tile.GetComponent<FarmTile>();
            }
        }
    }

    public FarmTile GetTile(Vector3 worldPostion)
    {
        int x = Mathf.RoundToInt(worldPostion.x);
        int y = Mathf.RoundToInt(worldPostion.z);

        if (x >= 0 && x <= _gridSize.x && y >= 0 && y <= _gridSize.y)
        {
            return grid[x, y];
        }

        return null;
    }
}
