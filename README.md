# dotnet-core-example
dotnet core web api auth example - req 3rd party resource though delegation

## docker
build: `docker build -t oauth .`

run: `docker run -it --rm -p 5000:80 -e "AzureAd__ClientSecret=a_value" oauth`

swagger can be found at `localhost:5000/swagger`

