This is soft for store key on your PC in encrypted binary format.
Encryption by public RSA key and decryption by private RSA key.

You can create new key by next instruction:

Generate new RSA key and convert in to PEM
ssh-keygen -t rsa -b 4096 -f ~/.ssh/[your_key_name]
ssh-keygen -p -m PEM -f ~/.ssh/[your_key_name]
openssl rsa -in ~/.ssh/[your_key_name] -pubout -out ~/.ssh/[your_key_name].pem

Create PEM key end extract public key
openssl genpkey -algorithm RSA -aes256 -out ~/.ssh/[your_key_name].pem -pkeyopt rsa_keygen_bits:4096
openssl rsa -in ~/.ssh/[your_key_name].pem -pubout -out ~/.ssh/[your_key_name]_pub.pem