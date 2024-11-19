using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree
{
    public class ChickenBT : Tree
    {
        [Header("References")]
        public NavMeshAgent agent;

        [Header("Parameters")]
        public LayerMask mask;
        public float speed = 2f;
        public float rotationSpeed = 2f;
        public float peckTime = 1f;

        [HideInInspector] public bool isPlaced;

        protected override Node SetupTree()
        {
            isPlaced = false;
            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new ChickenPatrol(this),

                }),
                

            });

            return root;
        }
    }
}
