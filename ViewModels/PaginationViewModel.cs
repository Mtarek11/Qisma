using System.Collections.Generic;

namespace ViewModels
{
    public class PaginationViewModel<T>
    {
        public List<T> ItemsList { get; set; } = new List<T>();
        public int TotalItemsNumber { get; set; }
        public int TotalPageNumbers { get; set; }
    }
}
