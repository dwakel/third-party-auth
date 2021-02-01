

echo ":: reset database"

export CONNSTR="Host=localhost;Username=tracker;Port=5401;Password=1;Database=tracker"
./scripts/migrator.sh rollback
./scripts/migrator.sh apply
./scripts/runsql.sh "./sql/seed.sql"

