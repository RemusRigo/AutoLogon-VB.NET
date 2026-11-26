color 02
dotnet restore AutoLogon/AutoLogon.vbproj

@timeout 3

dotnet build AutoLogon/AutoLogon.vbproj -f net48 -c Release -p:Platform=x64 -p:DebugType=None -p:DebugSymbols=false
dotnet build AutoLogon/AutoLogon.vbproj -f net48 -c Release -p:Platform=x86 -p:DebugType=None -p:DebugSymbols=false

dotnet build AutoLogon/AutoLogon.vbproj -f net8.0-windows -c Release -p:Platform=x64 -p:DebugType=None -p:DebugSymbols=false
dotnet build AutoLogon/AutoLogon.vbproj -f net8.0-windows -c Release -p:Platform=x86 -p:DebugType=None -p:DebugSymbols=false

dotnet build AutoLogon/AutoLogon.vbproj -f net10.0-windows -c Release -p:Platform=x64 -p:DebugType=None -p:DebugSymbols=false
dotnet build AutoLogon/AutoLogon.vbproj -f net10.0-windows -c Release -p:Platform=x86 -p:DebugType=None -p:DebugSymbols=false

@rem .NET 3.5 (AutoLogonXP) needs Visual Studio's MSBuild, the dotnet CLI cannot build it
@for /f "usebackq delims=" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do @set "MSBUILD=%%i"
"%MSBUILD%" AutoLogonXP/AutoLogonXP.vbproj -t:Rebuild -p:Configuration=Release -p:Platform=x64 -p:DebugType=None -p:DebugSymbols=false -v:m
"%MSBUILD%" AutoLogonXP/AutoLogonXP.vbproj -t:Rebuild -p:Configuration=Release -p:Platform=x86 -p:DebugType=None -p:DebugSymbols=false -v:m

copy *.md .\bin\Release /Y
copy *.txt .\bin\Release /Y

@timeout 7