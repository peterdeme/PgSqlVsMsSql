drop table if exists persons;

CREATE TABLE persons
(
    personid serial primary key,
    lastname character varying(255) NOT NULL,
    firstname character varying(255) NOT NULL,
    addressline1 character varying(255) NOT NULL,
    addressline2 character varying(255) NULL,
    city character varying(255) NOT NULL,
    zipcode character varying(20) NOT NULL
);
