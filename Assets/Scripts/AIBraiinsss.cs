using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;

public class AIBraiinsss : MonoBehaviour
{

    //References for the target and the agent
    public NavMeshAgent agent;
    public GameObject target;
    public float range;
    public Transform centrePoint;
    public GameObject _origin;


    //Variables for the brain to work it's magic

    private bool canSeePlayer;
    [SerializeField]
    private Vector3 targetPosition;
    private Vector3 targetCurrentPosition;



    //We need to this to build a behaviour tree
    [SerializeField]
    private BehaviorTree patrollTree;
    [SerializeField]
    private AIFoV aiFov_Script;

    private void Awake()
    {
        StartCoroutine(playerPositionCheck());

        //if we see the player go to player if we don't pick random point in range and go there repeat 
        patrollTree = new BehaviorTreeBuilder(gameObject)
            .Selector()
                .Sequence("Looking for Player")
                    .Condition("We don't see the player", () => !canSeePlayer)
                    .Do("Patrol around", () =>
                    {
                        if (agent.remainingDistance <= agent.stoppingDistance) //done with path
                        {
                            Vector3 point;
                            if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
                            {
                                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                                agent.SetDestination(point);
                            }
                        }
                        return TaskStatus.Success;
                    })
                .End()
                .Sequence("Sees Player")
                    .Condition("We see the player", () => canSeePlayer)
                    .Do("set Player As target", () =>
                    {
                        targetCurrentPosition = target.transform.position;
                        agent.SetDestination(targetCurrentPosition);
                        return TaskStatus.Success;
                    })
                    .RepeatUntilSuccess()
                        .Do("Go to the player", () =>
                        {
                            //so basically we check the players current position and move there if it happends to be same as the position player actually is in 
                            //When we reach there then we end the game other wise we continue patrolling. 
                            if (Vector3.Distance(agent.transform.position, targetCurrentPosition) <= 5f)
                            {
                                if (Vector3.Distance(agent.transform.position, targetPosition) <= 5f)
                                {
                                    //CapturePlayer();
                                    Debug.Log("We reached target");
                                    return TaskStatus.Success;
                                }
                                return TaskStatus.Success;
                            }
                            else
                                return TaskStatus.Failure;
                        })
                    .End()
                .Sequence("Reset Agent Path")
                    .Do("Reset the path", () =>
                    {
                        agent.ResetPath();
                        return TaskStatus.Success;
                    })
                .End()
                .Sequence("GameReset")
                    .Condition("Game is no longer running", () => GameManagerthingy.gameActive == false)
                    .Do("Reset Position", () =>
                    {
                        agent.SetDestination(_origin.transform.position);
                        return TaskStatus.Success;
                    })
                .End()
                .Build();
    }

    //for now I will check if we can or can't see the player in here 
    private void Update()
    {
        patrollTree.Tick();
        if (aiFov_Script.canSeePlayer == true)
        {
            canSeePlayer = true;
        }
        else
            canSeePlayer = false;
    }

    /// <summary>
    /// this function will eventually reset the scene. Propably it will call static function from another class that does all the resetting stuff. 
    /// </summary>
    private void CapturePlayer()
    {
        //GameManagerthingy.ResetScene();
    }


    /// <summary>
    /// Pick a random point with in range around the AI character. 
    /// </summary>
    /// <param name="center"></param>
    /// <param name="range"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    /// <summary>
    /// Performance wise not the best but for testing purposes it works fine.
    /// Here we simply make sure the AI always knows where the player is this is to fix issues I ran into while trying to update it only when we see the player 
    /// It crashed the project due to missing reference error. ,
    /// </summary>
    /// <returns>some wait time</returns>
    IEnumerator playerPositionCheck()
    {
        WaitForSeconds checkTime = new WaitForSeconds(0.2f);
        while (GameManagerthingy.gameActive)
        {
            targetPosition = target.transform.position;
            yield return checkTime;
        }
    }
}
