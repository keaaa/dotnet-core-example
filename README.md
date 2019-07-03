# dotnet-core-example
dotnet core web api auth example - request user information though graph api using user delegation

## Pre-requisite

Use Microsoft [guide](https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal#create-an-azure-active-directory-application) to create an application in Azure AD. **Ignore steps** not directly referenced to from here.

Use Application (client) ID from step [*Get values for signing in*](https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal#get-values-for-signing-in) and client secret from step [*Create a new application secret*](https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal#create-a-new-application-secret) as environment variables `AzureAd__ClientId` and `AzureAd__ClientSecret`. 

These need to be included when running the application.

## Test web api

swagger is available at `/swagger`

## Docker
build: `docker build -t oauth .`

run: `docker run -it --rm -p 5000:80 -e "AzureAd__ClientSecret=the_value" -e "AzureAd__ClientId=the_value" oauth`
