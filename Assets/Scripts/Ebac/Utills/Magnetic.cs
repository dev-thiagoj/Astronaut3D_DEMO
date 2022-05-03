using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetic : MonoBehaviour
{
    public float dist = .2f;
    public float coinSpeed = 3f;

    private void Update()
    {
        if(Vector3.Distance(transform.position, Player.Instance.transform.position) > dist) //- se quisesse que o magnetico acionasse pela distancia do player
        {
            coinSpeed++; //para dar aceleração ao movimento da moeda
            transform.position = Vector3.MoveTowards(transform.position, Player.Instance.transform.position, Time.deltaTime * coinSpeed);
        } 
    }
}
