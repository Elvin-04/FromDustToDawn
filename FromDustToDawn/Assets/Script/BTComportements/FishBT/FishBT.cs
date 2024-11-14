using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class FishBT : Tree
    {

        [Header("Parameters")]
        public LayerMask mask;
        public float speed = 2f;
        public float rotationSpeed = 2f;

        protected override Node SetupTree()
        {

            Node root = new Selector(new List<Node>
            {
                new FishPatrol(this),

            });

            return root;
        }
    }
}
