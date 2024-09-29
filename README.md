# bichiware-solutions


## Running Backend

The backend is based on .net core, as such, it should be run on Visual Studio with all needed extensions for running such a program, when those are installed open backend.sln file included in the backend folder (do this through the open solution option in Visual Studio), to run the program simply press the run button on VS.

## Running Frontend

To run the frontend of the site, it is necessary to install npm and some libraries used for templates and communication with the backend, to do this, open a terminal, go to the frontend folder and use the following commands:

        npm install
        npm install axios
        npm install bootstrap
        npm install vuex

With the libraries installed, run one of the following commands

        npm run serve
        npm run build

build is used for the production version, serve is used for debugging

## Database

To connect to the database, a database should be created using the .sql files included in the ScriptsDatabase folder, after creating the database, edit the connection strings at the bottom of backend/appsettings.json, changing the value of Data Source to the name of your local sql server