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
                //new Sequence(new List<Node>
                //{
                //    new NeedToGoLake(this),
                //    new GoToLake(this)
                //}),
                //new Sequence(new List<Node>
                //{
                //    new NeedToHarvest(this),
                //    new VakoomHarvest(this)
                //}),
                //new VakoomPatrol(transform, this)

            });

            return root;
        }

        public static void PrintMsg(string a)
        {
            Debug.Log(a);
        }
    }
}
