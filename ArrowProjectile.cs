using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour {

	public static ArrowProjectile Create(Vector3 position, Enemy enemy) {
		Transform pfArrowProjectile = Resources.Load<Transform>("pfArrowProjectile");
		Transform arrowTransform = Instantiate(pfArrowProjectile,position,Quaternion.identity);

		ArrowProjectile arrowProjectile = arrowTransform.GetComponent<ArrowProjectile>();
		arrowProjectile.SetTarget(enemy);
		return arrowProjectile;
	}

	private Enemy targetEnemy;
	private Vector3 lastMoveDirection;
	private float timeToDie = 1f;

	private void Update() {
		Vector3 moveDirection;
		if (targetEnemy != null) {
			moveDirection = (targetEnemy.transform.position - transform.position).normalized;
			lastMoveDirection = moveDirection;
		} else {
			moveDirection = lastMoveDirection;
		}
		transform.eulerAngles = new Vector3(0,0,UtilitiesClass.GetAngleFromVector(moveDirection));

		float moveSpeed = 20f;
		transform.position += moveDirection * moveSpeed * Time.deltaTime;

		timeToDie -= Time.deltaTime;
		if (timeToDie < 0f) {
			Destroy(gameObject);	
		}
	}

	private void SetTarget(Enemy targetEnemy) {
		this.targetEnemy = targetEnemy;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		Enemy enemy = collision.GetComponent<Enemy>();
		if (enemy != null) {//Hit an enemy
			int damageAmount = 10;
			enemy.GetComponent<HealthSystem>().Damage(damageAmount);
			Destroy(gameObject);
		}
	}
}
