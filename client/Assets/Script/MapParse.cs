using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class TerrainEleT {
    public int TileType {get;set;}
}

public class MapData {
    public List<List<TerrainEleT>> TerrainGrid { get; set; }
}

public class MapParse
{
    private static MapParse _instan;
    private MapParse() {}
    MapData mapdata;
    int width;
    int height;
    public static MapParse getInstance() {
        if (_instan == null) {
            _instan = new MapParse();
        }
        return _instan;
    }
    public bool loadmap(string mapfile) {
        if (mapfile == null) {
            Debug.LogError("map mapfile is null");
            return false;
        }

        if (mapfile == string.Empty) {
            Debug.LogError("map mapfile is empty");
            return false;
        }

        TextAsset txt = Resources.Load("MapData/" + mapfile) as TextAsset;
        if (txt == null) {
            Debug.LogError("map " + mapfile + " is load error");
            return false;
        }

        try {
            mapdata = JsonConvert.DeserializeObject<MapData>(txt.text);
            width   = mapdata.TerrainGrid.Count;
            height  = mapdata.TerrainGrid[0].Count;
            return true;
        } catch (System.Exception ex) {
            Debug.LogError("map " + mapfile + " parse error : " + ex.Message);
        }
        
        return false;
    }
    public MapData getmapdata() {
        return mapdata;
    }

    public int getwidth() {
        return width;
    }

    public int getheight() {
        return height;
    }

    public TerrainEleT getelement(int w, int h) {
        if (w < 0 || w >= getwidth()) {
            Debug.LogError("w " + w + " is out of range");
            return null;
        }

        if (h < 0 || h >= getheight()) {
            Debug.LogError("h " + h + " is out of range");
            return null;
        }

        return mapdata.TerrainGrid[w][h];
    }
}
