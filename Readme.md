# WebApi App
Api App using ASP.NET 6 Web Api.

## Dependencies
1. .Net Framework 6


## Deployment
Host the Api to a server of your choice.
Run migrations using the nuget package manager console.
Run:

		update-database

## Using the Api
The https://{rooturl}/swagger of the app will lead you to the Swagger documentation where details of all endpoints could be found there, where rooturl is the url that the app can be reached.

### Authentication
Use the register endpoint to register a user, and you'll get back a token which you can use to authenticate by using the Authorization header in your requests.
Once registered, you can login to get another token if your session expires.

## Running Tests
The test classes can be found in the Tests folder inside the project folder. Tests use Xunit, so you should have that package installed.

