using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Tower tower;
    public float health = 100;
    private Bullet bullet;
    private void OnDisable()
    {
        tower.RemoveEnemyFromList(this.GetComponent<Collider>());
    }

    public void SetTower(Tower tower)
    {
        this.tower = tower;
    }
}
