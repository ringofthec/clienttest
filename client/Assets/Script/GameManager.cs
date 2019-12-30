﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class GameManager : MonoBehaviour
{
    private const int WoodIndex = 3;
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
    private Grid[,] grids = new Grid[128, 128];
    void Awake() {
        instance = this;
        instance.Init();
    }
    void Init() {
        var gridData = JsonConvert.DeserializeObject<GridData>(mapData.text);
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
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null) {
                CheckPlace(hit.collider.GetComponentInParent<Grid>());
            }
        }
    }
    void SetPosition(Transform trans, int x, int y) {
        var pos1 = new Vector3(0.7f, 0.35f, 0) * x;
        var pos2 = new Vector3(-0.7f, 0.35f, 0) * y;
        trans.localPosition = pos1 + pos2;
    }
    void CheckPlace(Grid grid) {
        if (grid == null || grid.index != WoodIndex) { return; }
        print(grid.x + " : " + grid.y + "   " + grid.index);
        if (grid.x > 0 && grids[grid.x - 1, grid.y].index == WoodIndex) {
            var obj = Instantiate(tableX);
            obj.transform.SetParent(terrain);
            SetPosition(obj.transform, grid.x - 1, grid.y);
            return;
        }
        if (grid.y > 0 && grids[grid.x, grid.y - 1].index == WoodIndex) {
            var obj = Instantiate(tableY);
            obj.transform.SetParent(terrain);
            SetPosition(obj.transform, grid.x, grid.y - 1);
            return;
        }
        if (grids[grid.x + 1, grid.y] != null && grids[grid.x + 1, grid.y].index == WoodIndex) {
            var obj = Instantiate(tableX);
            obj.transform.SetParent(terrain);
            SetPosition(obj.transform, grid.x, grid.y);
            return;
        }
        if (grids[grid.x, grid.y + 1] != null && grids[grid.x, grid.y + 1].index == WoodIndex) {
            var obj = Instantiate(tableY);
            obj.transform.SetParent(terrain);
            SetPosition(obj.transform, grid.x, grid.y);
            return;
        }
    }
}