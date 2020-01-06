using System;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager;
using ResourceManagementClient = Microsoft.Azure.Management.ResourceManager.ResourceManagementClient;

namespace AzureCliAuthentication.Example
{
    public class Example
    {
        private readonly string _subscriptionName = "My Subscription";

        public void UsingAzureFluentClient()
        {
            var azureAuthenticator = new AzureAuthenticator();
            var azure = azureAuthenticator.GetAzureFluentClient(_subscriptionName);
            var resourceGroups = azure.ResourceGroups.List();
            foreach (var resourceGroup in resourceGroups)
            {
                Console.WriteLine($"I found resource group {resourceGroup.Name} in subscription {_subscriptionName}");
            }
        }

        public void UsingAzureCredentials()
        {
            var azureAuthenticator = new AzureAuthenticator();
            var azureCredentials = azureAuthenticator.GetAzureCredentials();
            var azureFluent = Azure.Authenticate(azureCredentials).WithDefaultSubscription();
            var resourceGroups = azureFluent.ResourceGroups.List();
            foreach (var resourceGroup in resourceGroups)
            {
                Console.WriteLine($"I found resource group {resourceGroup.Name} in subscription {_subscriptionName}");
            }

            var resourceManagementClient = new ResourceManagementClient(azureCredentials)
            {
                SubscriptionId = azureFluent.SubscriptionId
            };
            var resources = resourceManagementClient.Resources.List();
            foreach (var resource in resources)
            {
                Console.WriteLine($"I found resource group {resource.Name} in subscription {_subscriptionName}");
            }

        }

    }
}
