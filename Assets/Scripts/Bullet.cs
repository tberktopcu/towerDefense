using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{

    Collider enemyToFollow = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FollowEnemy();
    }

    public void FollowEnemy()
    {
        if(enemyToFollow != null)
        {
            transform.position = Vector3.Lerp(transform.position, enemyToFollow.transform.position, 5 * Time.deltaTime);
        }
    }

    public void SetEnemy(Collider enemyCollider)
    {
        enemyToFollow = enemyCollider;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.health -= 10;
            if(enemy.health <= 0)
            {
                Destroy(enemy.gameObject);
            }
            print(enemy.health);
            Destroy(this.gameObject);
        }
    }
}
