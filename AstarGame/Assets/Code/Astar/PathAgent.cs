using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Astar
{
    public abstract class PathAgent : MonoBehaviour
    {
        [SerializeField] private BakedDataSO bakedData;

        private Dictionary<Type, IAgentCompo> _agentCompoDic = new Dictionary<Type, IAgentCompo>();

        public virtual void Awake()
        {
            GetComponentsInChildren<IAgentCompo>().ToList().ForEach(v => {
                _agentCompoDic.Add(v.GetType(), v);
                v.Init(this);
            });
        }

        public int GetPath(Vector3Int startPosition, Vector3Int destination, Vector3[] pointArr)
        {
            List<AstarNode> result = CalculatePath(startPosition, destination);
            //result를 기반으로 pointArr에 채워넣어준다.
            int cornerIndex = 0;
            if (result.Count > 0)
            {
                pointArr[cornerIndex] = result[0].worldPosition;
                cornerIndex++;

                for (int i = 1; i < result.Count - 1; i++)
                {
                    if(cornerIndex >= pointArr.Length) break;

                    Vector3Int beforeDirection = result[i].cellPosition - result[i - 1].cellPosition;
                    Vector3Int nextDirection = result[i + 1].cellPosition - result[i].cellPosition;

                    if (beforeDirection != nextDirection)
                    {
                        pointArr[cornerIndex] = result[i].worldPosition;
                        cornerIndex++;
                    }
                }

                pointArr[cornerIndex] = result[^1].worldPosition; //최종목적지는 넣어준다.
                cornerIndex++;
            }

            return cornerIndex;
        }

        private List<AstarNode> CalculatePath(Vector3Int start, Vector3Int end)
        {
            PriorityQueue<AstarNode> openList = new PriorityQueue<AstarNode>();
            List<AstarNode> closeList = new List<AstarNode>();
            List<AstarNode> path = new List<AstarNode>();

            bool result = false;

            if (bakedData.TryGetNode(start, out NodeData startNodeData) == false)
                return path;
            if (bakedData.TryGetNode(end, out NodeData endNodeData) == false)
                return path;

            openList.Push(new AstarNode
            {
                nodeData = startNodeData,
                cellPosition = startNodeData.cellPosition,
                worldPosition = startNodeData.worldPosition,
                parentNode = null,
                G = 0, F = CalcH(startNodeData.cellPosition, endNodeData.cellPosition)
            });

            while (openList.Count > 0)
            {
                AstarNode currentNode = openList.Pop(); //가장 F값이 작은 녀석이 따라서 오게된다.
                foreach (LinkData link in currentNode.nodeData.neighbors)
                {
                    //해당 노드가 이미 방문한 노드인지를 검사해.
                    bool isVisited = closeList.Any(n => n.cellPosition == link.endCellPosition);
                    if(isVisited) continue; //이미 방문한 노드라면 패스한다.
                    
                    if(bakedData.TryGetNode(link.endCellPosition, out NodeData nextNode) == false)
                        continue;

                    float newG = link.cost + currentNode.G;
                    //현재 이동하려고 하는 링크의 값에 여태까지 이동한 G값을 더해준다.

                    AstarNode nextAstarNode = new AstarNode
                    {
                        nodeData = nextNode,
                        cellPosition = nextNode.cellPosition,
                        worldPosition = nextNode.worldPosition,
                        parentNode = currentNode,
                        G = newG, F = newG + CalcH(nextNode.cellPosition, endNodeData.cellPosition)
                    };

                    AstarNode existNode = openList.Contains(nextAstarNode);
                    if (existNode != null)
                    {
                        if (nextAstarNode.G < existNode.G)
                        {
                            existNode.G = nextAstarNode.G;
                            existNode.F = nextAstarNode.F;
                            existNode.parentNode = nextAstarNode.parentNode;
                        }
                    }
                    else
                    {
                        openList.Push(nextAstarNode);
                    }
                }// end of foreach
                
                closeList.Add(currentNode); //계산이 끝난 방문 노드는 closeList로 들어간다.

                if (currentNode.nodeData == endNodeData)
                {
                    result = true; //목적지에 도착한거
                    break;
                }
            } //end of while

            if (result)
            {
                AstarNode last = closeList[^1]; //마지막으로 방문한점을 시작점으로 잡고 
                while (last.parentNode != null)
                {
                    path.Add(last);
                    last = last.parentNode; //쭉 따라서 올라간다.
                }
                path.Add(last);
                path.Reverse(); //순서를 역순으로 변경하면 정순이 된다.
            }

            return path;
        }
        private float CalcH(Vector3Int start, Vector3Int end) => Vector3Int.Distance(start, end);

        public T GetCompo<T>() where T : IAgentCompo
        {
            if (_agentCompoDic.TryGetValue(typeof(T), out IAgentCompo component)) return (T)component;
            else return default(T);
        }
    }
}