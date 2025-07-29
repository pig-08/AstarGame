using System;
using UnityEngine;

namespace Code.Astar
{
    public class AstarNode  : IComparable<AstarNode>
    {

        public float G;
        public float F; //H값은 계산해서 사용한다.

        public Vector3 worldPosition;
        public Vector3Int cellPosition;
        public NodeData nodeData;

        public AstarNode parentNode;
        
        public int CompareTo(AstarNode other)
        {
            if (Mathf.Approximately(other.F, F))
                return 0;
            return other.F < F ? -1 : 1;
        }

        public override bool Equals(object obj)
        {
            if (obj is AstarNode node)
            {
                return Equals(node);
            }
            return false;
        }

        private bool Equals(AstarNode node)
        {
            if (node is null) return false;
            return cellPosition == node.cellPosition;
        }

        public override int GetHashCode() => cellPosition.GetHashCode();

        public static bool operator ==(AstarNode lhs, AstarNode rhs)
        {
            if (lhs is null)
            {
                if (rhs is null) return true;
                return false;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(AstarNode lhs, AstarNode rhs)
        {
            return !(lhs == rhs);
        }
    }
}