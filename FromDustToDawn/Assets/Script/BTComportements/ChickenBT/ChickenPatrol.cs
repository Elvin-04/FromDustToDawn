using Unity.AI.Navigation;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree
{
    public class ChickenPatrol : Node
    {
        ChickenBT bt;
        NavMeshPath path = new NavMeshPath();

        bool isMoving;
        bool needToPeck;

        public ChickenPatrol(ChickenBT bt)
        {
            this.bt = bt;
        }


        public override NodeState Evaluate()
        {
            if (!bt.isPlaced) return NodeState.FAILURE;

            if (bt.agent.hasPath && bt.agent.remainingDistance > bt.agent.stoppingDistance)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
                if (needToPeck)
                {
                    Peck();
                }
                else
                {
                    SetNextDestination();

                }
            }

            state = NodeState.RUNNING;
            return state;
        }

        private void SetNextDestination()
        {
            Debug.Log(bt.transform.position);
            GenerationOptions mapGen = TerrainGenerator.instance.genOptions;
            Vector3 startRay = new Vector3(Random.Range(0, mapGen.chunkSize * mapGen.meshWidthByChunk), 10, Random.Range(0, mapGen.chunkSize * mapGen.meshLengthByChunk));
            RaycastHit hit;

            if (Physics.Raycast(startRay, Vector3.down, out hit, Mathf.Infinity, bt.mask) && hit.point.y > mapGen.waterLevel)
            {
                if(!isMoving)
                {
                    bt.agent.CalculatePath(hit.point, path);
                    if(path.status == NavMeshPathStatus.PathComplete)
                    {
                        needToPeck = true;
                        bt.agent.SetDestination(hit.point);
                        return;
                    }

                   
                }
                
            }
            SetNextDestination();
        }

        private void Peck()
        {
            Debug.Log("Peck");
            //Start animation
            needToPeck = false;
        }

    }

}
