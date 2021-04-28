using System.Collections;
using System.Windows.Forms;
using System;
using System.Collections.Generic;

namespace TaRU_Jaster
{
    public class ListViewColumnSorter : IComparer
    {
        /// <summary>
        /// Specifies the column to be sorted
        /// </summary>
        private int ColumnToSort;
        /// <summary>
        /// Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder OrderOfSort;
        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private CaseInsensitiveComparer ObjectCompare;

        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public ListViewColumnSorter()
        {
            // Initialize the column to '0'
            ColumnToSort = 0;

            // Initialize the sort order to 'none'
            OrderOfSort = SortOrder.Ascending;

            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            // Deal with NEWKEY and AUTOKEY
            if (listviewX.SubItems[0].Text == "*NEW" || listviewY.SubItems[0].Text == "*NEW") return -1;
            if (listviewX.SubItems[0].Text == "[AUTO]") return (listviewY.SubItems[0].Text == "*NEW" ? 1 : -1);
            if (listviewY.SubItems[0].Text == "[AUTO]") return (listviewX.SubItems[0].Text == "*NEW" ? 1 : -1);

            
            // Always show updated targets first
            if (listviewX.SubItems[1].Text == "N/A" && listviewY.SubItems[1].Text != "N/A")
            {
                return -1;
            }
            else if (listviewX.SubItems[1].Text != "N/A" && listviewY.SubItems[1].Text == "N/A")
            {
                return 1;
            }

            // If the compare should be done numerically
            if (new List<int>(new[] { 0, 3, 4, 5 }).Contains(ColumnToSort))
            {
                compareResult = int.Parse(listviewX.SubItems[ColumnToSort].Text) - int.Parse(listviewY.SubItems[ColumnToSort].Text);
            }


            // Otherwise compare the two items alphabetically
            else
            {
                compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
            }

            // Calculate correct return value based on object comparison
            if (OrderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-compareResult);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }
    }
}
