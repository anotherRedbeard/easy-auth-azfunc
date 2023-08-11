# Project Name

This project is a sample Azure Functions project that demonstrates how to display user detail and claims from being hosted in a function app that is secured by Easy Auth. 

## Main Files

### `HttpTrigger1.cs`

This is the only function in the project and contains the code for an HTTP-triggered Azure Function. The function takes an HTTP request as input and returns an HTTP response with user details and claims from the Easy Auth authentication provider.

The function uses the `PrincipalValue` class to deserialize the user details and claims from the `x-ms-client-principal` header in the HTTP request. It then constructs an HTTP response with the user details and claims and returns it to the client.

Here's a summary of the code in `HttpTrigger1.cs`:

- The `HttpTrigger1` class defines an HTTP-triggered Azure Function.
- The `Run` method is the entry point for the function and takes an `HttpRequestData` object as input.
- The method extracts the `x-ms-client-principal` header from the HTTP request and deserializes it into a `PrincipalValue` object.
- The method constructs an HTTP response with the user details and claims from the `PrincipalValue` object.
- The method returns the HTTP response to the client.

### `ClientPrincipal.cs`

The `ClientPrincipal.cs` file contains the `ClientPrincipal` class, which is used to deserialize the `x-ms-client-principal` header in an HTTP request into a `PrincipalValue` object.

Here's a summary of the code in `ClientPrincipal.cs`:

- The `ClientPrincipal` class defines a set of properties that correspond to the fields in the `x-ms-client-principal` header.
- The `ClientPrincipal` class has a static `Deserialize` method that takes a base64-encoded string representation of the `x-ms-client-principal` header as input.
- The `Deserialize` method decodes the base64-encoded string and deserializes it into a `PrincipalValue` object.
- The `PrincipalValue` class defines a set of properties that correspond to the fields in the `x-ms-client-principal` header, as well as a `Claims` property that contains a dictionary of additional claims.
- The `Deserialize` method returns a `PrincipalValue` object that contains the user details and claims from the `x-ms-client-principal` header.

## Getting Started

To get started with this project, you'll need to do the following:

1. Clone the repository to your local machine:
    ```git clone https://github.com/anotherRedbeard/easy-auth-azfunc.git```

2. Open the project in Visual Studio Code:

3. Create a `local.settings.json` file:

    ```json
    { 
        "IsEncrypted": false, 
        "Values": { 
            "AzureWebJobsStorage": "",
            "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        }
    }
    ```

## Usage

To use the project, follow these steps:

1. Start the Azure Functions runtime:
    ```func start```

2. Send an HTTP request to the function endpoint:
    ```GET http://localhost:7071/api/HttpTrigger1```

This will start the Azure Functions runtime and allow you to test the functions locally.  Since this is displaying claims and user detail using the `X-MS-CLIENT-PRINCIPAL` header you will likely want to use a REST api testing tool such as Postman or Curl.  Here is an example curl command using a `X-MS-CLIENT-PRINCIPAL` header.  When using the real command, you will want to provide your own header that would replicate the header sent via AAD from the Azure Functions Easy Auth setup.

```curl
curl -X GET -H "X-MS-CLIENT-PRINCIPAL:  <header_value>" "http://localhost:7071/api/HttpTrigger1"
```

## GitHub Action Workflow

The `.github/workflows/deploy-to-azure.yaml` file represents a quick/basic example of deploying this solution to a function hosted in Azure.  For my example, I'm deploying this to a linux consumption plan using a [Zip Deployment](https://learn.microsoft.com/en-us/azure/azure-functions/deployment-zip-push).

- Here are the steps of the workflow
  - Checkout the code
  - Set the dotnet core version
  - Build the dotnet project
  - Publish the dotnet project to the `./publish` location
  - Use the Azure/functions-action@v1 github action to deploy to Azure
    - Since I'm deploying to a Linux consumption plan, you need to make sure and remove the WEBSITE_RUN_FROM_PACKAGE app setting. Refer to the [Run From Package Document](https://learn.microsoft.com/en-us/azure/azure-functions/run-functions-from-deployment-package#enable-functions-to-run-from-a-package) for more details on this.


## Contributing

If you'd like to contribute to this project, please fork the repository and submit a pull request. We welcome contributions of all kinds, including bug fixes, feature enhancements, and documentation improvements.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.
