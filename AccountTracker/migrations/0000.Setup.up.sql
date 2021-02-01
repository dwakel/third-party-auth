-- --
-- -- Name: pgcrypto; Type: EXTENSION; Schema: public; Owner: -
-- --

CREATE EXTENSION IF NOT EXISTS pgcrypto;


-- --
-- -- Name: gen_random_string(text, integer); Type: FUNCTION; Schema: public; Owner: -
-- --

CREATE OR REPLACE FUNCTION gen_random_string(prefix text, len integer) RETURNS text
    LANGUAGE plpgsql
    AS $$
   DECLARE
   BEGIN
      RETURN prefix || '_' || lower(substr(encode(gen_random_bytes(len / 2 + 1), 'hex'), 1, len));
   END; 
$$;


-- -- The following should be run once by a superuser. Before any migrations are executed

-- ### IDs ###
--
-- Name: global_id_sequence; Type: SEQUENCE; Schema: public; Owner: -
-- Info: should be created per instance
--
CREATE SEQUENCE IF NOT EXISTS global_id_sequence;

--
-- Name: next_id(OUT result bigint; Type: FUNCTION; Schema: public; Owner: -
-- Info: should be manually applied due to the instance id
--
CREATE OR REPLACE FUNCTION next_id(OUT result bigint) AS $$
  DECLARE
    our_epoch bigint := 156436515600;

    seq_id bigint;
    now_millis bigint;
    -- UNIQUE INSTANCE IDENTIFIER
    -- CHANGE THIS FOR EACH INSTANCE!!!
    instance_id int := 1;
  BEGIN
      SELECT nextval('global_id_sequence') % 1024 INTO seq_id;
      SELECT FLOOR(EXTRACT(EPOCH FROM clock_timestamp()) * 100) INTO now_millis;
      result := (now_millis - our_epoch) << 23;
      result := result | (instance_id << 10);
      result := result | (seq_id);
      SELECT nextval('global_id_sequence') INTO result;
  END;
$$ LANGUAGE PLPGSQL;

-- CREATE DATABASE TribeCo;
-- CREATE ROLE TribeCo;
-- CREATE ROLE migrator;

-- CREATE DATABASE kong;
-- CREATE ROLE kong;

-- -- CREATE ROLE reports;

-- -- -- ### Security ###

-- REVOKE ALL ON DATABASE TribeCo FROM PUBLIC;
-- REVOKE ALL ON DATABASE kong FROM PUBLIC;

-- GRANT ALL
--   ON DATABASE TribeCo TO migrator;
-- GRANT ALL 
--   ON DATABASE kong TO kong;


-- GRANT CONNECT 
--   ON DATABASE TribeCo TO TribeCo;

-- GRANT USAGE ON SCHEMA public 
--         TO TribeCo;

-- GRANT SELECT, INSERT, UPDATE, DELETE
--     ON ALL TABLES IN SCHEMA public
--     TO TribeCo;
-- ALTER DEFAULT PRIVILEGES IN SCHEMA public 
--     GRANT SELECT, INSERT, UPDATE, DELETE 
--     ON TABLES TO TribeCo;

-- GRANT EXECUTE
--     ON ALL FUNCTIONS IN SCHEMA public
--     TO TribeCo;
-- ALTER DEFAULT PRIVILEGES IN SCHEMA public 
--     GRANT EXECUTE 
--     ON FUNCTIONS TO TribeCo;

-- GRANT USAGE, SELECT
--     ON ALL SEQUENCES IN SCHEMA PUBLIC
--     TO TribeCo;
-- ALTER DEFAULT PRIVILEGES IN SCHEMA public 
--     GRANT USAGE, SELECT 
--     ON SEQUENCES TO TribeCo;

-- ALTER DEFAULT PRIVILEGES IN SCHEMA public 
--     GRANT USAGE 
--     ON TYPES TO TribeCo;



-- -- GRANT CONNECT 
-- --   ON DATABASE TribeCo TO reports;

-- -- GRANT USAGE ON SCHEMA public 
-- --         TO reports;
-- -- GRANT USAGE ON SCHEMA operations 
-- --         TO reports;

-- -- GRANT SELECT
-- --     ON ALL TABLES IN SCHEMA public
-- --     TO reports;
-- -- ALTER DEFAULT PRIVILEGES IN SCHEMA public 
-- --     GRANT SELECT
-- --     ON TABLES TO reports;
-- -- GRANT SELECT
-- --     ON ALL TABLES IN SCHEMA operations
-- --     TO reports;
-- -- ALTER DEFAULT PRIVILEGES IN SCHEMA operations 
-- --     GRANT SELECT
-- --     ON TABLES TO reports;

-- -- ALTER DEFAULT PRIVILEGES IN SCHEMA public 
-- --     GRANT USAGE 
-- --     ON TYPES TO reports;
-- -- ALTER DEFAULT PRIVILEGES IN SCHEMA operations 
-- --     GRANT USAGE 
-- --     ON TYPES TO reports;
