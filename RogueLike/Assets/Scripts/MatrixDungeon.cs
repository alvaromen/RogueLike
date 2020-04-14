using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixDungeon : MonoBehaviour
{
    static int dungeonWidth = 400;
    static int dungeonHeight = 400;

    int[,] matrixDungeon = new int[dungeonWidth, dungeonHeight];

    void generateDungeonMatrix()
    {
        for (int i = 0; i < dungeonWidth; i++)
        {
            for (int j = 0; j < dungeonHeight; j++)
            {
                //generate the whole map in this matrix so we can check for neighbourhood
            }
        }
    }
}
