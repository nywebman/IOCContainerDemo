using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace UnityIOC
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer();

            //ContainerControlledLifetimeManager  makes is a singleton
            container.RegisterType<ICreditCard, MasterCard>(new ContainerControlledLifetimeManager()); 

            var shopper = container.Resolve<Shopper>();
            shopper.Charge();
            Console.WriteLine(shopper.ChargesForCurrentCard);

            var shopper2 = container.Resolve<Shopper>();
            shopper2.Charge();
            Console.WriteLine(shopper2.ChargesForCurrentCard);

            Console.Read();
        }

        public class Shopper
        {
            private readonly ICreditCard creditCard;

            public Shopper(ICreditCard creditCard)
            {
                this.creditCard = creditCard;
            }

            public int ChargesForCurrentCard
            {
                get { return creditCard.chargeCount; }
            }

            public void Charge()
            {
                var chargeMessage = creditCard.Charge();
                Console.WriteLine(chargeMessage);
            }
        }

        public interface ICreditCard
        {
            int chargeCount { get; }
            string Charge();
        }

        public class MasterCard : ICreditCard
        {
            public int chargeCount { get; set; }

            public string Charge()
            {
                chargeCount++;
                return "swiping the master card";
            }
        }
        public class Visa : ICreditCard
        {
            public int chargeCount { get; set; }

            public string Charge()
            {
                chargeCount++;
                return "charging with visa";
            }
        }
    }
}
