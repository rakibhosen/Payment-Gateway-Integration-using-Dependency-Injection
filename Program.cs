using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System;

namespace DependencyInjection
{
    class Program
    {
        public interface IPaymentGateway
        {
            bool ProcessPayment(double amount);
        }

        public class PayPalPaymentGateway : IPaymentGateway
        {
            public bool ProcessPayment(double amount)
            {
                // Implement the PayPal payment processing logic here
                Console.WriteLine($"Processing PayPal payment of {amount:C}.");
                return true;
            }
        }

        public class StripePaymentGateway : IPaymentGateway
        {
            public bool ProcessPayment(double amount)
            {
                // Implement the Stripe payment processing logic here
                Console.WriteLine($"Processing Stripe payment of {amount:C}.");
                return true;
            }
        }

        static void Main()
        {
            // Set up the Dependency Injection container
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IPaymentGateway, PayPalPaymentGateway>()
                .AddSingleton<IPaymentGateway, StripePaymentGateway>()
                .BuildServiceProvider();

            // Get the available payment gateways as a list
            var paymentGateways = new List<IPaymentGateway>(serviceProvider.GetServices<IPaymentGateway>());

            // Display payment options to the user
            Console.WriteLine("Available Payment Options:");
            for (int i = 0; i < paymentGateways.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {paymentGateways[i].GetType().Name}");
            }

            // Ask the user to select a payment option
            int selectedOption;
            do
            {
                Console.Write("Select a payment option (1, 2, etc.): ");
            } while (!int.TryParse(Console.ReadLine(), out selectedOption) || selectedOption < 1 || selectedOption > paymentGateways.Count);

            // Process the payment using the selected payment gateway
            var selectedPaymentGateway = paymentGateways[selectedOption - 1];
            Console.Write("Enter the payment amount: ");
            double amount;
            if (!double.TryParse(Console.ReadLine(), out amount))
            {
                Console.WriteLine("Invalid amount entered.");
                return;
            }

            bool paymentResult = selectedPaymentGateway.ProcessPayment(amount);
            if (paymentResult)
            {
                Console.WriteLine($"Payment via {selectedPaymentGateway.GetType().Name} successful!");
            }
            else
            {
                Console.WriteLine($"Payment via {selectedPaymentGateway.GetType().Name} failed!");
            }
        }

    }
}
