using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapRenderer : MonoBehaviour
{

    int minimapRoomSize;
    int rows;
    int cols;

    int renderWidth = 256;
    int renderHeight = 256;

    Texture2D pixels;

    Vector2 player;

    void Start(){
        minimapRoomSize = 10;
        rows = 15 * minimapRoomSize;
        cols = 15 * minimapRoomSize;

        pixels = new Texture2D(renderWidth, renderHeight, TextureFormat.RGBA4444, false);
    }

    void LateUpdate(){
        PaintMinimap();
    }

    void OnGUI(){
        GUI.DrawTexture(new Rect(332, 101, 166, 166), pixels, ScaleMode.ScaleToFit, false);
    }

    void PaintMinimap(){
        player = GameObject.FindGameObjectWithTag("Player").transform.position;

        int xGrid = Mathf.FloorToInt(player.x) / 16;
        int yGrid = Mathf.FloorToInt(player.y) / 16;

        for (int i = xGrid; i < minimapRoomSize; i++)
        {
            for (int j = yGrid; j < minimapRoomSize; j++)
            {
                pixels.SetPixel(i, j, Color.black);
            }
        }
        // Apply all SetPixel calls
        pixels.Apply();
    }

}
