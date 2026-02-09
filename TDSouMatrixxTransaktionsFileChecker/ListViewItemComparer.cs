using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TransparentDesign.SouMatrixxTransaktionsFileChecker
{
    public class ListViewItemComparer : IComparer
    {
        private readonly int _column;
        private readonly SortOrder _sortOrder;

        public ListViewItemComparer(int column, SortOrder sortOrder)
        {
            _column = column;
            _sortOrder = sortOrder;
        }

        public int Compare(object? x, object? y)
        {
            var itemX = (ListViewItem)x!;
            var itemY = (ListViewItem)y!;

            string textX = itemX.SubItems[_column].Text;
            string textY = itemY.SubItems[_column].Text;

            int result;

            if (int.TryParse(textX, out var ix) && int.TryParse(textY, out var iy))
            {
                result = ix.CompareTo(iy);
            }
            else if (DateTime.TryParse(textX, out var dx) && DateTime.TryParse(textY, out var dy))
            {
                result = dx.CompareTo(dy);
            }
            else
            {
                result = string.Compare(textX, textY, StringComparison.CurrentCultureIgnoreCase);
            }

            return _sortOrder == SortOrder.Descending ? -result : result;
        }
    }
}
