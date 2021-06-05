using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menus
{
    public class ButtonTextColor : Button
    {
        private TMP_Text _text;
        private TextMeshProUGUI _ugui;

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            Color color;
            switch (state)
            {
                case SelectionState.Normal:
                    color = colors.normalColor;
                    break;
                case SelectionState.Highlighted:
                    color = colors.highlightedColor;
                    break;
                case SelectionState.Pressed:
                    color = colors.pressedColor;
                    break;
                case SelectionState.Disabled:
                    color = colors.disabledColor;
                    break;
                default:
                    color = Color.black;
                    break;
            }
            if (gameObject.activeInHierarchy)
            {
                switch (transition)
                {
                    case Transition.ColorTint:
                        ColorTween(color * colors.colorMultiplier, instant);
                        break;
                }
            }
        }

        private void ColorTween(Color targetColor, bool instant)
        {
            if (targetGraphic == null)
            {
                targetGraphic = image;
            }
            if (_text == null)
            {
                _text = GetComponentInChildren<TMP_Text>();
                _ugui = _text.GetComponent<TextMeshProUGUI>();
            }

            image.CrossFadeColor(targetColor, (!instant) ? colors.fadeDuration : 0f, true, true);
            _ugui.color = targetColor;
        }
    }
}
