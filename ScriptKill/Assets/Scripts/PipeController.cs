using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeController : MonoBehaviour
{
    [Tooltip("管道预制体")]
    public GameObject prefab;

    [Tooltip("管道")]
    public SinglePipe[,] singlePipes;

    [SerializeField,Tooltip("谜题")]
    public int[,] map = {
        { 1, 4, 1 },
        { 3, 1, 2 },
        { 1, 2, 1 }
    };

    private void Start()
    {
        singlePipes = new SinglePipe[3, 3];

        for (int x = 0; x < map.GetLength(1); x++)
        {
            for (int y = 0; y < map.GetLength(0); y++)
            {
                GameObject temp = Instantiate<GameObject>(prefab, new Vector3(x, 0, -y), Quaternion.identity);
                temp.name = map[y, x].ToString();
            }
        }
    }
}
