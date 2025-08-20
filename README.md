This is soft for store key on your PC in encrypted binary format.<br>
Encryption by public RSA key and decryption by private RSA key.<br><br>

You can create new key by next instruction:<br>

Generate new RSA key and convert in to PEM<br>
ssh-keygen -t rsa -b 4096 -f ~/.ssh/[your_key_name]<br>
ssh-keygen -p -m PEM -f ~/.ssh/[your_key_name]<br>
openssl rsa -in ~/.ssh/[your_key_name] -pubout -out ~/.ssh/[your_key_name].pem<br><br>

Create PEM key end extract public key<br>
openssl genpkey -algorithm RSA -aes256 -out ~/.ssh/[your_key_name].pem -pkeyopt rsa_keygen_bits:4096<br>
openssl rsa -in ~/.ssh/[your_key_name].pem -pubout -out ~/.ssh/[your_key_name]_pub.pem