# AzureCLIAuthentication

This library will remove the need to use Service Principal credentials in C# to authenticate for Azure clients. I prefer configuring Azure, when working with pipelines, in C# over PowerShell but the blocker for me how to handle login. 

If you use Azure DevOps you can use the Az Cli step to using [Service Connections](https://docs.microsoft.com/en-us/azure/devops/pipelines/library/connect-to-azure?view=azure-devops#create-an-azure-resource-manager-service-connection-with-an-existing-service-principal) to execute your dll. This means you can call C# as easily as you'd call PowerShell while keeping your Service Principal credentials secure. 

## Build Status
[![Build Status](https://dev.azure.com/simplymadesoftware/CSharp/_apis/build/status/PhilPeters304.AzureCliAuthentication?branchName=master)](https://dev.azure.com/simplymadesoftware/CSharp/_build/latest?definitionId=15&branchName=master)

## How to use 

### Login
Locally to use this login using the AZ Cli and select your desired subscription. 

When using this in Azure DevOps reference the AZ Cli task and select the subscription from the drop down. Select 'Inline Script' then just call the executable as if on the command line. 


### Azure Fluent

If you are in need of an Azure Fluent Client the library will create one of these for you: 

#### Using Subscription Name
``` c#
var azureAuthenticator = new AzureAuthenticator();
var azure = azureAuthenticator.GetAzureFluentClient("My Subscription");

```

#### Using Subscription Id
``` c#
var azureAuthenticator = new AzureAuthenticator();
var azure = azureAuthenticator.GetAzureFluentClient("700ad797-9035-4001-bab8-e1798325d074");

```

### Azure Credentials 
You may find the fluent libraries don't offer you what you need, Microsoft also offer a whole load of pre-release [management libraries](https://azure.github.io/azure-sdk-for-net/) which give more abilities when configuring Azure through the Rest API. This library can also support you in this: 

``` C#
var azureAuthenticator = new AzureAuthenticator();
var azureCredentials = azureAuthenticator.GetAzureCredentials();
var resourceManagementClient = new ResourceManagementClient(azureCredentials);
```