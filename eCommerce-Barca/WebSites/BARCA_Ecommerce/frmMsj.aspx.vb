Imports Modulo

Imports System.Xml
Imports System.Globalization

Partial Class frmMsj
    Inherits System.Web.UI.Page
    Public ws As DIS.DIServer
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then 
            'Busqueda de facturas venciadas Balance
            Dim tRow As New TableRow()
            Dim tCell As New TableCell()
            ws = New DIS.DIServer

            Try 
                Dim Respuesta As XmlNode 
                Dim sqldato As String = " select  COUNT(*) as total from [Ecom].[dbo].[@IL_MENSAJES] where CONVERT(NVARCHAR(MAX), U_cadena)= '" & Session("msj") & "'  "
                Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
                Dim totalrows As String = ReadXML(Respuesta.InnerXml, "total") 
                Dim twoDarray(CInt(totalrows), 10) As String 
                sqldato = " select   U_fecha,  U_asunto,U_mensaje as msj,U_cadena ,U_estado,U_emisor as edo from [Ecom].[dbo].[@IL_MENSAJES] where CONVERT(NVARCHAR(MAX), U_cadena)='" & Session("msj") & "' order by U_fecha ASC "
                 
                Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)  
                Dim doc2 As New XmlDocument()
                doc2.LoadXml(Respuesta.InnerXml)
                System.Diagnostics.Debug.Write(Respuesta.InnerXml & vbCrLf)
                Dim root2 As XmlNode = doc2.FirstChild
                Dim rootin As XmlNode

                root2 = root2.LastChild
                'root2 = root2.FirstChild
                  
                If root2.HasChildNodes Then
                    Dim s As Integer
                    For s = 0 To root2.ChildNodes.Count - 1 
                        rootin = root2.ChildNodes(s) 
                        If rootin.HasChildNodes Then
                            Dim x As Integer
                            tRow = New TableRow()
                            Session("fcsi") = 0
                            For x = 0 To rootin.ChildNodes.Count - 1
                                Select Case rootin.ChildNodes(x).Name
                                    Case "U_fecha"
                                        twoDarray(s, x) = rootin.ChildNodes(x).InnerText
                                    Case "U_asunto"
                                        twoDarray(s, x) = rootin.ChildNodes(x).InnerText
                                    Case "msj"
                                        twoDarray(s, x) = rootin.ChildNodes(x).InnerText
                                    Case "edo"
                                        twoDarray(s, x) = rootin.ChildNodes(x).InnerText
                                End Select
                            Next x
                        End If
                    Next s
                End If
                Dim divs As String = ""
                For v = 0 To CInt(totalrows) 
                    If twoDarray(v, 0) = Nothing Then 
                    Else
                        If twoDarray(v, 5) = Session("RazCode") Then
                            divs = divs + " <div class='bubble bubble-alt green'><p>" & twoDarray(v, 1) & "</p><Br><p>" & twoDarray(v, 2) & "</p></div>"
                        Else
                            divs = divs + " <div class='bubble'><p>" & twoDarray(v, 1) & "</p><Br><p>" & twoDarray(v, 2) & "</p></div>" 
                        End If
                    End If
                Next v
                mensajesapliados.InnerHtml = divs 
            Catch ex As Exception
            End Try
        End If 
    End Sub


    Protected Sub secretbutton_ServerClick(sender As Object, e As EventArgs) Handles secretbutton.ServerClick 
        Try 
            Session("msj") = actuaid.Value
            Response.Redirect("frmEmbarquesDiscrep.aspx") 
        Catch ex As Exception 
        End Try 
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

    Protected Sub btnregresar_ServerClick(sender As Object, e As EventArgs) Handles btnregresar.ServerClick

        If Session("usutipo") <> "admin" Then

            Response.Redirect("frmMensajes.aspx")
        Else

            Response.Redirect("frmMensajesAdm.aspx")
        End If

    End Sub

    Protected Sub enviarpinshimensaje_ServerClick(sender As Object, e As EventArgs) Handles enviarpinshimensaje.ServerClick
        Try
            ws = New DIS.DIServer


            Dim Respuesta As XmlNode

            Dim sqldato As String = "insert into  [Ecom].[dbo].[@IL_MENSAJES] (Code,Name,U_mensaje,U_emisor,U_receptor,U_cadena,U_fecha)  " &
            "values((select COUNT(*)+1  from  [Ecom].[dbo].[@IL_MENSAJES] ),'" & Session("usuName") & "','" & TextArea1.Value & "','" & Session("RazCode") & "','Admin','" & Session("msj") & "','" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "')"
            Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)

            If Session("usutipo") <> "admin" Then
                sqldato = " update [Ecom].[dbo].[@IL_CONVER] set U_estado='Enviado', U_fecha2='" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "' where CONVERT(NVARCHAR(MAX), U_conver)='" & Session("msj") & "'"
                Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
                sqldato = " <env:Envelope xmlns:env='http://schemas.xmlsoap.org/soap/envelope/'>   <env:Header>    <SessionID>" & Session("Token") & "</SessionID>   </env:Header>   <env:Body>    <dis:SendMessage xmlns:dis='http://www.sap.com/SBO/DIS'>     <Service>MessagesService</Service>      <Message>       <Subject>Respuesta de: " & Session("usuName") & "</Subject>       <Text>" & TextArea1.Value & "</Text>       <RecipientCollection>        <Recipient>         <UserCode>" & Session("usuMensajes") & "</UserCode>         <SendInternal>tYES</SendInternal>        </Recipient>       </RecipientCollection>              </Message>     </dis:SendMessage>   </env:Body>  </env:Envelope>"
                '" & TextArea1.Value & "','" & Session("RazCode") & "','Admin','" & id & "','" & DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") & "')"
                 ws.Interact(Session("Token"), sqldato)

            Else

                sqldato = " update [Ecom].[dbo].[@IL_CONVER] set U_estado='Nuevo', U_fecha2='" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "' where CONVERT(NVARCHAR(MAX), U_conver)='" & Session("msj") & "'"
                Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
                 
            End If


            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' Mensaje enviado!');document.location.href='frmMsj'; ", True)
        Catch ex As Exception
            System.Diagnostics.Debug.Write(ex.Message & vbCrLf)
            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' No se a enviado el mensaje intente de nuevo. ');document.location.href='frmMsj'; ", True)

        End Try
    End Sub
End Class
