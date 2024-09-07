using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private Animator animator;
    private NavMeshAgent agent;
    private GameObject player;
    private Vector3 lastKnownPosition;

    [SerializeField]
    private string currentState;
    [SerializeField]
    private Path path;
    
    [Header("Sight Values")]
    [SerializeField]
    private float sightDistance = 20f;
    [SerializeField]
    private float fieldOfView = 85f;
    [SerializeField]
    private float eyeHeight;

    [Header("Weapon Values")]
    [SerializeField]
    private Transform gunBarrel;
    [SerializeField, Range(0.1f, 10f)]
    private float fireRate;

    public NavMeshAgent Agent { get { return agent; } }
    public Path Path { get { return path; } set { path = value; } }
    public GameObject Player { get { return player; } }
    public Vector3 LastKnownPosition { get { return lastKnownPosition; } set { lastKnownPosition = value; } }
    public Transform GunBarrel {  get { return gunBarrel; } }
    public float FireRate { get { return fireRate; } set { fireRate = value; } }
    public float EyeHeight {  get { return eyeHeight; } set { eyeHeight = value; } }

    Voxelizer voxelizer;

    void Start()
    {
        stateMachine= GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        stateMachine.Initialise();
        player = GameObject.FindGameObjectWithTag("Player");

        voxelizer = GameObject.FindGameObjectWithTag("Voxel Grid").GetComponent<Voxelizer>();
    }

    void Update()
    {
        CanSeePlayer();
        currentState = stateMachine.ActiveState.ToString();
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    public bool CanSeePlayer()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
            {
                Vector3 targetDirection = (player.transform.position + (Vector3.up * eyeHeight)) - transform.position - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();

                    
                    if (Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        Debug.DrawRay(ray.origin, ray.direction * sightDistance);

                        if (hitInfo.transform.gameObject == player)
                        {
                            if (voxelizer.IsSmokeVoxelHit(hitInfo.transform) || voxelizer.IsSmokeVoxelHit(transform))
                            {
                                return false;
                            }

                            return true;
                        }
                    }
                }
            }
        }
        
        return false;
    }
}
