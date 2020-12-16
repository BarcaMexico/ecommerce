Imports Modulo

Imports System.Xml
Imports System.Globalization

Partial Class frmPagos
    Inherits System.Web.UI.Page
    Public ws As DIS.DIServer
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then



            Dim monedapago As String = ""

            'Busqueda de facturas venciadas Balance
            Dim tRow As New TableRow()
            Dim tCell As New TableCell()

            ws = New DIS.DIServer



            Try

                Dim today As Date = DateTime.Now
                Dim answer As Date = today.AddYears(-1)
                Dim fechastring = answer.ToString("yyyy-MM-dd")
                Dim ahora = DateTime.Now.ToString("yyyy-MM-dd")


                fecha1.Value = fechastring
                fecha2.Value = ahora

                Session("pagIni") = 1
                

                busquedapagos()




            Catch ex As Exception

            End Try
        End If

    End Sub

    Public Sub busquedapagos()

        Dim tRow As New TableRow()
        Dim tCell As New TableCell()






        ws = New DIS.DIServer



        Try

            For i = 1 To Table1.Rows.Count - 1
                Table1.Rows.RemoveAt(i)
            Next


            Dim Respuesta As XmlNode

            Dim sqldato As String = " select  CONVERT(int,ROW_NUMBER() OVER ( ORDER BY T2.[DocNum] asc)) AS 'RowNum' , T0.DocDate as fechapago , T0.DocEntry as pago, T2.DocNum as Factura , T2.DocCur as moneda,  T2.DocTotal as total ,T1.SumApplied as importe " &
    " from ORCT T0 inner join RCT2 T1 ON T0.[DocEntry]  = T1.[DocNum] inner join OINV T2 ON T1.[DocEntry]  = T2.[DocEntry] " &
     "where  T0.[CardCode]='" & Session("RazCode") & " ' " &
    "AND  T0.[DocDate] &gt;= '" & fecha1.Value & "'  " &
   " AND  T0.[DocDate] &lt;= '" & fecha2.Value & "'  " &
    "ORDER BY T0.DocDate asc"

            Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
            Dim totalrows As Integer = ReadXML(Respuesta.InnerXml, "RowNum")

            Dim n As Integer
            Dim contapag As Integer = 0
            Dim inic As Integer
            Dim finic As Integer
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
                        For x = 0 To rootin.ChildNodes.Count - 1


                            Select Case rootin.ChildNodes(x).Name
                                Case "RowNum"
                                    If CInt(rootin.ChildNodes(x).InnerText) > (Session("configartcan") * Session("pagIni")) Then

                                        bandera = False

                                    End If
                                    If Session("pagIni") <> 1 Then
                                        If CInt(rootin.ChildNodes(x).InnerText) <= (Session("configartcan") * (Session("pagIni") - 1)) Then

                                            bandera = False

                                        End If
                                    End If
                                Case "fechapago"
                                    tCell = New TableCell()
                                    tCell.Text = DateTime.ParseExact((rootin.ChildNodes(x).InnerText), "yyyyMMdd", CultureInfo.InvariantCulture)
                                    tRow.Cells.Add(tCell)
                                Case "pago"
                                    tCell = New TableCell()
                                    tCell.Text = rootin.ChildNodes(x).InnerText
                                    tRow.Cells.Add(tCell)
                                Case "Factura"
                                    tCell = New TableCell()
                                    tCell.Text = rootin.ChildNodes(x).InnerText
                                    tRow.Cells.Add(tCell)
                                Case "moneda"
                                    tCell = New TableCell()
                                    If rootin.ChildNodes(x).InnerText = "$" OrElse rootin.ChildNodes(x).InnerText = "MXP" OrElse rootin.ChildNodes(x).InnerText = "MXN" Then
                                        tCell.Text = "Moneda Local"
                                    Else
                                        tCell.Text = rootin.ChildNodes(x).InnerText
                                    End If
                                    tRow.Cells.Add(tCell)
                                Case "total"

                                    tCell = New TableCell()
                                    tCell.Text = String.Format("{0:c}", Convert.ToDouble(rootin.ChildNodes(x).InnerText))
                                    tCell.HorizontalAlign = HorizontalAlign.Right
                                    tRow.Cells.Add(tCell)
                                Case "importe"

                                    tCell = New TableCell()
                                    tCell.Text = String.Format("{0:c}", Convert.ToDouble(rootin.ChildNodes(x).InnerText))
                                    tCell.HorizontalAlign = HorizontalAlign.Right
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

        End Try
    End Sub
    Protected Sub buscaredo_ServerClick(sender As Object, e As EventArgs) Handles buscaredo.ServerClick
        Session("pagIni") = 1
        busquedapagos()
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
        busquedapagos()
    End Sub
End Class
