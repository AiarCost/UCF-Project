using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour, IActorTemplate
{
    int health;
    int travelSpeed;
    int fireSpeed;
    int hitPower;

    //wave enemy
    [SerializeField]
    float verticalSpeed = 2;
    [SerializeField]
    float verticalAmplitude = 1;
    Vector3 sineVer;
    float time;


    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    public void ActorStats(SOActorModel actorModel)
    {
        health = actorModel.health;
        travelSpeed = actorModel.speed;
        hitPower = actorModel.hitPower;
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        //if player or their bullet hits you...
        if (col.CompareTag("Player")){
            if (health >= 1)
            {
                health -= col.GetComponent<IActorTemplate>().SendDamage();
            }
            if(health <= 0)
            {
                Die();
            }
        }
    }

    public void TakeDamage(int incomingDamage)
    {
        health -= incomingDamage;
    }
    public int SendDamage()
    {
        return hitPower;
    }

    public void Attack()
    {
        time += Time.deltaTime;
        sineVer.y = Mathf.Sin(time * verticalSpeed) * verticalAmplitude;
        transform.position = new Vector3(transform.position.x + travelSpeed * Time.deltaTime, transform.position.y + sineVer.y,
                                         transform.position.z);
    }
}
