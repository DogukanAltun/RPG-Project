using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Combat;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour,IAction
    {
        [SerializeField] float timebetweenattack= 1f;
        [SerializeField] float weaponRange;
        [SerializeField] float weaponDamage;
        Health targetObject;
        float timeSinceLastAttack;
        public void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if(targetObject == null)
            {
                return;
            }
            if(targetObject.IsDead()== true)
            {
                GetComponent<Animator>().ResetTrigger("Attack");
                Cancel();
                return;
            }
            bool isInRange = GetIsInRange();
            if(isInRange == false)
            {
                GetComponent<Mover>().moveTo(targetObject.transform.position);
            }
            else
            {
                AttackMethod();
                GetComponent<Mover>().Cancel();
            }
        }

        private void AttackMethod()
        {
            transform.LookAt(targetObject.transform);
            if (timeSinceLastAttack > timebetweenattack)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
               
            }
        }
        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("StopAttack");
            GetComponent<Animator>().SetTrigger("Attack");
        }
        void Hit()
        {
            if(targetObject== null)
            {
                return;
            }
            targetObject.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange() 
        {
            return Vector3.Distance(transform.position, targetObject.transform.position) < weaponRange;
        }
        public void Attack(GameObject target)
        {
            print("EnemySaldýrdý!");
            GetComponent<ActionScheduler>().StartAction(this);
            targetObject = target.GetComponent<Health>();
            
        }
        public void Cancel()
        {
            StopAttack();
            targetObject = null;
        }
        public bool CanAttack(GameObject target)
        {
            if(target== null)
            {
                return false;
            }
            Health healthToTest = GetComponent<Health>();
            return healthToTest != null && !healthToTest.IsDead();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("StopAttack");
        }
    }
}
