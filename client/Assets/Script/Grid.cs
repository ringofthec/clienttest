using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int x, y;
    public int index;
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public void SetSprite(int index) {
        this.index = index;
        spriteRenderer.sprite = sprites[index];
        spriteRenderer.gameObject.AddComponent<PolygonCollider2D>();
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
