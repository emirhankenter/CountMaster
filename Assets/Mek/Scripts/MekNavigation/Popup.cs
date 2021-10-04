using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mek.Audio;
using UnityEngine;

namespace Mek.Navigation
{
    public class Popup : ContentBase
    {
        [SerializeField] private AudioClip _openingSound;
        [SerializeField] private AudioClip _closingSound;

        public bool IsAnimating { get; private set; } = false;
        
        public override void Open(ViewParams viewParams)
        {
            transform.localScale = Vector3.zero;
            if (_openingSound)
            {
                AudioController.Play(_openingSound);
            }

            IsAnimating = true;
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack)
                .OnComplete(() => IsAnimating = false);
            base.Open(viewParams);
        }

        public override void Close()
        {
            if (_closingSound)
            {
                AudioController.Play(_closingSound);
            }

            IsAnimating = true;
            transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                transform.localScale = Vector3.one;
                gameObject.SetActive(false);
                base.Close();
                IsAnimating = false;
            });
        }
    }
}