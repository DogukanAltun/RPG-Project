using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;

namespace RPG.Controller
{


    public class PlayerController : MonoBehaviour
    {
        Health health;

        private void Start()
        {
            health = GetComponent<Health>();
        }

        void Update()
        {
            if (health.IsDead())
            {
                return;
            }
            if (InteractWithCombat() == true)
            {
            return;
            }
            if (InteractWithMovement() == true)
            {
                return;
            }
        }

        private bool InteractWithMovement()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit Hit;
                bool hasHit = Physics.Raycast(GetMouseRay(), out Hit);
                if (hasHit == true)
                {
                    GetComponent<Mover>().StartMoveAction(Hit.point);
                    Debug.Log("Hareket Edildi!");
                }
                return true;
            }
            return false;
        }
        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach(RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null)
                {
                    continue;
                }
                if(!GetComponent<Fighter>().CanAttack(target.gameObject)) 
                {
                    continue;
                }
                if (target == null)
                {
                    continue;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                    Debug.Log("Attack yapýldý!");
                }
                return true;
            }
            return false;
        }

        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }


    }
}