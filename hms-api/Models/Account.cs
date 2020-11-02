using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HMS.Models
{
    public class Account
    {
        public Account()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            IsActive = true;
            AccountNumber = generateAccountNumber();
            AccountBalance = 0;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        [Column(TypeName = "decimal(18,4)")]

        public decimal AccountBalance { get; set; }
        public bool IsActive { get; set; }
       

        public string HealthPlanId { get; set; }
        public virtual HealthPlan HealthPlan  { get; set; }


        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }


        public string generateAccountNumber()
        {
            int length = 7;

            // creating a StringBuilder object()
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }

            string formatedAccountNumber = "HMS" + str_build.ToString();
            return formatedAccountNumber;
        }
    }
}
