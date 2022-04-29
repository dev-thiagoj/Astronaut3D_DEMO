using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//COISAS PARA FAZER:

//arrumar para que qdo o inimigo estiver morto não matar o player com colisão ainda
//arrumar para qdo for morto, congelar a posição
//arrumar congelamento de eixos na perseguição

namespace Enemy
{
    public class EnemyWalk : EnemieBase
    {
        [Header("Waypoints")]
        public GameObject[] waypoints;
        public float minDistance = 1f;
        public float speed = 1f;

        private int _index = 0;

        public override void Update()
        {
            base.Update();

            EnemyMove();

        }

        public void EnemyMove()
        {
            if (!_startPursuit)
            {
                if (Vector3.Distance(transform.position, waypoints[_index].transform.position) < minDistance)
                {
                    _index++;

                    if (_index >= waypoints.Length)
                    {
                        _index = 0;
                    }
                }

                transform.position = Vector3.MoveTowards(transform.position, waypoints[_index].transform.position, Time.deltaTime * speed);
                transform.LookAt(waypoints[_index].transform.position);
            }
            
        }

        protected override void Kill()
        {
            base.Kill();
            speed = 0;
        }
    }

}
