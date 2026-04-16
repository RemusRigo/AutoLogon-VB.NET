'--------------------------------------------------------------------------------------------------
' AutoLogon
'    (c) 2026 Remus Rigo
'       v1.1 2026-03-30
'--------------------------------------------------------------------------------------------------

Imports Microsoft.Win32
Imports System.Runtime.InteropServices
Imports System.Security.Principal

Public Class frmAutoLogon

   '-----------------------------------------------------------------------------------------------
   ' Add About menu item to system menu
   <DllImport("user32.dll")>
   Private Shared Function GetSystemMenu(hWnd As IntPtr, bRevert As Boolean) As IntPtr
   End Function

   <DllImport("user32.dll")>
   Private Shared Function AppendMenu(hMenu As IntPtr, uFlags As UInteger, uIDNewItem As UInteger, lpNewItem As String) As Boolean
   End Function

   ' Menu flags
   Private Const MF_SEPARATOR As UInteger = &H800
   Private Const MF_STRING As UInteger = &H0
   ' Custom menu item IDs (must be between 1 and &HF000 to avoid conflicts)
   Private Const WM_SYSCOMMAND As Integer = &H112
   Private Const SYSMENU_ABOUT_ID As UInteger = 1000

   Protected Overrides Sub OnHandleCreated(e As EventArgs)
      MyBase.OnHandleCreated(e)
      Dim hSysMenu As IntPtr = GetSystemMenu(Me.Handle, False)
      ' Add a separator and then your custom item
      AppendMenu(hSysMenu, MF_SEPARATOR, 0, String.Empty)
      AppendMenu(hSysMenu, MF_STRING, SYSMENU_ABOUT_ID, "About...")
   End Sub

   Protected Overrides Sub WndProc(ByRef m As Message)
      MyBase.WndProc(m)
      If m.Msg = WM_SYSCOMMAND Then
         If CUInt(m.WParam) = SYSMENU_ABOUT_ID Then
            frmAbout.ShowDialog()
         End If
      End If
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' LSA functions and structures
   Private Const LSA_KEY As String = "DefaultPassword"
   Private Const REG_WINLOGON As String = "SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"

   Friend Function OpenLsaPolicy() As IntPtr
      Dim objAttr As New LSA_OBJECT_ATTRIBUTES()
      objAttr.Length = CUInt(Marshal.SizeOf(objAttr))
      Dim systemName As New LSA_UNICODE_STRING()
      Dim handle As IntPtr = IntPtr.Zero
      LsaOpenPolicy(systemName, objAttr, POLICY_ALL_ACCESS, handle)
      Return handle
   End Function

   Friend Function InitLsaString(str As String) As LSA_UNICODE_STRING
      Dim lsa As New LSA_UNICODE_STRING()
      If String.IsNullOrEmpty(str) Then Return lsa
      lsa.Buffer = Marshal.StringToHGlobalUni(str)
      lsa.Length = CUShort(str.Length * 2)
      lsa.MaximumLength = CUShort(lsa.Length + 2)
      Return lsa
   End Function

   Friend Sub StoreLsaPassword(password As String)
      Dim handle = OpenLsaPolicy()
      If handle = IntPtr.Zero Then Throw New Exception("Could not open LSA policy.")
      Try
         Dim keyStr = InitLsaString(LSA_KEY)
         Dim pwdStr = InitLsaString(password)
         Try
            Dim result = LsaStorePrivateData(handle, keyStr, pwdStr)
            If result <> 0 Then Throw New Exception($"LsaStorePrivateData failed: 0x{result:X8}")
         Finally
            If keyStr.Buffer <> IntPtr.Zero Then Marshal.FreeHGlobal(keyStr.Buffer)
            If pwdStr.Buffer <> IntPtr.Zero Then Marshal.FreeHGlobal(pwdStr.Buffer)
         End Try
      Finally
         LsaClose(handle)
      End Try
   End Sub

   Friend Function RetrieveLsaPassword() As String
      Dim handle = OpenLsaPolicy()
      If handle = IntPtr.Zero Then Return ""
      Try
         Dim keyStr = InitLsaString(LSA_KEY)
         Dim dataPtr As IntPtr = IntPtr.Zero
         Try
            Dim result = LsaRetrievePrivateData(handle, keyStr, dataPtr)
            If result <> 0 OrElse dataPtr = IntPtr.Zero Then Return ""

            Dim lsaStr = Marshal.PtrToStructure(Of LSA_UNICODE_STRING)(dataPtr)
            Dim pwd = Marshal.PtrToStringUni(lsaStr.Buffer, lsaStr.Length \ 2)
            LsaFreeMemory(dataPtr)
            Return pwd
         Finally
            If keyStr.Buffer <> IntPtr.Zero Then Marshal.FreeHGlobal(keyStr.Buffer)
         End Try
      Finally
         LsaClose(handle)
      End Try
   End Function

   Private Sub frmAutoLogon_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      If Not IsAdministrator() Then
         grpBoxAutoLoginSettings.Enabled = False
         ToolStripStatusLabel.Text = "Must run as administrator to enable AutoLogon configuration"
      Else
         ToolStripStatusLabel.Text = "AutoLogon configuration is available"
      End If

      'check if AutoLogon is enabled
      If RegReadBool(Registry.LocalMachine, REG_WINLOGON, "AutoAdminLogon") Then
         rBtnEnabled.Checked = True
      Else
         rBtnDisabled.Checked = True
      End If

      If RegValueExists(Registry.LocalMachine, REG_WINLOGON, "DefaultUserName") Then
         txtBoxUser.Text = RegReadStr(Registry.LocalMachine, REG_WINLOGON, "DefaultUserName")
      Else
         txtBoxUser.Text = Environment.UserName
      End If

      txtBoxPass.Text = RetrieveLsaPassword()

      If RegValueExists(Registry.LocalMachine, REG_WINLOGON, "DefaultDomainName") Then
         txtBoxDomain.Text = RegReadStr(Registry.LocalMachine, REG_WINLOGON, "DefaultDomainName")
      End If
   End Sub

   Private Sub rBtnEnabled_CheckedChanged(sender As Object, e As EventArgs) Handles rBtnEnabled.CheckedChanged
      If rBtnEnabled.Checked Then
         grpBoxAutoLoginSettings.Enabled = True
         RegWriteBool(Registry.LocalMachine, REG_WINLOGON, "AutoAdminLogon", True)
      End If
   End Sub

   Private Sub rBtnDisabled_CheckedChanged(sender As Object, e As EventArgs) Handles rBtnDisabled.CheckedChanged
      If rBtnDisabled.Checked Then
         grpBoxAutoLoginSettings.Enabled = False
         RegWriteBool(Registry.LocalMachine, REG_WINLOGON, "AutoAdminLogon", False)
      End If
   End Sub

   Private Sub rBtnLocal_CheckedChanged(sender As Object, e As EventArgs) Handles rBtnLocal.CheckedChanged
      If rBtnLocal.Checked Then
         txtBoxDomain.Text = ""
      End If
   End Sub

   Private Sub rBtnMSAccount_CheckedChanged(sender As Object, e As EventArgs) Handles rBtnMSAccount.CheckedChanged
      If rBtnMSAccount.Checked Then
         txtBoxUser.Text = "MicrosoftAccount\your@email.com"
         txtBoxDomain.Text = "MicrosoftAccount"
         ' use password, not PIN
         If RegValueExists(Registry.LocalMachine, "Software\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\TestHooks", "Passwordless") Then
            RegWriteInt(Registry.LocalMachine, "Software\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\TestHooks", "Passwordless", 0)
         End If
         If RegValueExists(Registry.LocalMachine, "Software\Microsoft\Windows NT\CurrentVersion\PasswordLess\Device", "DevicePasswordLessBuildVersion") Then
            RegWriteInt(Registry.LocalMachine, "Software\Microsoft\Windows NT\CurrentVersion\PasswordLess\Device", "DevicePasswordLessBuildVersion", 0)
         End If
      End If
   End Sub

   Private Sub rBtnDomain_CheckedChanged(sender As Object, e As EventArgs) Handles rBtnDomain.CheckedChanged
      If rBtnDomain.Checked Then
         txtBoxDomain.Text = Environment.UserDomainName
      End If
   End Sub

   Private Sub btnRead_Click(sender As Object, e As EventArgs) Handles btnRead.Click
      txtBoxUser.Text = RegReadStr(Registry.LocalMachine, REG_WINLOGON, "DefaultUserName")
      txtBoxPass.Text = RetrieveLsaPassword()
      txtBoxDomain.Text = RegReadStr(Registry.LocalMachine, REG_WINLOGON, "DefaultDomainName")
   End Sub

   Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
      ' delete data from registry and LSA
      RegDeleteValue(Registry.LocalMachine, REG_WINLOGON, "DefaultUserName")
      RegDeleteValue(Registry.LocalMachine, REG_WINLOGON, "DefaultPassword")
      RegDeleteValue(Registry.LocalMachine, REG_WINLOGON, "DefaultDomainName")
      StoreLsaPassword("")
      ' read data again to update UI
      btnRead_Click(sender, e)
   End Sub

   Private Sub btnSet_Click(sender As Object, e As EventArgs) Handles btnSet.Click
      btnDelete_Click(sender, e) ' Clear existing values first

      RegWriteStr(Registry.LocalMachine, REG_WINLOGON, "DefaultUserName", txtBoxUser.Text)
      If rBtnLocal.Checked Then
         RegWriteStr(Registry.LocalMachine, REG_WINLOGON, "DefaultDomainName", "")
      Else
         RegWriteStr(Registry.LocalMachine, REG_WINLOGON, "DefaultDomainName", txtBoxDomain.Text)
      End If
      StoreLsaPassword(txtBoxPass.Text)
      ToolStripStatusLabel.Text = "Configuration set"
   End Sub

   Private Sub lblPassword_Click(sender As Object, e As EventArgs) Handles lblPassword.Click
      txtBoxPass.UseSystemPasswordChar = Not txtBoxPass.UseSystemPasswordChar
   End Sub
End Class
