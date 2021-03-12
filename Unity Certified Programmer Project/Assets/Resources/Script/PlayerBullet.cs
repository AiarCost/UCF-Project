using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour,IActorTemplate
{
    GameObject actor;
    int hitPower;
    int health;
    int travelSpeed;

    [SerializeField]
    SOActorModel bulletModel;

    void Awake()
    {
        ActorStats(bulletModel);
    }

    public int SendDamage()
    {
        return hitPower;
    }

    void Update()
    {
        transform.position += new Vector3(travelSpeed, 0, 0) * Time.deltaTime;
    }
    public void TakeDamage(int incomingDamage)
    {
        health -= incomingDamage;
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    public void ActorStats (SOActorModel actorModel)
    {
        hitPower = actorModel.hitPower;
        health = actorModel.health;
        travelSpeed = actorModel.speed;
        actor = actorModel.actor;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy"))
        {
            if(col.GetComponent<IActorTemplate>() != null)
            {
                if(health >= 1)
                {
                    health -= col.GetComponent<IActorTemplate>().SendDamage();
                }
                if(health <= 0)
                {
                    Die();
                }
            }
        }
    }

    void OnBecomeInvisible()
    {
        Destroy(gameObject);
    }
}
