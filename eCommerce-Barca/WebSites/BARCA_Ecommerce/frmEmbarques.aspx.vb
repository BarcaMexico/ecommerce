Imports Modulo

Imports System.Xml
Imports System.Globalization

Partial Class frmEmbarques
    Inherits System.Web.UI.Page
    Public ws As DIS.DIServer
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then 
            Try
                Session("pagIni") = 1
                cargaEmbarque() 
            Catch ex As Exception
                ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' error " & ex.Message & "');  ", True)
            End Try
        End If

    End Sub

    Protected Sub cargaEmbarque()

        'Busqueda de facturas venciadas Balance
        Dim tRow As New TableRow()
        Dim tCell As New TableCell()

        ws = New DIS.DIServer

        Try

            Dim Respuesta As XmlNode


            Dim sqldato As String = "select CONVERT(int,ROW_NUMBER() OVER ( ORDER BY T0.[docdate] desc)) AS 'RowNum' , T0.DocDate as fecha,  T0.DocNum as entrega  , T1.BaseRef as pedido ,T0.Address2 as destino,T0.TrackNo as guia " &
"from ODLN T0 inner join DLN1 T1 on T0.DocEntry = T1.DocEntry   where CardCode='" & Session("RazCode") & "' and    " &
"U_IL_estado  &lt; &gt;'c'  group by T1.BaseRef ,T0.DocDate  , T0.Address2,T0.TrackNo,T0.DocNum order by  T0.DocNum desc"



            Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
            Dim totalrows As Integer = ReadXML(Respuesta.InnerXml, "RowNum")



            Dim n As Integer
            Dim edoc As String = ""
            Dim contapag As Integer = 0
            Dim inic As Integer
            Dim finic As Integer

            'Revolution PAgin
            Dim configcant As Integer = Session("configartcan")
            If totalrows > configcant Then
                Dim nacho As Integer = Math.Ceiling(totalrows / Session("configartcan"))

                PaginationDiv.InnerHtml = " <ul class='pagination  pagination-lg'>"

                If nacho > 1 Then
                    If nacho > 3 Then
                        'checa inicio
                        If (Session("pagIni") - 1) <= 1 Then
                            inic = 1
                        ElseIf (Session("pagIni")) = nacho Then
                            inic = (Session("pagIni") - 2)
                        Else
                            inic = (Session("pagIni") - 1)
                        End If
                        'checa final
                        If (Session("pagIni") + 1) >= nacho Then
                            finic = nacho
                        ElseIf (Session("pagIni")) = 1 Then
                            finic = (Session("pagIni") + 2)
                        Else
                            finic = (Session("pagIni") + 1)
                        End If
                        If Session("pagIni") > 1 Then
                            PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + " <li style='cursor: pointer; ' class='' id='row" & (Session("pagIni") - 1) & "'       onclick='ver( id)'><a><i class='fa fa-arrow-left'></i></a></li>"
                        End If

                        If inic > 2 Then

                            PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + "<li style='cursor: pointer; '  class='' id='row1'       onclick='ver( id)'><a>1</a></li><li><a>...</a></li>"

                        End If

                        For n = inic To finic
                            If n = Session("pagIni") Then
                                PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + "<li style='cursor: pointer; '  class='active' id='row" & n & "'       onclick='ver( id)'><a>" & n & "</a></li>"
                            Else
                                PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + "<li style='cursor: pointer; '  class='' id='row" & n & "'       onclick='ver( id)'><a>" & n & "</a></li>"
                            End If
                        Next


                        If (Session("pagIni") + 2) <= nacho Then

                            PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + "<li><a>...</a></li><li style='cursor: pointer; '  class='' id='row" & nacho & "'       onclick='ver( id)'><a>" & nacho & "</a></li>"

                        End If

                        If Session("pagIni") < nacho Then
                            PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + "<li style='cursor: pointer; '  class='' id='row" & (Session("pagIni") + 1) & "'       onclick='ver( id)'><a><i class='fa fa-arrow-right'></i></a></li>"
                        End If

                    Else
                        For n = 1 To nacho
                            If n = Session("pagIni") Then
                                PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + "<li style='cursor: pointer; '  class='active' id='row" & n & "'       onclick='ver( id)'><a>" & n & "</a></li>"
                            Else
                                PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + "<li style='cursor: pointer; '  class='' id='row" & n & "'       onclick='ver( id)'><a>" & n & "</a></li>"
                            End If
                        Next
                    End If



                    PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + " </ul>"
                Else
                    PaginationDiv.InnerHtml = ""
                End If

            Else
                PaginationDiv.InnerHtml = ""
            End If


            'Finalize de paginacion
             
            Dim bandera As Boolean = True 
            If ReadXML(Respuesta.InnerXml, "factura") <> "0" Then
                Dim documentum As String = ""
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
                        bandera = True
                        If rootin.HasChildNodes Then
                            Dim x As Integer
                            tRow = New TableRow()
                            Session("fcsi") = 0
                            For x = 0 To rootin.ChildNodes.Count - 1


                                Select Case rootin.ChildNodes(x).Name
                                    Case "RowNum"
                                        If CInt(rootin.ChildNodes(x).InnerText) > (Session("configartcan") * Session("pagIni")) Then

                                            bandera = False

                                        End If
                                        If Session("pagIni") <> 1 Then
                                            If CInt(rootin.ChildNodes(x).InnerText) < (Session("configartcan") * (Session("pagIni") - 1)) Then

                                                bandera = False

                                            End If
                                        End If

                                    Case "fecha"
                                        tCell = New TableCell()
                                        tCell.Text = DateTime.ParseExact((rootin.ChildNodes(x).InnerText), "yyyyMMdd", CultureInfo.InvariantCulture)
                                        tRow.Cells.Add(tCell)
                                    Case "entrega"
                                        tCell = New TableCell()
                                        tCell.Text = rootin.ChildNodes(x).InnerText
                                        documentum = rootin.ChildNodes(x).InnerText
                                        tRow.Cells.Add(tCell)
                                    Case "pedido"
                                        tCell = New TableCell()
                                        tCell.Text = rootin.ChildNodes(x).InnerText

                                        tRow.Cells.Add(tCell)
                                    Case "destino"
                                        tCell = New TableCell()
                                        tCell.Text = rootin.ChildNodes(x).InnerText
                                        tRow.Cells.Add(tCell)
                                    Case "guia"
                                        tCell = New TableCell()
                                        tCell.Text = rootin.ChildNodes(x).InnerText
                                        tRow.Cells.Add(tCell)


                                        tCell = New TableCell()
                                        tCell.HorizontalAlign = HorizontalAlign.Center
                                        tCell.Text = "<button class='btn btn-info' id='row" & documentum & "'       onclick='discre( id)'> <i class='fa fa-thumbs-o-down'></i></button>"
                                        tCell.HorizontalAlign = HorizontalAlign.Center
                                        tRow.Cells.Add(tCell)
                                        tCell = New TableCell()
                                        tCell.Text = "<button class='btn btn-info' id='row" & documentum & "'       onclick='confirpedido( id)'> <i class='fa fa-check-square-o'></i></button>"
                                        tCell.HorizontalAlign = HorizontalAlign.Center
                                        tRow.Cells.Add(tCell)
                                          
                                End Select
                            Next x 
                            If bandera = True Then
                                Table1.Rows.Add(tRow)
                            End If

                        End If
                    Next s
                End If 
            Else
                Table1.Visible = False 
            End If



        Catch ex As Exception

        End Try
    End Sub
    Protected Sub secretbutton_ServerClick(sender As Object, e As EventArgs) Handles secretbutton.ServerClick
        ws = New DIS.DIServer

        Try 
            If actuacan.Value = "confir" Then
                Session("DocNumDis") = actuaid.Value
                Response.Redirect("frmConfir.aspx") 
                Else
                Session("DocNumDis") = actuaid.Value
                Response.Redirect("frmEmbarquesDiscrep.aspx")
            End If
             
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

    Protected Sub botonpagina_ServerClick(sender As Object, e As EventArgs) Handles botonpagina.ServerClick
        Session("pagIni") = idpagina.Value
        cargaEmbarque()
    End Sub
End Class
