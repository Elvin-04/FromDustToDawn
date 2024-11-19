using UnityEngine;


namespace BehaviorTree
{
    public class FishPatrol : Node
    {
        FishBT bt;
        TerrainGenerator mapGen;

        Vector3 nextDestination;

        public FishPatrol(FishBT bt)
        {
            this.bt = bt;
            mapGen = TerrainGenerator.instance;
            
        }

        public override NodeState Evaluate()
        {
            if(!CanGoNextDestination(nextDestination))
                nextDestination = GetNextPosition();

            if (Vector3.Distance(bt.transform.position, nextDestination) > 0.2f)
            {
                Vector3 lookDirection = nextDestination - bt.transform.position;
                bt.transform.rotation = Quaternion.Slerp(bt.transform.rotation, Quaternion.LookRotation(lookDirection), bt.rotationSpeed * Time.deltaTime);
                bt.transform.position = Vector3.MoveTowards(bt.transform.position, nextDestination, Time.deltaTime * bt.speed);
            }
            else
            {
                nextDestination = GetNextPosition();
            }

            state = NodeState.RUNNING;
            return state;
        }

        private Vector3 GetNextPosition()
        {

            Vector3 startRay = new Vector3(Random.Range(0, mapGen.genOptions.chunkSize * mapGen.genOptions.meshWidthByChunk), 10, Random.Range(0, mapGen.genOptions.chunkSize * mapGen.genOptions.meshLengthByChunk));
            RaycastHit hit;

            if (Physics.Raycast(startRay, Vector3.down, out hit, Mathf.Infinity, bt.mask) && hit.point.y < mapGen.genOptions.waterLevel)
            {
                Vector3 toPosition = new Vector3(hit.point.x, ((mapGen.genOptions.waterLevel - hit.point.y) / 2) + hit.point.y, hit.point.z);
                if (CanGoNextDestination(hit.point))
                {
                    return toPosition;
                }
            }
            return GetNextPosition();
        }
        private bool CanGoNextDestination(Vector3 point)
        {
            Vector3 fromPosition = bt.transform.position;

            Vector3 toPosition = new Vector3(point.x, ((mapGen.genOptions.waterLevel - point.y) / 2) + point.y, point.z);
            Vector3 direction = toPosition - fromPosition;

            return !Physics.Raycast(bt.transform.position, direction, Vector3.Distance(bt.transform.position, toPosition), bt.mask);
        }
    }
}

