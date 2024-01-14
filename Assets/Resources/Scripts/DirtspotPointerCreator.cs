using UnityEngine;

public class DirtspotPointerCreator : MonoBehaviour
{
	private void Start()
	{
		foreach (Trash t in Globals.TrashMap[Globals.CurrentFloorName])
		{
			// Instantiate pointer from prefab
			GameObject pointer = Instantiate(Resources.Load<GameObject>("Prefabs/Dirtspots/DirtspotPointer"), transform);
			
			// Set target and name
			Vector3 targetPos = t.Position;
			pointer.GetComponent<DirtspotPointer>().target = targetPos;
			pointer.name = $"DirtspotPointer_{targetPos}";

			CleaningTask ct = GameObject.Find($"DirtSpot_{targetPos}").GetComponent<CleaningTask>();
			ct.onCleaned.AddListener(() => Destroy(pointer));
		}
	}
}
