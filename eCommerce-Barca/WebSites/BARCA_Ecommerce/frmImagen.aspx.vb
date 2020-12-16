Imports Modulo

Imports System.Xml
Imports System.Globalization

Imports System.IO
Imports System.Net
Imports System.Text


Partial Class frmImagen
    Inherits System.Web.UI.Page
    Public ws As DIS.DIServer
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            If Session("Imagen") = Session("ftp") Then
                imagenperfil.Src = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAhIAAAISCAMAAACu49aNAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMC1jMDYwIDYxLjEzNDc3NywgMjAxMC8wMi8xMi0xNzozMjowMCAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNSBNYWNpbnRvc2giIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6MEQyQzNERDE0QTgzMTFFMUI0N0FDQ0U5NDgzQTI2NEYiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6MEQyQzNERDI0QTgzMTFFMUI0N0FDQ0U5NDgzQTI2NEYiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDowRDJDM0RDRjRBODMxMUUxQjQ3QUNDRTk0ODNBMjY0RiIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDowRDJDM0REMDRBODMxMUUxQjQ3QUNDRTk0ODNBMjY0RiIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/PmY2mEcAAAGAUExURevr7Jydn5ma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"

            Else

                imagenperfil.Src = Session("Imagen")


            End If



            ''Busqueda de facturas venciadas Balance
            'Dim tRow As New TableRow()
            'Dim tCell As New TableCell()

            'ws = New DIS.DIServer
            'ws.Url = "http://localhost/SAP/DIServer.asmx"


            'Try

            '    Dim Respuesta As XmlNode

            '    Dim sqldato As String = "  select U_art,U_cantidad,U_observacion,U_imagen,Code from  [@IL_DISCREPANCIA] where U_factura='" & Session("DocNumDis") & "' and U_status='1'  "
            '    Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)


            '    ReadXML(Respuesta.InnerXml, "U_art")



            'Catch ex As Exception
            'End Try



        End If

    End Sub


    Protected Sub secretbutton_ServerClick(sender As Object, e As EventArgs) Handles secretbutton.ServerClick




    End Sub





    Private Function ReadXML(Xml As String, NodeName As String)
        Dim reader As System.Xml.XmlTextReader = New System.Xml.XmlTextReader(New System.IO.StringReader(Xml))
        Dim valor As String
        valor = ""
        Do While (reader.Read())
            If reader.NodeType = System.Xml.XmlNodeType.Element Then
                If reader.Name.ToUpper = NodeName.ToUpper Then
                    valor = reader.ReadElementContentAsString
                End If
            End If
        Loop
        reader.Close()
        reader.Dispose()
        Return valor
    End Function

    Public Overloads Shared Function ResizeImage(bmSource As Drawing.Bitmap, TargetWidth As Int32, TargetHeight As Int32) As Drawing.Bitmap
        Dim bmDest As New Drawing.Bitmap(TargetWidth, TargetHeight, Drawing.Imaging.PixelFormat.Format32bppArgb)

        Dim nSourceAspectRatio = bmSource.Width / bmSource.Height
        Dim nDestAspectRatio = bmDest.Width / bmDest.Height

        Dim NewX = 0
        Dim NewY = 0
        Dim NewWidth = bmDest.Width
        Dim NewHeight = bmDest.Height

        If nDestAspectRatio = nSourceAspectRatio Then
            'same ratio
        ElseIf nDestAspectRatio > nSourceAspectRatio Then
            'Source is taller
            NewWidth = Convert.ToInt32(Math.Floor(nSourceAspectRatio * NewHeight))
            NewX = Convert.ToInt32(Math.Floor((bmDest.Width - NewWidth) / 2))
        Else
            'Source is wider
            NewHeight = Convert.ToInt32(Math.Floor((1 / nSourceAspectRatio) * NewWidth))
            NewY = Convert.ToInt32(Math.Floor((bmDest.Height - NewHeight) / 2))
        End If

        Using grDest = Drawing.Graphics.FromImage(bmDest)
            With grDest
                .CompositingQuality = Drawing.Drawing2D.CompositingQuality.HighQuality
                .InterpolationMode = Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
                .PixelOffsetMode = Drawing.Drawing2D.PixelOffsetMode.HighQuality
                .SmoothingMode = Drawing.Drawing2D.SmoothingMode.AntiAlias
                .CompositingMode = Drawing.Drawing2D.CompositingMode.SourceOver

                .DrawImage(bmSource, NewX, NewY, NewWidth, NewHeight)
            End With
        End Using

        Return bmDest
    End Function

    Protected Sub subirr_ServerClick(sender As Object, e As EventArgs) Handles subirr.ServerClick






        '    Dim misdatos As String = "ftp://192.168.52.105/" & File1.PostedFile.FileName

        ''FTP Server URL.
        'Dim ftp As String = "ftp://192.168.52.105/"

        ''FTP Folder name. Leave blank if you want to upload to root folder.
        'Dim ftpFolder As String = "/"

        'Dim fileBytes As Byte() = Nothing

        ''Read the FileName and convert it to Byte array.
        'Dim fileName As String = Path.GetFileName(File1.PostedFile.FileName)
        'Using fileStream As New StreamReader(File1.PostedFile.InputStream)
        '    fileBytes = Encoding.UTF8.GetBytes(fileStream.ReadToEnd())
        '    fileStream.Close()
        'End Using

        'Try
        '    'Create FTP Request.
        '    Dim request As FtpWebRequest = DirectCast(WebRequest.Create(ftp & ftpFolder & fileName), FtpWebRequest)
        '    request.Method = WebRequestMethods.Ftp.UploadFile

        '    'Enter FTP Server credentials.
        '    'request.Credentials = New NetworkCredential("UserName", "Password")
        '    request.ContentLength = fileBytes.Length
        '    request.UsePassive = True
        '    request.UseBinary = True
        '    request.ServicePoint.ConnectionLimit = fileBytes.Length
        '    request.EnableSsl = False

        '    Using requestStream As Stream = request.GetRequestStream()
        '        requestStream.Write(fileBytes, 0, fileBytes.Length)
        '        requestStream.Close()
        '    End Using

        '    Dim response As FtpWebResponse = DirectCast(request.GetResponse(), FtpWebResponse)


        '    response.Close()
        'Catch ex As WebException
        '    'Throw New Exception(TryCast(ex.Response, FtpWebResponse).StatusDescription)
        'End Try
        'Dim unicoID As String = Guid.NewGuid().ToString() & System.IO.Path.GetExtension(File1.PostedFile.FileName)

        'Try 
        '    Dim request As System.Net.FtpWebRequest = DirectCast(System.Net.WebRequest.Create(Session("ftp") & unicoID), System.Net.FtpWebRequest)
        ''request.Credentials = New System.Net.NetworkCredential("user", "password")
        'request.Method = System.Net.WebRequestMethods.Ftp.UploadFile 
        'Dim s As IO.Stream = File1.PostedFile.InputStream
        'Dim b(s.Length) As Byte
        's.Read(b, 0, s.Length)
        'Dim c(Encoding.UTF8.GetCharCount(b)) As Char
        'Encoding.UTF8.GetChars(b, 0, s.Length, c, 0) 
        'Dim strz As System.IO.Stream = request.GetRequestStream()
        'strz.Write(b, 0, b.Length)
        'strz.Close()
        'strz.Dispose()

        'Catch ex As Exception

        'End Try


        Dim fn As String = System.IO.Path.GetFileName(File1.PostedFile.FileName)

        Dim bmp As System.Drawing.Bitmap
        Dim ms As System.IO.MemoryStream
        Dim byteimage() As Byte
        Dim imgstring As String = ""
        Dim idunico As String = Guid.NewGuid().ToString()




        'Try

        '    If File1.PostedFile.ContentLength <> 0 Then

        Try
            'File1.PostedFile.SaveAs(Server.MapPath("/imagenes/") + idunico + System.IO.Path.GetExtension(File1.PostedFile.FileName))
            'imagenperfil.Src = "file:///" + Server.MapPath("/imagenes/") + idunico + System.IO.Path.GetExtension(File1.PostedFile.FileName)
            'Session("Imagen") = "file:///" + Server.MapPath("/imagenes/") + idunico + System.IO.Path.GetExtension(File1.PostedFile.FileName)
            bmp = New System.Drawing.Bitmap(File1.PostedFile.InputStream)

            Dim ratioX = 480 / bmp.Width
            Dim ratioY = 480 / bmp.Height
            Dim ratio = Math.Min(ratioX, ratioY)

            Dim newWidth = (bmp.Width * ratio)
            Dim newHeight = (bmp.Height * ratio)
            bmp = New System.Drawing.Bitmap(bmp, New Drawing.Size(newWidth, newHeight))


            ms = New System.IO.MemoryStream
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
            byteimage = ms.ToArray()
            imgstring = Convert.ToBase64String(byteimage)






            ws = New DIS.DIServer



            Dim Respuesta As XmlNode
            Dim ahora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            Dim sqldato As String = ""



 
                sqldato = "select count(id) as existe from [Ecom].[dbo].[perfil] where usu='" & Session("usuCode") & "'"
                Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
                If ReadXML(Respuesta.InnerXml, "existe") = "0" Then
                    sqldato = "INSERT INTO [Ecom].[dbo].[perfil] ([tipo], [usu] ,[img]) VALUES('" & Session("usutipo") & "','" & Session("usuCode") & "' ,'" & imgstring & "') "
                    Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
                Else
                    sqldato = "UPDATE [Ecom].[dbo].[perfil] SET img= '" & imgstring & "' where usu='" & Session("usuCode") & "'"
                    Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
                End If 
            Session("Imagen") = "data:image/png;base64," & imgstring
            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert('Imagen guardada');document.location.href='frmImagen';", True)




        Catch ex As Exception

        End Try




        'Try
        '    If File1.PostedFile.ContentLength <> 0 Then
        '        ws = New DIS.DIServer
        '        ws.Url = "http://localhost/SAP/DIServer.asmx"


        '        Dim Respuesta As XmlNode
        '        Dim ahora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        '        Dim sqldato As String = ""
        '        If Session("usutipo") = "cliente" Then

        '            sqldato = "insert into [@IL_IMGS] (Code,Name,U_codigo ,U_tipo ,U_imagen,u_fecha )values((select COUNT(*)+1  from  [@IL_IMGS] ),(select COUNT(*)+1  from  [@IL_IMGS] ),'" & Session("usuCode") & "','cliente','" & unicoID & "','" & ahora & "') "
        '            Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
        '            'sqldato = "update OCRD set U_IL_imagen='data:image/png;base64," & imgstring & "' where CardCode='" & Session("usuCode") & "'"
        '            'Respuesta = ws.UpdateObject(Session("Token"), "oBusinessPartners", "<CardCode>" & Session("usuCode") & "</CardCode>", "<BusinessPartners><row><U_IL_imagen>" & imgstring & "</U_IL_imagen></row></BusinessPartners>", "")

        '        End If

        '        If Session("usutipo") = "venta" Then
        '            sqldato = "insert into [@IL_IMGS] (Code,Name,U_codigo ,U_tipo ,U_imagen,u_fecha )values((select COUNT(*)+1  from  [@IL_IMGS] ),(select COUNT(*)+1  from  [@IL_IMGS] ),'" & Session("usuCode") & "','venta','" & unicoID & "','" & ahora & "') "
        '            Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)

        '            'sqldato = "update OSLP set U_IL_imagen='data:image/png;base64," & imgstring & "' where SlpCode='" & Session("usuCode") & "'"
        '            ' Respuesta = ws.UpdateObject(Session("Token"), "oSalesPersons", "<SalesEmployeeCode>" & Session("usuCode") & "</SalesEmployeeCode>", "<SalesPersons><row><U_IL_imagen>" & imgstring & "</U_IL_imagen></row></SalesPersons>", "")
        '        End If

        '        If Session("usutipo") = "admin" Then
        '            sqldato = "insert into [@IL_IMGS] (Code,Name,U_codigo ,U_tipo ,U_imagen,u_fecha )values((select COUNT(*)+1  from  [@IL_IMGS] ),(select COUNT(*)+1  from  [@IL_IMGS] ),'" & Session("usuCode") & "','admin','" & unicoID & "','" & ahora & "') "
        '            Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)

        '            'sqldato = "update OSLP set U_IL_imagen='data:image/png;base64," & imgstring & "' where SlpCode='" & Session("usuCode") & "'"
        '            ' Respuesta = ws.UpdateObject(Session("Token"), "oSalesPersons", "<SalesEmployeeCode>" & Session("usuCode") & "</SalesEmployeeCode>", "<SalesPersons><row><U_IL_imagen>" & imgstring & "</U_IL_imagen></row></SalesPersons>", "")
        '        End If

        '        Session("Imagen") = Session("ftp") + unicoID
        '        ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert('Imagen guardada');document.location.href='frmImagen';", True)


        '    Else

        '        ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert('seleccione una imagen valida');document.location.href='frmImagen';", True)

        '    End If



        'Catch ex As Exception

        'End Try


    End Sub






End Class
