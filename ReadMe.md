### Solution

I design this application to have an API, AccountTracker which essentially sits and acts as a middle man in creating accounts.
You create an account using account tracker.
You then log into  Human resource (hrs) applications and link to account tracker, create and account on these application as well,
And link your newly created account with the account tracker account.

For brevity, a lot of implmentation have been labeled with todo, to fastern the development process, and prevent deviation from te main concept of this project

#### 🚀 Preriquisits
- Docker
- Internet connection
- Docker Compose


#### 🚀 How To Test
Extract zip into Default bash directory.
In a separate terminal window (2 terminals) RUN the following commads:

#### Terminal 1: Human resource application
``` Bash
cd hrs

sh scripts/create.sh

```
Open Visual studio and run the project or run:

``` Bash
dotnet run --project hrs/hrs.csproj

```
Create an account and link the account to your existing account tracker accounts


#### Terminal 1: AccountTracker
``` Bash
cd accounttracker

sh scripts/create.sh

```
Open Visual studio and run the project or run:

``` Bash
dotnet run --project accounttracker/accounttracker.csproj

```
After project launch, use swagger to create a new account for the account tracker, and test your account creation by loggin in.



#### Relevant commands (must cd into project directory)
Run Migrations
``` Bash
sh scripts/migrate.sh apply

```

RoleBack Migrations
``` Bash
sh scripts/migrate.sh rollback

```

Destroy Database
``` Bash
sh scripts/destroy.sh

```

Start Database
``` Bash
sh scripts/start.sh

```

-Migrations (database scripts) can be found in the migration directory within project directory
- Migration are written in raw sql



#### Point to note
Make sure application are run in the specified ports in the launch.json file
- The application should be run using kestrel

AccountTracker Database Port: 5401, running on: https://localhost:5000;http://localhost:5001
Human Resource: Database Port: 5400, running on: https://localhost:3000;http://localhost:3001
