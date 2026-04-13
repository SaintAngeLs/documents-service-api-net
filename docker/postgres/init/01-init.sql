DO
$$
BEGIN
   IF NOT EXISTS (
      SELECT FROM pg_catalog.pg_roles WHERE rolname = 'documents_user'
   ) THEN
      CREATE ROLE documents_user WITH LOGIN PASSWORD 'documents_pass';
   END IF;
END
$$;

ALTER ROLE documents_user WITH LOGIN PASSWORD 'documents_pass';

GRANT ALL PRIVILEGES ON DATABASE documents_db TO documents_user;