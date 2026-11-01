# Install Dot-net sdk-8.0 in ubuntu
```
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x dotnet-install.sh

./dotnet-install.sh --channel 8.0

echo 'export DOTNET_ROOT=$HOME/.dotnet' >> ~/.bashrc
echo 'export PATH=$DOTNET_ROOT:$PATH' >> ~/.bashrc
source ~/.bashrc

dotnet --list-sdks

```
