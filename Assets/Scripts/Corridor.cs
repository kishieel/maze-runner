using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Corridor : MonoBehaviour
{
    public Gateway[] gateways;

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, 10f);
	}
}
