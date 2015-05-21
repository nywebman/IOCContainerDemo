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
            resolver.Register<Shopper, Shopper>();
            resolver.Register<ICreditCard, MasterCard>();

            var shopper = resolver.Resolve<Shopper>();

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
        private Dictionary<Type,Type> dependancyMap=new Dictionary<Type, Type>();
        public T Resolve<T>()
        {
            //cast to type T
            return (T)Resolve(typeof(T));
        }

        /// <summary>
        /// is it in our dictionary or not
        /// </summary>
        /// <param name="typeToResolve"></param>
        /// <returns></returns>
        private object Resolve(Type typeToResolve)
        {
            Type resolvedType = null;
            try
            {
                resolvedType = dependancyMap[typeToResolve];
            }
            catch
            {
                throw new Exception(string.Format("Could not resolve type {0}",typeToResolve.FullName));
            }
            var firstConstructor = resolvedType.GetConstructors().First();
            var constructorParameters = firstConstructor.GetParameters();

            if (!constructorParameters.Any())
                //reflection way of calling a type
                return Activator.CreateInstance(resolvedType);

            IList<object> parameters= constructorParameters.Select(parameterToResolve => Resolve(parameterToResolve.ParameterType)).ToList();
            //IList<object> parameters = new List<object>();
            //foreach (var parameterToResolve in constructorParameters)
            //{
            //    parameters.Add(Resolve(parameterToResolve.ParameterType));
            //}


            return firstConstructor.Invoke(parameters.ToArray());
        }

        public void Register<TFrom, TTo>()
        {
            dependancyMap.Add(typeof(TFrom), typeof(TTo));
        }
    }
}
