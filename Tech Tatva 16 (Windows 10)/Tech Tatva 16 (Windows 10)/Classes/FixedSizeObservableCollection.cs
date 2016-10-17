using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech_Tatva_16__Windows_10_.Classes
{
    public class FixedSizeObservableCollection<T> : ObservableCollection<T>
    {
        private readonly int maxSize;
        public FixedSizeObservableCollection(int maxSize)
        {
            this.maxSize = maxSize;
        }

        protected override void InsertItem(int index, T item)
        {
            if (Count == maxSize)
                return; // or throw exception
            base.InsertItem(index, item);
        }
    }
}
