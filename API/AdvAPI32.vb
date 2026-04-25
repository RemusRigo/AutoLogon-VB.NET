'--------------------------------------------------------------------------------------------------
' AdvAPI32.dll - Advanced Windows API functions related to security and policy management.
'    © 2026 Remus Rigo
'       v1.1 2026-03-27
'--------------------------------------------------------------------------------------------------

Imports System.Runtime.InteropServices

Module AdvAPI32

   Friend Const POLICY_ALL_ACCESS As UInteger = &HF0FFF


   <StructLayout(LayoutKind.Sequential)>
   Friend Structure LSA_UNICODE_STRING
      Public Length As UShort
      Public MaximumLength As UShort
      Public Buffer As IntPtr
   End Structure

   <StructLayout(LayoutKind.Sequential)>
   Friend Structure LSA_OBJECT_ATTRIBUTES
      Public Length As UInteger
      Public RootDirectory As IntPtr
      Public ObjectName As IntPtr
      Public Attributes As UInteger
      Public SecurityDescriptor As IntPtr
      Public SecurityQualityOfService As IntPtr
   End Structure

   <DllImport("advapi32.dll", SetLastError:=True)>
   Friend Function LsaOpenPolicy(ByRef SystemName As LSA_UNICODE_STRING, ByRef ObjectAttributes As LSA_OBJECT_ATTRIBUTES, AccessMask As UInteger,
                                 ByRef PolicyHandle As IntPtr) As UInteger
   End Function

   <DllImport("advapi32.dll", SetLastError:=True)>
   Friend Function LsaStorePrivateData(PolicyHandle As IntPtr, ByRef KeyName As LSA_UNICODE_STRING, ByRef PrivateData As LSA_UNICODE_STRING) As UInteger
   End Function

   <DllImport("advapi32.dll", SetLastError:=True)>
   Friend Function LsaRetrievePrivateData(PolicyHandle As IntPtr, ByRef KeyName As LSA_UNICODE_STRING, ByRef PrivateData As IntPtr) As UInteger
   End Function

   <DllImport("advapi32.dll")>
   Friend Function LsaClose(ObjectHandle As IntPtr) As UInteger
   End Function

   <DllImport("advapi32.dll")>
   Friend Function LsaFreeMemory(Buffer As IntPtr) As UInteger
   End Function



End Module
