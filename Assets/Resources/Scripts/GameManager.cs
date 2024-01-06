using System;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Vector3 playerPosition;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        // Disable any guest that is not active
        foreach (var guest in Globals.ActiveGuestMap
                     .Where(guest => !guest.Value))
            try
            {
                var guestObj = GameObject.Find(guest.Key);
                if (guestObj == null) continue;
                Debug.Log($"{guest} had already left the hotel");
                guestObj.SetActive(false);
                Destroy(guestObj);
            }
            catch (Exception)
            {
                // Ignore
            }
    }

    private void FixedUpdate()
    {
        Globals.TotalTimeRunning += Time.fixedDeltaTime;

        // Don't do anything if time is below X seconds or above Y seconds
        if (Globals.TotalTimeRunning < Globals.MIN_TIME_UNTIL_LEAVE ||
            Globals.MAX_TIME_UNTIL_LEAVE > Globals.TotalTimeRunning) return;

        // Get all names of active guests
        var activeGuests = Globals.ActiveGuestMap
            .Where(g => g.Value)
            .Select(g => g.Key)
            .ToList();

        // Check if any active guest is able to should leave
        foreach (var guest in activeGuests)
        {
            // Check if guest should leave
            if (!(Globals.TimeUntilGuestLeaves[guest] <=
                  Globals.TotalTimeRunning)) continue;

            // Disable guest
            Globals.ActiveGuestMap[guest] = false;
            try
            {
                // Find guest by name of game object and destroy lazily
                var guestObj = GameObject.Find(guest);
                Debug.Log($"{guest} left the hotel");
                guestObj.SetActive(false);
                Destroy(guestObj);
            }
            catch (Exception)
            {
                // Ignore
            }
        }
    }
}
