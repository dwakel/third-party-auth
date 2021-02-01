

echo ":: starting containers"
docker-compose up --build -d

echo ":: waiting for containers to start..."
secs=10
while [ $secs -gt 0 ]; do
   echo -ne "$secs\033[0K\r"
   sleep 1
   : $((secs--))
done

echo ":: applying migrations"
export CONNSTR="Host=localhost;Username=hr;Port=5400;Password=1;Database=hr"
./scripts/migrator.sh apply

echo ":: seeding data"
./scripts/runsql.sh "./sql/seed.sql"
