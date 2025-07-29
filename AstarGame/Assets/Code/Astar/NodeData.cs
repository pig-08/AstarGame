using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Astar
{
    [Serializable] //직렬화를 하지 않으면 구워진 데이터가 저장되지 않고 휘발되서 날아간다.
    public struct NodeData
    {
        public Vector3 worldPosition; //월드 좌표
        public Vector3Int cellPosition; //타일맵 좌표
        public List<LinkData> neighbors;

        public NodeData(Vector3 worldPosition, Vector3Int cellPosition)
        {
            this.worldPosition = worldPosition;
            this.cellPosition = cellPosition;
            neighbors = new List<LinkData>();
        }

        public void AddNeighbor(NodeData neighbor)
        {
            neighbors.Add(new LinkData
            {
                startPosition = worldPosition,
                startCellPosition = cellPosition,
                endPosition = neighbor.worldPosition,
                endCellPosition = neighbor.cellPosition,
                cost = Vector3Int.Distance(cellPosition, neighbor.cellPosition)
            });
        }

        public override int GetHashCode() => cellPosition.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj is NodeData data)
            {
                return data.cellPosition == cellPosition;
            }

            return false;
        }

        public static bool operator ==(NodeData lhs, NodeData rhs)
        {
            return lhs.Equals(rhs);
        }
        
        public static bool operator !=(NodeData lhs, NodeData rhs)
        {
            return !(lhs == rhs);
        }
    }
}