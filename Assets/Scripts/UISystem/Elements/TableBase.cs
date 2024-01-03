using System.Collections.Generic;
using UnityEngine;

namespace UISystem.Elements
{
    public abstract class TableBase<TRow, TRowData> : MonoBehaviour
        where TRow : TablePageRowBase<TRowData>
    {
        [SerializeField] private GameObject loadScreen;
        protected TRow[] Rows;
        
        public void Init()
        {
            Rows = gameObject.GetComponentsInChildren<TRow>();
            OnInit();
        }

        protected virtual void OnInit() { }
        
        public void SetData(IReadOnlyList<TRowData> someData)
        {
            foreach (var row in Rows)
                row.SwitchVisible(false);

            if (someData is null) return;
            
            for (int i = 0; i < Rows.Length && i < someData.Count; i++)
                SetRowData(Rows[i], someData[i]);
        }

        protected abstract void SetRowData(TRow row, TRowData newData);
        
        public void SwitchLoadScreenVisible(bool show) => loadScreen.SetActive(show);
    }
}