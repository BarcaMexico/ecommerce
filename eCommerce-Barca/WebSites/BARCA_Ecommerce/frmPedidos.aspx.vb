Imports Modulo

Imports System.Xml
Imports System.Globalization

Partial Class frmPedidos
    Inherits System.Web.UI.Page
    Public ws As DIS.DIServer
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
             
            Try
                Session("pagIni") = 1
                cargaPedidos()
            Catch ex As Exception
                ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' error " & ex.Message & "');  ", True)
            End Try
        End If

    End Sub

    Protected Sub cargaPedidos()

        'Busqueda de facturas venciadas Balance
        Dim tRow As New TableRow()
        Dim tCell As New TableCell()
        Dim sqldato As String = ""
        Dim Respuesta As XmlNode

        ws = New DIS.DIServer
        Try
            'Validamos si debemos de filtrar la información
            'HRS Cambio en RowNow de  ORDER BY T0.[docdate] DESC
            'A  ORDER BY T0.[DocNum] DESC
            If IsNothing(Session("txtBusquedaPedidos")) Then
                sqldato = "SELECT CONVERT(int,ROW_NUMBER() OVER ( ORDER BY T0.[DocNum] DESC)) AS 'RowNum'  , T0.[CardCode], T0.[DocDate], T0.[DocNum], T0.[Comments],T0.[DocTotalFC], T0.[DocTotal]," &
                          "T0.[PaidToDate], T0.[ObjType],T0.[DocEntry] FROM  [dbo].[ORDR] T0 " &
                          "WHERE T0.[CardCode] = '" & Session("RazCode") & "' AND T0.[DocStatus] = 'O' AND  T0.[CANCELED] = 'N' order by DocNum DESC"
            Else
                If Session("txtBusquedaPedidos") = "" Then
                    sqldato = "SELECT CONVERT(int,ROW_NUMBER() OVER ( ORDER BY T0.[DocNum] DESC)) AS 'RowNum'  , T0.[CardCode], T0.[DocDate], T0.[DocNum], T0.[Comments],T0.[DocTotalFC], T0.[DocTotal]," &
                              "T0.[PaidToDate], T0.[ObjType],T0.[DocEntry] FROM  [dbo].[ORDR] T0 " &
                              "WHERE T0.[CardCode] = '" & Session("RazCode") & "' AND T0.[DocStatus] = 'O' AND  T0.[CANCELED] = 'N' order by DocNum DESC"
                Else
                    sqldato = "SELECT CONVERT(int,ROW_NUMBER() OVER ( ORDER BY T0.[DocNum] DESC)) AS 'RowNum'  , T0.[CardCode], T0.[DocDate], T0.[DocNum], T0.[Comments],T0.[DocTotalFC], T0.[DocTotal]," &
                              "T0.[PaidToDate], T0.[ObjType],T0.[DocEntry] FROM  [dbo].[ORDR] T0 " &
                              "WHERE T0.[DocNum] like '%" & Session("txtBusquedaPedidos").ToString.Trim & "%' AND T0.[CardCode] = '" & Session("RazCode") & "' AND T0.[DocStatus] = 'O' AND  T0.[CANCELED] = 'N' order by DocNum DESC"
                End If
            End If

            'Eliminamos session busqueda 
            Session("txtBusquedaPedidos") = Nothing

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
                            PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + " <li style='cursor: pointer; ' class='' id='row" & (Session("pagIni") - 1) & "'       onclick='discre( id)'><a><i class='fa fa-arrow-left'></i></a></li>"
                        End If

                        If inic > 2 Then

                            PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + "<li style='cursor: pointer; '  class='' id='row1'       onclick='discre( id)'><a>1</a></li><li><a>...</a></li>"

                        End If

                        For n = inic To finic
                            If n = Session("pagIni") Then
                                PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + "<li style='cursor: pointer; '  class='active' id='row" & n & "'       onclick='discre( id)'><a>" & n & "</a></li>"
                            Else
                                PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + "<li style='cursor: pointer; '  class='' id='row" & n & "'       onclick='discre( id)'><a>" & n & "</a></li>"
                            End If
                        Next


                        If (Session("pagIni") + 2) <= nacho Then

                            PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + "<li><a>...</a></li><li style='cursor: pointer; '  class='' id='row" & nacho & "'       onclick='discre( id)'><a>" & nacho & "</a></li>"

                        End If

                        If Session("pagIni") < nacho Then
                            PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + "<li style='cursor: pointer; '  class='' id='row" & (Session("pagIni") + 1) & "'       onclick='discre( id)'><a><i class='fa fa-arrow-right'></i></a></li>"
                        End If

                    Else
                        For n = 1 To nacho
                            If n = Session("pagIni") Then
                                PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + "<li style='cursor: pointer; '  class='active' id='row" & n & "'       onclick='discre( id)'><a>" & n & "</a></li>"
                            Else
                                PaginationDiv.InnerHtml = PaginationDiv.InnerHtml + "<li style='cursor: pointer; '  class='' id='row" & n & "'       onclick='discre( id)'><a>" & n & "</a></li>"
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

                                Case "DocDate"
                                    tCell = New TableCell()
                                    'tCell.Text = DateTime.ParseExact((rootin.ChildNodes(x).InnerText), "yyyyMMdd", CultureInfo.InvariantCulture)
                                    tCell.Text = rootin.ChildNodes(x).InnerText

                                    tRow.Cells.Add(tCell)
                                Case "DocNum"
                                    tCell = New TableCell()
                                    tCell.Text = rootin.ChildNodes(x).InnerText
                                    tRow.Cells.Add(tCell)
                                Case "Comments"
                                    tCell = New TableCell()
                                    tCell.Text = rootin.ChildNodes(x).InnerText
                                    tRow.Cells.Add(tCell)
                                Case "DocTotalFC"
                                    If Convert.ToDouble(rootin.ChildNodes(x).InnerText) > 0 Then
                                        tCell = New TableCell()
                                        tCell.Text = Session("RazMON") + " " + String.Format("{0:N}", Convert.ToDouble(rootin.ChildNodes(x).InnerText))
                                        tCell.HorizontalAlign = HorizontalAlign.Right
                                        tRow.Cells.Add(tCell)
                                        Session("fcsi") = 1
                                    End If
                                Case "DocTotal"
                                    If Session("fcsi") = 0 Then
                                        tCell = New TableCell()
                                        tCell.Text = Session("RazMON") + " " + String.Format("{0:N}", Convert.ToDouble(rootin.ChildNodes(x).InnerText))
                                        tCell.HorizontalAlign = HorizontalAlign.Right
                                        tRow.Cells.Add(tCell)
                                    End If
                                Case "DocEntry"

                                    tCell = New TableCell()

                                    tCell.Text = "<button class='btn btn-info' id='row" & rootin.ChildNodes(x).InnerText & "'       onclick='ver( id)'>Ver <i class='fa fa-eye'></i></button>"
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
        Catch ex As Exception
            ''  MsgBox(ex.Message)
            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' " & ex.Message & " '); ", True)

        End Try

    End Sub
    Protected Sub secretbutton_ServerClick(sender As Object, e As EventArgs) Handles secretbutton.ServerClick


        Session("pedidoview") = actuaid.Value
        Response.Redirect("frmPedidosView.aspx")



    End Sub


    Private Function ReadXML(Xml As String, NodeName As String)
        Dim reader As System.Xml.XmlTextReader = New System.Xml.XmlTextReader(New System.IO.StringReader(Xml))
        Dim mayor As Integer = 0
        Dim valor As String
        valor = ""
        Do While (reader.Read())
            If reader.NodeType = System.Xml.XmlNodeType.Element Then
                If reader.Name.ToUpper = NodeName.ToUpper Then

                    valor = reader.ReadElementContentAsString
                    If mayor < CInt(valor) Then
                        mayor = CInt(valor)
                    End If


                End If
            End If
        Loop
        reader.Close()
        reader.Dispose()
        Return mayor
    End Function

    Protected Sub botonpagina_ServerClick(sender As Object, e As EventArgs) Handles botonpagina.ServerClick
        Session("pagIni") = idpagina.Value
        cargaPedidos()
    End Sub

    Protected Sub btnBuscarOrden_ServerClick(sender As Object, e As EventArgs) Handles btnBuscarOrden.ServerClick
        Session("txtBusquedaPedidos") = txtBusquedaPed.Value.ToString
        cargaPedidos()
    End Sub

End Class
