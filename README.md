# This is soft for store key on your PC in encrypted binary format.<br>
**Encryption by public RSA key and decryption by private RSA key.<br><br>**

For start ro use it to have to make PEM key
You can create new key by next instruction:<br>

Generate new RSA key and convert in to PEM<br>
ssh-keygen -t rsa -b 4096 -f ~/.ssh/[your_key_name]<br>
ssh-keygen -p -m PEM -f ~/.ssh/[your_key_name]<br>
openssl rsa -in ~/.ssh/[your_key_name] -pubout -out ~/.ssh/[your_key_name].pem<br><br>
OR
Create PEM key end extract public key<br>
openssl genpkey -algorithm RSA -aes256 -out ~/.ssh/[your_key_name].pem -pkeyopt rsa_keygen_bits:4096<br>
openssl rsa -in ~/.ssh/[your_key_name].pem -pubout -out ~/.ssh/[your_key_name]_pub.pem<br><br>

If you have PEM key you need setup config file. Can find it after first start in next folder:<br>
Linux - /home/[user_name]/.local/share/binstorage/configuration.json<br>
Windows - C:\Users\[user_name]\AppData\Local\binstorage\configuration.json<br>
Should set path for private, public PEM keys and path to bin file for storage.<br><br>

After this simple steps you can use soft by instructions --help
