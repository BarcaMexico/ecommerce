Imports Microsoft.VisualBasic

Public Class License
    Private Sub OSInfo(ByRef OS As String, ByRef OSArchitecture As String, ByRef OSVer As String, ByRef InstallDate As String, ByRef SerialNumber As String)
        Dim searcher As System.Management.ManagementObjectSearcher
        searcher = New System.Management.ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem")
        Dim queryCollection As System.Management.ManagementObjectCollection
        queryCollection = searcher.Get()
        Dim m As System.Management.ManagementObject
        For Each m In queryCollection
            OS = m("Caption")
            OSVer = m("Version")
            Try
                OSArchitecture = m("OSArchitecture")
            Catch ex As Exception
                OSArchitecture = "32 bits"
            End Try
            InstallDate = m("InstallDate")
            SerialNumber = m("SerialNumber")
        Next
    End Sub
    Private Sub HWInfo(ByRef CPUType As String, ByRef NoCPUs As String, ByRef CPUId As String)
        Dim searcher As System.Management.ManagementObjectSearcher
        searcher = New System.Management.ManagementObjectSearcher("SELECT * FROM Win32_Processor")
        Dim queryCollection As System.Management.ManagementObjectCollection
        queryCollection = searcher.Get()
        Dim m As System.Management.ManagementObject
        For Each m In queryCollection
            CPUType = m("Name")
            NoCPUs = m("NumberOfCores")
            CPUId = m("ProcessorId")
        Next
    End Sub


    '//*************************************************************
    '// Decrypt Connection
    '//*************************************************************
    Private Function RC2_Decrypt(ByVal input As String, ByVal pass As String) As String
        Dim RC2 As New System.Security.Cryptography.RC2CryptoServiceProvider
        Dim Hash_RC2 As New System.Security.Cryptography.MD5CryptoServiceProvider
        Dim decrypted As String = ""
        Try
            Dim hash() As Byte = Hash_RC2.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(pass))
            RC2.Key = hash
            RC2.Mode = System.Security.Cryptography.CipherMode.ECB
            Dim DESDecrypter As System.Security.Cryptography.ICryptoTransform = RC2.CreateDecryptor
            Dim Buffer As Byte() = Convert.FromBase64String(input)
            decrypted = System.Text.ASCIIEncoding.ASCII.GetString(DESDecrypter.TransformFinalBlock(Buffer, 0, Buffer.Length))
            'Return decrypted
        Catch ex As Exception
        End Try
        RC2_Decrypt = decrypted
    End Function

    '//*************************************************************
    '// Decrypt Connection
    '//*************************************************************
    Private Function RC2_Encrypt(ByVal input As String, ByVal pass As String) As String
        Dim RC2 As New System.Security.Cryptography.RC2CryptoServiceProvider
        Dim Hash_RC2 As New System.Security.Cryptography.MD5CryptoServiceProvider
        Dim encrypted As String = ""
        Try
            Dim hash() As Byte = Hash_RC2.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(pass))

            RC2.Key = hash
            RC2.Mode = System.Security.Cryptography.CipherMode.ECB
            Dim DESEncrypter As System.Security.Cryptography.ICryptoTransform = RC2.CreateEncryptor
            Dim Buffer As Byte() = System.Text.ASCIIEncoding.ASCII.GetBytes(input)
            encrypted = Convert.ToBase64String(DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length))
            'Return encrypted
        Catch ex As Exception
        End Try
        RC2_Encrypt = encrypted
    End Function

    Public Function HWK(ByVal OSVer As String, ByVal InstallDate As String, ByVal SerialNumber As String, ByVal CPUId As String) As String
        Dim HK As String = ""
        HK = RC2_Encrypt(OSVer & "|" & InstallDate, CPUId & "|" & SerialNumber)
        Return HK
    End Function

    Public Function GDT(ByVal AddOnKey As String, ByVal HardwareKey As String, ByVal Database As String, ByVal UserSBO As String, ByVal DBServer As String, ByVal LicenseKey As String, ByRef Type As String) As String
        Dim Valid As String = "Licensed"
        'Dim Datos As String = ""
        'Type = "X"
        'Try
        '    If LicenseKey <> "" Then
        '        Datos = RC2_Decrypt(LicenseKey, HardwareKey & "|" & UserSBO.ToUpper)
        '        Dim DS As String = Datos.Substring(0, Datos.IndexOf("|"))
        '        Dim L1 As String = Datos.Substring(Datos.IndexOf("|") + 1, Datos.Length() - Datos.IndexOf("|") - 1)
        '        Dim db As String = L1.Substring(0, L1.IndexOf("|"))
        '        Dim L2 As String = L1.Substring(L1.IndexOf("|") + 1, L1.Length() - L1.IndexOf("|") - 1)
        '        Dim AK As String = L2.Substring(0, L2.IndexOf("|"))
        '        Dim L3 As String = L2.Substring(L2.IndexOf("|") + 1, L2.Length() - L2.IndexOf("|") - 1)
        '        Dim fi As String = L3.Substring(0, L3.IndexOf("|"))
        '        Dim L4 As String = L3.Substring(L3.IndexOf("|") + 1, L3.Length() - L3.IndexOf("|") - 1)
        '        Dim ff As String = L4.Substring(0, L4.IndexOf("|"))
        '        Dim ty As String = L4.Substring(L4.IndexOf("|") + 1, L4.Length() - L4.IndexOf("|") - 1)
        '        If DS = DBServer.ToUpper And db = Database.ToUpper And AK = AddOnKey And (Today >= Convert.ToDateTime(fi) And Today <= Convert.ToDateTime(ff)) Then
        '            Valid = "Licensed"
        '            Type = ty
        '        End If
        '    Else
        '        Valid = "Invalid"
        '    End If
        'Catch ex As Exception
        '    Valid = "Invalid"
        'End Try
        Return Valid
    End Function
    Public Function Licenses() As String

      




        Dim HardwareKey, AddOnName, AddOnDescription, Hostname, Database, IPA, IPB As String
        'Dim ILic As ILicenses.Service
        'ILic = New ILicenses.Service
        'Dim Url_IL As String = "c:\Windows\Temp\IL.ini"
        'If System.IO.File.Exists(Url_IL) Then
        '    Url_IL = System.IO.File.ReadAllText(Url_IL)
        '    ILic.Url = Url_IL
        'Else
        '    'Default
        '    ILic.Url = "http://soportesap.interlatin.com.mx:81/ILicense/ILicenser.asmx"
        'End If
        Dim licenseserver, dbserver, dbname, dbtype, dbusername, dbuserpass, dbcompanyuser, dbcompanypass, lenguaje, OSver, InstallDate, SerialNumber, CPUId, AddOnKey, LicenseKey, LicenseType, IsValid, OS, OSArchitecture, CPUType, NoCPUs As String



        dbserver = System.Web.Configuration.WebConfigurationManager.AppSettings("dbserver")
        dbname = System.Web.Configuration.WebConfigurationManager.AppSettings("dbname")
        dbtype = System.Web.Configuration.WebConfigurationManager.AppSettings("dbtype")
        dbusername = System.Web.Configuration.WebConfigurationManager.AppSettings("dbusername")
        dbuserpass = System.Web.Configuration.WebConfigurationManager.AppSettings("dbuserpass")
        dbcompanyuser = System.Web.Configuration.WebConfigurationManager.AppSettings("dbcompanyuser")
        dbcompanypass = System.Web.Configuration.WebConfigurationManager.AppSettings("dbcompanypass")
        lenguaje = System.Web.Configuration.WebConfigurationManager.AppSettings("lenguaje")
        licenseserver = System.Web.Configuration.WebConfigurationManager.AppSettings("licenseserver")

         


        AddOnKey = "ECOMV1.0-2016-05-LDG"
        AddOnName = "E-Commerce"
        AddOnDescription = "e-Commerce for IIS."
        Database = dbname
        IsValid = "Activa"
        OSver = ""
        InstallDate = ""
        SerialNumber = ""
        CPUId = ""
        LicenseType = ""
        HardwareKey = ""
        OSArchitecture = ""
        OS = ""
        CPUType = ""
        NoCPUs = ""
        Hostname = System.Net.Dns.GetHostName()
        OSInfo(OS, OSArchitecture, OSver, InstallDate, SerialNumber)
        HWInfo(CPUType, NoCPUs, CPUId)
        'Try
        '    IPA = System.Net.Dns.GetHostEntry(Hostname).AddressList(0).ToString()
        'Catch ex As Exception
        '    IPA = "NONE"
        'End Try
        'Try
        '    IPB = System.Net.Dns.GetHostEntry(Hostname).AddressList(1).ToString()
        'Catch ex As Exception
        '    IPB = "NONE"
        'End Try
        'Dim Path As String = IO.Path.GetTempPath() & "License.ini"
        'LicenseType = ""
        'If System.IO.File.Exists(Path) Then
        '    HardwareKey = HWK(OSver, InstallDate, SerialNumber, CPUId)
        '    LicenseKey = System.IO.File.ReadAllText(Path)
        '    IsValid = GDT(AddOnKey, HardwareKey, Database, "ECOM", Hostname, LicenseKey, LicenseType)
        '    If IsValid = "Invalid" Then
        '        If System.IO.File.Exists(Path) Then
        '            System.IO.File.Delete(Path)
        '        End If
        '        HardwareKey = ILic.HWK(OSver, InstallDate, SerialNumber, CPUId)
        '        LicenseKey = ILic.GLI(AddOnKey, HardwareKey, Database, "ECOM", Hostname)
        '        IsValid = ILic.GDT(AddOnKey, HardwareKey, Database, "ECOM", Hostname, LicenseKey, LicenseType)
        '    End If
        'Else
        '    HardwareKey = ILic.HWK(OSver, InstallDate, SerialNumber, CPUId)
        '    LicenseKey = ILic.GLI(AddOnKey, HardwareKey, Database, "ECOM", Hostname)
        '    IsValid = ILic.GDT(AddOnKey, HardwareKey, Database, "ECOM", Hostname, LicenseKey, LicenseType)
        'End If
        'Select Case IsValid
        '    Case "Licensed"
        '        If Not System.IO.File.Exists(Path) Then
        '            System.IO.File.WriteAllText(Path, LicenseKey)
        '        End If
        '        IsValid = "Activa"
        '    Case "Fixed"
        '        If Not System.IO.File.Exists(Path) Then
        '            System.IO.File.WriteAllText(Path, LicenseKey)
        '        End If
        '        IsValid = "Activa"
        '    Case "Blocked"
        '        If System.IO.File.Exists(Path) Then
        '            System.IO.File.Delete(Path)
        '        End If
        '        IsValid = "Inactiva"
        '    Case "Invalid"
        '        If System.IO.File.Exists(Path) Then
        '            System.IO.File.Delete(Path)
        '        End If
        '        ILic.HWS(HardwareKey, Environment.UserDomainName.ToUpper, Environment.UserName, Hostname, OS, OSver, "", OSArchitecture, InstallDate, SerialNumber, CPUType, NoCPUs, CPUId, System.Math.Round(My.Computer.Info.TotalPhysicalMemory / (1024 * 1024)) & " MB", IPA, IPB)
        '        ILic.COMS(HardwareKey, Database, "9.0", Database, "ECOM", "ECOM")
        '        ILic.SRVR(HardwareKey, Hostname, dbusername, dbtype, licenseserver)
        '        ILic.USRS(HardwareKey, Database, "ECOM", Hostname, Environment.UserName, Hostname, IPA, IPB, Environment.UserName)
        '        ILic.ADDN(AddOnKey, AddOnName, AddOnDescription)
        '        ILic.ASSG(AddOnKey, HardwareKey, Database, "ECOM", Hostname)
        '        IsValid = "Inactiva"
        'End Select
        'TextBox3.Text = IsValid
        Return IsValid
    End Function
End Class
