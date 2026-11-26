<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAutoLogon
   Inherits System.Windows.Forms.Form

   'Form overrides dispose to clean up the component list.
   <System.Diagnostics.DebuggerNonUserCode()>
   Protected Overrides Sub Dispose(disposing As Boolean)
      Try
         If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
         End If
      Finally
         MyBase.Dispose(disposing)
      End Try
   End Sub

   'Required by the Windows Form Designer
   Private components As System.ComponentModel.IContainer

   'NOTE: The following procedure is required by the Windows Form Designer
   'It can be modified using the Windows Form Designer.
   'Do not modify it using the code editor.
   <System.Diagnostics.DebuggerStepThrough()>
   Private Sub InitializeComponent()
      components = New ComponentModel.Container()
      grpBoxAutoLoginSettings = New GroupBox()
      grpBoxAutoLoginType = New GroupBox()
      rBtnDomain = New RadioButton()
      rBtnMSAccount = New RadioButton()
      rBtnLocal = New RadioButton()
      btnSet = New Button()
      btnRead = New Button()
      btnDelete = New Button()
      txtBoxDomain = New TextBox()
      txtBoxPass = New TextBox()
      lblDomain = New Label()
      lblPassword = New Label()
      txtBoxUser = New TextBox()
      lblUser = New Label()
      chkBoxAutologon = New CheckBox()
      ToolTip = New ToolTip(components)
      lblStatus = New Label()
      grpBoxAutoLoginSettings.SuspendLayout()
      grpBoxAutoLoginType.SuspendLayout()
      SuspendLayout()
      ' 
      ' grpBoxAutoLoginSettings
      ' 
      grpBoxAutoLoginSettings.Controls.Add(grpBoxAutoLoginType)
      grpBoxAutoLoginSettings.Controls.Add(btnSet)
      grpBoxAutoLoginSettings.Controls.Add(btnRead)
      grpBoxAutoLoginSettings.Controls.Add(btnDelete)
      grpBoxAutoLoginSettings.Controls.Add(txtBoxDomain)
      grpBoxAutoLoginSettings.Controls.Add(txtBoxPass)
      grpBoxAutoLoginSettings.Controls.Add(lblDomain)
      grpBoxAutoLoginSettings.Controls.Add(lblPassword)
      grpBoxAutoLoginSettings.Controls.Add(txtBoxUser)
      grpBoxAutoLoginSettings.Controls.Add(lblUser)
      grpBoxAutoLoginSettings.Location = New Point(3, 25)
      grpBoxAutoLoginSettings.Name = "grpBoxAutoLoginSettings"
      grpBoxAutoLoginSettings.Size = New Size(377, 185)
      grpBoxAutoLoginSettings.TabIndex = 32
      grpBoxAutoLoginSettings.TabStop = False
      grpBoxAutoLoginSettings.Text = "Settings"
      ' 
      ' grpBoxAutoLoginType
      ' 
      grpBoxAutoLoginType.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
      grpBoxAutoLoginType.Controls.Add(rBtnDomain)
      grpBoxAutoLoginType.Controls.Add(rBtnMSAccount)
      grpBoxAutoLoginType.Controls.Add(rBtnLocal)
      grpBoxAutoLoginType.Location = New Point(7, 20)
      grpBoxAutoLoginType.Name = "grpBoxAutoLoginType"
      grpBoxAutoLoginType.Size = New Size(358, 44)
      grpBoxAutoLoginType.TabIndex = 42
      grpBoxAutoLoginType.TabStop = False
      grpBoxAutoLoginType.Text = "Account Type"
      ' 
      ' rBtnDomain
      ' 
      rBtnDomain.AutoSize = True
      rBtnDomain.Location = New Point(190, 20)
      rBtnDomain.Name = "rBtnDomain"
      rBtnDomain.Size = New Size(67, 19)
      rBtnDomain.TabIndex = 32
      rBtnDomain.TabStop = True
      rBtnDomain.Text = "Domain"
      rBtnDomain.UseVisualStyleBackColor = True
      ' 
      ' rBtnMSAccount
      ' 
      rBtnMSAccount.AutoSize = True
      rBtnMSAccount.Location = New Point(60, 20)
      rBtnMSAccount.Name = "rBtnMSAccount"
      rBtnMSAccount.Size = New Size(124, 19)
      rBtnMSAccount.TabIndex = 31
      rBtnMSAccount.TabStop = True
      rBtnMSAccount.Text = "Microsoft Account"
      rBtnMSAccount.UseVisualStyleBackColor = True
      ' 
      ' rBtnLocal
      ' 
      rBtnLocal.AutoSize = True
      rBtnLocal.Location = New Point(5, 20)
      rBtnLocal.Name = "rBtnLocal"
      rBtnLocal.Size = New Size(53, 19)
      rBtnLocal.TabIndex = 30
      rBtnLocal.TabStop = True
      rBtnLocal.Text = "Local"
      rBtnLocal.UseVisualStyleBackColor = True
      ' 
      ' btnSet
      ' 
      btnSet.Anchor = AnchorStyles.Top Or AnchorStyles.Right
      btnSet.Location = New Point(319, 150)
      btnSet.Name = "btnSet"
      btnSet.Size = New Size(45, 23)
      btnSet.TabIndex = 37
      btnSet.Text = "&Set"
      btnSet.UseVisualStyleBackColor = True
      ' 
      ' btnRead
      ' 
      btnRead.Anchor = AnchorStyles.Top Or AnchorStyles.Right
      btnRead.Location = New Point(207, 150)
      btnRead.Name = "btnRead"
      btnRead.Size = New Size(50, 23)
      btnRead.TabIndex = 39
      btnRead.Text = "&Read"
      btnRead.UseVisualStyleBackColor = True
      ' 
      ' btnDelete
      ' 
      btnDelete.Anchor = AnchorStyles.Top Or AnchorStyles.Right
      btnDelete.Location = New Point(263, 150)
      btnDelete.Name = "btnDelete"
      btnDelete.Size = New Size(50, 23)
      btnDelete.TabIndex = 38
      btnDelete.Text = "&Delete"
      btnDelete.UseVisualStyleBackColor = True
      ' 
      ' txtBoxDomain
      ' 
      txtBoxDomain.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
      txtBoxDomain.Location = New Point(102, 122)
      txtBoxDomain.Name = "txtBoxDomain"
      txtBoxDomain.Size = New Size(262, 23)
      txtBoxDomain.TabIndex = 36
      ' 
      ' txtBoxPass
      ' 
      txtBoxPass.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
      txtBoxPass.Location = New Point(102, 98)
      txtBoxPass.Name = "txtBoxPass"
      txtBoxPass.Size = New Size(262, 23)
      txtBoxPass.TabIndex = 35
      txtBoxPass.UseSystemPasswordChar = True
      ' 
      ' lblDomain
      ' 
      lblDomain.AutoSize = True
      lblDomain.Location = New Point(5, 129)
      lblDomain.Name = "lblDomain"
      lblDomain.Size = New Size(52, 15)
      lblDomain.TabIndex = 34
      lblDomain.Text = "Domain:"
      ' 
      ' lblPassword
      ' 
      lblPassword.AutoSize = True
      lblPassword.Location = New Point(5, 105)
      lblPassword.Name = "lblPassword"
      lblPassword.Size = New Size(60, 15)
      lblPassword.TabIndex = 33
      lblPassword.Text = "Password:"
      ' 
      ' txtBoxUser
      ' 
      txtBoxUser.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
      txtBoxUser.Location = New Point(102, 70)
      txtBoxUser.Name = "txtBoxUser"
      txtBoxUser.Size = New Size(262, 23)
      txtBoxUser.TabIndex = 32
      ' 
      ' lblUser
      ' 
      lblUser.AutoSize = True
      lblUser.Location = New Point(5, 78)
      lblUser.Name = "lblUser"
      lblUser.Size = New Size(68, 15)
      lblUser.TabIndex = 31
      lblUser.Text = "User Name:"
      ' 
      ' chkBoxAutologon
      ' 
      chkBoxAutologon.AutoSize = True
      chkBoxAutologon.Location = New Point(3, 3)
      chkBoxAutologon.Name = "chkBoxAutologon"
      chkBoxAutologon.Size = New Size(124, 19)
      chkBoxAutologon.TabIndex = 33
      chkBoxAutologon.Text = "Enable AutoLogon"
      chkBoxAutologon.UseVisualStyleBackColor = True
      ' 
      ' lblStatus
      ' 
      lblStatus.AutoSize = True
      lblStatus.Location = New Point(0, 219)
      lblStatus.Name = "lblStatus"
      lblStatus.Size = New Size(66, 15)
      lblStatus.TabIndex = 34
      lblStatus.Text = "Checking..."
      ' 
      ' frmAutoLogon
      ' 
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      AutoSize = True
      AutoSizeMode = AutoSizeMode.GrowAndShrink
      ClientSize = New Size(384, 234)
      Controls.Add(lblStatus)
      Controls.Add(chkBoxAutologon)
      Controls.Add(grpBoxAutoLoginSettings)
      FormBorderStyle = FormBorderStyle.FixedSingle
      MaximizeBox = False
      MinimizeBox = False
      Name = "frmAutoLogon"
      StartPosition = FormStartPosition.CenterScreen
      Text = "AutoLogon"
      grpBoxAutoLoginSettings.ResumeLayout(False)
      grpBoxAutoLoginSettings.PerformLayout()
      grpBoxAutoLoginType.ResumeLayout(False)
      grpBoxAutoLoginType.PerformLayout()
      ResumeLayout(False)
      PerformLayout()
   End Sub
   Friend WithEvents grpBoxAutoLoginSettings As GroupBox
   Friend WithEvents grpBoxAutoLoginType As GroupBox
   Friend WithEvents btnSet As Button
   Friend WithEvents btnRead As Button
   Friend WithEvents btnDelete As Button
   Friend WithEvents txtBoxDomain As TextBox
   Friend WithEvents txtBoxPass As TextBox
   Friend WithEvents lblDomain As Label
   Friend WithEvents lblPassword As Label
   Friend WithEvents txtBoxUser As TextBox
   Friend WithEvents lblUser As Label
   Friend WithEvents rBtnDomain As RadioButton
   Friend WithEvents rBtnMSAccount As RadioButton
   Friend WithEvents rBtnLocal As RadioButton
   Friend WithEvents chkBoxAutologon As CheckBox
   Friend WithEvents ToolTip As ToolTip
   Friend WithEvents lblStatus As Label

End Class
