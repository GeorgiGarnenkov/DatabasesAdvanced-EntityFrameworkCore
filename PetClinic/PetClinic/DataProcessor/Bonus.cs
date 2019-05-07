using System.Linq;

namespace PetClinic.DataProcessor
{
    using System;

    using PetClinic.Data;

    public class Bonus
    {
        private const string SuccessMessage = "{0}'s profession updated from {1} to {2}."; 
                                             // vet.Name , oldProfession, newProfession
        private const string ErrorMessage = "Vet with phone number {0} not found!";
                                             // phoneNumber

        public static string UpdateVetProfession(PetClinicContext context, string phoneNumber, string newProfession)
        {
            var vet = context.Vets.FirstOrDefault(x => x.PhoneNumber == phoneNumber);

            if (vet == null)
            {
                return string.Format(ErrorMessage, phoneNumber);
            }

            var oldProfession = vet.Profession;

            vet.Profession = newProfession;

            context.SaveChanges();

            return string.Format(SuccessMessage, vet.Name, oldProfession, newProfession);
        }
    }
}
