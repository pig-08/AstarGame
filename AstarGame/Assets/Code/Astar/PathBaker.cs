#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.Astar
{
    public class PathBaker : MonoBehaviour
    {
        [SerializeField] private Tilemap groundMap;
        [SerializeField] private Tilemap obstacleMap;
        [SerializeField] private BakedDataSO bakedData;
        
        [SerializeField] private bool isCornerCheck = true;
        [SerializeField] private bool isDrawGizmo = true;
        [SerializeField] private Color nodeColor, edgeColor;

        [ContextMenu("Bake map data")]
        private void BakeMapData()
        {
            Debug.Assert(groundMap != null && obstacleMap != null, "Target tilemap must be attached!");

            WritePointData();
            RecordNeighbors();
            SaveIfInUnityEditor();
        }

        private void SaveIfInUnityEditor()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(bakedData);
            AssetDatabase.SaveAssets();
#endif
        }

        private void WritePointData()
        {
            bakedData.ClearPoints(); //모든 포인트를 지우고 다시 적는다.
            groundMap.CompressBounds();

            BoundsInt mapBound = groundMap.cellBounds;

            for (int x = mapBound.xMin; x < mapBound.xMax; x++)
            {
                for (int y = mapBound.yMin; y < mapBound.yMax; y++)
                {
                    Vector3Int searchPoint = new Vector3Int(x, y);
                    if (CanMovePosition(searchPoint))
                    {
                        AddPoint(searchPoint);
                    }
                }
            }
        }
        
        private void RecordNeighbors()
        {
            foreach (NodeData nodeData in bakedData.points)
            {
                nodeData.neighbors.Clear(); //이웃데이터를 클리어 해주고, 8방향 체크
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if(x == 0 && y == 0) continue; //자기 자신은 포함하지 않는다.

                        Vector3Int nextPoint = new Vector3Int(x, y) + nodeData.cellPosition; //현재위치에서 방향 더해준다.

                        if (bakedData.TryGetNode(nextPoint, out NodeData adjacentNode))
                        {
                            if(CheckCorner(nextPoint, nodeData.cellPosition))
                                nodeData.AddNeighbor(adjacentNode);
                        }
                    }
                }
            }
        }

        private bool CheckCorner(Vector3Int nextPoint, Vector3Int currentPoint)
        {
            if (isCornerCheck == false) return true;

            return CanMovePosition(new Vector3Int(nextPoint.x, currentPoint.y)) &&
                    CanMovePosition(new Vector3Int(currentPoint.x, nextPoint.y)); 
        }


        private void AddPoint(Vector3Int searchPoint)
        {
            Vector3 worldPosition = groundMap.GetCellCenterWorld(searchPoint);
            bakedData.AddPoint(worldPosition, searchPoint);
        }

        private bool CanMovePosition(Vector3Int searchPoint)
        {
            bool hasObstacle = obstacleMap.HasTile(searchPoint);
            bool hasFloor = groundMap.HasTile(searchPoint); //해당 위치에 땅이 있는가?

            return hasObstacle == false && hasFloor;
        }
        
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (isDrawGizmo == false) return;

            foreach (NodeData nodeData in bakedData.points)
            {
                Gizmos.color = nodeColor;
                Gizmos.DrawWireSphere(nodeData.worldPosition, 0.15f);
                
                //여기에는 링크데이터를 그려줘야 한다.
                foreach (LinkData link in nodeData.neighbors)
                {
                    Gizmos.color = edgeColor;
                    DrawArrowGizmo(link.startPosition, link.endPosition);
                }
            }
        }

        private void DrawArrowGizmo(Vector3 start, Vector3 end)
        {
            Vector3 direction = (end - start).normalized;
            
            Vector3 arrowStart = end - direction * 0.2f;
            Vector3 arrowEnd = end - direction.normalized * 0.15f;
            const float arrowSize = 0.05f;

            Vector3 triangleA = arrowStart + (Quaternion.Euler(0, 0, -90f) * direction) * arrowSize;
            Vector3 triangleB = arrowStart + (Quaternion.Euler(0, 0, 90f) * direction) * arrowSize;
            
            Gizmos.DrawLine(start, arrowStart);
            Gizmos.DrawLine(triangleA, arrowEnd);
            Gizmos.DrawLine(triangleB, arrowEnd);
            Gizmos.DrawLine(triangleA, triangleB);
        }
#endif
    }
}