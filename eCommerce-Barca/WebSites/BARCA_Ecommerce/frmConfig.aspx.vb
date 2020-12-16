Imports Modulo

Imports System.Xml
Imports System.Globalization
Imports System.IO

Partial Class frmConfig
    Inherits System.Web.UI.Page
    Public ws As DIS.DIServer
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try

     

        If Session("colorTop") <> "" Then

            estilo.InnerText = "#page-wrapper { background:" & Session("colorContenido") & ";} #barratop{ background:" & Session("colorTop") & ";} .navbar-top-links {background: " & Session("colorTop") & ";}    "
            estilo.InnerText += ".infousu{background: " & Session("colorInfo") & ";} body { background :" & Session("colorMenu") & " } .nav{ background :inherit }"

        End If

        Catch ex As Exception

        End Try

        If Not Page.IsPostBack Then


            Color0.Text = Session("colorTop")
            Color1.Text = Session("colorInfo")
            Color2.Text = Session("colorMenu")
            Color3.Text = Session("colorContenido")




            Dim tRow As New TableRow()
            Dim tCell As New TableCell()

            ws = New DIS.DIServer



            Try

                Dim Respuesta As XmlNode


                Dim sqldato As String = "select top 1 U_arttype as tipo,U_artcan as cantidad ,rutaxml,rutapdf,tipodoc,[smtp],[puerto],[username], [acount],[password],[ssl],usumensajes ,usualertas from [Ecom].[dbo].[@IL_CONFIG_E]   "
                Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
                Session("configarttype") = ReadXML(Respuesta.InnerXml, "tipo")
                Session("configartcan") = ReadXML(Respuesta.InnerXml, "cantidad")
                Session("rutapdf") = ReadXML(Respuesta.InnerXml, "rutaxml")
                Session("rutaxml") = ReadXML(Respuesta.InnerXml, "rutapdf")
                Session("tipodocventas") = ReadXML(Respuesta.InnerXml, "tipodoc")
                Session("usuMensajes") = ReadXML(Respuesta.InnerXml, "usumensajes")
                Session("usuAlertas") = ReadXML(Respuesta.InnerXml, "usualertas")

                Session("smtp") = ReadXML(Respuesta.InnerXml, "smtp")
                Session("puerto") = ReadXML(Respuesta.InnerXml, "puerto")
                Session("username") = ReadXML(Respuesta.InnerXml, "username")
                Session("acount") = ReadXML(Respuesta.InnerXml, "acount")
                Session("password") = ReadXML(Respuesta.InnerXml, "password")
                Session("ssl") = ReadXML(Respuesta.InnerXml, "ssl")



                txtUsuAlertas.Value = Session("usuAlertas")
                txtUsuMensajes.Value = Session("usuMensajes")
                inputpdf.Value = Session("rutapdf")
                inputxml.Value = Session("rutaxml")
                DropDownList1.SelectedValue = Session("configarttype")
                DropDownList2.SelectedValue = Session("configartcan")
                DropDownList3.SelectedValue = Session("tipodocventas")
                DropDownList4.SelectedValue = Session("ssl")
                inputSMTP.Value = Session("smtp")
                inputPUERTO.Value = Session("puerto")
                inputUsername.Value = Session("username")
                inputAcount.Value = Session("acount")
                inputPassword.Value = Session("password")






            Catch ex As Exception
            End Try
        End If

    End Sub

    'Private Function ReadXML(Xml As String, NodeName As String)
    '    Dim reader As System.Xml.XmlTextReader = New System.Xml.XmlTextReader(New System.IO.StringReader(Xml))
    '    Dim valor As String
    '    valor = ""
    '    Do While (reader.Read())
    '        If reader.NodeType = System.Xml.XmlNodeType.Element Then
    '            If reader.Name.ToUpper = NodeName.ToUpper Then
    '                valor = reader.ReadElementContentAsString
    '            End If
    '        End If
    '    Loop
    '    reader.Close()
    '    reader.Dispose()
    '    Return valor
    'End Function

    Protected Sub confirmar_Click(sender As Object, e As EventArgs) Handles confirmar.Click


        'Busqueda de facturas venciadas Balance
        Dim tRow As New TableRow()
        Dim tCell As New TableCell()

        ws = New DIS.DIServer



        Try

          

            Dim Respuesta As XmlNode
            Dim sqldato As String = "update [Ecom].[dbo].[@IL_CONFIG_E] set rutaxml='" & inputxml.Value & "', rutapdf='" & inputpdf.Value & "', U_arttype='" & DropDownList1.SelectedValue & "',U_artcan='" & DropDownList2.SelectedValue & "',tipodoc='" & DropDownList3.SelectedValue & "' ,usumensajes ='" & txtUsuMensajes.Value & "',usualertas='" & txtUsuAlertas.Value & "' "
            sqldato = sqldato & " ,ssl='" & DropDownList4.SelectedValue & "' "
            sqldato = sqldato & " ,smtp='" & inputSMTP.Value & "' "
            sqldato = sqldato & " ,puerto='" & inputPUERTO.Value & "' "
            sqldato = sqldato & " ,username='" & inputUsername.Value & "' "
            sqldato = sqldato & " ,acount='" & inputAcount.Value & "' "
            sqldato = sqldato & " ,password='" & inputPassword.Value & "' "
            
            Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
            Dim fn As String = System.IO.Path.GetFileName(File1.PostedFile.FileName)
            Dim bmp As System.Drawing.Bitmap
            Dim ms As System.IO.MemoryStream
            Dim byteimage() As Byte
            Dim imgstring As String = ""
            Dim idunico As String = Guid.NewGuid().ToString()

            If File1.PostedFile.FileName <> "" Then
                bmp = New System.Drawing.Bitmap(File1.PostedFile.InputStream)
                ms = New System.IO.MemoryStream
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
                byteimage = ms.ToArray()
                imgstring = Convert.ToBase64String(byteimage)
                sqldato = "select count(id) as existe from [Ecom].[dbo].[color]  "
                Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
                If ReadXML(Respuesta.InnerXml, "existe") = "0" Then
                    sqldato = "INSERT INTO [Ecom].[dbo].[color] ( [barratop] ,[infousu] ,[menu] ,[contenido],[img]) TextS('" & Color0.Text & "','" & Color1.Text & "' ,'" & Color2.Text & "' ,'" & Color3.Text & "','" & imgstring & "') "
                    Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
                Else
                    sqldato = "UPDATE [Ecom].[dbo].[color] SET img='" & imgstring & "', barratop= '" & Color0.Text & "', infousu= '" & Color1.Text & "' ,menu= '" & Color2.Text & "',contenido= '" & Color3.Text & "'  "
                    Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
                End If
                Session("logo") = imgstring
            Else
                sqldato = "select count(id) as existe from [Ecom].[dbo].[color]  "
                Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
                If ReadXML(Respuesta.InnerXml, "existe") = "0" Then
                    sqldato = "INSERT INTO [Ecom].[dbo].[color] ( [barratop] ,[infousu] ,[menu] ,[contenido] ) TextS('" & Color0.Text & "','" & Color1.Text & "' ,'" & Color2.Text & "' ,'" & Color3.Text & "') "
                    Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
                Else
                    sqldato = "UPDATE [Ecom].[dbo].[color] SET  barratop= '" & Color0.Text & "', infousu= '" & Color1.Text & "' ,menu= '" & Color2.Text & "',contenido= '" & Color3.Text & "'  "
                    Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
                End If
            End If



            'sqldato = "select count(id) as existe from [Ecom].[dbo].[color]  "
            'Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
            'If ReadXML(Respuesta.InnerXml, "existe") = "0" Then
            '    sqldato = "INSERT INTO [Ecom].[dbo].[color] ( [barratop] ,[infousu] ,[menu] ,[contenido],[img]) VALUES('" & Color0.Value & "','" & Color1.Value & "' ,'" & Color2.Value & "' ,'" & Color3.Value & "','" & imgstring & "') "
            '    Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
            'Else
            '    sqldato = "UPDATE [Ecom].[dbo].[color] SET img='" & imgstring & "', barratop= '" & Color0.Value & "', infousu= '" & Color1.Value & "' ,menu= '" & Color2.Value & "',contenido= '" & Color3.Value & "'  "
            '    Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
            'End If
            ' pagewrapper.Style("background") = Color3.Value

            Session("colorTop") = Color0.Text
            Session("colorInfo") = Color1.Text
            Session("colorMenu") = Color2.Text
            Session("colorContenido") = Color3.Text

            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert('Configuracion Guardada');document.location.href='frmConfig';", True)

        Catch ex As Exception
        End Try


    End Sub

    'Protected Sub DESCARGAPRUEBA_Click(sender As Object, e As EventArgs) Handles DESCARGAPRUEBA.Click
    '    Dim filePath As String = "C:\\export\029BF843-7E57-4436-B3BD-9CF06672C55E.xml"
    '    Dim File = New FileInfo(filePath)
    '    If File.Exists Then

    '         Response.Clear() 
    '        Response.ClearHeaders()
    '        Response.ClearContent()
    '        Response.AddHeader("Content-Disposition", "attachment; filename=" + File.Name)
    '        Response.AddHeader("Content-Length", File.Length.ToString())
    '        'Response.ContentType = "text/plain" 
    '        Response.Flush()
    '        Response.TransmitFile(File.FullName)
    '        Response.End()
    '    End If 
    'End Sub

    
    Protected Sub PruebaEnvioMail_Click(sender As Object, e As EventArgs) Handles PruebaEnvioMail.Click
        Dim dilon As String = "Envio Correcto"
        Try
            Dim oMsg As CDO.Message = New CDO.Message()
            Dim iConfg As CDO.Configuration
            Dim oFields As ADODB.Fields
            Dim oField As ADODB.Field
            iConfg = oMsg.Configuration
            oFields = iConfg.Fields

            'Canal de comunicacion
            oField = oFields("http://schemas.microsoft.com/cdo/configuration/sendusing")
            oField.Value = CDO.CdoSendUsing.cdoSendUsingPort
            '1: cdoSendUsingPickup 2: cdoSendUsingPort

            'IP del servidor SMTP
            oField = oFields("http://schemas.microsoft.com/cdo/configuration/smtpserver")
            oField.Value = inputSMTP.Value

            'Tiempo de espera
            oField = oFields("http://schemas.microsoft.com/cdo/configuration/smtpconnectiontimeout")
            oField.Value = 20 'Segs

            'Puerto del SMTP
            oField = oFields("http://schemas.microsoft.com/cdo/configuration/smtpserverport")
            oField.Value = inputPUERTO.Value   '585 TCP/IP

            'Tipo de autenticacion
            oField = oFields("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate")
            oField.Value = CDO.CdoProtocolsAuthentication.cdoBasic

            'Nombre de usuario
            oField = oFields("http://schemas.microsoft.com/cdo/configuration/sendusername")
            oField.Value = inputUsername.Value

            'Contraseña
            oField = oFields("http://schemas.microsoft.com/cdo/configuration/sendpassword")
            oField.Value = inputPassword.Value

            'Utiliza SSL
            oField = oFields("http://schemas.microsoft.com/cdo/configuration/smtpusessl")


            If DropDownList4.SelectedValue = "SI" Then
                oField.Value = True
            Else
                oField.Value = False
            End If

            'Actualizamos los datos de la cuenta SMTP
            oFields.Update()

            oMsg.Configuration = iConfg
            oMsg.Subject = "Prueba"
            '1) Solo texto
            oMsg.HTMLBody = "Configuracion Correcta"
            '2) Correo en HTML
            'oMsg.HTMLBody = "<body></body>"

            'Cuentas de Email
            oMsg.ReplyTo = Session("acount")
            oMsg.To = Session("acount")
            oMsg.From = Session("acount")



            'Adjuntos
            'Dim Attchs As CDO.IBodyPart
            'Attchs = oMsg.AddAttachment(adjXML)
            'Attchs = oMsg.AddAttachment(adjPDF)
            'Attchs = oMsg.AddAttachment("c:\Windows\Factura.pdf")

            'Enviamos el correo
            oMsg.Send()

            'Limpiamos las variables
            oMsg = Nothing
            iConfg = Nothing
            oFields = Nothing
            oField = Nothing
            ' Return "Envio Correcto"
            Session("errormail") = Nothing
            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrta", "alert('Envio Correcto');document.location.href='frmConfig'; ", True)
        Catch ex As Exception
            'ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert('" & ex.Message & "');document.location.href='frmConfig';", True)

            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrta", "alert('" & Replace(ex.Message, "'", "") & "');document.location.href='frmConfig'; ", True)

            'Return ex.Message
        End Try
         
    End Sub


End Class
