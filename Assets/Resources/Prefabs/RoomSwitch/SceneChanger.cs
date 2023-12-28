using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
	public string nextSceneName;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			LevelLoader ll = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
			StartCoroutine(ll.LoadLevel(nextSceneName));
		}
	}
	
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		var position = transform.position;
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(position, new Vector3(transform.localScale.x, transform.localScale.y, 1));
		// Write up or down on the cube
	}
#endif
}