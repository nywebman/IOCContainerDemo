using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IOCContainerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            ICreditCard creditCard=new MasterCard();
            ICreditCard otherCard = new Visa();

            Resolver resolver = new Resolver();

            var shopper = new Shopper(resolver.ResolveCreditCard()); 
            //var shopper = new Shopper(otherCard); 

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

    public class Resolver
    {
        public ICreditCard ResolveCreditCard()
        {
            if (new Random().Next(2) == 1)
                return new Visa();
            return new MasterCard();
        }
    }
}
