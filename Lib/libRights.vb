Imports System.Security.Principal

Public Module libRights

   Public Function IsAdministrator() As Boolean
      Dim identity = WindowsIdentity.GetCurrent()
      Dim principal = New WindowsPrincipal(identity)
      Return principal.IsInRole(WindowsBuiltInRole.Administrator)
   End Function

End Module

