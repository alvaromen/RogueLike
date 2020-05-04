using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionGrid : MonoBehaviour
{
    float steps;
    private void Start(){

        Mesh planeMesh = GetComponent<MeshFilter>().mesh;
        Bounds bounds = planeMesh.bounds;
        // size in pixels
        steps = transform.localScale.x * bounds.size.x;

    }

    private void LateUpdate(){
        Vector2 player = GameObject.FindGameObjectWithTag("Player").transform.position;
        int xGrid = Mathf.FloorToInt(player.x) / 16;
        int yGrid = Mathf.FloorToInt(player.y) / 16;


        transform.position = new Vector2(xGrid, yGrid) * (steps + 2);

    }

}
