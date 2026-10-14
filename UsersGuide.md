# AutoLogon: User's Guide



## Downloads & Runtimes

Download the latest release from the [Releases page](https://github.com/RemusRigo/AutoLogon-VB.NET/releases).

Each release contains one folder per framework and architecture:

|Folder|Target Framework|Architecture|Required Runtime|
|-|-|-|-|
|`net35\x64`, `net35\x86`|.NET Framework 3.5|64/32-bit|[.NET Framework 3.5](https://dotnet.microsoft.com/download/dotnet-framework/net35-sp1) (Windows XP and later)|
|`net48\x64`, `net48\x86`|.NET Framework 4.8|64/32-bit|[.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48) (included in Windows 10/11)|
|`net8.0-windows\x64`, `\x86`|.NET 8.0|64/32-bit|[.NET 8.0 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)|
|`net10.0-windows\x64`, `\x86`|.NET 10.0|64/32-bit|[.NET 10.0 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/10.0)|

> **Which one do I need?** On Windows 10/11, the **net48** build runs without installing anything.
> On 64-bit Windows use **x64**. Use **net35** (`AutoLogonXP.exe`) only on Windows XP.
> For the .NET 8.0/10.0 builds, the plain ".NET Runtime" is not enough; install the **Desktop** Runtime.



## Install

AutoLogon is portable: there is no installer.

1. Download and extract the folder that matches your system (see the table above).
2. Copy the **whole folder** to a permanent location, for example `D:\My Apps\AutoLogon\`.
3. Double-click `AutoLogon.exe` and accept the administrator prompt (AutoLogon writes to `HKEY_LOCAL_MACHINE`).

> If Windows blocks the file ("Windows protected your PC"), right-click `AutoLogon.exe` →
> **Properties** → check **Unblock** → **OK**, then try again.



## Usage

1. Check **Enable AutoLogon**. The **Settings** group is shown.
2. Choose the **Account Type**:

   |Account Type|User Name|Domain|
   |-|-|-|
   |**Local**|your Windows user name|(hidden)|
   |**Microsoft Account**|your Microsoft account e-mail|`MicrosoftAccount` (filled in automatically)|
   |**Domain**|your domain user name|your domain name (filled in automatically when you are on a domain)|

3. Enter the **Password**. Click the **Password:** label to show/hide it.
4. Click **Set**, then restart Windows: it signs in automatically.

|Button|Description|
|-|-|
|**Set**|Save the user name, password and domain|
|**Read**|Reload the saved settings|
|**Delete**|Remove the saved user name, password and domain|

To turn AutoLogon off, uncheck **Enable AutoLogon**.
Right-click the title bar → **About...** for version information.

> **Microsoft Account:** **Set** also turns off the Windows Hello / passwordless sign-in options, so
> Windows can sign in with the password. **Delete** and unchecking **Enable AutoLogon** don't turn them back on.

> **Security:** anyone who turns on the PC is signed in to your account, and an administrator on this PC
> can read the saved password. Use AutoLogon only on a PC that is physically secure.

### Screenshots

Local Account:<br> ![Local Account](Screenshots/local.png)

Microsoft Account:<br> ![Microsoft Account](Screenshots/microsoft.png)

Domain Account:<br> ![Domain Account](Screenshots/domain.png)



## Where are the settings stored?

|Setting|Location|
|-|-|
|AutoLogon on/off, user name, domain|`HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon` (`AutoAdminLogon`, `DefaultUserName`, `DefaultDomainName`)|
|Password|LSA secret `DefaultPassword`|



## Uninstall

1. Click **Delete**, then uncheck **Enable AutoLogon**.
2. Delete the folder you copied in step 2 of *Install*.
