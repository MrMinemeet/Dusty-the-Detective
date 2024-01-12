using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UnlockManifolds : MonoBehaviour
{
   public List<Button> buttons;
   public List<Button> shuffledButtons;
   private int counter = 0;

   public Animator animator;

   private static bool played = false;

   public void Update()
   {
      if (Globals.LeftoverTrash == 1 && !played)
      {
         played = true;
         animator.Play("show");
         RestartNumberMiniGame();
      }
   }

   private void RestartNumberMiniGame()
   {
      Globals.IsMiniGameActive = true;
      shuffledButtons = buttons.OrderBy(a => Random.Range(0, 100)).ToList();

      for (int i = 1; i < 11; i++)
      {
         shuffledButtons[i - 1].GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
         shuffledButtons[i - 1].interactable = true;
         shuffledButtons[i - 1].image.color = new Color32(177, 220, 233, 255);
      }
   }

   public void pressButton(Button button)
   {
      if (int.Parse(button.GetComponentInChildren<TextMeshProUGUI>().text) - 1 == counter)
      {
         counter++;
         button.interactable = false;
         button.image.color = Color.green;
         if (counter == 10)
         {
            StartCoroutine(presentResult(true));
         }
      }
      else
      {
         StartCoroutine(presentResult(false));
      }
   }

   // ReSharper disable Unity.PerformanceAnalysis
   public IEnumerator presentResult(bool win)
   {
      if (!win)
      {
         foreach (var button in shuffledButtons)
         {
            button.image.color = Color.red;
            button.interactable = false;
         }
         yield return new WaitForSeconds(2f);
         RestartNumberMiniGame();
      }
      else
      {
         yield return new WaitForSeconds(2f);
         animator.Play("hide");
         Globals.IsMiniGameActive = false;
      }
   }
}
