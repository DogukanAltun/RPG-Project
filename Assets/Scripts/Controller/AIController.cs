using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Movement;
using System;

namespace RPG.Controller
{
    public class AIController : MonoBehaviour
    {
        Health health;
        [SerializeField] float ChaseDistance;
        [SerializeField] float SuspiciousTime= 5f;
        [SerializeField] float WaitingTime = 3f;
        [SerializeField] float waypointTolerence = 1f;
        [SerializeField] PatrolPath patrolpath;
        float PassedTime;
        float PassedTimeForWaiting;
        GameObject Player;
        Fighter fighter;
        Vector3 EnemyLocation;
        Mover move;
        int CurrentWayPointIndex=0;
        void Start()
        {
            Player = GameObject.FindWithTag("Player");
            fighter= GetComponent<Fighter>();
            health = GetComponent<Health>();
            EnemyLocation = transform.position;
            move = GetComponent<Mover>();

        }
        void Update()
        {
            if (health.IsDead())
            {
                return;
            }
           if( DistanceToPlayer() < ChaseDistance && fighter.CanAttack(Player))
            {
                PassedTime = 0f;
                fighter.Attack(Player);
            }
           else if (SuspiciousTime>PassedTime)
            {
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
            else
            {
                Vector3 nextPosition = EnemyLocation;
                if(patrolpath != null)
                {
                    if (AtWayPoint())
                    {
                        PassedTimeForWaiting = 0;
                        CycleWayPoint();
                    }
                    nextPosition = NextGetWayPoint();
                }
                if (WaitingTime < PassedTimeForWaiting)
                {
                    move.StartMoveAction(nextPosition);
                }
                
            }
            PassedTimeForWaiting += Time.deltaTime;
            PassedTime += Time.deltaTime;
        }

        private Vector3 NextGetWayPoint()
        {
            return patrolpath.getWayPointPosition(CurrentWayPointIndex);
        }

        private void CycleWayPoint()
        {
            CurrentWayPointIndex = patrolpath.GetNextIndex(CurrentWayPointIndex);
        }

        private bool AtWayPoint()
        {
            float distancewaypoint = Vector3.Distance(transform.position, NextGetWayPoint());
            return distancewaypoint < waypointTolerence;
        }

        private float DistanceToPlayer()
        {
            return Vector3.Distance(Player.transform.position, gameObject.transform.position);
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, ChaseDistance);
        }
    }
}
