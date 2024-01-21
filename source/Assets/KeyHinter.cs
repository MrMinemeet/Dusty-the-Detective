using System;
using UnityEngine;

public class KeyHinter : MonoBehaviour
{
    [SerializeField] private GameObject eKey;
    private Animator _animator;

    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
    }

    private void Start()
    {
        eKey.SetActive(false);
        Globals.OnShowKeyHint.AddListener(() =>
        {
            eKey.SetActive(true);
            _animator.Play("key_hint");
        });
        Globals.OnHideKeyHint.AddListener(() => eKey.SetActive(false));
    }
}
