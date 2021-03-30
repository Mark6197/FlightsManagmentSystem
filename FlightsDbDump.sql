PGDMP         ;                y           flights_managment_system    13.1    13.1 �    b           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            c           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            d           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            e           1262    16585    flights_managment_system    DATABASE     |   CREATE DATABASE flights_managment_system WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE = 'English_United States.1252';
 (   DROP DATABASE flights_managment_system;
                postgres    false            �            1255    16783 1   sp_add_administrator(text, text, integer, bigint)    FUNCTION     �  CREATE FUNCTION public.sp_add_administrator(_first_name text, _last_name text, _level integer, _user_id bigint) RETURNS integer
    LANGUAGE plpgsql
    AS $$
            DECLARE
                record_id integer;

            BEGIN
                INSERT INTO administrators(first_name, last_name, level, user_id) values (_first_name, _last_name, _level, _user_id)
                    returning id into record_id;

                return record_id;
            END;
    $$;
 o   DROP FUNCTION public.sp_add_administrator(_first_name text, _last_name text, _level integer, _user_id bigint);
       public          postgres    false                       1255    62035 -   sp_add_airline_company(text, integer, bigint)    FUNCTION     �  CREATE FUNCTION public.sp_add_airline_company(_name text, _country_id integer, _user_id bigint) RETURNS bigint
    LANGUAGE plpgsql
    AS $$
DECLARE
                record_id bigint;
            BEGIN
                INSERT INTO airline_companies(name, country_id, user_id) values (_name, _country_id, _user_id)
                    returning id into record_id;

                return record_id;
            END;
$$;
 _   DROP FUNCTION public.sp_add_airline_company(_name text, _country_id integer, _user_id bigint);
       public          postgres    false                       1255    36867    sp_add_country(text)    FUNCTION     C  CREATE FUNCTION public.sp_add_country(_name text) RETURNS integer
    LANGUAGE plpgsql
    AS $$
DECLARE
                record_id integer;

            BEGIN
                INSERT INTO countries(name) values (_name)
                    returning id into record_id;

                return record_id;
            END;
$$;
 1   DROP FUNCTION public.sp_add_country(_name text);
       public          postgres    false                       1255    58695 5   sp_add_customer(text, text, text, text, text, bigint)    FUNCTION     5  CREATE FUNCTION public.sp_add_customer(_first_name text, _last_name text, _address text, _phone_number text, _credit_card_number text, _user_id bigint) RETURNS bigint
    LANGUAGE plpgsql
    AS $$
DECLARE
                record_id bigint;
            BEGIN
                INSERT INTO customers(first_name, last_name, address, phone_number, credit_card_number, user_id) values (_first_name, _last_name, _address, _phone_number, _credit_card_number, _user_id)
                    returning id into record_id;

                return record_id;
            END;
$$;
 �   DROP FUNCTION public.sp_add_customer(_first_name text, _last_name text, _address text, _phone_number text, _credit_card_number text, _user_id bigint);
       public          postgres    false                       1255    70887 j   sp_add_flight(bigint, integer, integer, timestamp without time zone, timestamp without time zone, integer)    FUNCTION     �  CREATE FUNCTION public.sp_add_flight(_airline_company_id bigint, _origin_country_id integer, _destination_country_id integer, _departure_time timestamp without time zone, _landing_time timestamp without time zone, _remaining_tickets integer) RETURNS bigint
    LANGUAGE plpgsql
    AS $$
DECLARE
                record_id bigint;
            BEGIN
                INSERT INTO flights(airline_company_id, origin_country_id, destination_country_id, departure_time, landing_time, remaining_tickets) values (_airline_company_id, _origin_country_id, _destination_country_id, _departure_time, _landing_time, _remaining_tickets)
                    returning id into record_id;

                return record_id;
            END;
$$;
 �   DROP FUNCTION public.sp_add_flight(_airline_company_id bigint, _origin_country_id integer, _destination_country_id integer, _departure_time timestamp without time zone, _landing_time timestamp without time zone, _remaining_tickets integer);
       public          postgres    false                       1255    24777 z   sp_add_flight_history(bigint, bigint, integer, integer, timestamp without time zone, timestamp without time zone, integer)    FUNCTION       CREATE FUNCTION public.sp_add_flight_history(_original_id bigint, _airline_company_id bigint, _origin_country_id integer, _destination_country_id integer, _departure_time timestamp without time zone, _landing_time timestamp without time zone, _remaining_tickets integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$
DECLARE
                record_id integer;
            BEGIN
                INSERT INTO flights_history(flight_original_id, airline_company_id, origin_country_id, destination_country_id, departure_time, landing_time, remaining_tickets) values (_original_id,_airline_company_id, _origin_country_id, _destination_country_id, _departure_time, _landing_time, _remaining_tickets)
                    returning id into record_id;

                return record_id;
            END;
$$;
   DROP FUNCTION public.sp_add_flight_history(_original_id bigint, _airline_company_id bigint, _origin_country_id integer, _destination_country_id integer, _departure_time timestamp without time zone, _landing_time timestamp without time zone, _remaining_tickets integer);
       public          postgres    false                       1255    88272    sp_add_ticket(bigint, bigint)    FUNCTION     ~  CREATE FUNCTION public.sp_add_ticket(_flight_id bigint, _customer_id bigint) RETURNS bigint
    LANGUAGE plpgsql
    AS $$
DECLARE
                record_id bigint;
            BEGIN
                INSERT INTO tickets(flight_id, customer_id) values (_flight_id, _customer_id)
                    returning id into record_id;

                return record_id;
            END;
$$;
 L   DROP FUNCTION public.sp_add_ticket(_flight_id bigint, _customer_id bigint);
       public          postgres    false                       1255    24778 -   sp_add_ticket_history(bigint, bigint, bigint)    FUNCTION     �  CREATE FUNCTION public.sp_add_ticket_history(_original_id bigint, _flight_id bigint, _customer_id bigint) RETURNS integer
    LANGUAGE plpgsql
    AS $$
DECLARE
                record_id integer;
            BEGIN
                INSERT INTO tickets_history(original_ticket_id, flight_id, customer_id) values (_original_id, _flight_id, _customer_id)
                    returning id into record_id;

                return record_id;
            END;
$$;
 i   DROP FUNCTION public.sp_add_ticket_history(_original_id bigint, _flight_id bigint, _customer_id bigint);
       public          postgres    false                       1255    25533 &   sp_add_user(text, text, text, integer)    FUNCTION     �  CREATE FUNCTION public.sp_add_user(_username text, _password text, _email text, _user_role_id integer) RETURNS bigint
    LANGUAGE plpgsql
    AS $$
DECLARE
                record_id bigint;

            BEGIN
                INSERT INTO users(username, password, email, user_role) values (_username, _password, _email, _user_role_id)
                    returning id into record_id;

                return record_id;
            END;
$$;
 f   DROP FUNCTION public.sp_add_user(_username text, _password text, _email text, _user_role_id integer);
       public          postgres    false                       1255    24792    sp_clear_db() 	   PROCEDURE     �  CREATE PROCEDURE public.sp_clear_db()
    LANGUAGE plpgsql
    AS $$
BEGIN
TRUNCATE TABLE tickets RESTART IDENTITY CASCADE;
TRUNCATE TABLE flights RESTART IDENTITY CASCADE;
TRUNCATE TABLE airline_companies RESTART IDENTITY CASCADE;
TRUNCATE TABLE customers RESTART IDENTITY CASCADE;
TRUNCATE TABLE administrators RESTART IDENTITY CASCADE;
TRUNCATE TABLE users RESTART IDENTITY CASCADE;
TRUNCATE TABLE countries RESTART IDENTITY CASCADE;
END;
$$;
 %   DROP PROCEDURE public.sp_clear_db();
       public          postgres    false            �            1255    16786    sp_get_administrator(integer)    FUNCTION     -  CREATE FUNCTION public.sp_get_administrator(_id integer) RETURNS TABLE(admin_id integer, first_name text, last_name text, level integer, user_id bigint, username text, password text, email text, user_role_id integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select a.id,a.first_name,a.last_name,a.level, u.id, u.username, u.password, u.email, u.user_role  from administrators a
                    join users u on u.id = a.user_id
                    where a.id =_id;
            END;
    $$;
 8   DROP FUNCTION public.sp_get_administrator(_id integer);
       public          postgres    false                       1255    16828 9   sp_get_administrator_by_username_and_password(text, text)    FUNCTION     ~  CREATE FUNCTION public.sp_get_administrator_by_username_and_password(_username text, _password text) RETURNS TABLE(admin_id integer, first_name text, last_name text, level integer, user_id bigint, username text, password text, email text, user_role_id integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select a.id,a.first_name,a.last_name,a.level, u.id, u.username, u.password, u.email, u.user_role  from administrators a
                    join users u on u.id = a.user_id
                    where u.username =_username and u.password=_password;
            END;
    $$;
 d   DROP FUNCTION public.sp_get_administrator_by_username_and_password(_username text, _password text);
       public          postgres    false            �            1255    16793    sp_get_airline_company(bigint)    FUNCTION       CREATE FUNCTION public.sp_get_airline_company(_id bigint) RETURNS TABLE(airline_company_id bigint, name text, country_id integer, user_id bigint, username text, password text, email text, user_role_id integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select a.id, a.name, a.country_id, u.id, u.username, u.password, u.email, u.user_role  from airline_companies a
                    join users u on u.id = a.user_id
                    where a.id =_id;
            END;
    $$;
 9   DROP FUNCTION public.sp_get_airline_company(_id bigint);
       public          postgres    false                       1255    16812 (   sp_get_airline_company_by_username(text)    FUNCTION     :  CREATE FUNCTION public.sp_get_airline_company_by_username(_username text) RETURNS TABLE(airline_company_id bigint, name text, country_id integer, user_id bigint, username text, password text, email text, user_role_id integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select a.id, a.name, a.country_id, u.id, u.username, u.password, u.email, u.user_role  from airline_companies a
                    join users u on u.id = a.user_id
                    where u.username =_username;
            END;
    $$;
 I   DROP FUNCTION public.sp_get_airline_company_by_username(_username text);
       public          postgres    false                       1255    16827 ;   sp_get_airline_company_by_username_and_password(text, text)    FUNCTION     p  CREATE FUNCTION public.sp_get_airline_company_by_username_and_password(_username text, _password text) RETURNS TABLE(airline_company_id bigint, name text, country_id integer, user_id bigint, username text, password text, email text, user_role_id integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select a.id, a.name, a.country_id, u.id, u.username, u.password, u.email, u.user_role  from airline_companies a
                    join users u on u.id = a.user_id
                    where u.username =_username and u.password=_password;
            END;
    $$;
 f   DROP FUNCTION public.sp_get_airline_company_by_username_and_password(_username text, _password text);
       public          postgres    false            �            1255    16787    sp_get_all_administrators()    FUNCTION       CREATE FUNCTION public.sp_get_all_administrators() RETURNS TABLE(admin_id integer, first_name text, last_name text, level integer, user_id bigint, username text, password text, email text, user_role_id integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select a.id,a.first_name,a.last_name,a.level, u.id, u.username, u.password, u.email, u.user_role  from administrators a
                    join users u on u.id = a.user_id;
                END;
    $$;
 2   DROP FUNCTION public.sp_get_all_administrators();
       public          postgres    false            �            1255    16794    sp_get_all_airline_companies()    FUNCTION     �  CREATE FUNCTION public.sp_get_all_airline_companies() RETURNS TABLE(airline_company_id bigint, name text, country_id integer, user_id bigint, username text, password text, email text, user_role_id integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select a.id, a.name, a.country_id, u.id, u.username, u.password, u.email, u.user_role  from airline_companies a
                    join users u on u.id = a.user_id;
                END;
    $$;
 5   DROP FUNCTION public.sp_get_all_airline_companies();
       public          postgres    false                       1255    16813 0   sp_get_all_airline_companies_by_country(integer)    FUNCTION     K  CREATE FUNCTION public.sp_get_all_airline_companies_by_country(_country_id integer) RETURNS TABLE(airline_company_id bigint, name text, country_id integer, user_id bigint, username text, password text, email text, user_role_id integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select a.id, a.name, a.country_id, u.id, u.username, u.password, u.email, u.user_role  from airline_companies a
                    join users u on u.id = a.user_id
                    where a.country_id=_country_id;
                END;
    $$;
 S   DROP FUNCTION public.sp_get_all_airline_companies_by_country(_country_id integer);
       public          postgres    false            �            1255    16762    sp_get_all_countries()    FUNCTION     �   CREATE FUNCTION public.sp_get_all_countries() RETURNS TABLE(id integer, name text)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select * from countries;
            END;
    $$;
 -   DROP FUNCTION public.sp_get_all_countries();
       public          postgres    false            �            1255    16798    sp_get_all_customers()    FUNCTION     T  CREATE FUNCTION public.sp_get_all_customers() RETURNS TABLE(customer_id bigint, first_name text, last_name text, address text, phone_number text, credit_card_number text, user_id bigint, username text, password text, email text, user_role_id integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select a.id, a.first_name, a.last_name, a.address, a.phone_number, a.credit_card_number, u.id, u.username, u.password, u.email, u.user_role  from customers a
                    join users u on u.id = a.user_id;
                END;
    $$;
 -   DROP FUNCTION public.sp_get_all_customers();
       public          postgres    false            �            1255    16802    sp_get_all_flights()    FUNCTION     �  CREATE FUNCTION public.sp_get_all_flights() RETURNS TABLE(flight_id bigint, airline_company_id bigint, airline_company_name text, airline_company_country_id integer, origin_country_id integer, destination_country_id integer, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select f.id, a.id,a.name,a.country_id, f.origin_country_id, f.destination_country_id, f.departure_time, f.landing_time, f.remaining_tickets  from flights f
                    join airline_companies a on a.id = f.airline_company_id;
                END;
    $$;
 +   DROP FUNCTION public.sp_get_all_flights();
       public          postgres    false                       1255    16810    sp_get_all_tickets()    FUNCTION     0  CREATE FUNCTION public.sp_get_all_tickets() RETURNS TABLE(ticket_id bigint, flight_id bigint, airline_company_id bigint, airline_company_name text, airline_company_country_id integer, origin_country_id integer, destination_country_id integer, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer, customer_id bigint, first_name text, last_name text, address text, phone_number text, credit_card_number text)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                      select t.id, f.id, a.id, a.name, a.country_id, f.origin_country_id, f.destination_country_id, f.departure_time, f.landing_time, f.remaining_tickets,
                           c.id, c.first_name, c.last_name, c.address, c.phone_number, c.credit_card_number from tickets t
                    join flights f on f.id = t.flight_id
                    join airline_companies a on a.id = f.airline_company_id
                    join customers c on c.id = t.customer_id;
                END;
    $$;
 +   DROP FUNCTION public.sp_get_all_tickets();
       public          postgres    false                       1255    91666 &   sp_get_all_tickets_by_customer(bigint)    FUNCTION     \  CREATE FUNCTION public.sp_get_all_tickets_by_customer(_customer_id bigint) RETURNS TABLE(ticket_id bigint, flight_id bigint, airline_company_id bigint, airline_company_name text, airline_company_country_id integer, origin_country_id integer, destination_country_id integer, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer, customer_id bigint, first_name text, last_name text, address text, phone_number text, credit_card_number text)
    LANGUAGE plpgsql
    AS $$
BEGIN
                RETURN QUERY
                      select t.id, f.id, a.id, a.name, a.country_id, f.origin_country_id, f.destination_country_id, f.departure_time, f.landing_time, f.remaining_tickets,
                           c.id, c.first_name, c.last_name, c.address, c.phone_number, c.credit_card_number from tickets t
                    join flights f on f.id = t.flight_id
                    join airline_companies a on a.id = f.airline_company_id
                    join customers c on c.id = t.customer_id
					where t.customer_id=_customer_id;
                END;
$$;
 J   DROP FUNCTION public.sp_get_all_tickets_by_customer(_customer_id bigint);
       public          postgres    false            �            1255    16772    sp_get_all_users()    FUNCTION       CREATE FUNCTION public.sp_get_all_users() RETURNS TABLE(id bigint, username text, password text, email text, user_role_id integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select * from users;
            END;
    $$;
 )   DROP FUNCTION public.sp_get_all_users();
       public          postgres    false            �            1255    16757    sp_get_country(integer)    FUNCTION       CREATE FUNCTION public.sp_get_country(_id integer) RETURNS TABLE(id integer, name text)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select * from countries
                    where countries.id =_id;
            END;
    $$;
 2   DROP FUNCTION public.sp_get_country(_id integer);
       public          postgres    false            �            1255    16797    sp_get_customer(bigint)    FUNCTION     z  CREATE FUNCTION public.sp_get_customer(_id bigint) RETURNS TABLE(customer_id bigint, first_name text, last_name text, address text, phone_number text, credit_card_number text, user_id bigint, username text, password text, email text, user_role_id integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select a.id, a.first_name, a.last_name, a.address, a.phone_number, a.credit_card_number, u.id, u.username, u.password, u.email, u.user_role  from customers a
                    join users u on u.id = a.user_id
                    where a.id =_id;
            END;
    $$;
 2   DROP FUNCTION public.sp_get_customer(_id bigint);
       public          postgres    false                       1255    16815 !   sp_get_customer_by_username(text)    FUNCTION     �  CREATE FUNCTION public.sp_get_customer_by_username(_username text) RETURNS TABLE(customer_id bigint, first_name text, last_name text, address text, phone_number text, credit_card_number text, user_id bigint, username text, password text, email text, user_role_id integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select a.id, a.first_name, a.last_name, a.address, a.phone_number, a.credit_card_number, u.id, u.username, u.password, u.email, u.user_role  from customers a
                    join users u on u.id = a.user_id
                    where u.username =_username;
            END;
    $$;
 B   DROP FUNCTION public.sp_get_customer_by_username(_username text);
       public          postgres    false                       1255    16826 4   sp_get_customer_by_username_and_password(text, text)    FUNCTION     �  CREATE FUNCTION public.sp_get_customer_by_username_and_password(_username text, _password text) RETURNS TABLE(customer_id bigint, first_name text, last_name text, address text, phone_number text, credit_card_number text, user_id bigint, username text, password text, email text, user_role_id integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select a.id, a.first_name, a.last_name, a.address, a.phone_number, a.credit_card_number, u.id, u.username, u.password, u.email, u.user_role  from customers a
                    join users u on u.id = a.user_id
                    where u.username =_username and u.password=_password;
            END;
    $$;
 _   DROP FUNCTION public.sp_get_customer_by_username_and_password(_username text, _password text);
       public          postgres    false            �            1255    16801    sp_get_flight(bigint)    FUNCTION     �  CREATE FUNCTION public.sp_get_flight(_id bigint) RETURNS TABLE(flight_id bigint, airline_company_id bigint, airline_company_name text, airline_company_country_id integer, origin_country_id integer, destination_country_id integer, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select f.id, a.id,a.name,a.country_id, f.origin_country_id, f.destination_country_id, f.departure_time, f.landing_time, f.remaining_tickets  from flights f
                    join airline_companies a on a.id = f.airline_company_id
                    where f.id =_id;
            END;
    $$;
 0   DROP FUNCTION public.sp_get_flight(_id bigint);
       public          postgres    false                       1255    24784 ,   sp_get_flight_history_by_original_id(bigint)    FUNCTION     (  CREATE FUNCTION public.sp_get_flight_history_by_original_id(_id bigint) RETURNS TABLE(id bigint, flight_original_id bigint, airline_company_id bigint, airline_company_name text, airline_company_country_id integer, origin_country_id integer, destination_country_id integer, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
BEGIN
                RETURN QUERY
                    select f.id, f.flight_original_id, a.id,a.name,a.country_id, f.origin_country_id, f.destination_country_id, f.departure_time, f.landing_time, f.remaining_tickets  from flights_history f
                    join airline_companies a on a.id = f.airline_company_id
                    where f.flight_original_id =_id;
            END;
$$;
 G   DROP FUNCTION public.sp_get_flight_history_by_original_id(_id bigint);
       public          postgres    false                       1255    16829 )   sp_get_flights_by_airline_company(bigint)    FUNCTION       CREATE FUNCTION public.sp_get_flights_by_airline_company(_airline_company_id bigint) RETURNS TABLE(flight_id bigint, airline_company_id bigint, airline_company_name text, airline_company_country_id integer, origin_country_id integer, destination_country_id integer, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select f.id, a.id,a.name,a.country_id, f.origin_country_id, f.destination_country_id, f.departure_time, f.landing_time, f.remaining_tickets  from flights f
                    join airline_companies a on a.id = f.airline_company_id
                    where a.id =_airline_company_id;
            END;
    $$;
 T   DROP FUNCTION public.sp_get_flights_by_airline_company(_airline_company_id bigint);
       public          postgres    false            	           1255    16819 "   sp_get_flights_by_customer(bigint)    FUNCTION     F  CREATE FUNCTION public.sp_get_flights_by_customer(_customer_id bigint) RETURNS TABLE(flight_id bigint, airline_company_id bigint, airline_company_name text, airline_company_country_id integer, origin_country_id integer, destination_country_id integer, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select f.id, a.id,a.name,a.country_id, f.origin_country_id, f.destination_country_id, f.departure_time, f.landing_time, f.remaining_tickets  from flights f
                    join airline_companies a on a.id = f.airline_company_id
                    join tickets t on t.flight_id=f.id
                    where t.customer_id =_customer_id;
            END;
    $$;
 F   DROP FUNCTION public.sp_get_flights_by_customer(_customer_id bigint);
       public          postgres    false            
           1255    16824 &   sp_get_flights_by_departure_date(date)    FUNCTION     "  CREATE FUNCTION public.sp_get_flights_by_departure_date(_departure_date date) RETURNS TABLE(flight_id bigint, airline_company_id bigint, airline_company_name text, airline_company_country_id integer, origin_country_id integer, destination_country_id integer, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select f.id, a.id,a.name,a.country_id, f.origin_country_id, f.destination_country_id, f.departure_time, f.landing_time, f.remaining_tickets  from flights f
                    join airline_companies a on a.id = f.airline_company_id
                    where f.departure_time:: date =_departure_date;
            END;
    $$;
 M   DROP FUNCTION public.sp_get_flights_by_departure_date(_departure_date date);
       public          postgres    false                       1255    16817 .   sp_get_flights_by_destination_country(integer)    FUNCTION     ;  CREATE FUNCTION public.sp_get_flights_by_destination_country(_destination_country_id integer) RETURNS TABLE(flight_id bigint, airline_company_id bigint, airline_company_name text, airline_company_country_id integer, origin_country_id integer, destination_country_id integer, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select f.id, a.id,a.name,a.country_id, f.origin_country_id, f.destination_country_id, f.departure_time, f.landing_time, f.remaining_tickets  from flights f
                    join airline_companies a on a.id = f.airline_company_id
                    where f.destination_country_id =_destination_country_id;
            END;
    $$;
 ]   DROP FUNCTION public.sp_get_flights_by_destination_country(_destination_country_id integer);
       public          postgres    false                       1255    16825 $   sp_get_flights_by_landing_date(date)    FUNCTION       CREATE FUNCTION public.sp_get_flights_by_landing_date(_landing_date date) RETURNS TABLE(flight_id bigint, airline_company_id bigint, airline_company_name text, airline_company_country_id integer, origin_country_id integer, destination_country_id integer, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select f.id, a.id,a.name,a.country_id, f.origin_country_id, f.destination_country_id, f.departure_time, f.landing_time, f.remaining_tickets  from flights f
                    join airline_companies a on a.id = f.airline_company_id
                    where f.landing_time:: date =_landing_date;
            END;
    $$;
 I   DROP FUNCTION public.sp_get_flights_by_landing_date(_landing_date date);
       public          postgres    false                       1255    16818 )   sp_get_flights_by_origin_country(integer)    FUNCTION     '  CREATE FUNCTION public.sp_get_flights_by_origin_country(_origin_country_id integer) RETURNS TABLE(flight_id bigint, airline_company_id bigint, airline_company_name text, airline_company_country_id integer, origin_country_id integer, destination_country_id integer, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select f.id, a.id,a.name,a.country_id, f.origin_country_id, f.destination_country_id, f.departure_time, f.landing_time, f.remaining_tickets  from flights f
                    join airline_companies a on a.id = f.airline_company_id
                    where f.origin_country_id =_origin_country_id;
            END;
    $$;
 S   DROP FUNCTION public.sp_get_flights_by_origin_country(_origin_country_id integer);
       public          postgres    false                       1255    24791 /   sp_get_flights_with_tickets_that_landed(bigint)    FUNCTION       CREATE FUNCTION public.sp_get_flights_with_tickets_that_landed(_seconds bigint) RETURNS TABLE(flight_id bigint, airline_company_id bigint, origin_country_id integer, destination_country_id integer, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer, ticket_id bigint, customer_id bigint)
    LANGUAGE plpgsql
    AS $$
BEGIN
return query
	(SELECT f.id,
	 		f.airline_company_id,
	 		f.origin_country_id,
	 		f.destination_country_id,
	 		f.departure_time,
	 		f.landing_time,
	 		f.remaining_tickets,
	 		t.id,
	 		t.customer_id
	 FROM flights f
	left JOIN tickets t on f.id = t.flight_id
	WHERE (SELECT EXTRACT(EPOCH FROM current_timestamp::timestamp without time zone) - EXTRACT(EPOCH FROM f.landing_time))>_seconds);
END;
$$;
 O   DROP FUNCTION public.sp_get_flights_with_tickets_that_landed(_seconds bigint);
       public          postgres    false                       1255    16809    sp_get_ticket(bigint)    FUNCTION     T  CREATE FUNCTION public.sp_get_ticket(_id bigint) RETURNS TABLE(ticket_id bigint, flight_id bigint, airline_company_id bigint, airline_company_name text, airline_company_country_id integer, origin_country_id integer, destination_country_id integer, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer, customer_id bigint, first_name text, last_name text, address text, phone_number text, credit_card_number text)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select t.id, f.id, a.id, a.name, a.country_id, f.origin_country_id, f.destination_country_id, f.departure_time, f.landing_time, f.remaining_tickets,
                           c.id, c.first_name, c.last_name, c.address, c.phone_number, c.credit_card_number from tickets t
                    join flights f on f.id = t.flight_id
                    join airline_companies a on a.id = f.airline_company_id
                    join customers c on c.id = t.customer_id
                    where t.id =_id;
            END;
    $$;
 0   DROP FUNCTION public.sp_get_ticket(_id bigint);
       public          postgres    false                       1255    16830 )   sp_get_tickets_by_airline_company(bigint)    FUNCTION     �  CREATE FUNCTION public.sp_get_tickets_by_airline_company(_airline_company_id bigint) RETURNS TABLE(ticket_id bigint, flight_id bigint, airline_company_id bigint, airline_company_name text, airline_company_country_id integer, origin_country_id integer, destination_country_id integer, departure_time timestamp without time zone, landing_time timestamp without time zone, remaining_tickets integer, customer_id bigint, first_name text, last_name text, address text, phone_number text, credit_card_number text)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select t.id, f.id, a.id, a.name, a.country_id, f.origin_country_id, f.destination_country_id, f.departure_time, f.landing_time, f.remaining_tickets,
                           c.id, c.first_name, c.last_name, c.address, c.phone_number, c.credit_card_number from tickets t
                    join flights f on f.id = t.flight_id
                    join airline_companies a on a.id = f.airline_company_id
                    join customers c on c.id = t.customer_id
                    where a.id =_airline_company_id;
            END;
    $$;
 T   DROP FUNCTION public.sp_get_tickets_by_airline_company(_airline_company_id bigint);
       public          postgres    false            �            1255    16773    sp_get_user(integer)    FUNCTION     G  CREATE FUNCTION public.sp_get_user(_id integer) RETURNS TABLE(id bigint, username text, password text, email text, user_role_id integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                RETURN QUERY
                    select * from users
                    where users.id =_id;
            END;
    $$;
 /   DROP FUNCTION public.sp_get_user(_id integer);
       public          postgres    false            �            1255    16778     sp_remove_administrator(integer) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_remove_administrator(_id integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                DELETE FROM administrators WHERE id=_id;
            END;
    $$;
 <   DROP PROCEDURE public.sp_remove_administrator(_id integer);
       public          postgres    false            �            1255    16779 !   sp_remove_airline_company(bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_remove_airline_company(_id bigint)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                DELETE FROM airline_companies WHERE id=_id;
            END;
    $$;
 =   DROP PROCEDURE public.sp_remove_airline_company(_id bigint);
       public          postgres    false            �            1255    16760    sp_remove_country(integer) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_remove_country(_id integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                DELETE FROM countries WHERE id=_id;
            END;
    $$;
 6   DROP PROCEDURE public.sp_remove_country(_id integer);
       public          postgres    false            �            1255    16780    sp_remove_customer(bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_remove_customer(_id bigint)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                DELETE FROM customers WHERE id=_id;
            END;
    $$;
 6   DROP PROCEDURE public.sp_remove_customer(_id bigint);
       public          postgres    false            �            1255    16781    sp_remove_flight(bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_remove_flight(_id bigint)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                DELETE FROM flights WHERE id=_id;
            END;
    $$;
 4   DROP PROCEDURE public.sp_remove_flight(_id bigint);
       public          postgres    false            �            1255    16782    sp_remove_ticket(bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_remove_ticket(_id bigint)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                DELETE FROM tickets WHERE id=_id;
            END;
    $$;
 4   DROP PROCEDURE public.sp_remove_ticket(_id bigint);
       public          postgres    false            �            1255    16776    sp_remove_user(bigint) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_remove_user(_id bigint)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                DELETE FROM users WHERE id=_id;
            END;
    $$;
 2   DROP PROCEDURE public.sp_remove_user(_id bigint);
       public          postgres    false            �            1255    16788 =   sp_update_administrator(integer, text, text, integer, bigint) 	   PROCEDURE     v  CREATE PROCEDURE public.sp_update_administrator(_id integer, _first_name text, _last_name text, _level integer, _user_id bigint)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                UPDATE administrators
                SET first_name=_first_name, last_name=_last_name, level=_level, user_id=_user_id
                WHERE id=_id;
            END;
    $$;
 �   DROP PROCEDURE public.sp_update_administrator(_id integer, _first_name text, _last_name text, _level integer, _user_id bigint);
       public          postgres    false            �            1255    16795 8   sp_update_airline_company(bigint, text, integer, bigint) 	   PROCEDURE     P  CREATE PROCEDURE public.sp_update_airline_company(_id bigint, _name text, _country_id integer, _user_id bigint)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                UPDATE airline_companies
                SET name=_name, country_id=_country_id, user_id=_user_id
                WHERE id=_id;
            END;
    $$;
 o   DROP PROCEDURE public.sp_update_airline_company(_id bigint, _name text, _country_id integer, _user_id bigint);
       public          postgres    false            �            1255    16761     sp_update_country(integer, text) 	   PROCEDURE     �   CREATE PROCEDURE public.sp_update_country(_id integer, _name text)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                UPDATE countries
                SET name=_name
                WHERE id=_id;
            END;
    $$;
 B   DROP PROCEDURE public.sp_update_country(_id integer, _name text);
       public          postgres    false            �            1255    16799 @   sp_update_customer(bigint, text, text, text, text, text, bigint) 	   PROCEDURE     �  CREATE PROCEDURE public.sp_update_customer(_id bigint, _first_name text, _last_name text, _address text, _phone_number text, _credit_card_number text, _user_id bigint)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                UPDATE customers
                SET first_name=_first_name, last_name=_last_name, address=_address, phone_number=_phone_number,credit_card_number=_credit_card_number, user_id=_user_id
                WHERE id=_id;
            END;
    $$;
 �   DROP PROCEDURE public.sp_update_customer(_id bigint, _first_name text, _last_name text, _address text, _phone_number text, _credit_card_number text, _user_id bigint);
       public          postgres    false                        1255    16803 u   sp_update_flight(bigint, bigint, integer, integer, timestamp without time zone, timestamp without time zone, integer) 	   PROCEDURE     �  CREATE PROCEDURE public.sp_update_flight(_id bigint, _airline_company_id bigint, _origin_country_id integer, _destination_country_id integer, _departure_time timestamp without time zone, _landing_time timestamp without time zone, _remaining_tickets integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                UPDATE flights
                SET airline_company_id=_airline_company_id, origin_country_id=_origin_country_id, destination_country_id=_destination_country_id, departure_time=_departure_time,landing_time=_landing_time, remaining_tickets=_remaining_tickets
                WHERE id=_id;
            END;
    $$;
   DROP PROCEDURE public.sp_update_flight(_id bigint, _airline_company_id bigint, _origin_country_id integer, _destination_country_id integer, _departure_time timestamp without time zone, _landing_time timestamp without time zone, _remaining_tickets integer);
       public          postgres    false                       1255    16807 (   sp_update_ticket(bigint, bigint, bigint) 	   PROCEDURE     -  CREATE PROCEDURE public.sp_update_ticket(_id bigint, _flight_id bigint, _customer_id bigint)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                UPDATE tickets
                SET flight_id=_flight_id, customer_id=_customer_id
                WHERE id=_id;
            END;
    $$;
 \   DROP PROCEDURE public.sp_update_ticket(_id bigint, _flight_id bigint, _customer_id bigint);
       public          postgres    false            �            1255    16777 1   sp_update_user(bigint, text, text, text, integer) 	   PROCEDURE     d  CREATE PROCEDURE public.sp_update_user(_id bigint, _username text, _password text, _email text, _user_role_id integer)
    LANGUAGE plpgsql
    AS $$
            BEGIN
                UPDATE users
                SET username=_username, password=_password, email=_email, user_role=_user_role_id
                WHERE id=_id;
            END;
    $$;
 v   DROP PROCEDURE public.sp_update_user(_id bigint, _username text, _password text, _email text, _user_role_id integer);
       public          postgres    false            �            1259    16654    administrators    TABLE     �   CREATE TABLE public.administrators (
    id integer NOT NULL,
    first_name text NOT NULL,
    last_name text NOT NULL,
    level integer DEFAULT 0 NOT NULL,
    user_id bigint NOT NULL
);
 "   DROP TABLE public.administrators;
       public         heap    postgres    false            �            1259    16652    administrators_id_seq    SEQUENCE     �   CREATE SEQUENCE public.administrators_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 ,   DROP SEQUENCE public.administrators_id_seq;
       public          postgres    false    207            f           0    0    administrators_id_seq    SEQUENCE OWNED BY     O   ALTER SEQUENCE public.administrators_id_seq OWNED BY public.administrators.id;
          public          postgres    false    206            �            1259    16691    airline_companies    TABLE     �   CREATE TABLE public.airline_companies (
    id bigint NOT NULL,
    name text NOT NULL,
    country_id integer NOT NULL,
    user_id bigint NOT NULL
);
 %   DROP TABLE public.airline_companies;
       public         heap    postgres    false            �            1259    16689    airline_companies_id_seq    SEQUENCE     �   CREATE SEQUENCE public.airline_companies_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 /   DROP SEQUENCE public.airline_companies_id_seq;
       public          postgres    false    211            g           0    0    airline_companies_id_seq    SEQUENCE OWNED BY     U   ALTER SEQUENCE public.airline_companies_id_seq OWNED BY public.airline_companies.id;
          public          postgres    false    210            �            1259    16672 	   customers    TABLE     �   CREATE TABLE public.customers (
    id bigint NOT NULL,
    first_name text NOT NULL,
    last_name text NOT NULL,
    address text,
    phone_number text NOT NULL,
    credit_card_number text,
    user_id bigint NOT NULL
);
    DROP TABLE public.customers;
       public         heap    postgres    false            �            1259    16670    costumers_id_seq    SEQUENCE     y   CREATE SEQUENCE public.costumers_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 '   DROP SEQUENCE public.costumers_id_seq;
       public          postgres    false    209            h           0    0    costumers_id_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public.costumers_id_seq OWNED BY public.customers.id;
          public          postgres    false    208            �            1259    16612 	   countries    TABLE     S   CREATE TABLE public.countries (
    id integer NOT NULL,
    name text NOT NULL
);
    DROP TABLE public.countries;
       public         heap    postgres    false            �            1259    16610    countries_id_seq    SEQUENCE     �   CREATE SEQUENCE public.countries_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 '   DROP SEQUENCE public.countries_id_seq;
       public          postgres    false    201            i           0    0    countries_id_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public.countries_id_seq OWNED BY public.countries.id;
          public          postgres    false    200            �            1259    16714    flights    TABLE     W  CREATE TABLE public.flights (
    id bigint NOT NULL,
    airline_company_id bigint NOT NULL,
    origin_country_id integer NOT NULL,
    destination_country_id integer NOT NULL,
    departure_time timestamp without time zone NOT NULL,
    landing_time timestamp without time zone NOT NULL,
    remaining_tickets integer DEFAULT 0 NOT NULL
);
    DROP TABLE public.flights;
       public         heap    postgres    false            �            1259    16843    flights_history    TABLE     t  CREATE TABLE public.flights_history (
    id bigint NOT NULL,
    flight_original_id bigint NOT NULL,
    airline_company_id bigint NOT NULL,
    origin_country_id integer NOT NULL,
    destination_country_id integer,
    departure_time timestamp without time zone NOT NULL,
    landing_time timestamp without time zone NOT NULL,
    remaining_tickets integer NOT NULL
);
 #   DROP TABLE public.flights_history;
       public         heap    postgres    false            �            1259    16841    flights_history_id_seq    SEQUENCE        CREATE SEQUENCE public.flights_history_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 -   DROP SEQUENCE public.flights_history_id_seq;
       public          postgres    false    219            j           0    0    flights_history_id_seq    SEQUENCE OWNED BY     Q   ALTER SEQUENCE public.flights_history_id_seq OWNED BY public.flights_history.id;
          public          postgres    false    218            �            1259    16712    flights_id_seq    SEQUENCE     w   CREATE SEQUENCE public.flights_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public.flights_id_seq;
       public          postgres    false    213            k           0    0    flights_id_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE public.flights_id_seq OWNED BY public.flights.id;
          public          postgres    false    212            �            1259    16738    tickets    TABLE     x   CREATE TABLE public.tickets (
    id bigint NOT NULL,
    flight_id bigint NOT NULL,
    customer_id bigint NOT NULL
);
    DROP TABLE public.tickets;
       public         heap    postgres    false            �            1259    16833    tickets_history    TABLE     �   CREATE TABLE public.tickets_history (
    id bigint NOT NULL,
    original_ticket_id bigint NOT NULL,
    flight_id bigint NOT NULL,
    customer_id bigint NOT NULL
);
 #   DROP TABLE public.tickets_history;
       public         heap    postgres    false            �            1259    16831    tickets_history_id_seq    SEQUENCE        CREATE SEQUENCE public.tickets_history_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 -   DROP SEQUENCE public.tickets_history_id_seq;
       public          postgres    false    217            l           0    0    tickets_history_id_seq    SEQUENCE OWNED BY     Q   ALTER SEQUENCE public.tickets_history_id_seq OWNED BY public.tickets_history.id;
          public          postgres    false    216            �            1259    16736    tickets_id_seq    SEQUENCE     w   CREATE SEQUENCE public.tickets_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public.tickets_id_seq;
       public          postgres    false    215            m           0    0    tickets_id_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE public.tickets_id_seq OWNED BY public.tickets.id;
          public          postgres    false    214            �            1259    16624 
   user_roles    TABLE     Y   CREATE TABLE public.user_roles (
    id integer NOT NULL,
    role_name text NOT NULL
);
    DROP TABLE public.user_roles;
       public         heap    postgres    false            �            1259    16622    user_roles_id_seq    SEQUENCE     �   CREATE SEQUENCE public.user_roles_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE public.user_roles_id_seq;
       public          postgres    false    203            n           0    0    user_roles_id_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE public.user_roles_id_seq OWNED BY public.user_roles.id;
          public          postgres    false    202            �            1259    16636    users    TABLE     �   CREATE TABLE public.users (
    id bigint NOT NULL,
    username text NOT NULL,
    password text NOT NULL,
    email text NOT NULL,
    user_role integer NOT NULL
);
    DROP TABLE public.users;
       public         heap    postgres    false            �            1259    16634    users_id_seq    SEQUENCE     u   CREATE SEQUENCE public.users_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 #   DROP SEQUENCE public.users_id_seq;
       public          postgres    false    205            o           0    0    users_id_seq    SEQUENCE OWNED BY     =   ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;
          public          postgres    false    204            �           2604    16657    administrators id    DEFAULT     v   ALTER TABLE ONLY public.administrators ALTER COLUMN id SET DEFAULT nextval('public.administrators_id_seq'::regclass);
 @   ALTER TABLE public.administrators ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    207    206    207            �           2604    16694    airline_companies id    DEFAULT     |   ALTER TABLE ONLY public.airline_companies ALTER COLUMN id SET DEFAULT nextval('public.airline_companies_id_seq'::regclass);
 C   ALTER TABLE public.airline_companies ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    210    211    211            �           2604    16615    countries id    DEFAULT     l   ALTER TABLE ONLY public.countries ALTER COLUMN id SET DEFAULT nextval('public.countries_id_seq'::regclass);
 ;   ALTER TABLE public.countries ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    201    200    201            �           2604    16675    customers id    DEFAULT     l   ALTER TABLE ONLY public.customers ALTER COLUMN id SET DEFAULT nextval('public.costumers_id_seq'::regclass);
 ;   ALTER TABLE public.customers ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    209    208    209            �           2604    16717 
   flights id    DEFAULT     h   ALTER TABLE ONLY public.flights ALTER COLUMN id SET DEFAULT nextval('public.flights_id_seq'::regclass);
 9   ALTER TABLE public.flights ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    212    213    213            �           2604    16846    flights_history id    DEFAULT     x   ALTER TABLE ONLY public.flights_history ALTER COLUMN id SET DEFAULT nextval('public.flights_history_id_seq'::regclass);
 A   ALTER TABLE public.flights_history ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    218    219    219            �           2604    16741 
   tickets id    DEFAULT     h   ALTER TABLE ONLY public.tickets ALTER COLUMN id SET DEFAULT nextval('public.tickets_id_seq'::regclass);
 9   ALTER TABLE public.tickets ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    215    214    215            �           2604    16836    tickets_history id    DEFAULT     x   ALTER TABLE ONLY public.tickets_history ALTER COLUMN id SET DEFAULT nextval('public.tickets_history_id_seq'::regclass);
 A   ALTER TABLE public.tickets_history ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    216    217    217            �           2604    16627    user_roles id    DEFAULT     n   ALTER TABLE ONLY public.user_roles ALTER COLUMN id SET DEFAULT nextval('public.user_roles_id_seq'::regclass);
 <   ALTER TABLE public.user_roles ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    202    203    203            �           2604    16639    users id    DEFAULT     d   ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);
 7   ALTER TABLE public.users ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    205    204    205            S          0    16654    administrators 
   TABLE DATA           S   COPY public.administrators (id, first_name, last_name, level, user_id) FROM stdin;
    public          postgres    false    207   �      W          0    16691    airline_companies 
   TABLE DATA           J   COPY public.airline_companies (id, name, country_id, user_id) FROM stdin;
    public          postgres    false    211   
      M          0    16612 	   countries 
   TABLE DATA           -   COPY public.countries (id, name) FROM stdin;
    public          postgres    false    201   '      U          0    16672 	   customers 
   TABLE DATA           r   COPY public.customers (id, first_name, last_name, address, phone_number, credit_card_number, user_id) FROM stdin;
    public          postgres    false    209   D      Y          0    16714    flights 
   TABLE DATA           �   COPY public.flights (id, airline_company_id, origin_country_id, destination_country_id, departure_time, landing_time, remaining_tickets) FROM stdin;
    public          postgres    false    213   a      _          0    16843    flights_history 
   TABLE DATA           �   COPY public.flights_history (id, flight_original_id, airline_company_id, origin_country_id, destination_country_id, departure_time, landing_time, remaining_tickets) FROM stdin;
    public          postgres    false    219   ~      [          0    16738    tickets 
   TABLE DATA           =   COPY public.tickets (id, flight_id, customer_id) FROM stdin;
    public          postgres    false    215   �      ]          0    16833    tickets_history 
   TABLE DATA           Y   COPY public.tickets_history (id, original_ticket_id, flight_id, customer_id) FROM stdin;
    public          postgres    false    217   �      O          0    16624 
   user_roles 
   TABLE DATA           3   COPY public.user_roles (id, role_name) FROM stdin;
    public          postgres    false    203   �      Q          0    16636    users 
   TABLE DATA           I   COPY public.users (id, username, password, email, user_role) FROM stdin;
    public          postgres    false    205         p           0    0    administrators_id_seq    SEQUENCE SET     D   SELECT pg_catalog.setval('public.administrators_id_seq', 1, false);
          public          postgres    false    206            q           0    0    airline_companies_id_seq    SEQUENCE SET     G   SELECT pg_catalog.setval('public.airline_companies_id_seq', 1, false);
          public          postgres    false    210            r           0    0    costumers_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.costumers_id_seq', 1, false);
          public          postgres    false    208            s           0    0    countries_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.countries_id_seq', 1, false);
          public          postgres    false    200            t           0    0    flights_history_id_seq    SEQUENCE SET     E   SELECT pg_catalog.setval('public.flights_history_id_seq', 19, true);
          public          postgres    false    218            u           0    0    flights_id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public.flights_id_seq', 1, false);
          public          postgres    false    212            v           0    0    tickets_history_id_seq    SEQUENCE SET     D   SELECT pg_catalog.setval('public.tickets_history_id_seq', 6, true);
          public          postgres    false    216            w           0    0    tickets_id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public.tickets_id_seq', 1, false);
          public          postgres    false    214            x           0    0    user_roles_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.user_roles_id_seq', 3, true);
          public          postgres    false    202            y           0    0    users_id_seq    SEQUENCE SET     ;   SELECT pg_catalog.setval('public.users_id_seq', 1, false);
          public          postgres    false    204            �           2606    16663     administrators administrators_pk 
   CONSTRAINT     ^   ALTER TABLE ONLY public.administrators
    ADD CONSTRAINT administrators_pk PRIMARY KEY (id);
 J   ALTER TABLE ONLY public.administrators DROP CONSTRAINT administrators_pk;
       public            postgres    false    207            �           2606    16699 &   airline_companies airline_companies_pk 
   CONSTRAINT     d   ALTER TABLE ONLY public.airline_companies
    ADD CONSTRAINT airline_companies_pk PRIMARY KEY (id);
 P   ALTER TABLE ONLY public.airline_companies DROP CONSTRAINT airline_companies_pk;
       public            postgres    false    211            �           2606    16680    customers costumers_pk 
   CONSTRAINT     T   ALTER TABLE ONLY public.customers
    ADD CONSTRAINT costumers_pk PRIMARY KEY (id);
 @   ALTER TABLE ONLY public.customers DROP CONSTRAINT costumers_pk;
       public            postgres    false    209            �           2606    16620    countries countries_pk 
   CONSTRAINT     T   ALTER TABLE ONLY public.countries
    ADD CONSTRAINT countries_pk PRIMARY KEY (id);
 @   ALTER TABLE ONLY public.countries DROP CONSTRAINT countries_pk;
       public            postgres    false    201            �           2606    16720    flights flights_pk 
   CONSTRAINT     P   ALTER TABLE ONLY public.flights
    ADD CONSTRAINT flights_pk PRIMARY KEY (id);
 <   ALTER TABLE ONLY public.flights DROP CONSTRAINT flights_pk;
       public            postgres    false    213            �           2606    16838 "   tickets_history tickets_history_pk 
   CONSTRAINT     `   ALTER TABLE ONLY public.tickets_history
    ADD CONSTRAINT tickets_history_pk PRIMARY KEY (id);
 L   ALTER TABLE ONLY public.tickets_history DROP CONSTRAINT tickets_history_pk;
       public            postgres    false    217            �           2606    16743    tickets tickets_pk 
   CONSTRAINT     P   ALTER TABLE ONLY public.tickets
    ADD CONSTRAINT tickets_pk PRIMARY KEY (id);
 <   ALTER TABLE ONLY public.tickets DROP CONSTRAINT tickets_pk;
       public            postgres    false    215            �           2606    16632    user_roles user_roles_pk 
   CONSTRAINT     V   ALTER TABLE ONLY public.user_roles
    ADD CONSTRAINT user_roles_pk PRIMARY KEY (id);
 B   ALTER TABLE ONLY public.user_roles DROP CONSTRAINT user_roles_pk;
       public            postgres    false    203            �           2606    16644    users users_pk 
   CONSTRAINT     L   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pk PRIMARY KEY (id);
 8   ALTER TABLE ONLY public.users DROP CONSTRAINT users_pk;
       public            postgres    false    205            �           1259    16669    administrators_user_id_uindex    INDEX     b   CREATE UNIQUE INDEX administrators_user_id_uindex ON public.administrators USING btree (user_id);
 1   DROP INDEX public.administrators_user_id_uindex;
       public            postgres    false    207            �           1259    16710    airline_companies_name_uindex    INDEX     b   CREATE UNIQUE INDEX airline_companies_name_uindex ON public.airline_companies USING btree (name);
 1   DROP INDEX public.airline_companies_name_uindex;
       public            postgres    false    211            �           1259    16711     airline_companies_user_id_uindex    INDEX     h   CREATE UNIQUE INDEX airline_companies_user_id_uindex ON public.airline_companies USING btree (user_id);
 4   DROP INDEX public.airline_companies_user_id_uindex;
       public            postgres    false    211            �           1259    16686 #   costumers_credit_card_number_uindex    INDEX     n   CREATE UNIQUE INDEX costumers_credit_card_number_uindex ON public.customers USING btree (credit_card_number);
 7   DROP INDEX public.costumers_credit_card_number_uindex;
       public            postgres    false    209            �           1259    16687    costumers_phone_number_uindex    INDEX     b   CREATE UNIQUE INDEX costumers_phone_number_uindex ON public.customers USING btree (phone_number);
 1   DROP INDEX public.costumers_phone_number_uindex;
       public            postgres    false    209            �           1259    16688    costumers_user_id_uindex    INDEX     X   CREATE UNIQUE INDEX costumers_user_id_uindex ON public.customers USING btree (user_id);
 ,   DROP INDEX public.costumers_user_id_uindex;
       public            postgres    false    209            �           1259    16621    countries_name_uindex    INDEX     R   CREATE UNIQUE INDEX countries_name_uindex ON public.countries USING btree (name);
 )   DROP INDEX public.countries_name_uindex;
       public            postgres    false    201            �           1259    16847 )   flights_history_flight_original_id_uindex    INDEX     z   CREATE UNIQUE INDEX flights_history_flight_original_id_uindex ON public.flights_history USING btree (flight_original_id);
 =   DROP INDEX public.flights_history_flight_original_id_uindex;
       public            postgres    false    219            �           1259    16749 $   tickets_flight_id_customer_id_uindex    INDEX     q   CREATE UNIQUE INDEX tickets_flight_id_customer_id_uindex ON public.tickets USING btree (flight_id, customer_id);
 8   DROP INDEX public.tickets_flight_id_customer_id_uindex;
       public            postgres    false    215    215            �           1259    16839 ,   tickets_history_flight_id_customer_id_uindex    INDEX     �   CREATE UNIQUE INDEX tickets_history_flight_id_customer_id_uindex ON public.tickets_history USING btree (flight_id, customer_id);
 @   DROP INDEX public.tickets_history_flight_id_customer_id_uindex;
       public            postgres    false    217    217            �           1259    16840 )   tickets_history_original_ticket_id_uindex    INDEX     z   CREATE UNIQUE INDEX tickets_history_original_ticket_id_uindex ON public.tickets_history USING btree (original_ticket_id);
 =   DROP INDEX public.tickets_history_original_ticket_id_uindex;
       public            postgres    false    217            �           1259    16633    user_roles_role_name_uindex    INDEX     ^   CREATE UNIQUE INDEX user_roles_role_name_uindex ON public.user_roles USING btree (role_name);
 /   DROP INDEX public.user_roles_role_name_uindex;
       public            postgres    false    203            �           1259    16650    users_email_uindex    INDEX     L   CREATE UNIQUE INDEX users_email_uindex ON public.users USING btree (email);
 &   DROP INDEX public.users_email_uindex;
       public            postgres    false    205            �           1259    16651    users_username_uindex    INDEX     R   CREATE UNIQUE INDEX users_username_uindex ON public.users USING btree (username);
 )   DROP INDEX public.users_username_uindex;
       public            postgres    false    205            �           2606    16664 )   administrators administrators_users_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.administrators
    ADD CONSTRAINT administrators_users_id_fk FOREIGN KEY (user_id) REFERENCES public.users(id);
 S   ALTER TABLE ONLY public.administrators DROP CONSTRAINT administrators_users_id_fk;
       public          postgres    false    207    2984    205            �           2606    16700 3   airline_companies airline_companies_countries_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.airline_companies
    ADD CONSTRAINT airline_companies_countries_id_fk FOREIGN KEY (country_id) REFERENCES public.countries(id);
 ]   ALTER TABLE ONLY public.airline_companies DROP CONSTRAINT airline_companies_countries_id_fk;
       public          postgres    false    2978    201    211            �           2606    16705 /   airline_companies airline_companies_users_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.airline_companies
    ADD CONSTRAINT airline_companies_users_id_fk FOREIGN KEY (user_id) REFERENCES public.users(id);
 Y   ALTER TABLE ONLY public.airline_companies DROP CONSTRAINT airline_companies_users_id_fk;
       public          postgres    false    2984    211    205            �           2606    16681    customers costumers_users_id_fk    FK CONSTRAINT     ~   ALTER TABLE ONLY public.customers
    ADD CONSTRAINT costumers_users_id_fk FOREIGN KEY (user_id) REFERENCES public.users(id);
 I   ALTER TABLE ONLY public.customers DROP CONSTRAINT costumers_users_id_fk;
       public          postgres    false    205    2984    209            �           2606    16721 '   flights flights_airline_companies_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.flights
    ADD CONSTRAINT flights_airline_companies_id_fk FOREIGN KEY (airline_company_id) REFERENCES public.airline_companies(id);
 Q   ALTER TABLE ONLY public.flights DROP CONSTRAINT flights_airline_companies_id_fk;
       public          postgres    false    211    2996    213            �           2606    16726    flights flights_countries_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.flights
    ADD CONSTRAINT flights_countries_id_fk FOREIGN KEY (origin_country_id) REFERENCES public.countries(id);
 I   ALTER TABLE ONLY public.flights DROP CONSTRAINT flights_countries_id_fk;
       public          postgres    false    2978    213    201            �           2606    16731 !   flights flights_countries_id_fk_2    FK CONSTRAINT     �   ALTER TABLE ONLY public.flights
    ADD CONSTRAINT flights_countries_id_fk_2 FOREIGN KEY (destination_country_id) REFERENCES public.countries(id);
 K   ALTER TABLE ONLY public.flights DROP CONSTRAINT flights_countries_id_fk_2;
       public          postgres    false    213    201    2978            �           2606    16751    tickets tickets_customers_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.tickets
    ADD CONSTRAINT tickets_customers_id_fk FOREIGN KEY (customer_id) REFERENCES public.customers(id);
 I   ALTER TABLE ONLY public.tickets DROP CONSTRAINT tickets_customers_id_fk;
       public          postgres    false    215    2992    209            �           2606    16744    tickets tickets_flights_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.tickets
    ADD CONSTRAINT tickets_flights_id_fk FOREIGN KEY (flight_id) REFERENCES public.flights(id);
 G   ALTER TABLE ONLY public.tickets DROP CONSTRAINT tickets_flights_id_fk;
       public          postgres    false    215    213    2999            �           2606    16645    users users_user_roles_id_fk    FK CONSTRAINT     �   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_user_roles_id_fk FOREIGN KEY (user_role) REFERENCES public.user_roles(id);
 F   ALTER TABLE ONLY public.users DROP CONSTRAINT users_user_roles_id_fk;
       public          postgres    false    2980    203    205            S      x������ � �      W      x������ � �      M      x������ � �      U      x������ � �      Y      x������ � �      _      x������ � �      [      x������ � �      ]      x������ � �      O   :   x�3�LL����,.)J,�/�2�L.-.��M-�2�L�,���KUH��-H̫����� ��f      Q      x������ � �     