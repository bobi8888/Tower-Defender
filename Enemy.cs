using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public static Enemy Create(Vector3 position) {
		Transform pfEnemy = Resources.Load<Transform>("pfEnemy");
		Transform enemyTransform = Instantiate(pfEnemy, position, Quaternion.identity);

		Enemy enemy = enemyTransform.GetComponent<Enemy>();
		return enemy;
	}

	private Transform targetTransform;
	private Rigidbody2D rigidbody2d;
	private HealthSystem healthSystem;
	private float lookForTargetTimer;
	private float lookForTargetTimerMax = .2f;


	private void Start() {
		rigidbody2d = GetComponent<Rigidbody2D>();
		targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
		healthSystem = GetComponent<HealthSystem>();
		healthSystem.OnDied += HealthSystem_OnDied;
		lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);
	}

	private void HealthSystem_OnDied(object sender, System.EventArgs e) {
		Destroy(gameObject);
	}


	private void Update() {
		HandleMovement();
		HandleTargeting();	
	}

	private void OnCollisionEnter2D(Collision2D collision){
		Building building = collision.gameObject.GetComponent<Building>();
		if (building != null){//Collided with a building
			HealthSystem healthSystem = building.GetComponent<HealthSystem>();
			healthSystem.Damage(10);
			Destroy(gameObject);
		}
	}

	private void HandleMovement() {

		if (targetTransform != null){
			Vector3 moveDirection = (targetTransform.position - transform.position).normalized;
			float moveSpeed = 6f;
			rigidbody2d.velocity = moveDirection * moveSpeed;
		}else{
			rigidbody2d.velocity = Vector2.zero;
		}
	}

	private void HandleTargeting() {
		lookForTargetTimer -= Time.deltaTime;
		if (lookForTargetTimer < 0f){
			lookForTargetTimer += lookForTargetTimerMax;
			LookForTargets();
		}
	}



	private void LookForTargets() {
		float targetMaxRadius = 10f;
		Collider2D[] collider2DArray =  Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

		foreach (Collider2D collider2D in collider2DArray){
			Building building = collider2D.GetComponent<Building>();
			if (building != null){//Is a building
				if (targetTransform == null) {
					targetTransform = building.transform;
				} else {
					if (Vector3.Distance(transform.position, building.transform.position) <
						Vector3.Distance(transform.position, targetTransform.position)) {//closer!
						targetTransform = building.transform;
					}
				}
			}
		}

		if (targetTransform == null) {//Found no targets in range
			targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
		}
	}

}
