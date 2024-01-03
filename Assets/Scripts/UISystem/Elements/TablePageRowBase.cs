using TMPro;
using UnityEngine;

namespace UISystem.Elements
{
    public abstract class TablePageRowBase<T> : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI playerName;

        public abstract void SetData(T newData);

        public void SwitchVisible(bool show) => gameObject.SetActive(show);
    }
}