using UnityEngine;

namespace Assets.Scripts.UI.Loading
{
    public class ErrorScreen : MonoBehaviour
    {
        [SerializeField] private GameObject _errorPanel;

        void Start()
        {
            _errorPanel.GetComponent<ErrorPanel>().ErrorMessage = ErrorData.ErrorMessage;
        }
    }
}
