'--------------------------------------------------------------------------------------------------
' AutoLogon
'    © 2026 Remus Rigo
'       v1.1.20261126
'--------------------------------------------------------------------------------------------------

Imports Microsoft.Win32
Imports System.Runtime.InteropServices
Imports AutoLogon.API

Public Class frmAutoLogon

   '-----------------------------------------------------------------------------------------------
   ' Add About menu item to system menu


   ' Custom menu item IDs (must be between 1 and &HF000 to avoid conflicts)

   Private Const SYSMENU_ABOUT_ID As UInteger = 1000
   Private moveUp As Boolean


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

   '-----------------------------------------------------------------------------------------------
   ' Retrieve Lsa Password
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

   '-----------------------------------------------------------------------------------------------
   ' frmAutoLogon onLoad
   Private Sub frmAutoLogon_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      Me.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath)   ' exe icon (Res\Main.ico), avoids a binary resource in the .resx
      Me.Text = appTitle

      'check if AutoLogon is enabled
      If RegValueExists(Registry.LocalMachine, REG_WINLOGON, "AutoAdminLogon") Then
         ' if exists, check if it's set to 1 (enabled) or 0 (disabled)
         If RegReadSZ(Registry.LocalMachine, REG_WINLOGON, "AutoAdminLogon") = "1" Then
            chkBoxAutologon.Checked = True
            lblStatus.Text = "Autologon Enabled"
            lblStatus.Top = 220
         Else
            chkBoxAutologon.Checked = False
            lblStatus.Text = "Autologon Disabled"
            lblStatus.Top = 19
         End If
      Else
         ' AutoLogon is disabled
         chkBoxAutologon.Checked = False
      End If

      ' check DefaultUserName
      If RegValueExists(Registry.LocalMachine, REG_WINLOGON, "DefaultUserName") Then
         txtBoxUser.Text = RegReadSZ(Registry.LocalMachine, REG_WINLOGON, "DefaultUserName")
      Else
         txtBoxUser.Text = Environment.UserName
      End If

      ' get password from LSA
      txtBoxPass.Text = RetrieveLsaPassword()

      ' check account type
      If RegValueExists(Registry.LocalMachine, REG_WINLOGON, "DefaultDomainName") Then
         txtBoxDomain.Text = RegReadSZ(Registry.LocalMachine, REG_WINLOGON, "DefaultDomainName")
         If RegReadSZ(Registry.LocalMachine, REG_WINLOGON, "DefaultDomainName") = "" Then
            rBtnLocal.Checked = True
         ElseIf RegReadSZ(Registry.LocalMachine, REG_WINLOGON, "DefaultDomainName") = "MicrosoftAccount" Then
            rBtnMSAccount.Checked = True
         Else
            rBtnDomain.Checked = True
         End If
      End If
   End Sub

   Private Sub frmAutoLogon_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
      grpBoxAutoLoginSettings.Visible = chkBoxAutologon.Checked
      Me.AutoSize = True
   End Sub

   Private Sub chkBoxAutologon_CheckedChanged(sender As Object, e As EventArgs) Handles chkBoxAutologon.CheckedChanged
      If chkBoxAutologon.Checked Then
         RegWriteSZ(Registry.LocalMachine, REG_WINLOGON, "AutoAdminLogon", "1")
         lblStatus.Text = "Autologon Enabled"
         lblStatus.Top = 220
      Else
         RegWriteSZ(Registry.LocalMachine, REG_WINLOGON, "AutoAdminLogon", "0")
         lblStatus.Text = "Autologon Disabled"
         lblStatus.Top = 19
      End If
      grpBoxAutoLoginSettings.Visible = chkBoxAutologon.Checked
      Me.AutoSize = True
   End Sub

   Private Sub rBtnLocal_CheckedChanged(sender As Object, e As EventArgs) Handles rBtnLocal.CheckedChanged
      If rBtnLocal.Checked Then
         lblStatus.Text = "Local account selected"
         ' hide/unhide
         lblDomain.Visible = Not rBtnLocal.Checked
         txtBoxDomain.Visible = Not rBtnLocal.Checked
         btnRead.Top = btnRead.Top - 23
         btnDelete.Top = btnDelete.Top - 23
         btnSet.Top = btnSet.Top - 23
         grpBoxAutoLoginSettings.Height = grpBoxAutoLoginSettings.Height - 23
         lblStatus.Top = lblStatus.Top - 23
         moveUp = True

         ToolTip.SetToolTip(txtBoxUser, "Username")
         txtBoxDomain.Text = ""
      End If
   End Sub

   Private Sub rBtnMSAccount_CheckedChanged(sender As Object, e As EventArgs) Handles rBtnMSAccount.CheckedChanged
      If rBtnMSAccount.Checked Then
         lblStatus.Text = "Microsoft account selected"
         ' hide/unhide
         lblDomain.Visible = rBtnMSAccount.Checked
         txtBoxDomain.Visible = rBtnMSAccount.Checked
         If moveUp Then
            btnRead.Top = btnRead.Top + 23
            btnDelete.Top = btnDelete.Top + 23
            btnSet.Top = btnSet.Top + 23
            grpBoxAutoLoginSettings.Height = grpBoxAutoLoginSettings.Height + 23
            lblStatus.Top = lblStatus.Top + 23
            moveUp = False
         End If

         ToolTip.SetToolTip(txtBoxUser, "MicrosoftAccount\your@email.com")
         txtBoxDomain.Text = "MicrosoftAccount"
      End If
   End Sub

   Private Sub rBtnDomain_CheckedChanged(sender As Object, e As EventArgs) Handles rBtnDomain.CheckedChanged
      If rBtnDomain.Checked Then
         lblStatus.Text = "Domain selected"
         ' hide/unhide
         lblDomain.Visible = rBtnDomain.Checked
         txtBoxDomain.Visible = rBtnDomain.Checked
         If moveUp Then
            btnRead.Top = btnRead.Top + 23
            btnDelete.Top = btnDelete.Top + 23
            btnSet.Top = btnSet.Top + 23
            grpBoxAutoLoginSettings.Height = grpBoxAutoLoginSettings.Height + 23
            moveUp = False
         End If

         ToolTip.SetToolTip(txtBoxUser, "DomainName\yourusername")
         If Environment.MachineName <> Environment.UserDomainName Then
            txtBoxDomain.Text = Environment.UserDomainName
         End If
      End If
   End Sub

   Private Sub btnRead_Click(sender As Object, e As EventArgs) Handles btnRead.Click
      ' read data from registry and LSA
      txtBoxUser.Text = RegReadSZ(Registry.LocalMachine, REG_WINLOGON, "DefaultUserName")
      txtBoxPass.Text = RetrieveLsaPassword()
      txtBoxDomain.Text = RegReadSZ(Registry.LocalMachine, REG_WINLOGON, "DefaultDomainName")
      lblStatus.Text = "Settings loaded"
   End Sub

   Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
      ' delete data from registry and LSA
      RegDeleteValue(Registry.LocalMachine, REG_WINLOGON, "DefaultUserName")
      RegDeleteValue(Registry.LocalMachine, REG_WINLOGON, "DefaultPassword")
      RegDeleteValue(Registry.LocalMachine, REG_WINLOGON, "DefaultDomainName")
      StoreLsaPassword("")
      ' read data again to update UI
      btnRead_Click(sender, e)
      lblStatus.Text = "Settings deleted"
   End Sub

   Private Sub btnSet_Click(sender As Object, e As EventArgs) Handles btnSet.Click
      'user name
      RegWriteSZ(Registry.LocalMachine, REG_WINLOGON, "DefaultUserName", txtBoxUser.Text)

      ' password
      StoreLsaPassword(txtBoxPass.Text)

      ' local account
      If rBtnLocal.Checked Then
         RegWriteSZ(Registry.LocalMachine, REG_WINLOGON, "DefaultDomainName", "")
      End If

      ' Microsoft account
      If rBtnMSAccount.Checked Then
         RegWriteSZ(Registry.LocalMachine, REG_WINLOGON, "DefaultDomainName", "MicrosoftAccount")

         ' use password, not PIN
         If RegValueExists(Registry.LocalMachine, "Software\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\TestHooks", "Passwordless") Then
            RegWriteDWord(Registry.LocalMachine, "Software\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\TestHooks", "Passwordless", 0)
         End If
         If RegValueExists(Registry.LocalMachine, "Software\Microsoft\Windows NT\CurrentVersion\PasswordLess\Device", "DevicePasswordLessBuildVersion") Then
            RegWriteDWord(Registry.LocalMachine, "Software\Microsoft\Windows NT\CurrentVersion\PasswordLess\Device", "DevicePasswordLessBuildVersion", 0)
         End If
         ' Disable Windows Hello sign-in (allows autologon to take over)
         RegWriteDWord(Registry.LocalMachine, "SOFTWARE\Microsoft\PolicyManager\default\Settings", "AllowSignInOptions", 0)

      End If

      ' domain account
      If rBtnDomain.Checked Then
         RegWriteSZ(Registry.LocalMachine, REG_WINLOGON, "DefaultDomainName", txtBoxDomain.Text)
      End If

      lblStatus.Text = "Configuration set"
   End Sub

   Private Sub lblPassword_Click(sender As Object, e As EventArgs) Handles lblPassword.Click
      txtBoxPass.UseSystemPasswordChar = Not txtBoxPass.UseSystemPasswordChar
   End Sub

End Class
