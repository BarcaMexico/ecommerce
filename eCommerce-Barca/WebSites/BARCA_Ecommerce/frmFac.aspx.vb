Imports Modulo

Imports System.Xml
Imports System.Globalization
Imports System.Drawing
Imports System.IO

Partial Class frmFac
    Inherits System.Web.UI.Page
    Public ws As DIS.DIServer
    Dim totalSuma As Double = 0
    Dim sqldato As String
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If radios.SelectedIndex = 0 Then 
            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "$('#fechas').hide();  ", True) 
        End If
        If Not Page.IsPostBack Then
            ' Session("configartcan") = 10
            Dim today As Date = DateTime.Now
            Dim answer As Date = today.AddYears(-1)
            Dim fechastring = answer.ToString("yyyy-MM-dd")
            Dim ahora = DateTime.Now.ToString("yyyy-MM-dd") 
            fecha1.Value = fechastring
            fecha2.Value = ahora 
            radios.SelectedIndex = 0
            If radios.SelectedIndex = 0 Then 
                ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "$('#fechas').hide();  ", True) 
            End If 
            Try
                Session("pagIni") = 1
                buscarFacturas() 
            Catch ex As Exception
                ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert(' error " & ex.Message & "');  ", True)
            End Try
        End If 
    End Sub

    Public Sub buscarFacturas()
        'Busqueda de facturas venciadas Balance
        Dim tRow As New TableRow()
        Dim tCell As New TableCell()

        ws = New DIS.DIServer

        Try
            Dim Rea As XmlNode 
            sqldato = "select top 1 rutaxml,rutaxml from [Ecom].[dbo].[@IL_CONFIG_E]"
            Rea = ws.ExecuteSQL(Session("Token"), sqldato)
            Session("rutaxml") = ReadXML(Rea.InnerXml, "rutaxml")
            Session("rutapdf") = ReadXML(Rea.InnerXml, "rutaxml") 
            For i = 1 To Table1.Rows.Count - 1
                Table1.Rows.RemoveAt(i)
            Next

            Dim Respuesta As XmlNode 
            Select Case radios.SelectedItem.Value
                Case "factuavencida"
                    sqldato = "SELECT CONVERT(int,ROW_NUMBER() OVER ( ORDER BY T0.[DocNum] asc)) AS 'RowNum' , T0.EDocNum,T0.[DocDate], T1.[DueDate], T0.[DocNum],  T0.[DocCur], COALESCE(T2.[CardCode],     T0.[CardCode]) AS 'CardCode', T1.[InsTotal], T1.[PaidToDate],T1.[InsTotalSy],     T1.[PaidSys], T1.[VatSum], T1.[VatPaid], T1.[VatSumSy], T1.[VatPaidSys]   FROM [dbo].[OINV] T0  		INNER JOIN [dbo].[INV6] T1 ON T0.[DocEntry] = T1.[DocEntry]  		LEFT OUTER JOIN [dbo].[OCRD] T2 ON T0.[FatherCard] = T2.[CardCode] AND T0.[FatherType] = 'P'     WHERE T0.[CardCode]  = '" & Session("RazCode") & "'  " &
                        "     and ((T0.[isIns] = 'N' AND T0.[DocStatus] = 'O')        OR (T1.[Status] = 'O' AND T0.[CANCELED] = 'N'))"

                Case "factuavencidaxfecha"
                    sqldato = "SELECT CONVERT(int,ROW_NUMBER() OVER ( ORDER BY T0.[DocNum] asc)) AS 'RowNum' ,  T0.EDocNum,T0.[DocDate], T1.[DueDate], T0.[DocNum],  T0.[DocCur], COALESCE(T2.[CardCode],     T0.[CardCode]) AS 'CardCode', T1.[InsTotal], T1.[PaidToDate],T1.[InsTotalSy],     T1.[PaidSys], T1.[VatSum], T1.[VatPaid], T1.[VatSumSy], T1.[VatPaidSys]   FROM [dbo].[OINV] T0  		INNER JOIN [dbo].[INV6] T1 ON T0.[DocEntry] = T1.[DocEntry]  		LEFT OUTER JOIN [dbo].[OCRD] T2 ON T0.[FatherCard] = T2.[CardCode] AND T0.[FatherType] = 'P'     WHERE T0.[CardCode]  = '" & Session("RazCode") & "'  " &
                        "     and ((T0.[isIns] = 'N' AND T0.[DocStatus] = 'O')        OR (T1.[Status] = 'O' AND T0.[CANCELED] = 'N'))" &
                        "AND  T0.[DocDate] &gt;= '" & fecha1.Value & "'  " &
                        " AND  T0.[DocDate] &lt;= '" & fecha2.Value & "'  "
                Case "factuaxfecha"
                    sqldato = "SELECT CONVERT(int,ROW_NUMBER() OVER ( ORDER BY T0.[DocNum] asc)) AS 'RowNum' , T0.EDocNum,T0.[DocDate], T1.[DueDate], T0.[DocNum],  T0.[DocCur], COALESCE(T2.[CardCode],     T0.[CardCode]) AS 'CardCode', T1.[InsTotal], T1.[PaidToDate],T1.[InsTotalSy],     T1.[PaidSys], T1.[VatSum], T1.[VatPaid], T1.[VatSumSy], T1.[VatPaidSys]   FROM [dbo].[OINV] T0  		INNER JOIN [dbo].[INV6] T1 ON T0.[DocEntry] = T1.[DocEntry]  		LEFT OUTER JOIN [dbo].[OCRD] T2 ON T0.[FatherCard] = T2.[CardCode] AND T0.[FatherType] = 'P'     WHERE T0.[CardCode]  = '" & Session("RazCode") & "'  " &
                    "AND  T0.[DocDate] &gt;= '" & fecha1.Value & "'  " &
                    " AND  T0.[DocDate] &lt;= '" & fecha2.Value & "'  "
            End Select
             
            Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
            Dim totalrows As Integer = ReadXML(Respuesta.InnerXml, "RowNum")

            Dim n As Integer
            Dim edoc As String = ""
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
                                        If CInt(rootin.ChildNodes(x).InnerText) < (Session("configartcan") * (Session("pagIni") - 1)) Then

                                            bandera = False

                                        End If
                                    End If


                                Case "EDocNum"
                                    edoc = rootin.ChildNodes(x).InnerText

                                Case "DocDate"
                                    tCell = New TableCell()
                                    tCell.Text = DateTime.ParseExact((rootin.ChildNodes(x).InnerText), "yyyyMMdd", CultureInfo.InvariantCulture)
                                    tRow.Cells.Add(tCell)
                                Case "DueDate"
                                    tCell = New TableCell()
                                    tCell.Text = DateTime.ParseExact((rootin.ChildNodes(x).InnerText), "yyyyMMdd", CultureInfo.InvariantCulture)
                                    tRow.Cells.Add(tCell)
                                Case "DocNum"
                                    tCell = New TableCell()
                                    tCell.Text = rootin.ChildNodes(x).InnerText
                                    tRow.Cells.Add(tCell)
                                Case "DocCur"
                                    tCell = New TableCell()
                                    If rootin.ChildNodes(x).InnerText = "$" OrElse rootin.ChildNodes(x).InnerText = "MXP" OrElse rootin.ChildNodes(x).InnerText = "MXN" Then
                                        tCell.Text = "Moneda Local"
                                    Else
                                        tCell.Text = rootin.ChildNodes(x).InnerText
                                    End If
                                    tRow.Cells.Add(tCell)
                                Case "InsTotal"

                                    tCell = New TableCell()
                                    tCell.Text = String.Format("{0:c}", Convert.ToDouble(rootin.ChildNodes(x).InnerText))
                                    tCell.HorizontalAlign = HorizontalAlign.Right
                                    tRow.Cells.Add(tCell)
                                    Session("comodin1") = Convert.ToDouble(rootin.ChildNodes(x).InnerText)
                                Case "PaidToDate"

                                    tCell = New TableCell()
                                    tCell.Text = String.Format("{0:c}", Convert.ToDouble(rootin.ChildNodes(x).InnerText))
                                    tCell.HorizontalAlign = HorizontalAlign.Right
                                    tRow.Cells.Add(tCell)
                                    Session("comodin2") = Convert.ToDouble(rootin.ChildNodes(x).InnerText)
                            End Select
                        Next x

                        Session("comodin1") = Session("comodin1") - Session("comodin2")
                        totalSuma = totalSuma + Convert.ToDouble(Session("comodin1"))
                        If bandera = True Then

                            tCell = New TableCell()
                            tCell.Text = String.Format("{0:c}", Convert.ToDouble(Session("comodin1")))
                            tCell.HorizontalAlign = HorizontalAlign.Right


                            If Convert.ToDouble(Session("comodin1")) > 0 Then
                                tRow.Attributes.Add("Class", "rowrojo")
                                tCell.ForeColor = Color.FromArgb(80, ColorTranslator.FromHtml("#d94736"))
                            Else
                                tRow.Attributes.Add("Class", "rowverde")
                                tCell.ForeColor = Color.FromArgb(80, ColorTranslator.FromHtml("#60a917"))
                            End If

                            tRow.Cells.Add(tCell)

                            If edoc <> "" Then


                                Dim filePath As String = Session("rutaxml") & "/" & edoc & ".xml" 
                                Dim filePath2 As String = Session("rutapdf") & "/" & edoc & ".pdf"

                                Dim File = New FileInfo(filePath)
                                Dim File2 = New FileInfo(filePath2)

                                If File.Exists AndAlso File2.Exists Then
                                    tCell = New TableCell()
                                    tCell.Text = "<button class='btn btn-info' id='row" & edoc & "'       onclick='descarga( id)'>XML <i class='fa fa-file-code-o '></i></button><button class='btn btn-info' id='row" & edoc & "'       onclick='descargapdf( id)'>PDF <i class='fa fa-file-pdf-o  '></i></button>"
                                    tCell.HorizontalAlign = HorizontalAlign.Center
                                    tRow.Cells.Add(tCell)
                                ElseIf File.Exists Then
                                    tCell = New TableCell()
                                    tCell.Text = "<button class='btn btn-info' id='row" & edoc & "'       onclick='descarga( id)'>XML <i class='fa fa-file-code-o '></i></button> "
                                    tCell.HorizontalAlign = HorizontalAlign.Center
                                    tRow.Cells.Add(tCell)
                                ElseIf File2.Exists Then
                                    tCell = New TableCell()
                                    tCell.Text = " <button class='btn btn-info' id='row" & edoc & "'       onclick='descargapdf( id)'>PDF <i class='fa fa-file-pdf-o  '></i></button>"
                                    tCell.HorizontalAlign = HorizontalAlign.Center
                                    tRow.Cells.Add(tCell)

                                End If


                                 
                            End If

                            Table1.Rows.Add(tRow)
                        End If


                    End If
                Next s
            End If



            tRow = New TableRow()

            tCell = New TableCell()
            tCell.Text = ""
            tRow.Cells.Add(tCell)
            tCell = New TableCell()
            tCell.Text = ""
            tRow.Cells.Add(tCell)
            tCell = New TableCell()
            tCell.Text = ""
            tRow.Cells.Add(tCell)
            tCell = New TableCell()
            tCell.Text = ""
            tRow.Cells.Add(tCell)
            tCell = New TableCell()
            tCell.Text = ""
            tRow.Cells.Add(tCell)
            tCell = New TableHeaderCell()
            tCell.Text = "Suma Total:"

            tRow.Cells.Add(tCell)

            tCell = New TableCell()
            'tCell.Text = Session("currency") + " " + String.Format("{0:N}", Convert.ToDouble(root.ChildNodes(i).InnerText))
            tCell.Text = Session("currency") + " " + String.Format("{0:N}", totalSuma)
            tCell.HorizontalAlign = HorizontalAlign.Right
            tRow.Cells.Add(tCell)
            Table1.Rows.Add(tRow)




        Catch ex As Exception

        End Try

    End Sub

    Protected Sub buscarfac_ServerClick(sender As Object, e As EventArgs) Handles buscarfac.ServerClick
        Session("pagIni") = 1
        buscarFacturas()

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
        buscarFacturas()
    End Sub

    

    Protected Sub descargaboton_ServerClick(sender As Object, e As EventArgs) Handles descargaboton.ServerClick
        Dim filePath As String = Session("rutaxml") & "/" & actuaid.Value & ".xml"
        Dim File = New FileInfo(filePath)
        If File.Exists Then

            Response.Clear()
            Response.ClearHeaders()
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", "attachment; filename=" + File.Name)
            Response.AddHeader("Content-Length", File.Length.ToString())
            'Response.ContentType = "text/plain" 
            Response.Flush()
            Response.TransmitFile(File.FullName)
            Response.End()
        Else
            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert('Documento no disponible'); ", True)

        End If

        buscarFacturas()
    End Sub

    Protected Sub descargabotonpdf_ServerClick(sender As Object, e As EventArgs) Handles descargabotonpdf.ServerClick
        Dim filePath As String = Session("rutaxml") & "/" & actuaid.Value & ".pdf"
        Dim File = New FileInfo(filePath)
        If File.Exists Then

            Response.Clear()
            Response.ClearHeaders()
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", "attachment; filename=" + File.Name)
            Response.AddHeader("Content-Length", File.Length.ToString())
            'Response.ContentType = "text/plain" 
            Response.Flush()
            Response.TransmitFile(File.FullName)
            Response.End()
        Else
            ClientScript.RegisterStartupScript(Me.[GetType](), "aleasrt", "alert('Documento no disponible'); ", True)

        End If

        buscarFacturas()
    End Sub
End Class
