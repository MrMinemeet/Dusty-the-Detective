using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnlockManifolds : MonoBehaviour
{
   public List<Button> buttons;
   public List<Button> shuffledButtons;
   private int _counter;

   public Animator animator;

   private static bool _played;
   private GameObject _hud;

   private void Awake()
   {
      _hud = GameObject.Find("HUD");
   }

   public void Update()
   {
      if (Globals.LeftoverTrash == 1 && !_played)
      {
         _played = true;
         _hud.SetActive(false);
         animator.Play("show");
         RestartNumberMiniGame();
      }
   }

   private void RestartNumberMiniGame()
   {
      Globals.IsMiniGameActive = true;
      _counter = 0;
      shuffledButtons = buttons.OrderBy(a => Random.Range(0, 100)).ToList();

      for (int i = 1; i < 11; i++)
      {
         shuffledButtons[i - 1].GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
         shuffledButtons[i - 1].interactable = true;
         shuffledButtons[i - 1].image.color = new Color32(177, 220, 233, 255);
      }
   }

   public void PressButton(Button button)
   {
      if (int.Parse(button.GetComponentInChildren<TextMeshProUGUI>().text) - 1 == _counter)
      {
         _counter++;
         button.interactable = false;
         button.image.color = Color.green;
         if (_counter == 10)
         {
            StartCoroutine(PresentResult(true));
         }
      }
      else
      {
         StartCoroutine(PresentResult(false));
      }
   }

   // ReSharper disable Unity.PerformanceAnalysis
   private IEnumerator PresentResult(bool win)
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
         _hud.SetActive(true);
         Globals.IsMiniGameActive = false;
      }
   }
}
