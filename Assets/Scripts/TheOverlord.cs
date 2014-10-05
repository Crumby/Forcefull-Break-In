using UnityEngine;
using System.Collections;

public class TheOverlord : MonoBehaviour {

	public GameObject asteroid;
	public GameObject grunt;
	private Vector3 spawnPoint = new Vector3(0, 0, 24);
	public float spawnBoundary;

	public float spawnPause;
	public float startPause;
	public float wavePause;

	public int asteroidCount;
	public int waveCount;

	public GUIText gameOverText;
	public GUIText scoreText;

	private bool gameOver;
	private int score;

	void Start()
	{
		gameOver = false;
		gameOverText.text = "";
		score = 0;
		UpdateScore ();
		StartCoroutine (SpawnWaves ());
	}

	// waits for a set amount of time then spawns waves of increasing level until all waves pass or Game over
	IEnumerator SpawnWaves() 
	{	
		yield return new WaitForSeconds(startPause);
		for(int waveRank = 1; waveRank <= waveCount; waveRank++)
		{
			StartCoroutine(sendWave(waveRank));
			if(gameOver)
			{
				break;
			}
			yield return new WaitForSeconds(wavePause);
		}
	}

	// list of all waves, sends a wave of a set rank

	// TODO: solve the pause between waves depending on wave length! now hardcoded to X seconds
	IEnumerator sendWave(int waveRank)
	{
		Vector3 spawnPosition = spawnPoint;
		switch (waveRank) 
		{	
			// a wave of asteroids
			case 1: 
				for (int i = 0; i < asteroidCount; i++) 
				{
				// for each asteroid, the X axis position is generated randomly					
					spawnPosition.x = Random.Range (-spawnBoundary, spawnBoundary);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (asteroid, spawnPosition, spawnRotation);
					yield return new WaitForSeconds(spawnPause);
				}
				break;
			case 2: 
				// generates a wave of Grunts from center to the right
				for (int i = 1; i <= 5; i++) 
				{	
					spawnPosition.x = (1.5f * i); // keep ideal spacing
					Instantiate (grunt, spawnPosition, Quaternion.identity);
					yield return new WaitForSeconds(spawnPause);
				}
				break;
			case 3: 
				// generates a wave of Grunts from center to the left
				for (int i = 1; i <= 5; i++) 
				{
					spawnPosition.x = -(1.5f * i); // keep ideal spacing
					Instantiate (grunt, spawnPosition, Quaternion.identity);
					yield return new WaitForSeconds(spawnPause);
				}
				break;
		}
	}

	public void AddScore(int amount)
	{
		score += amount;
		UpdateScore();
	}

	void UpdateScore ()
	{
		scoreText.text = "Score: " + score;
	}

	public void GameOver()
	{
		gameOverText.text = "GAME OVER!";
		gameOver = true;
	}

}
