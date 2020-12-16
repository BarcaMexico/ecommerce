Imports Modulo

Imports System.Xml
Imports System.Globalization

Partial Class frmMensajes
    Inherits System.Web.UI.Page
    Public ws As DIS.DIServer
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            'Busqueda de facturas venciadas Balance
            Dim tRow As New TableRow()
            Dim tCell As New TableCell()

            ws = New DIS.DIServer



            Try
                Table1.Visible = False
                Dim Respuesta As XmlNode

                Dim sqldato As String = "select   U_fecha,  U_asunto ,U_estado,U_conver from [Ecom].[dbo].[@IL_CONVER] where U_emisor='" & Session("RazCode") & "' or U_receptor= '" & Session("RazCode") & "' order by U_fecha DESC"

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
                                        tCell = New TableCell()
                                        If rootin.ChildNodes(x).InnerText = "" Then
                                            Exit Sub
                                        End If
                                        tCell.Text = rootin.ChildNodes(x).InnerText
                                        tRow.Cells.Add(tCell)
                                        Table1.Visible = True
                                    Case "U_asunto"
                                        tCell = New TableCell()
                                        tCell.Text = rootin.ChildNodes(x).InnerText
                                        tRow.Cells.Add(tCell)
                                    Case "U_estado"
                                        tCell = New TableCell()
                                        tCell.Text = rootin.ChildNodes(x).InnerText
                                        tRow.Cells.Add(tCell)
                                    Case "U_conver"
                                        tCell = New TableCell()
                                        tCell.HorizontalAlign = HorizontalAlign.Center
                                        tCell.Text = "<button class='btn btn-info' id='row" & rootin.ChildNodes(x).InnerText & "'       onclick='discre( id)'>Leer <i class='fa fa-eye'></i></button>"
                                        tCell.HorizontalAlign = HorizontalAlign.Center
                                        tRow.Cells.Add(tCell)
                                End Select
                            Next x
                            Table1.Rows.Add(tRow)
                        End If
                    Next s
                End If

            Catch ex As Exception
                ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' " & ex.Message & " '); ", True)
            End Try
        End If

    End Sub


    Protected Sub secretbutton_ServerClick(sender As Object, e As EventArgs) Handles secretbutton.ServerClick


        Try

            Session("msj") = actuaid.Value
            Response.Redirect("frmMsj.aspx")


        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' " & ex.Message & " '); ", True)
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


    Protected Sub enviarmensajillo_ServerClick(sender As Object, e As EventArgs) Handles enviarmensajillo.ServerClick
        Try

            Dim quepaso As String = ""
            If asuntini.Value = "" Then
                ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' No se a enviado el mensaje, complete los campos. ');document.location.href='frmMensajes'; ", True)
            Else


                ws = New DIS.DIServer

                Dim id As String = Guid.NewGuid().ToString("N")
                Dim Respuesta As XmlNode
                Dim sqldato As String = ""


                sqldato = "insert into  [Ecom].[dbo].[@IL_CONVER] (Code,Name, U_emisor,U_receptor,U_asunto,U_fecha,U_fecha2,U_estado,U_conver)  " &
                  "values((select COUNT(*)+1  from  [Ecom].[dbo].[@IL_CONVER] ),'" & Session("usuName") & "'" &
                ",'" & Session("RazCode") & "','Admin','" & asuntini.Value & "',  GETDATE() ,GETDATE(),'Enviado','" & id & "')"

                Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)

                If Respuesta.InnerText.Contains("Failed") Then
                    quepaso = Respuesta.InnerText
                End If
                sqldato = "insert into  [Ecom].[dbo].[@IL_MENSAJES] (Code,Name,U_mensaje,U_emisor,U_receptor,U_cadena,U_fecha)  " &
                 "values((select COUNT(*)+1  from  [Ecom].[dbo].[@IL_MENSAJES] ),'" & Session("usuName") & "' ,'" & TextArea1.Value & "','" & Session("RazCode") & "','Admin','" & id & "',GETDATE())"

                Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)

                If Respuesta.InnerText.Contains("Failed") Then
                    quepaso = quepaso & Respuesta.InnerText
                End If


                If quepaso <> "" Then
                    ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert('" & Regex.Replace(quepaso, "[^A-Za-z0-9\-/]", " ") & "');document.location.href='frmMensajes'; ", True)

                Else

                    sqldato = " <env:Envelope xmlns:env='http://schemas.xmlsoap.org/soap/envelope/'>   <env:Header>    <SessionID>" & Session("Token") & "</SessionID>   </env:Header>   <env:Body>    <dis:SendMessage xmlns:dis='http://www.sap.com/SBO/DIS'>     <Service>MessagesService</Service>      <Message>       <Subject>Mensaje de " & Session("usuName") & " : " & asuntini.Value & "</Subject>       <Text>" & TextArea1.Value & "</Text>       <RecipientCollection>        <Recipient>         <UserCode>" & Session("usuMensajes") & "</UserCode>         <SendInternal>tYES</SendInternal>        </Recipient>       </RecipientCollection>              </Message>     </dis:SendMessage>   </env:Body>  </env:Envelope>"

                    '" & TextArea1.Value & "','" & Session("RazCode") & "','Admin','" & id & "','" & DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") & "')"

                    ws.Interact(Session("Token"), sqldato)

                    ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert('mensaje enviado');document.location.href='frmMensajes'; ", True)
                End If



            End If


        Catch ex As Exception
            System.Diagnostics.Debug.Write(ex.Message & vbCrLf)
            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' No se a enviado el mensaje intente de nuevo. ');document.location.href='frmMensajes'; ", True)

        End Try
    End Sub
End Class
