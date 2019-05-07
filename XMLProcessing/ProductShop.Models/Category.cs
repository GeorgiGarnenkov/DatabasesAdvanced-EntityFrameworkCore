using System.Collections.Generic;

namespace ProductShop.Models
{
    public class Category
    {
        public Category()
        {
            this.CategoryProducts = new HashSet<CategoryProduct>();
        }

        public int Id { get; set; }

        public string Name { get; set; }  // (from 3 to 15 characters)

        public ICollection<CategoryProduct> CategoryProducts { get; set; }  
    }
}