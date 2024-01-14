using UnityEngine;

public class ActivateOnSceneSwitch : MonoBehaviour
{
    public GameObject dirtDist;
    private GameObject _dirtSpotPointers;

    private void Awake()
    {
        _dirtSpotPointers = GameObject.FindWithTag("Player").transform.Find("DirtPointers").gameObject;
    }

    private void Update()
    {
        dirtDist.SetActive(Globals.AllowDirtPlacement);
        _dirtSpotPointers.SetActive(Globals.AllowDirtPlacement);
    }
}
