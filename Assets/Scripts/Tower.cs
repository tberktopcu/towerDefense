using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tower : MonoBehaviour
{
    public List<Collider> collidersInRange =  new List<Collider>();
    [HideInInspector]public Collider targetEnemyCollider;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject muzzleTip;

    private float attackTime = 2f;
    private float currentTime;
    // Update is called once per frame
    void Update()
    {
        if (collidersInRange.Count > 0)
        {
            LookAtEnemy(targetEnemyCollider);
        }


        currentTime += Time.deltaTime;
        if (currentTime > attackTime && collidersInRange.Count > 0)
        {
            Shoot(targetEnemyCollider);
            currentTime = 0;
        }

    }

    private void LookAtEnemy(Collider enemyCollider)
    {
        Vector3 direction = enemyCollider.transform.position - transform.position;
        direction.y = 0; // Ignore Y-axis rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
    }

    private void Shoot(Collider enemyCollider)
    {
        GameObject bullet = Instantiate(bulletPrefab,muzzleTip.transform.position,Quaternion.identity);
        bullet.GetComponent<Bullet>().SetEnemy(enemyCollider);
    }


    #region ENEMY REGISTER
    private void OnTriggerEnter(Collider enemyCollider)
    {
        if (enemyCollider.GetComponent<Enemy>() != null)
        {
            RegisterEnemies(enemyCollider);
            enemyCollider.GetComponent<Enemy>().SetTower(this);
        }
    }
    private void OnTriggerExit(Collider enemyCollider)
    {
        if (enemyCollider.GetComponent<Enemy>() != null)
        {
            RemoveEnemyFromList(enemyCollider);
        }
    }

    void RegisterEnemies(Collider enemyCollider)
    {
        int colliderIterationCount = 0;
        if (collidersInRange.Count == 0)// first time registering a collider
        {
            collidersInRange.Add(enemyCollider);
            UpdateTargetEnemy();
        }
        foreach (var collider in collidersInRange)
        {
            if (collider == enemyCollider)//check if collider is already registered
            {
                colliderIterationCount++;//if yes count iteration
            }
        }
        if (colliderIterationCount == 0) // if not registered
        {
            collidersInRange.Add(enemyCollider); // register collider    
        }
    }
    public void RemoveEnemyFromList(Collider enemyCollider)
    {
        collidersInRange.Remove(enemyCollider);
        UpdateTargetEnemy();
    }

    private void UpdateTargetEnemy()
    {
        if (collidersInRange.Count > 0)
        {
            targetEnemyCollider = collidersInRange[0];
        } 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, GetComponent<SphereCollider>().radius);
    }
    #endregion
}
