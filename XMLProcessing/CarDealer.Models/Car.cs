namespace CarDealer.Models
{
    using System.Collections.Generic;

    public class Car
    {
        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public double TravelledDistance { get; set; }

        public ICollection<PartCar> PartCars { get; set; } = new HashSet<PartCar>();
    }
}