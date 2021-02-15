using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MazeBuilder : MonoBehaviour
{
	public Corridor startCorridorPrefab, endCorridorPrefab;
	public List<Corridor> corridorPrefabs = new List<Corridor>();
	public Vector2Int corridorsAmountRange = new Vector2Int(2, 10);
	public List<GameObject> plugPrefabs = new List<GameObject>();
	public LayerMask corridorsLayer;

	// public GameObject loaderMenu;
	// public GameObject imageMenu;
	// public Slider loader;
	// public Camera menuCamera;

	private List<Gateway> availableGateways = new List<Gateway>();
	private List<Gateway> activeGateways = new List<Gateway>();

	public void StartBuilding() {
		foreach (Transform child in transform) {
			Destroy(child.gameObject);
		}
		StartCoroutine("BuildMaze");
	}

	IEnumerator BuildMaze() {
		Corridor startCorridor = Instantiate(startCorridorPrefab, transform) as Corridor;
      	startCorridor.transform.position = Vector3.zero;
      	startCorridor.transform.rotation = Quaternion.identity;
      	AddGatewaysToList(startCorridor, ref availableGateways);
		AddGatewaysToList(startCorridor, ref activeGateways);

		yield return new WaitForFixedUpdate();

		int corridorsAmount = Random.Range(corridorsAmountRange.x, corridorsAmountRange.y);

		for( int i = 0; i <= corridorsAmount; i++ ) {
			if( availableGateways.Count == 0 ) break;

			float buildProgress = Mathf.Clamp01( (float)(i) / (float)(corridorsAmount) );
	        GameManager.instance.SetLoaderMenuValue(buildProgress);

			Corridor currentCorridor;
			if( i == corridorsAmount ) {
				currentCorridor = Instantiate(endCorridorPrefab, transform) as Corridor;
			} else {
				currentCorridor = Instantiate(corridorPrefabs[Random.Range(0, corridorPrefabs.Count)], transform) as Corridor;
			}

			bool isCorridorOverlap = true;
			do {
				if( availableGateways.Count == 0 ) break;

				currentCorridor.transform.position = Vector3.zero;
				currentCorridor.transform.rotation = Quaternion.identity;

				Gateway connectingGateway = currentCorridor.gateways[Random.Range(0, currentCorridor.gateways.Length)];
				Gateway targetGateway = availableGateways[Random.Range(0,availableGateways.Count)];

				float deltaAngle = Mathf.DeltaAngle(connectingGateway.transform.eulerAngles.y, targetGateway.transform.eulerAngles.y);
				currentCorridor.transform.rotation = Quaternion.AngleAxis(deltaAngle, Vector3.up) * Quaternion.Euler(0,180f,0);

				Vector3 corridorOffset = targetGateway.transform.position - ( connectingGateway.transform.position - currentCorridor.transform.position );
				currentCorridor.transform.position = corridorOffset;

				yield return new WaitForFixedUpdate();

				isCorridorOverlap = CheckCorridorOverlap( currentCorridor );
				if( isCorridorOverlap ) {
					availableGateways.Remove(targetGateway);
				} else {
					AddGatewaysToList(currentCorridor, ref availableGateways);
					AddGatewaysToList(currentCorridor, ref activeGateways);
					availableGateways.Remove(targetGateway);
					activeGateways.Remove(targetGateway);
					targetGateway.gameObject.SetActive(false);
					availableGateways.Remove(connectingGateway);
					activeGateways.Remove(connectingGateway);
					connectingGateway.gameObject.SetActive(false);
				}
			} while( isCorridorOverlap );

			// if( i == corridorsAmount && isCorridorOverlap ) {
			// 	Debug.LogWarning("NIE MOZNA UMIESCICI KORYTARZA KONCOWEGO");
			// }
		}

		foreach( Gateway gateway in activeGateways) {
			GameObject plug = Instantiate(plugPrefabs[Random.Range(0, plugPrefabs.Count)]) as GameObject;
			plug.transform.parent = gateway.transform;

			float deltaAngle = Mathf.DeltaAngle(plug.transform.eulerAngles.y, gateway.transform.eulerAngles.y);
			plug.transform.rotation = Quaternion.AngleAxis(deltaAngle, Vector3.up) * Quaternion.Euler(-90f,180f,0);;

			plug.transform.position = gateway.transform.position;

			gateway.GetComponent<Renderer>().enabled = false;
		}

		foreach (Transform corridor in transform) {
			corridor.transform.GetComponent<SphereCollider>().enabled = false;
		}

		availableGateways = new List<Gateway>();
		activeGateways = new List<Gateway>();

		GameManager.instance.StartGame();

		StopCoroutine("BuildMaze");
	}

	bool CheckCorridorOverlap(Corridor corridor) {
		Collider[] hitColliders = Physics.OverlapSphere( corridor.transform.position, 10f, corridorsLayer );
		if( hitColliders.Length > 2 ) {
			return true;
		}
		return false;
	}

	void AddGatewaysToList(Corridor corridor, ref List<Gateway> list) {
		foreach(Gateway gateways in corridor.gateways) {
			list.Add(gateways);
		}
	}
}
