# Install Dot-net sdk-8.0 in ubuntu
```
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x dotnet-install.sh

./dotnet-install.sh --channel 8.0

echo 'export DOTNET_ROOT=$HOME/.dotnet' >> ~/.bashrc
echo 'export PATH=$DOTNET_ROOT:$PATH' >> ~/.bashrc
source ~/.bashrc

dotnet --list-sdks


verify space
------
df -i /
df -hT /tmp
df -hT "$HOME"

If /tmp directory is out of space remount extry spece on that Because /tm is bydefault mounted on /tmpfs file system
which uses RAM as memory so to you need to increse its space.

Jenkins Also use same Space to Download and cache from it only So if you get out of space error Increse /tmp spce

sudo mount -o remount,size=2G /tmp

```
