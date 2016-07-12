@ECHO OFF
rm -rv newcerts crl certs index.txt* crlnumber serial* *.pem *.csr *.crt *.pfx
mkdir newcerts crl certs
touch index.txt crlnumber
echo 01 > serial
openssl genrsa -out rootca.pem 4096
echo This is the Root CA
openssl req -config openssl.cnf -new -x509 -days 18250 -subj "/CN=Test Root CA" -key rootca.pem -out rootca.crt
openssl genrsa -out subca_0.pem 4096
echo This is the Sub CA
openssl req -config openssl.cnf -new -subj "/CN=Test Sub CA: 0" -key subca_0.pem -out subca_0.csr
openssl ca -config openssl.cnf -batch -extensions v3_ca -in subca_0.csr -out subca_0.crt -keyfile rootca.pem -cert rootca.crt
openssl genrsa -out agent_0.pem 4096
echo This is the Agent Certificate
openssl req -config openssl.cnf -new -subj "/CN=Test Agent: 0" -key agent_0.pem -out agent_0.csr
openssl ca -batch -config openssl.cnf -in agent_0.csr -out agent_0.crt
cat subca_0.crt rootca.crt > agent_0_chain.crt
cat agent_0.pem agent_0.crt > agent_0_pair.pem
openssl pkcs12 -export -nodes -passout pass: -CSP "Microsoft Enhanced RSA and AES Cryptographic Provider" -name "Test Agent: 0" -caname "Test Sub CA: 0" -caname "Test Root CA" -certfile agent_0_chain.crt -out "agent_0.pfx" -in "agent_0_pair.pem"
pause