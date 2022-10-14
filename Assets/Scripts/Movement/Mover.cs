using RPG.Combat;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour,IAction
    {
        NavMeshAgent NavMeshAgent;
        Health health;
        public void Start()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }
        public void Update()
        {
            if (health.IsDead())
            {
                NavMeshAgent.enabled = false;
            }
            UpdateAnimator();
        }
        public void StartMoveAction(Vector3 hit)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            GetComponent<Fighter>().Cancel();
            NavMeshAgent.destination = hit;
            NavMeshAgent.isStopped = false;
        }
        public void moveTo(Vector3 hit)
        {
            NavMeshAgent.destination = hit;
            NavMeshAgent.isStopped = false;
        }
        public void Cancel()
        {
            NavMeshAgent.isStopped = true;
        }
        public void UpdateAnimator()
        {
            Vector3 velocity = NavMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
        }
    }
}
