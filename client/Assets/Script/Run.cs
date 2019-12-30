using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : MonoBehaviour
{
    bool maploadsuccess = false;
    MapParse map;

    void Start() {
        map = MapParse.getInstance();
        this.maploadsuccess = map.loadmap("data");
        if (!this.maploadsuccess)
            return ;

        this.genmap();
    }

    void genmap() {
        int w = map.getwidth();
        int h = map.getheight();
        for (int x = 0; x < w; ++x) {
            for (int y = 0; y < h; ++y) {
                TerrainEleT ele = map.getelement(x, y);
                genele(x, y, ele);
            }
        }
    }

    void genele(int x, int y, TerrainEleT ele) {
        int eletype = ele.TileType;
        
        // Debug.LogError("x = " + x + ", y = " + y + ". ele = " + eletype);
        //
        // var mapele = Resource.Load("")
    }

    void Update() {
        if (!this.maploadsuccess)
            return ;

        //
    }
}
