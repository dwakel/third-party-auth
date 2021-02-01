

echo ":: reset database"

export CONNSTR="Host=localhost;Username=hr;Port=5400;Password=1;Database=hr"
./scripts/migrator.sh rollback
./scripts/migrator.sh apply
./scripts/runsql.sh "./sql/seed.sql"

