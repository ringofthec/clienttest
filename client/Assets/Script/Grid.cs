using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static int WoodIndex = 3;
    public int x, y;
    public int TileType;
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private bool used = false;
    public void Start() {
        
    }
    public void SetSprite(int TileType) {
        this.TileType = TileType;
        spriteRenderer.sprite = sprites[TileType];

        // 增加碰撞用于检查射线
        spriteRenderer.gameObject.AddComponent<PolygonCollider2D>();
    }
    public bool CanBuild() {
        return TileType == WoodIndex && !used;
    }
    public void Used() {
        used = true;
    }
    public void SetPosition(int x, int y) {
        this.x = x;
        this.y = y;
        var pos1 = new Vector3(0.7f, 0.35f, 0) * x;
        var pos2 = new Vector3(-0.7f, 0.35f, 0) * y;
        transform.localPosition = pos1 + pos2;
        this.gameObject.name = $"x:{x} y:{y}";
    }
}
