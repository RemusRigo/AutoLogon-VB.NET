'--------------------------------------------------------------------------------------------------
' libReg - Registry functions
'    (c) 2026 Remus Rigo
'       v1.0 2026-03-27
'--------------------------------------------------------------------------------------------------


Imports Microsoft.Win32

Public Module libReg

	'-----------------------------------------------------------------------------------------------
	' Check if Value exists

	Public Function RegValueExists(root As RegistryKey, path As String, valueName As String) As Boolean
		Using key As RegistryKey = root.OpenSubKey(path, writable:=False)
			If key Is Nothing Then Return False ' Key does not exist
			Dim names() As String = key.GetValueNames()
			Return names.Contains(valueName)
		End Using
	End Function

	'-----------------------------------------------------------------------------------------------
	' Delete Value
	Public Function RegDeleteValue(root As RegistryKey, path As String, valueName As String) As Boolean
		Try
			Using key As RegistryKey = root.CreateSubKey(path, True)
				key.DeleteValue(valueName)
			End Using
			Return True
		Catch ex As Exception
			Return False
		End Try
	End Function

	'-----------------------------------------------------------------------------------------------
	' Read/Write Boolean

	Public Function RegReadBool(root As RegistryKey, path As String, name As String) As Boolean
		Using key As RegistryKey = root.OpenSubKey(path, False)
			If key Is Nothing Then Return False
			Dim val = key.GetValue(name, "0")?.ToString()
			Return val = "1"
		End Using
	End Function

	Public Function RegWriteBool(root As RegistryKey, path As String, name As String, value As Boolean) As Boolean
		Try
			Using key As RegistryKey = root.CreateSubKey(path, True)
				key.SetValue(name, If(value, "1", "0"), RegistryValueKind.String)
			End Using
			Return True
		Catch ex As Exception
			Return False
		End Try
	End Function

	'-----------------------------------------------------------------------------------------------
	' Read/Write REG_DWORD
	Public Function RegReadInt(root As RegistryKey, path As String, name As String) As Integer
		Dim r As Integer = -1
		Using key As RegistryKey = root.OpenSubKey(path, False)
			If key IsNot Nothing Then
				r = Convert.ToInt32(key.GetValue(name, Nothing))
			End If
		End Using
		Return r
	End Function

	Public Function RegWriteInt(root As RegistryKey, path As String, name As String, value As Integer) As Boolean
		Try
			Using key As RegistryKey = root.CreateSubKey(path, True)
				key.SetValue(name, value, RegistryValueKind.DWord)
			End Using
			Return True
		Catch ex As Exception
			Return False
		End Try
	End Function

	'-----------------------------------------------------------------------------------------------
	' Read/Write REG_SZ / String
	Public Function RegReadStr(root As RegistryKey, path As String, name As String) As String
		Using key As RegistryKey = root.OpenSubKey(path, False)
			If key Is Nothing Then Return ""
			Dim val = key.GetValue(name, Nothing)
			If val Is Nothing Then Return ""
			Return val.ToString()
		End Using
	End Function

	Public Function RegWriteStr(root As RegistryKey, path As String, name As String, value As String) As Boolean
		Try
			Using key As RegistryKey = root.CreateSubKey(path, True)
				key.SetValue(name, value, RegistryValueKind.String)
			End Using
			Return True
		Catch ex As Exception
			Return False
		End Try
	End Function
End Module
