﻿using System;
using System.Linq;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Rest;

namespace AzureCliAuthentication
{
    public class AzureAuthenticator
    {
        /// <summary>
        /// Will return a set of Azure credentials you can pass to create new Azure clients. 
        /// </summary>
        /// <returns>AzureCredentials for athenticating an Azure Client</returns>
        public AzureCredentials GetAzureCredentials()
        {
            var creds = GetCreds();
            return creds;

        }

        /// <summary>
        /// Returns an IAzure client for the Azure Fluent libraries logged into the requested subscription.
        /// </summary>
        /// <param name="subscriptionInfo">Either the subscription id of the subscription to login to or the subscription display name</param>
        /// <returns>IAzure A client for the Azure Fluent libraries.</returns>
        public IAzure GetAzureFluentClient(string subscriptionInfo)
        {
            var isSubscriptionId = Guid.TryParse(subscriptionInfo, out var subscriptionId);
            var creds = GetCreds();
            IAzure azure; 
            if (isSubscriptionId)
            {
                 azure = Azure.Authenticate(creds).WithSubscription(subscriptionId.ToString());
            }
            else
            {
                var subscriptions = Azure.Authenticate(creds).Subscriptions.List().ToList();
                var subscription = subscriptions.FirstOrDefault(x =>
                    x.DisplayName.Equals(subscriptionInfo, StringComparison.CurrentCultureIgnoreCase));
                if (subscription == null)
                {
                    throw new Exception($"Could not find subscription named {subscriptionInfo}; Subscriptions listed were {string.Join(",", subscriptions)}.");
                }
                azure = Azure.Authenticate(creds).WithSubscription(subscription.SubscriptionId);
            }

            return azure;
        }

        private static AzureCredentials GetCreds()
        {
            try
            {
                var astp = new AzureServiceTokenProvider("RunAs=Developer;DeveloperTool=AzureCli");
                var graphToken = astp.GetAccessTokenAsync($"https://graph.windows.net/").Result;
                var astp2 = new AzureServiceTokenProvider("RunAs=Developer;DeveloperTool=AzureCli");
                var rmToken = astp2.GetAccessTokenAsync($"https://management.azure.com/").Result;
                var tenantId = astp.PrincipalUsed.TenantId;
                var creds = new AzureCredentials(
                    new TokenCredentials(rmToken),
                    new TokenCredentials(graphToken),
                    tenantId,
                    AzureEnvironment.AzureGlobalCloud);
            return creds;
            }
            catch (System.AggregateException e)
            {
                throw new AggregateException(
                    "Local login required. Open command prompt and enter 'az login', login in through browser that is automatically opened. Then enter 'az account set --subscription \"<subscriptionName>\"'.");
            }

        }
    }
}
