using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class GameManager : MonoBehaviour
{
    
    public class TerrainGrid {
        public int TileType;
    }
    public class GridData {
        public List<List<TerrainGrid>> TerrainGrid;
    }
    private static GameManager instance = null;
    public static GameManager Instance { get { return Instance; }}
    public Transform mainCamera;
    public Transform terrain;
    public TextAsset mapData;
    public Grid grid;
    public GameObject tableX, tableY;
    private bool isMove = false;
    private Vector3 lastMousePosition = Vector3.zero;
    private Grid[,] grids;
    private Grid last_select_grid;
    
    private GameObject tableXInstance, tableYInstance;
    public GameObject tableXObject {
        get {
            if (tableXInstance == null) {
                tableXInstance = Instantiate(tableX);
                tableXInstance.transform.SetParent(terrain);
                tableXInstance.GetComponent<SpriteRenderer>().sortingOrder = 100;
            }
            return tableXInstance;
        }
    }
    public GameObject tableYObject {
        get {
            if (tableYInstance == null) {
                tableYInstance = Instantiate(tableY);
                tableYInstance.transform.SetParent(terrain);
                tableYInstance.GetComponent<SpriteRenderer>().sortingOrder = 100;
            }
            return tableYInstance;
        }
    }
    void Awake() {
        instance = this;
        instance.Init();
    }
    void Init() {
        var gridData = JsonConvert.DeserializeObject<GridData>(mapData.text);

        grids = new Grid[gridData.TerrainGrid.Count, gridData.TerrainGrid[0].Count];

        for (var y = 0; y < gridData.TerrainGrid.Count; ++y) {
            var row = gridData.TerrainGrid[y];
            for (var x = 0 ; x < row.Count; ++x) {
                var newGrid = Instantiate(grid);
                newGrid.transform.SetParent(terrain);
                newGrid.SetSprite(row[x].TileType);
                newGrid.SetPosition(x, y);
                grids[x, y] = newGrid;
            }
        } 
    }
    void Update() {
        if (Input.GetMouseButtonDown(2)) {
            isMove = true;
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(2)) {
            isMove = false;
        }
        if (isMove) {
            var mousePosition = Input.mousePosition;
            if (lastMousePosition != mousePosition) {
                var delta = lastMousePosition - mousePosition;
                lastMousePosition = mousePosition;
                var width = (float)Screen.width / (float)Screen.height * 10f;
                mainCamera.position += new Vector3(delta.x / (float)Screen.width * width, delta.y / (float)Screen.height * 10, 0);
            }
        }
        RaycastHit2D hit = Physics2D.Raycast(mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null) {
            PreView(hit.collider.GetComponentInParent<Grid>());
            if (Input.GetMouseButtonDown(0)) {
                CheckPlace(hit.collider.GetComponentInParent<Grid>());
            }
        }
    }
    void SetPosition(Transform trans, int x, int y) {
        var pos1 = new Vector3(0.7f, 0.35f, 0) * x;
        var pos2 = new Vector3(-0.7f, 0.35f, 0) * y;
        trans.localPosition = pos1 + pos2;
    }

    void PreView(Grid grid) {
        if (last_select_grid == grid)
            return ;

        last_select_grid = grid;

        tableXObject.SetActive(false);
        tableYObject.SetActive(false);
        if (grid == null || !grid.CanBuild()) { return; }
        if (grid.x > 0 && grids[grid.x - 1, grid.y].CanBuild()) {
            tableXObject.SetActive(true);
            tableYObject.SetActive(false);
            SetPosition(tableXObject.transform, grid.x - 1, grid.y);
            return;
        }
        if (grid.y > 0 && grids[grid.x, grid.y - 1].CanBuild()) {
            tableXObject.SetActive(false);
            tableYObject.SetActive(true);
            SetPosition(tableYObject.transform, grid.x, grid.y - 1);
            return;
        }
        if (grids[grid.x + 1, grid.y] != null && grids[grid.x + 1, grid.y].CanBuild()) {
            tableXObject.SetActive(true);
            tableYObject.SetActive(false);
            SetPosition(tableXObject.transform, grid.x, grid.y);
            return;
        }
        if (grids[grid.x, grid.y + 1] != null && grids[grid.x, grid.y + 1].CanBuild()) {
            tableXObject.SetActive(false);
            tableYObject.SetActive(true);
            SetPosition(tableYObject.transform, grid.x, grid.y);
            return;
        }
    }
    void CheckPlace(Grid grid) {
        if (grid == null || !grid.CanBuild()) { return; }
        if (grid.x > 0 && grids[grid.x - 1, grid.y].CanBuild()) {
            var obj = Instantiate(tableX);;
            obj.transform.SetParent(terrain);
            SetPosition(obj.transform, grid.x - 1, grid.y);

            grid.Used();
            grids[grid.x - 1, grid.y].Used();
            return;
        }
        if (grid.y > 0 && grids[grid.x, grid.y - 1].CanBuild()) {
            var obj = Instantiate(tableY);
            obj.transform.SetParent(terrain);
            SetPosition(obj.transform, grid.x, grid.y - 1);

            grid.Used();
            grids[grid.x, grid.y - 1].Used();
            return;
        }
        if (grids[grid.x + 1, grid.y] != null && grids[grid.x + 1, grid.y].CanBuild()) {
            var obj = Instantiate(tableX);
            obj.transform.SetParent(terrain);
            SetPosition(obj.transform, grid.x, grid.y);

            grid.Used();
            grids[grid.x + 1, grid.y].Used();
            return;
        }
        if (grids[grid.x, grid.y + 1] != null && grids[grid.x, grid.y + 1].CanBuild()) {
            var obj = Instantiate(tableY);
            obj.transform.SetParent(terrain);
            SetPosition(obj.transform, grid.x, grid.y);

            grid.Used();
            grids[grid.x, grid.y + 1].Used();
            return;
        }
    }
}
