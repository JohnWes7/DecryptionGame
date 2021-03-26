using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeController : MonoBehaviour
{
    [Tooltip("管道预制体")]
    public List<GameObject> prefabs;

    [Tooltip("管道")]
    public SinglePipe[,] singlePipes;

    [SerializeField,Tooltip("谜题")]
    public int[,] map = {
        { 1, 4, 1 },
        { 3, 1, 2 },
        { 1, 2, 1 }
    };

    public Vector2 startPos = new Vector2(0, 0);
    public Vector2 endPos = new Vector2(2, 2);

    private void Start()
    {
        singlePipes = new SinglePipe[3, 3];

        for (int x = 0; x < map.GetLength(1); x++)
        {
            for (int y = 0; y < map.GetLength(0); y++)
            {
                GameObject temp = Instantiate<GameObject>(prefabs[map[y,x]], new Vector3(x, 0, -y), Quaternion.identity);
                SinglePipe pipe = temp.GetComponent<SinglePipe>();

                pipe.pos = new Vector2(x, -y);

                //初始化赋值dir
                switch (map[y,x])
                {
                    case 1:
                        pipe.dir = new Vector4(0, 0, 1, 1);
                        break;
                    case 2:
                        pipe.dir = new Vector4(0, 1, 0, 1);
                        break;
                    case 3:
                        pipe.dir = new Vector4(0, 1, 1, 1);
                        break;
                    case 4:
                        pipe.dir = new Vector4(1, 1, 1, 1);
                        break;
                    default:
                        break;
                }

                singlePipes[y, x] = pipe;

                temp.name = map[y, x].ToString();
            }
        }

        
    }

    public bool ScaneCorrect()
    {
        

        return false;
    }

    public bool ReFind(int x, int y)
    {
        //找到的节点标记为已被找到
        singlePipes[x, y].isFind = true;

        //判断是否是终点
        if (x == endPos.x&&y == endPos.y)
        {
            return true;
        }

        //相四个方向进行深度搜索
        for (int i = 0; i < 4; i++)
        {
            bool temp = false;

            switch (i)
            {
                //判断上方向上的链接
                case 0:
                    if (singlePipes[x,y].dir.x == 1 && IsAvailable(x, y - 1))
                    {
                        temp = ReFind(x, y - 1);
                        if (temp)
                        {
                            return true;
                        }
                    } 
                    break;
                //判断右方向的链接
                case 1:
                    if (singlePipes[x,y].dir.y == 1 && IsAvailable(x + 1, y))
                    {
                        temp = ReFind(x + 1, y);
                        if (temp)
                        {
                            return true;
                        }
                    }
                    break;
                //判断下方向的链接
                case 2:
                    if (singlePipes[x,y].dir.z == 1 && IsAvailable(x, y-1))
                    {
                        temp = ReFind(x, y - 1);
                        if (temp)
                        {
                            return true;
                        }
                    }
                    break;
                //判断左方向的链接
                case 3:
                    if (singlePipes[x,y].dir.w == 1 && IsAvailable(x - 1, y))
                    {
                        temp = ReFind(x - 1, y);
                        if (temp)
                        {
                            return temp;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        return false;
    }

    public bool IsAvailable(int targetX, int targetY)
    {
        if (targetX < singlePipes.GetLength(1) && targetY < singlePipes.GetLength(0) && singlePipes[targetX, targetY] != null && singlePipes[targetY, targetX].isFind == false)
        {
            return true;
        }

        return false;
    }
}


