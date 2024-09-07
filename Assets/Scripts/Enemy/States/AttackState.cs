using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    private float shotTimer;

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Perform()
    {
        if (Enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;

            Enemy.transform.LookAt(Enemy.Player.transform);

            if (shotTimer > Enemy.FireRate)
            {
                Shoot();
            }

            if (moveTimer > Random.Range(3, 7))
            {
                Enemy.Agent.SetDestination(Enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }

            Enemy.LastKnownPosition = Enemy.Player.transform.position;
        }
        else
        {
            losePlayerTimer += Time.deltaTime;

            if (losePlayerTimer > 8)
            {
                StateMachine.ChangeState(new SearchState());
            }
        }    
    }


    void Shoot()
    {
        shotTimer = 0;

        Transform gunBarrel = Enemy.GunBarrel;

        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunBarrel.position, Enemy.transform.rotation);

        Vector3 shootDirection = ((Enemy.Player.transform.position + (Vector3.up * Enemy.EyeHeight)) - gunBarrel.transform.position).normalized;

        bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(0.1f, 0.2f), Vector3.up) * shootDirection * 80;
    }
}
