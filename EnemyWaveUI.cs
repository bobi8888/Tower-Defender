using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyWaveUI : MonoBehaviour {

	[SerializeField] private EnemyWaveManager enemyWaveManager;

	private Camera mainCamera;
	private TextMeshProUGUI waveNumberText;
	private TextMeshProUGUI waveMessageText;
	private RectTransform enemyWaveSpawnPositionIndicator;
	private RectTransform enemyClosestPositionIndicator;

	private void Awake() {
		waveNumberText = transform.Find("waveNumberText").GetComponent<TextMeshProUGUI>();
		waveMessageText = transform.Find("waveMessageText").GetComponent<TextMeshProUGUI>();
		enemyWaveSpawnPositionIndicator = transform.Find("enemyWaveSpawnPositionIndicator").GetComponent<RectTransform>();
		enemyClosestPositionIndicator = transform.Find("enemyClosestPositionIndicator").GetComponent<RectTransform>();
	}

	private void Start() {
		mainCamera = Camera.main;
		enemyWaveManager.OnWaveNumberChanged += EnemyWaveManager_OnWaveNumberChanged;
		SetWaveNumberText("Wave " + enemyWaveManager.GetWaveNumber());

	}

	private void EnemyWaveManager_OnWaveNumberChanged(object sender, System.EventArgs e) {
		SetWaveNumberText("Wave " + enemyWaveManager.GetWaveNumber());
	}

	private void Update() {
		

		HandleEnemyClosestPositionIndicator();
		HandleEnemyWaveSpawnPositionIndicator();
		HandleNextWaveMessage();
	}

	private void HandleNextWaveMessage () {
		float nextWaveSpawnTimer = enemyWaveManager.GetNextWaveSpawnTimer();
		if (nextWaveSpawnTimer <= 0f) {
			SetMessageText("");
		} else {
			SetMessageText("Next Wave In " + nextWaveSpawnTimer.ToString("F1") + "Second(s)");
		}
	}

	private void HandleEnemyWaveSpawnPositionIndicator() {
		Vector3 directionToNextSpawnPosition = (enemyWaveManager.GetSpawnPosition() - mainCamera.transform.position).normalized;
		enemyWaveSpawnPositionIndicator.anchoredPosition = directionToNextSpawnPosition * 300f;
		enemyWaveSpawnPositionIndicator.eulerAngles = new Vector3(0,0,UtilitiesClass.GetAngleFromVector(directionToNextSpawnPosition));

		float distanceToNextSpawnPosition = Vector3.Distance(enemyWaveManager.GetSpawnPosition(),mainCamera.transform.position);
		enemyWaveSpawnPositionIndicator.gameObject.SetActive(distanceToNextSpawnPosition > mainCamera.orthographicSize * 1.5);
	}

	private void HandleEnemyClosestPositionIndicator() {
		float targetMaxRadius = 9999f;
		Collider2D [] collider2DArray = Physics2D.OverlapCircleAll(mainCamera.transform.position,targetMaxRadius);

		Enemy targetEnemy = null;

		foreach (Collider2D collider2D in collider2DArray) {
			Enemy enemy = collider2D.GetComponent<Enemy>();
			if (enemy != null) {//Is a enemy
				if (targetEnemy == null) {
					targetEnemy = enemy;
				} else {
					if (Vector3.Distance(transform.position,enemy.transform.position) <
						Vector3.Distance(transform.position,targetEnemy.transform.position)) {//closer!
						targetEnemy = enemy;
					}
				}
			}
		}

		if (targetEnemy != null) {
			Vector3 directionToClosestEnemy = (targetEnemy.transform.position - mainCamera.transform.position).normalized;
			enemyClosestPositionIndicator.anchoredPosition = directionToClosestEnemy * 250f;
			enemyClosestPositionIndicator.eulerAngles = new Vector3(0,0,UtilitiesClass.GetAngleFromVector(directionToClosestEnemy));

			float distanceToClosestEnemy = Vector3.Distance(targetEnemy.transform.position,mainCamera.transform.position);
			enemyClosestPositionIndicator.gameObject.SetActive(distanceToClosestEnemy > mainCamera.orthographicSize * 1.5);
		} else {//no enemy alive
			enemyWaveSpawnPositionIndicator.gameObject.SetActive(false);
		}
	}

	private void SetMessageText(string message) {
		waveMessageText.SetText(message);
	}
	
	private void SetWaveNumberText(string text) {
		waveNumberText.SetText(text);
	}
}
