using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            ICreditCard creditCard=new MasterCard();
            ICreditCard otherCard = new Visa();

            //shopper is not dependant on MasterCard
            var shopper = new Shopper(otherCard); 

            shopper.Charge();
            Console.Read();
        }

    }

    public class Shopper
    {
        private readonly ICreditCard creditCard;

        public Shopper(ICreditCard creditCard)
        {
            this.creditCard = creditCard;
        }

        public void Charge()
        {
            var chargeMessage=creditCard.Charge();
            Console.WriteLine(chargeMessage);
        }
    }

    public interface ICreditCard
    {
        string Charge();
    }

    public class MasterCard : ICreditCard
    {
        public string Charge()
        {
            return "swiping the master card";
        }
    }
    public class Visa : ICreditCard
    {
        public string Charge()
        {
            return "charging with visa";
        }
    }
}
