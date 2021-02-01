CREATE TABLE public.user (
    id bigint DEFAULT next_id() NOT NULL,
    name text NOT NULL,
    email text NOT NULL,
    password text NOT NULL,
    description text NULL,
    phone_number text NOT NULL,
    gender text NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL
);

ALTER TABLE ONLY public.user
    ADD CONSTRAINT user_pkey PRIMARY KEY (id);
ALTER TABLE ONLY public.user
    ADD CONSTRAINT user_email_key UNIQUE (email);

CREATE TABLE public.third_party_auth (
    id bigint DEFAULT next_id() NOT NULL,
    user_id bigint NOT NULL,
    key text NOT NULL,
    email text NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL
);

ALTER TABLE ONLY public.third_party_auth
    ADD CONSTRAINT third_party_auth_user_pkey PRIMARY KEY (id);
ALTER TABLE ONLY public.third_party_auth
    ADD CONSTRAINT third_party_auth_user_email_key UNIQUE (email);