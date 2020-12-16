Imports Modulo

Imports System.Xml
Imports System.Globalization

Partial Class frmMensajesAdm
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

                Dim sqldato As String = "select   t1.Name as name  ,t1.U_fecha as fecha,  t1.U_asunto as asunto , t1.U_estado as edo ,t1.U_conver as conver from [Ecom].[dbo].[@IL_CONVER] t1    order by t1.U_fecha2 DESC "
                  
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
                                    Case "name"
                                        tCell = New TableCell()
                                        tCell.Text = rootin.ChildNodes(x).InnerText
                                        tRow.Cells.Add(tCell)
                                    Case "fecha"
                                        tCell = New TableCell()
                                        tCell.Text = DateTime.ParseExact((rootin.ChildNodes(x).InnerText), "yyyyMMdd", CultureInfo.InvariantCulture)
                                        tRow.Cells.Add(tCell)
                                    Case "asunto"
                                        tCell = New TableCell()
                                        tCell.Text = rootin.ChildNodes(x).InnerText
                                        tRow.Cells.Add(tCell)
                                    Case "edo"
                                        tCell = New TableCell()

                                        If rootin.ChildNodes(x).InnerText = "Nuevo" Then
                                            tCell.Text = "Contestado"
                                        Else
                                            tCell.Text = "Nuevo"
                                        End If



                                        tRow.Cells.Add(tCell)
                                    Case "conver"
                                        tCell = New TableCell()
                                        tCell.HorizontalAlign = HorizontalAlign.Center
                                        tCell.Text = "<button class='btn btn-info' id='row" & rootin.ChildNodes(x).InnerText & "'       onclick='discre( id)'> Leer <i class='fa fa-eye'> </i></button>"
                                        tCell.HorizontalAlign = HorizontalAlign.Center
                                        tRow.Cells.Add(tCell)
                                End Select
                            Next x


                            Table1.Rows.Add(tRow)

                        End If
                    Next s
                End If










            Catch ex As Exception

                Logger.LogInfo(ex)
            End Try
        End If

    End Sub


    Protected Sub secretbutton_ServerClick(sender As Object, e As EventArgs) Handles secretbutton.ServerClick


        Try

            Session("msj") = actuaid.Value
            Response.Redirect("frmMsj.aspx")


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


    '    Protected Sub enviarmensajillo_ServerClick(sender As Object, e As EventArgs) Handles enviarmensajillo.ServerClick
    '        Try
    '            ws = New DIS.DIServer
    '            ws.Url = "http://localhost/SAP/DIServer.asmx"
    '            Dim id As String = Guid.NewGuid().ToString("N")
    '            Dim Respuesta As XmlNode

    '            Dim sqldato As String = "insert into  [@IL_CONVER] (Code,Name, U_emisor,U_receptor,U_asunto,U_fecha,U_fecha2,U_estado,U_conver)  " &
    '"values((select COUNT(*)+1  from  [@IL_CONVER] ),(select COUNT(*)+1  from  [@IL_CONVER] )" &
    '",'" & Session("RazCode") & "','Admin','" & asuntini.Value & "','" & DateTime.Now.ToString("yyyy-MM-dd") & "','" & DateTime.Now.ToString("yyyy-MM-dd") & "','Enviado','" & id & "')"

    '            Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)



    '            sqldato = "insert into  [@IL_MENSAJES] (Code,Name,U_mensaje,U_emisor,U_receptor,U_cadena,U_fecha)  " &
    '             "values((select COUNT(*)+1  from  [@IL_MENSAJES] ),(select COUNT(*)+1  from  [@IL_MENSAJES] ),'" & TextArea1.Value & "','" & Session("RazCode") & "','Admin','" & id & "','" & DateTime.Now.ToString("yyyy-MM-dd") & "')"

    '            Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)




    '            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' Mensaje enviado!');document.location.href='frmMensajes'; ", True)
    '        Catch ex As Exception
    '            System.Diagnostics.Debug.Write(ex.Message & vbCrLf)
    '            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' No se a enviado el mensaje intente de nuevo. ');document.location.href='frmMensajes'; ", True)

    '        End Try
    '    End Sub
End Class
