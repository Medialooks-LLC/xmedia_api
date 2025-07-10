# Windows
###### Create and change build directory:
```shell
md xmedia
cd xmedia
```
###### Checkout sources:
```shell
git clone https://github.com/Medialooks-LLC/xmedia_api.git . --branch=v1.0.1.1
```
###### Extract binaries from `https://github.com/Medialooks-LLC/xmedia_api/releases/download/v1.0.1.1/xmedia-api-1.0.1.1-windows.zip` to `lib` folder.
###### Build project:
Open solution `XMedia.sln` in Visual Studio 2022+ and build solution with it.

Or use command line tools of .Net SDK:
```shell
dotnet build
```

After that in folder `.\build_output\Debug\net8.0`(`.\build_output\Release\net8.0`) you will find compiled samples of usage the XMedia library.

# Linux (Ubuntu 22.04 LTS (jammy))

###### Setup build environment:
```shell
sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-8.0
```
For more information about installation of .NET SDK go to [official site](https://learn.microsoft.com/en-us/dotnet/core/install/linux-ubuntu-install?tabs=dotnet9&pivots=os-linux-ubuntu-2204).

###### Create and change build directory:
```shell
mkdir -p xmedia && cd xmedia
```
###### Checkout sources:
```shell
git clone https://github.com/Medialooks-LLC/xmedia_api.git . --branch=v1.0.1.1
```
###### Download and extract binaries from `https://github.com/Medialooks-LLC/xmedia_api/releases/download/v1.0.1.1/xmedia-api-1.0.1.1-linux.tar.gz` to `lib` folder.
```shell script
wget https://github.com/Medialooks-LLC/xmedia_api/releases/download/v1.0.1.1/xmedia-api-1.0.1.1-linux.tar.gz
tar xvfz xmedia-api-1.0.1.1-linux.tar.gz -C lib/
```
###### Build project:

Use command line tools of .Net SDK:
```shell
dotnet build
```

After that in folder `./build_output/Debug/net8.0`(`./build_output/Release/net8.0`) you will find compiled samples of usage the XMedia library.