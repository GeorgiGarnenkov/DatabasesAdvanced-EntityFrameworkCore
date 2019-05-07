using System.Collections.Generic;

namespace ProductShop.Models
{
    public class User
    {
        public User()
        {
            this.ProductsSold = new HashSet<Product>();
            this.ProductsBought = new HashSet<Product>();
        }

        public int Id { get; set; }

        public string FirstName { get; set; }    //(optional)
        public string LastName { get; set; }     //(at least 3 characters)
        public int? Age { get; set; }            //(optional)

        public ICollection<Product> ProductsSold { get; set; }
        public ICollection<Product> ProductsBought { get; set; }
    }
}
