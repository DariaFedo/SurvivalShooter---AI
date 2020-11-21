using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Complete;

public class StateController : MonoBehaviour {

	public State currentState;
	public EnemyStats enemyStats;
	public Transform eyes;
	public State remainState;
	public List<Transform> wayPointsForAI;


	[HideInInspector] public NavMeshAgent navMeshAgent;
	[HideInInspector] public TankShooting tankShooting;
	[HideInInspector] public List<Transform> wayPointList;
	[HideInInspector] public int nextWayPoint;
	[HideInInspector] public Transform chaseTarget;
	[HideInInspector] public float stateTimeElapsed;

	private bool aiActive;
	
	void Awake () 
	{
		tankShooting = GetComponent<TankShooting> ();
		navMeshAgent = GetComponent<NavMeshAgent> ();
		SetupAI(true, wayPointsForAI);
	}

	public void SetupAI(bool aiActivationFromTankManager, List<Transform> wayPointsFromTankManager)
	{
		wayPointList = wayPointsFromTankManager;
		aiActive = aiActivationFromTankManager;
		if (aiActive)
		{
			navMeshAgent.enabled = true;
		}
		else
		{
			navMeshAgent.enabled = false;
		}
	}

	public void TransitionToState(State nextState)
	{
		if (nextState != remainState)
		{
			currentState = nextState;
			OnExitState();
		}
	}

	public bool CheckIfCountDownElapsed(float duration)
	{
		stateTimeElapsed += Time.deltaTime;
		return (stateTimeElapsed >= duration);
	}

	private void Update() // this function is monobehaviour so it will call awake and update function but scriptable object class will not !!!!
	{
		if (!aiActive) // if AI is not active - do nothing
			return;
		currentState.UpdateState(this); // if it is active then we pass "this" and this is where we start the chain of passing through a ref. to this state controller 
										// which is attatched to a tank into scriptable object assets
	}

	private void OnExitState()
	{
		stateTimeElapsed = 0;
	}

	private void OnDrawGizmos() // draw things in the scene that are not displayed in the game view
	{
		if (currentState != null && eyes != null)
		{
			Gizmos.color = currentState.sceneGizmoColor; // chceck the agent state by the color of the gizmo
			Gizmos.DrawWireSphere(eyes.position, enemyStats.lookSphereCastRadius); // draws a radius of two at the eyes phys. 
		}
	}

	

	

	


}