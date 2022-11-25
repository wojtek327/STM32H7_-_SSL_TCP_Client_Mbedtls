ważność certyfikatu 10 lat
#hasło dla klucza prywatnego server – test1324@
#haslo klient - test5432!
#"Country Name (2 letter code)" - PL
#"State or Province Name (full name)" - Malopolska
#"Locality Name (eg, city)" - Krakow
#"Organization Name (eg, company)" – Test
#"Organizational Unit Name (eg, section)" -
#"Common Name (e.g. server FQDN or YOUR name)" - test_ssl
#"Email Address".

Generacja certyfikatu z pakietu GIT pod windowsem (openssl należy uruchomić jakos administrator):
req -x509 -newkey rsa:4096 -keyout test/key.pem -out test/cert.pem -days 3650 -sha256

Pod linuxem przed komendą należy dodać openssl.
