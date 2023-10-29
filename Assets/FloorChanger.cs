using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorChanger : MonoBehaviour
{
    [Tooltip("If true, the player will go up a floor. If false, the player will go down a floor.")]
    public bool goesUp;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if ((Globals.CurrentFloor == Globals.Floors.Count - 1 && goesUp) ||
                (Globals.CurrentFloor == 0 && !goesUp))
            {
                return;
            }
            
            if (goesUp)
            {
                Globals.CurrentFloor++;
            }
            else
            {
                Globals.CurrentFloor--;
            }

            SceneManager.LoadScene(Globals.Floors[Globals.CurrentFloor]);
        }
    }
}
