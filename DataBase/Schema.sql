--
-- PostgreSQL database dump
--

\restrict STZweefJ5uq0ekDl1ujqKjg1QF5PBLY4M98dplNLw3D1T0oOyfcDebfazg0eUXO

-- Dumped from database version 16.13 (Ubuntu 16.13-0ubuntu0.24.04.1)
-- Dumped by pg_dump version 18.1

-- Started on 2026-03-06 16:46:37

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 9 (class 2615 OID 16690)
-- Name: admin; Type: SCHEMA; Schema: -; Owner: LouNexus_Admin
--

CREATE SCHEMA admin;


ALTER SCHEMA admin OWNER TO "LouNexus_Admin";

--
-- TOC entry 6 (class 2615 OID 16396)
-- Name: core; Type: SCHEMA; Schema: -; Owner: LouNexus_Admin
--

CREATE SCHEMA core;


ALTER SCHEMA core OWNER TO "LouNexus_Admin";

--
-- TOC entry 7 (class 2615 OID 16443)
-- Name: prod; Type: SCHEMA; Schema: -; Owner: LouNexus_Admin
--

CREATE SCHEMA prod;


ALTER SCHEMA prod OWNER TO "LouNexus_Admin";

--
-- TOC entry 8 (class 2615 OID 16639)
-- Name: quality; Type: SCHEMA; Schema: -; Owner: LouNexus_Admin
--

CREATE SCHEMA quality;


ALTER SCHEMA quality OWNER TO "LouNexus_Admin";

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 219 (class 1259 OID 16398)
-- Name: factory; Type: TABLE; Schema: core; Owner: LouNexus_Admin
--

CREATE TABLE core.factory (
    factory_id integer NOT NULL,
    factory_name text NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_utc timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE core.factory OWNER TO "LouNexus_Admin";

--
-- TOC entry 222 (class 1259 OID 16444)
-- Name: part; Type: TABLE; Schema: core; Owner: LouNexus_Admin
--

CREATE TABLE core.part (
    part_id integer NOT NULL,
    part_number text NOT NULL,
    part_name text NOT NULL,
    part_description text,
    print_url text,
    is_active boolean DEFAULT true NOT NULL,
    created_utc timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE core.part OWNER TO "LouNexus_Admin";

--
-- TOC entry 226 (class 1259 OID 16515)
-- Name: part_measurement; Type: TABLE; Schema: core; Owner: LouNexus_Admin
--

CREATE TABLE core.part_measurement (
    part_measurement_id integer NOT NULL,
    part_measurement_name text NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_utc timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE core.part_measurement OWNER TO "LouNexus_Admin";

--
-- TOC entry 225 (class 1259 OID 16499)
-- Name: part_measurement_spec; Type: TABLE; Schema: core; Owner: LouNexus_Admin
--

CREATE TABLE core.part_measurement_spec (
    part_measurement_spec_id integer NOT NULL,
    part_id integer NOT NULL,
    workstation_type_id integer NOT NULL,
    part_measurement_id integer NOT NULL,
    target_value numeric DEFAULT 0 NOT NULL,
    upper_limit numeric DEFAULT 0 NOT NULL,
    lower_limit numeric,
    is_active boolean DEFAULT true NOT NULL,
    created_utc timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE core.part_measurement_spec OWNER TO "LouNexus_Admin";

--
-- TOC entry 227 (class 1259 OID 16529)
-- Name: part_tracking_attribute; Type: TABLE; Schema: core; Owner: LouNexus_Admin
--

CREATE TABLE core.part_tracking_attribute (
    part_tracking_attribute_id integer NOT NULL,
    part_id integer NOT NULL,
    workstation_type_id integer NOT NULL,
    attribute_name text NOT NULL,
    is_required boolean DEFAULT false NOT NULL,
    sort_order integer DEFAULT 0 NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_utc timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE core.part_tracking_attribute OWNER TO "LouNexus_Admin";

--
-- TOC entry 223 (class 1259 OID 16457)
-- Name: part_workstation_requirement; Type: TABLE; Schema: core; Owner: LouNexus_Admin
--

CREATE TABLE core.part_workstation_requirement (
    part_workstation_requirement_id integer NOT NULL,
    part_id integer NOT NULL,
    workstation_type_id integer NOT NULL,
    sequence_order integer NOT NULL,
    is_required boolean NOT NULL,
    created_utc timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE core.part_workstation_requirement OWNER TO "LouNexus_Admin";

--
-- TOC entry 221 (class 1259 OID 16432)
-- Name: reject_code; Type: TABLE; Schema: core; Owner: LouNexus_Admin
--

CREATE TABLE core.reject_code (
    reject_code_id integer NOT NULL,
    code text NOT NULL,
    description text NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_utc timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE core.reject_code OWNER TO "LouNexus_Admin";

--
-- TOC entry 220 (class 1259 OID 16409)
-- Name: workstation; Type: TABLE; Schema: core; Owner: LouNexus_Admin
--

CREATE TABLE core.workstation (
    workstation_id integer NOT NULL,
    factory_id integer NOT NULL,
    workstation_name text NOT NULL,
    workstation_code text NOT NULL,
    workstation_mode text NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    sort_order integer DEFAULT 0 NOT NULL,
    notes text,
    created_utc timestamp with time zone DEFAULT now() NOT NULL,
    workstation_type_id integer NOT NULL
);


ALTER TABLE core.workstation OWNER TO "LouNexus_Admin";

--
-- TOC entry 224 (class 1259 OID 16472)
-- Name: workstation_type; Type: TABLE; Schema: core; Owner: LouNexus_Admin
--

CREATE TABLE core.workstation_type (
    workstation_type_id integer NOT NULL,
    workstation_type_name text NOT NULL,
    workstation_type_code text NOT NULL,
    supports_cpk boolean DEFAULT false NOT NULL,
    supports_reject_entry boolean DEFAULT true NOT NULL,
    supports_tracking_attributes boolean DEFAULT false NOT NULL,
    supports_clockout boolean DEFAULT false NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_utc timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE core.workstation_type OWNER TO "LouNexus_Admin";

--
-- TOC entry 228 (class 1259 OID 16553)
-- Name: inspection; Type: TABLE; Schema: prod; Owner: LouNexus_Admin
--

CREATE TABLE prod.inspection (
    inspection_id integer NOT NULL,
    inspection_number text NOT NULL,
    factory_id integer NOT NULL,
    part_id integer NOT NULL,
    initial_quantity integer NOT NULL,
    status text NOT NULL,
    notes text,
    created_utc timestamp with time zone DEFAULT now() NOT NULL,
    CONSTRAINT "quantity greater than 0" CHECK ((initial_quantity > 0))
);


ALTER TABLE prod.inspection OWNER TO "LouNexus_Admin";

--
-- TOC entry 229 (class 1259 OID 16574)
-- Name: station_event; Type: TABLE; Schema: prod; Owner: LouNexus_Admin
--

CREATE TABLE prod.station_event (
    station_event_id integer NOT NULL,
    inspection_id integer NOT NULL,
    workstation_id integer NOT NULL,
    event_type text NOT NULL,
    good_quantity integer NOT NULL,
    notes text,
    event_time_utc timestamp with time zone,
    create_time_utc timestamp with time zone DEFAULT now() NOT NULL,
    CONSTRAINT "good parts greater or equal to 0" CHECK ((good_quantity >= 0))
);


ALTER TABLE prod.station_event OWNER TO "LouNexus_Admin";

--
-- TOC entry 231 (class 1259 OID 16618)
-- Name: station_event_attribute; Type: TABLE; Schema: prod; Owner: LouNexus_Admin
--

CREATE TABLE prod.station_event_attribute (
    station_event_attribute_id integer NOT NULL,
    station_event_id integer NOT NULL,
    part_tracking_attribute_id integer NOT NULL,
    attribute_value text NOT NULL,
    notes text,
    created_utc timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE prod.station_event_attribute OWNER TO "LouNexus_Admin";

--
-- TOC entry 230 (class 1259 OID 16593)
-- Name: station_event_reject; Type: TABLE; Schema: prod; Owner: LouNexus_Admin
--

CREATE TABLE prod.station_event_reject (
    station_event_reject_id integer NOT NULL,
    station_even_id integer NOT NULL,
    reject_code_id integer NOT NULL,
    quantity integer NOT NULL,
    notes text,
    created_utc timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE prod.station_event_reject OWNER TO "LouNexus_Admin";

--
-- TOC entry 232 (class 1259 OID 16641)
-- Name: measurement_set; Type: TABLE; Schema: quality; Owner: LouNexus_Admin
--

CREATE TABLE quality.measurement_set (
    measurement_set_id integer NOT NULL,
    station_event_id integer NOT NULL,
    part_measurement_id integer NOT NULL,
    notes text,
    created_utc timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE quality.measurement_set OWNER TO "LouNexus_Admin";

--
-- TOC entry 233 (class 1259 OID 16670)
-- Name: measurement_value; Type: TABLE; Schema: quality; Owner: LouNexus_Admin
--

CREATE TABLE quality.measurement_value (
    measurement_value_id integer NOT NULL,
    measurement_set_id integer NOT NULL,
    sample_index integer NOT NULL,
    measured_value numeric NOT NULL,
    created_utc timestamp with time zone DEFAULT now() NOT NULL,
    CONSTRAINT "sample index > 0" CHECK ((sample_index > 0))
);


ALTER TABLE quality.measurement_value OWNER TO "LouNexus_Admin";

--
-- TOC entry 3350 (class 2606 OID 16406)
-- Name: factory factory_pkey; Type: CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.factory
    ADD CONSTRAINT factory_pkey PRIMARY KEY (factory_id);


--
-- TOC entry 3382 (class 2606 OID 16551)
-- Name: part_tracking_attribute name unique; Type: CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.part_tracking_attribute
    ADD CONSTRAINT "name unique" UNIQUE (attribute_name);


--
-- TOC entry 3380 (class 2606 OID 16523)
-- Name: part_measurement part_measurement_pkey; Type: CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.part_measurement
    ADD CONSTRAINT part_measurement_pkey PRIMARY KEY (part_measurement_id);


--
-- TOC entry 3378 (class 2606 OID 16509)
-- Name: part_measurement_spec part_measurement_spec_pkey; Type: CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.part_measurement_spec
    ADD CONSTRAINT part_measurement_spec_pkey PRIMARY KEY (part_measurement_spec_id);


--
-- TOC entry 3364 (class 2606 OID 16452)
-- Name: part part_pkey; Type: CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.part
    ADD CONSTRAINT part_pkey PRIMARY KEY (part_id);


--
-- TOC entry 3384 (class 2606 OID 16539)
-- Name: part_tracking_attribute part_tracking_attribute_pkey; Type: CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.part_tracking_attribute
    ADD CONSTRAINT part_tracking_attribute_pkey PRIMARY KEY (part_tracking_attribute_id);


--
-- TOC entry 3370 (class 2606 OID 16461)
-- Name: part_workstation_requirement part_workstation_requirement_pkey; Type: CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.part_workstation_requirement
    ADD CONSTRAINT part_workstation_requirement_pkey PRIMARY KEY (part_workstation_requirement_id);


--
-- TOC entry 3360 (class 2606 OID 16440)
-- Name: reject_code reject_code_pkey; Type: CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.reject_code
    ADD CONSTRAINT reject_code_pkey PRIMARY KEY (reject_code_id);


--
-- TOC entry 3362 (class 2606 OID 16442)
-- Name: reject_code unique code; Type: CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.reject_code
    ADD CONSTRAINT "unique code" UNIQUE (code);


--
-- TOC entry 3354 (class 2606 OID 16426)
-- Name: workstation unique factory id; Type: CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.workstation
    ADD CONSTRAINT "unique factory id" UNIQUE (factory_id);


--
-- TOC entry 3352 (class 2606 OID 16408)
-- Name: factory unique factory name; Type: CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.factory
    ADD CONSTRAINT "unique factory name" UNIQUE (factory_name);


--
-- TOC entry 3366 (class 2606 OID 16454)
-- Name: part unique part number; Type: CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.part
    ADD CONSTRAINT "unique part number" UNIQUE (part_number);


--
-- TOC entry 3368 (class 2606 OID 16456)
-- Name: part unique url; Type: CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.part
    ADD CONSTRAINT "unique url" UNIQUE (print_url);


--
-- TOC entry 3356 (class 2606 OID 16424)
-- Name: workstation unique work station code; Type: CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.workstation
    ADD CONSTRAINT "unique work station code" UNIQUE (workstation_code);


--
-- TOC entry 3358 (class 2606 OID 16422)
-- Name: workstation workstation_pkey; Type: CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.workstation
    ADD CONSTRAINT workstation_pkey PRIMARY KEY (workstation_id);


--
-- TOC entry 3372 (class 2606 OID 16484)
-- Name: workstation_type workstation_type_pkey; Type: CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.workstation_type
    ADD CONSTRAINT workstation_type_pkey PRIMARY KEY (workstation_type_id);


--
-- TOC entry 3374 (class 2606 OID 16488)
-- Name: workstation_type ws_type_code unique; Type: CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.workstation_type
    ADD CONSTRAINT "ws_type_code unique" UNIQUE (workstation_type_code);


--
-- TOC entry 3376 (class 2606 OID 16486)
-- Name: workstation_type ws_type_name unique; Type: CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.workstation_type
    ADD CONSTRAINT "ws_type_name unique" UNIQUE (workstation_type_name);


--
-- TOC entry 3386 (class 2606 OID 16561)
-- Name: inspection inspection_pkey; Type: CONSTRAINT; Schema: prod; Owner: LouNexus_Admin
--

ALTER TABLE ONLY prod.inspection
    ADD CONSTRAINT inspection_pkey PRIMARY KEY (inspection_id);


--
-- TOC entry 3398 (class 2606 OID 16627)
-- Name: station_event_attribute no dupes; Type: CONSTRAINT; Schema: prod; Owner: LouNexus_Admin
--

ALTER TABLE ONLY prod.station_event_attribute
    ADD CONSTRAINT "no dupes" UNIQUE (station_event_id, part_tracking_attribute_id);


--
-- TOC entry 3400 (class 2606 OID 16625)
-- Name: station_event_attribute station_event_attribute_pkey; Type: CONSTRAINT; Schema: prod; Owner: LouNexus_Admin
--

ALTER TABLE ONLY prod.station_event_attribute
    ADD CONSTRAINT station_event_attribute_pkey PRIMARY KEY (station_event_attribute_id);


--
-- TOC entry 3390 (class 2606 OID 16582)
-- Name: station_event station_event_pkey; Type: CONSTRAINT; Schema: prod; Owner: LouNexus_Admin
--

ALTER TABLE ONLY prod.station_event
    ADD CONSTRAINT station_event_pkey PRIMARY KEY (station_event_id);


--
-- TOC entry 3392 (class 2606 OID 16600)
-- Name: station_event_reject station_event_reject_pkey; Type: CONSTRAINT; Schema: prod; Owner: LouNexus_Admin
--

ALTER TABLE ONLY prod.station_event_reject
    ADD CONSTRAINT station_event_reject_pkey PRIMARY KEY (station_event_reject_id);


--
-- TOC entry 3388 (class 2606 OID 16563)
-- Name: inspection unique inspection number; Type: CONSTRAINT; Schema: prod; Owner: LouNexus_Admin
--

ALTER TABLE ONLY prod.inspection
    ADD CONSTRAINT "unique inspection number" UNIQUE (inspection_number);


--
-- TOC entry 3394 (class 2606 OID 16614)
-- Name: station_event_reject unique reject code id; Type: CONSTRAINT; Schema: prod; Owner: LouNexus_Admin
--

ALTER TABLE ONLY prod.station_event_reject
    ADD CONSTRAINT "unique reject code id" UNIQUE (reject_code_id);


--
-- TOC entry 3396 (class 2606 OID 16612)
-- Name: station_event_reject unique station event id; Type: CONSTRAINT; Schema: prod; Owner: LouNexus_Admin
--

ALTER TABLE ONLY prod.station_event_reject
    ADD CONSTRAINT "unique station event id" UNIQUE (station_even_id);


--
-- TOC entry 3402 (class 2606 OID 16648)
-- Name: measurement_set measurement_set_pkey; Type: CONSTRAINT; Schema: quality; Owner: LouNexus_Admin
--

ALTER TABLE ONLY quality.measurement_set
    ADD CONSTRAINT measurement_set_pkey PRIMARY KEY (measurement_set_id);


--
-- TOC entry 3406 (class 2606 OID 16678)
-- Name: measurement_value measurement_value_pkey; Type: CONSTRAINT; Schema: quality; Owner: LouNexus_Admin
--

ALTER TABLE ONLY quality.measurement_value
    ADD CONSTRAINT measurement_value_pkey PRIMARY KEY (measurement_value_id);


--
-- TOC entry 3404 (class 2606 OID 16650)
-- Name: measurement_set no dupes; Type: CONSTRAINT; Schema: quality; Owner: LouNexus_Admin
--

ALTER TABLE ONLY quality.measurement_set
    ADD CONSTRAINT "no dupes" UNIQUE (station_event_id, part_measurement_id);


--
-- TOC entry 3408 (class 2606 OID 16680)
-- Name: measurement_value no dupes measurements; Type: CONSTRAINT; Schema: quality; Owner: LouNexus_Admin
--

ALTER TABLE ONLY quality.measurement_value
    ADD CONSTRAINT "no dupes measurements" UNIQUE (measurement_set_id, sample_index);


--
-- TOC entry 3409 (class 2606 OID 16427)
-- Name: workstation factory id foreign key; Type: FK CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.workstation
    ADD CONSTRAINT "factory id foreign key" FOREIGN KEY (factory_id) REFERENCES core.factory(factory_id);


--
-- TOC entry 3411 (class 2606 OID 16462)
-- Name: part_workstation_requirement foreignkeys - part id; Type: FK CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.part_workstation_requirement
    ADD CONSTRAINT "foreignkeys - part id" FOREIGN KEY (part_id) REFERENCES core.part(part_id) NOT VALID;


--
-- TOC entry 3412 (class 2606 OID 16494)
-- Name: part_workstation_requirement foreignkeys - workstation type id; Type: FK CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.part_workstation_requirement
    ADD CONSTRAINT "foreignkeys - workstation type id" FOREIGN KEY (workstation_type_id) REFERENCES core.workstation_type(workstation_type_id) NOT VALID;


--
-- TOC entry 3413 (class 2606 OID 16510)
-- Name: part_measurement_spec part id foreign key; Type: FK CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.part_measurement_spec
    ADD CONSTRAINT "part id foreign key" FOREIGN KEY (part_id) REFERENCES core.part(part_id);


--
-- TOC entry 3414 (class 2606 OID 16524)
-- Name: part_measurement_spec part measurement id foreign key; Type: FK CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.part_measurement_spec
    ADD CONSTRAINT "part measurement id foreign key" FOREIGN KEY (part_measurement_id) REFERENCES core.part_measurement(part_measurement_id) NOT VALID;


--
-- TOC entry 3415 (class 2606 OID 16540)
-- Name: part_tracking_attribute partidFK; Type: FK CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.part_tracking_attribute
    ADD CONSTRAINT "partidFK" FOREIGN KEY (part_id) REFERENCES core.part(part_id);


--
-- TOC entry 3416 (class 2606 OID 16545)
-- Name: part_tracking_attribute wks type id fk; Type: FK CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.part_tracking_attribute
    ADD CONSTRAINT "wks type id fk" FOREIGN KEY (workstation_type_id) REFERENCES core.workstation_type(workstation_type_id);


--
-- TOC entry 3410 (class 2606 OID 16489)
-- Name: workstation workstation_type_id foreign key; Type: FK CONSTRAINT; Schema: core; Owner: LouNexus_Admin
--

ALTER TABLE ONLY core.workstation
    ADD CONSTRAINT "workstation_type_id foreign key" FOREIGN KEY (workstation_type_id) REFERENCES core.workstation_type(workstation_type_id) NOT VALID;


--
-- TOC entry 3417 (class 2606 OID 16564)
-- Name: inspection factory id fk; Type: FK CONSTRAINT; Schema: prod; Owner: LouNexus_Admin
--

ALTER TABLE ONLY prod.inspection
    ADD CONSTRAINT "factory id fk" FOREIGN KEY (factory_id) REFERENCES core.factory(factory_id);


--
-- TOC entry 3419 (class 2606 OID 16583)
-- Name: station_event insp id fk; Type: FK CONSTRAINT; Schema: prod; Owner: LouNexus_Admin
--

ALTER TABLE ONLY prod.station_event
    ADD CONSTRAINT "insp id fk" FOREIGN KEY (inspection_id) REFERENCES prod.inspection(inspection_id);


--
-- TOC entry 3418 (class 2606 OID 16569)
-- Name: inspection part id fk; Type: FK CONSTRAINT; Schema: prod; Owner: LouNexus_Admin
--

ALTER TABLE ONLY prod.inspection
    ADD CONSTRAINT "part id fk" FOREIGN KEY (part_id) REFERENCES core.part(part_id);


--
-- TOC entry 3423 (class 2606 OID 16633)
-- Name: station_event_attribute part tracking attribute id fk; Type: FK CONSTRAINT; Schema: prod; Owner: LouNexus_Admin
--

ALTER TABLE ONLY prod.station_event_attribute
    ADD CONSTRAINT "part tracking attribute id fk" FOREIGN KEY (part_tracking_attribute_id) REFERENCES core.part_tracking_attribute(part_tracking_attribute_id);


--
-- TOC entry 3421 (class 2606 OID 16601)
-- Name: station_event_reject reject code fk; Type: FK CONSTRAINT; Schema: prod; Owner: LouNexus_Admin
--

ALTER TABLE ONLY prod.station_event_reject
    ADD CONSTRAINT "reject code fk" FOREIGN KEY (reject_code_id) REFERENCES core.reject_code(reject_code_id);


--
-- TOC entry 3424 (class 2606 OID 16628)
-- Name: station_event_attribute station event id fk; Type: FK CONSTRAINT; Schema: prod; Owner: LouNexus_Admin
--

ALTER TABLE ONLY prod.station_event_attribute
    ADD CONSTRAINT "station event id fk" FOREIGN KEY (station_event_id) REFERENCES prod.station_event(station_event_id);


--
-- TOC entry 3422 (class 2606 OID 16606)
-- Name: station_event_reject station_event_id; Type: FK CONSTRAINT; Schema: prod; Owner: LouNexus_Admin
--

ALTER TABLE ONLY prod.station_event_reject
    ADD CONSTRAINT station_event_id FOREIGN KEY (station_even_id) REFERENCES prod.station_event(station_event_id);


--
-- TOC entry 3420 (class 2606 OID 16588)
-- Name: station_event workstation_id; Type: FK CONSTRAINT; Schema: prod; Owner: LouNexus_Admin
--

ALTER TABLE ONLY prod.station_event
    ADD CONSTRAINT workstation_id FOREIGN KEY (workstation_id) REFERENCES core.workstation(workstation_id);


--
-- TOC entry 3427 (class 2606 OID 16681)
-- Name: measurement_value measurement set id fk; Type: FK CONSTRAINT; Schema: quality; Owner: LouNexus_Admin
--

ALTER TABLE ONLY quality.measurement_value
    ADD CONSTRAINT "measurement set id fk" FOREIGN KEY (measurement_set_id) REFERENCES quality.measurement_set(measurement_set_id);


--
-- TOC entry 3425 (class 2606 OID 16656)
-- Name: measurement_set part measurement id fk; Type: FK CONSTRAINT; Schema: quality; Owner: LouNexus_Admin
--

ALTER TABLE ONLY quality.measurement_set
    ADD CONSTRAINT "part measurement id fk" FOREIGN KEY (part_measurement_id) REFERENCES core.part_measurement(part_measurement_id);


--
-- TOC entry 3426 (class 2606 OID 16651)
-- Name: measurement_set station event id fk; Type: FK CONSTRAINT; Schema: quality; Owner: LouNexus_Admin
--

ALTER TABLE ONLY quality.measurement_set
    ADD CONSTRAINT "station event id fk" FOREIGN KEY (station_event_id) REFERENCES prod.station_event(station_event_id);


--
-- TOC entry 3576 (class 0 OID 0)
-- Dependencies: 9
-- Name: SCHEMA admin; Type: ACL; Schema: -; Owner: LouNexus_Admin
--

GRANT USAGE ON SCHEMA admin TO "LouNexus";


--
-- TOC entry 3577 (class 0 OID 0)
-- Dependencies: 6
-- Name: SCHEMA core; Type: ACL; Schema: -; Owner: LouNexus_Admin
--

GRANT USAGE ON SCHEMA core TO "LouNexus";


--
-- TOC entry 3578 (class 0 OID 0)
-- Dependencies: 7
-- Name: SCHEMA prod; Type: ACL; Schema: -; Owner: LouNexus_Admin
--

GRANT USAGE ON SCHEMA prod TO "LouNexus";


--
-- TOC entry 3579 (class 0 OID 0)
-- Dependencies: 8
-- Name: SCHEMA quality; Type: ACL; Schema: -; Owner: LouNexus_Admin
--

GRANT USAGE ON SCHEMA quality TO "LouNexus";


--
-- TOC entry 3580 (class 0 OID 0)
-- Dependencies: 219
-- Name: TABLE factory; Type: ACL; Schema: core; Owner: LouNexus_Admin
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE core.factory TO "LouNexus";


--
-- TOC entry 3581 (class 0 OID 0)
-- Dependencies: 222
-- Name: TABLE part; Type: ACL; Schema: core; Owner: LouNexus_Admin
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE core.part TO "LouNexus";


--
-- TOC entry 3582 (class 0 OID 0)
-- Dependencies: 226
-- Name: TABLE part_measurement; Type: ACL; Schema: core; Owner: LouNexus_Admin
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE core.part_measurement TO "LouNexus";


--
-- TOC entry 3583 (class 0 OID 0)
-- Dependencies: 225
-- Name: TABLE part_measurement_spec; Type: ACL; Schema: core; Owner: LouNexus_Admin
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE core.part_measurement_spec TO "LouNexus";


--
-- TOC entry 3584 (class 0 OID 0)
-- Dependencies: 227
-- Name: TABLE part_tracking_attribute; Type: ACL; Schema: core; Owner: LouNexus_Admin
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE core.part_tracking_attribute TO "LouNexus";


--
-- TOC entry 3585 (class 0 OID 0)
-- Dependencies: 223
-- Name: TABLE part_workstation_requirement; Type: ACL; Schema: core; Owner: LouNexus_Admin
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE core.part_workstation_requirement TO "LouNexus";


--
-- TOC entry 3586 (class 0 OID 0)
-- Dependencies: 221
-- Name: TABLE reject_code; Type: ACL; Schema: core; Owner: LouNexus_Admin
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE core.reject_code TO "LouNexus";


--
-- TOC entry 3587 (class 0 OID 0)
-- Dependencies: 220
-- Name: TABLE workstation; Type: ACL; Schema: core; Owner: LouNexus_Admin
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE core.workstation TO "LouNexus";


--
-- TOC entry 3588 (class 0 OID 0)
-- Dependencies: 224
-- Name: TABLE workstation_type; Type: ACL; Schema: core; Owner: LouNexus_Admin
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE core.workstation_type TO "LouNexus";


--
-- TOC entry 3589 (class 0 OID 0)
-- Dependencies: 228
-- Name: TABLE inspection; Type: ACL; Schema: prod; Owner: LouNexus_Admin
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE prod.inspection TO "LouNexus";


--
-- TOC entry 3590 (class 0 OID 0)
-- Dependencies: 229
-- Name: TABLE station_event; Type: ACL; Schema: prod; Owner: LouNexus_Admin
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE prod.station_event TO "LouNexus";


--
-- TOC entry 3591 (class 0 OID 0)
-- Dependencies: 231
-- Name: TABLE station_event_attribute; Type: ACL; Schema: prod; Owner: LouNexus_Admin
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE prod.station_event_attribute TO "LouNexus";


--
-- TOC entry 3592 (class 0 OID 0)
-- Dependencies: 230
-- Name: TABLE station_event_reject; Type: ACL; Schema: prod; Owner: LouNexus_Admin
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE prod.station_event_reject TO "LouNexus";


--
-- TOC entry 3593 (class 0 OID 0)
-- Dependencies: 232
-- Name: TABLE measurement_set; Type: ACL; Schema: quality; Owner: LouNexus_Admin
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE quality.measurement_set TO "LouNexus";


--
-- TOC entry 3594 (class 0 OID 0)
-- Dependencies: 233
-- Name: TABLE measurement_value; Type: ACL; Schema: quality; Owner: LouNexus_Admin
--

GRANT SELECT,INSERT,DELETE,UPDATE ON TABLE quality.measurement_value TO "LouNexus";


--
-- TOC entry 2101 (class 826 OID 16692)
-- Name: DEFAULT PRIVILEGES FOR SEQUENCES; Type: DEFAULT ACL; Schema: admin; Owner: postgres
--

ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA admin GRANT SELECT ON SEQUENCES TO "LouNexus";


--
-- TOC entry 2102 (class 826 OID 16693)
-- Name: DEFAULT PRIVILEGES FOR TYPES; Type: DEFAULT ACL; Schema: admin; Owner: postgres
--

ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA admin GRANT ALL ON TYPES TO "LouNexus";


--
-- TOC entry 2100 (class 826 OID 16691)
-- Name: DEFAULT PRIVILEGES FOR TABLES; Type: DEFAULT ACL; Schema: admin; Owner: postgres
--

ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA admin GRANT SELECT ON TABLES TO "LouNexus";


--
-- TOC entry 2097 (class 826 OID 16397)
-- Name: DEFAULT PRIVILEGES FOR TABLES; Type: DEFAULT ACL; Schema: core; Owner: postgres
--

ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA core GRANT SELECT,INSERT,DELETE,UPDATE ON TABLES TO "LouNexus";


--
-- TOC entry 2098 (class 826 OID 16552)
-- Name: DEFAULT PRIVILEGES FOR TABLES; Type: DEFAULT ACL; Schema: prod; Owner: postgres
--

ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA prod GRANT SELECT,INSERT,DELETE,UPDATE ON TABLES TO "LouNexus";


--
-- TOC entry 2099 (class 826 OID 16640)
-- Name: DEFAULT PRIVILEGES FOR TABLES; Type: DEFAULT ACL; Schema: quality; Owner: postgres
--

ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA quality GRANT SELECT,INSERT,DELETE,UPDATE ON TABLES TO "LouNexus";


-- Completed on 2026-03-06 16:46:37

--
-- PostgreSQL database dump complete
--

\unrestrict STZweefJ5uq0ekDl1ujqKjg1QF5PBLY4M98dplNLw3D1T0oOyfcDebfazg0eUXO

