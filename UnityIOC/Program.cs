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
            container.RegisterType<ICreditCard, MasterCard>();
            //property injection
            container.RegisterType<ICreditCard, MasterCard>(new InjectionProperty("chargeCount", 5));
            //container.RegisterType<ICreditCard, MasterCard>("DefaultCard");
            //container.RegisterType<ICreditCard, Visa>("BackupCard");

            //var card = new MasterCard();
            //container.RegisterInstance(card);
            
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
