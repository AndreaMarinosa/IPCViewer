using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IPCViewer.Forms.ViewModels
{
    public class CityList<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }

        public CityList (K key, IEnumerable<T> items)
        {
            Key = key;
            foreach ( var item in items )
                this.Items.Add(item);
        }
    }
}
