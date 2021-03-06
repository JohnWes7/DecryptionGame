using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PipeController : UseItems
{
    [Tooltip("管道预制体")]
    public List<GameObject> prefabs;

    [Tooltip("管道")]
    [SerializeField]
    public SinglePipe[,] singlePipes;

    [SerializeField, Tooltip("谜题")]
    public int[,] map = {
        { 1, 4, 1 },
        { 3, 1, 2 },
        { 1, 2, 1 }
    };


    public struct Pos
    {
        public int x;
        public int y;

        public Pos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public Pos[] dirArray = { new Pos(0, -1), new Pos(1, 0), new Pos(0, 1), new Pos(-1, 0) };

    public Vector2 startPos = new Vector2(0, 0);
    public Vector2 endPos = new Vector2(2, 2);

    
    private bool state = false;
    public bool State { get => state; }//谜题是否被解开



    [Tooltip("注视视角的时候相机位置")]
    public Transform CameraPos;

    [Tooltip("成功解锁回调")]
    public UnityEvent OnPuzzleTrue;


    private void Start()
    {
        //初始化
        InIt();

        //加入谜题解完的回调
        GamePanelController ui = GameObject.Find("Canvas/GamePanel").GetComponent<GamePanelController>();
        OnPuzzleTrue.AddListener(ui.BackFormLookAtCallBack);

        //开始的扫描
        ScaneCorrect();
    }

    /// <summary>
    /// 生成的 初始化函数
    /// </summary>
    public void CreatAndInIt()
    {
        singlePipes = new SinglePipe[3, 3];

        for (int x = 0; x < map.GetLength(1); x++)
        {
            for (int y = 0; y < map.GetLength(0); y++)
            {
                GameObject temp = Instantiate<GameObject>(prefabs[map[y, x]], new Vector3(x, 0, -y), Quaternion.identity, transform);
                SinglePipe pipe = temp.GetComponent<SinglePipe>();

                pipe.pos = new Vector2(x, -y);

                //初始化赋值dir
                switch (map[y, x])
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

                singlePipes[x, y] = pipe;

                temp.name = map[y, x].ToString();
            }
        }

    }

    /// <summary>
    /// 从子物体中获取谜题 初始化函数
    /// </summary>
    public void InIt()
    {
        singlePipes = new SinglePipe[3, 3];

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            { 
                singlePipes[x, y] = transform.Find(x.ToString() + y.ToString()).GetComponent<SinglePipe>();
            }
        }
    }

    /// <summary>
    /// 检测谜题是否完成
    /// </summary>
    /// <returns>是否完成</returns>
    public bool ScaneCorrect()
    {
        for (int x = 0; x < singlePipes.GetLength(0); x++)
        {
            for (int y = 0; y < singlePipes.GetLength(1); y++)
            {
                singlePipes[x, y].BeNormalColor();
                singlePipes[x, y].isFind = false;
            }
        }

        bool resaule = ReFind((int)startPos.x, (int)startPos.y);
        Debug.Log("终点: " + resaule);

        if (resaule)
        {
            OnPuzzleTrueCallBack();
        } 

        return resaule;
    }

    /// <summary>
    /// 路径查找递归
    /// </summary>
    /// <param name="x">x坐标</param>
    /// <param name="y">y坐标</param>
    /// <returns></returns>
    public bool ReFind(int x, int y)
    {
        //找到的节点标记为已被找到
        singlePipes[x, y].isFind = true;
        singlePipes[x, y].BeChangeColor();

        //判断是否是终点
        if (x == endPos.x && y == endPos.y && singlePipes[x, y].dir.z == 1)
        {
            return true;
        }

        //相四个方向进行深度搜索
        for (int i = 0; i < 4; i++)
        {
            bool temp = false;
            int TargetX;
            int TargetY;


            switch (i)
            {
                //判断上方向上的链接
                case 0:
                    TargetX = x + dirArray[i].x;
                    TargetY = y + dirArray[i].y;

                    if (singlePipes[x, y].dir.x == 1 && IsAvailable(TargetX, TargetY))
                    {
                        if (singlePipes[TargetX, TargetY].dir.z == 1)
                        {
                            Debug.Log("当前点:" + x + "," + y + "正在查找" + i + "方向: " + TargetX + "," + TargetY);
                            temp = ReFind(TargetX, TargetY);
                            if (temp)
                            {
                                return true;
                            }
                        }

                    }
                    break;
                //判断右方向的链接
                case 1:
                    TargetX = x + dirArray[i].x;
                    TargetY = y + dirArray[i].y;

                    if (singlePipes[x, y].dir.y == 1 && IsAvailable(TargetX, TargetY))
                    {
                        if (singlePipes[TargetX,TargetY].dir.w == 1)
                        {
                            Debug.Log("当前点:" + x + "," + y + "正在查找" + i + "方向: " + TargetX + "," + TargetY);
                            temp = ReFind(TargetX, TargetY);
                            if (temp)
                            {
                                return true;
                            }
                        }

                    }
                    break;
                //判断下方向的链接
                case 2:
                    TargetX = x + dirArray[i].x;
                    TargetY = y + dirArray[i].y;

                    if (singlePipes[x, y].dir.z == 1 && IsAvailable(TargetX, TargetY))
                    {
                        if (singlePipes[TargetX, TargetY].dir.x == 1)
                        {
                            Debug.Log("当前点:" + x + "," + y + "正在查找" + i + "方向: " + TargetX + "," + TargetY);
                            temp = ReFind(TargetX, TargetY);
                            if (temp)
                            {
                                return true;
                            }
                        }

                    }
                    break;
                //判断左方向的链接
                case 3:
                    TargetX = x + dirArray[i].x;
                    TargetY = y + dirArray[i].y;

                    if (singlePipes[x, y].dir.w == 1 && IsAvailable(TargetX, TargetY))
                    {
                        if (singlePipes[TargetX, TargetY].dir.y == 1)
                        {
                            Debug.Log("当前点:" + x + "," + y + "正在查找" + i + "方向: " + TargetX + "," + TargetY);
                            temp = ReFind(TargetX, TargetY);
                            if (temp)
                            {
                                return temp;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        return false;
    }

    /// <summary>
    /// 判断该坐标是否可获得
    /// </summary>
    /// <param name="targetX">坐标x</param>
    /// <param name="targetY">坐标y</param>
    /// <returns></returns>
    public bool IsAvailable(int targetX, int targetY)
    {
        
        if (targetX < singlePipes.GetLength(1) && targetX >= 0 && targetY < singlePipes.GetLength(0) && targetY >= 0 && singlePipes[targetX, targetY] != null)
        {
            if (!singlePipes[targetX, targetY].isFind)
            {
                return true;
            } 
        }

        return false;
    }

    /// <summary>
    /// 被使用
    /// </summary>
    public override void BeUse(RaycastHit hitInfo)
    {
        //如果被解开了就不能再被use
        if (state)
        {
            return;
        }

        //注视动作
        PlayerController.Instance.LookAt(CameraPos.position, CameraPos.rotation, LookType.PipePuzzle);
        //显示退出注视按钮
        GamePanelController ui = GameObject.Find("Canvas/GamePanel").GetComponent<GamePanelController>();

        ui.ShowBackFromLookAtButton();
    }

    /// <summary>
    /// 谜题解完后执行一遍该方法
    /// </summary>
    public void OnPuzzleTrueCallBack()
    {
        state = true;

        OnPuzzleTrue.Invoke();
    }
}


