Imports Modulo

Imports System.Xml
Imports System.Globalization

Partial Class frmEdo
    Inherits System.Web.UI.Page
    Public ws As DIS.DIServer
    Dim numori As String = ""
    Dim transtipo As String = ""
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then





            'Busqueda de facturas venciadas Balance
            Dim tRow As New TableRow()
            Dim tCell As New TableCell()

            ws = New DIS.DIServer



            Try
                Dim cosa As String = ""
                Dim today As Date = DateTime.Now
                Dim answer As Date = today.AddYears(-1)
                Dim fechastring = answer.ToString("yyyy-MM-dd")
                Dim ahora = DateTime.Now.ToString("yyyy-MM-dd")


                fecha1.Value = fechastring
                fecha2.Value = ahora


                Dim Respuesta As XmlNode

                Dim sqldato As String = "SELECT   T0.TransType , T0.[RefDate] as Fechaconta,T0.[BaseRef] as numori,T0.[ContraAct] as cuentacontra ,T0.[LineMemo] as infodeta ,  T0.[Debit]-T0.[Credit] as saldo ,  T0.[FCDebit]-T0.[FCCredit] as saldoFC  " &
        "FROM  [dbo].[JDT1] T0   LEFT OUTER  JOIN [dbo].[OUSR] T1  ON  T0.[UserSign] = T1.[USERID]   LEFT OUTER  JOIN (SELECT T0.[TransId] AS 'TransId', " &
        "T0.[TransRowId] AS 'TransRowId', MAX(T0.[ReconNum]) AS 'MaxReconNum' FROM  [dbo].[ITR1] T0  GROUP BY T0.[TransId], T0.[TransRowId]) T2  ON  T0.[TransId] = T2.[TransId]  " &
        "AND  T0.[Line_ID] = T2.[TransRowId]   WHERE T0.[ShortName] = '" & Session("RazCode") & "'  " &
         "AND  (T0.[BalDueDeb] &lt; &gt; 0  " &
        "OR  T0.[BalDueCred]  &lt; &gt; 0 " &
        "OR  T0.[BalFcDeb]  &lt; &gt; 0 " &
        "OR  T0.[BalFcCred]  &lt; &gt; 0) " &
        "AND  T0.[RefDate] &gt;= '" & fecha1.Value & "'  " &
       " AND  T0.[RefDate] &lt;= '" & fecha2.Value & "'  " &
        "and (T0.[Debit]-T0.[Credit]) &gt; 0 ORDER BY T0.RefDate asc"

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
                            For x = 0 To rootin.ChildNodes.Count - 1


                                Select Case rootin.ChildNodes(x).Name
                                    Case "TransType"
                                        transtipo = rootin.ChildNodes(x).InnerText
                                    Case "Fechaconta"
                                        tCell = New TableCell()
                                        tCell.Text = DateTime.ParseExact((rootin.ChildNodes(x).InnerText), "yyyyMMdd", CultureInfo.InvariantCulture)
                                        tRow.Cells.Add(tCell)
                                    Case "numori"
                                        tCell = New TableCell()
                                        tCell.Text = rootin.ChildNodes(x).InnerText
                                        numori = rootin.ChildNodes(x).InnerText
                                        tRow.Cells.Add(tCell)
                                    Case "cuentacontra"
                                        tCell = New TableCell()
                                        tCell.Text = rootin.ChildNodes(x).InnerText
                                        tRow.Cells.Add(tCell)
                                    Case "infodeta"
                                        tCell = New TableCell()
                                        tCell.Text = rootin.ChildNodes(x).InnerText
                                        tRow.Cells.Add(tCell)
                                    Case "saldo"
                                        totalSuma = totalSuma + Convert.ToDouble(rootin.ChildNodes(x).InnerText)
                                        tCell = New TableCell()

                                        'If transtipo = "13" Then
                                        '    transtipo = "oinv"
                                        'ElseIf transtipo = "14" Then
                                        '    transtipo = "orin"
                                        'ElseIf transtipo = "24" Then
                                        '    transtipo = "orct"
                                        'End If
                                        'Dim cosa As String = ""
                                        'If transtipo = "orct" Then
                                        '    Respuesta = ws.ExecuteSQL(Session("Token"), " select DocCurr from " & transtipo & " where DocNum  =" & numori & "")
                                        '    cosa = ReadXML(Respuesta.InnerXml, "DocCurr")
                                        'Else
                                        '    Respuesta = ws.ExecuteSQL(Session("Token"), " select DocCur from " & transtipo & " where DocNum  =" & numori & "")
                                        '    cosa = ReadXML(Respuesta.InnerXml, "DocCur")
                                        'End If 
                                        tCell.Text = Session("ML") + " " + String.Format("{0:N}", Convert.ToDouble(rootin.ChildNodes(x).InnerText))
                                        tCell.HorizontalAlign = HorizontalAlign.Right
                                        tRow.Cells.Add(tCell)
                                    Case "saldoFC"
                                        totalSumaFC = totalSumaFC + Convert.ToDouble(rootin.ChildNodes(x).InnerText)
                                        tCell = New TableCell()

                                        If transtipo = "13" Then
                                            transtipo = "oinv"
                                        ElseIf transtipo = "14" Then
                                            transtipo = "orin"
                                        ElseIf transtipo = "24" Then
                                            transtipo = "orct"
                                        End If

                                        If transtipo = "orct" Then
                                            Respuesta = ws.ExecuteSQL(Session("Token"), " select DocCurr from " & transtipo & " where DocNum  =" & numori & "")
                                            cosa = ReadXML(Respuesta.InnerXml, "DocCurr")
                                        Else
                                            Respuesta = ws.ExecuteSQL(Session("Token"), " select DocCur from " & transtipo & " where DocNum  =" & numori & "")
                                            cosa = ReadXML(Respuesta.InnerXml, "DocCur")
                                        End If
                                        If cosa = Session("ML") Then
                                            cosa = ""
                                        End If

                                        tCell.Text = cosa + " " + String.Format("{0:N}", Convert.ToDouble(rootin.ChildNodes(x).InnerText))

                                        tCell.HorizontalAlign = HorizontalAlign.Right
                                        tRow.Cells.Add(tCell)
                                End Select
                            Next x
                            Table1.Rows.Add(tRow)



                        End If
                    Next s
                End If




                If Table1.Rows.Count = 1 Then
                    Table1.Visible = False
                End If


                sqldato = "select SUM(T0.[Debit])-sum(T0.[Credit]) as [Saldo],SUM(T0.[FCDebit])-sum(T0.[FCCredit]) as [SaldoFC]  " &
         "FROM  [dbo].[JDT1] T0   LEFT OUTER  JOIN [dbo].[OUSR] T1  ON  T0.[UserSign] = T1.[USERID]   LEFT OUTER  JOIN (SELECT T0.[TransId] AS 'TransId', " &
        "T0.[TransRowId] AS 'TransRowId', MAX(T0.[ReconNum]) AS 'MaxReconNum' FROM  [dbo].[ITR1] T0  GROUP BY T0.[TransId], T0.[TransRowId]) T2  ON  T0.[TransId] = T2.[TransId]  " &
        "AND  T0.[Line_ID] = T2.[TransRowId]   WHERE T0.[ShortName] = '" & Session("RazCode") & "'   " &
         "AND  (T0.[BalDueDeb] &lt; &gt; 0  " &
        "OR  T0.[BalDueCred]  &lt; &gt; 0 " &
        "OR  T0.[BalFcDeb]  &lt; &gt; 0 " &
        "OR  T0.[BalFcCred]  &lt; &gt; 0 ) "
                 
                Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
                Dim doc As New XmlDocument()
                doc.LoadXml(Respuesta.InnerXml)
                System.Diagnostics.Debug.Write(Respuesta.InnerXml & vbCrLf)
                Dim root As XmlNode = doc.FirstChild
                root = root.LastChild
                root = root.FirstChild
                If root.HasChildNodes Then
                     
                    Dim i As Integer
                    For i = 0 To root.ChildNodes.Count - 1
                        Select Case root.ChildNodes(i).Name
                            Case "Saldo"
                                'Session("currency") = root.ChildNodes(i).InnerText
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

                                tCell = New TableHeaderCell()
                                tCell.Text = "Total:"
                                tRow.Cells.Add(tCell)

                                tCell = New TableCell()
                                ' tCell.Text = Session("currency") + " " + String.Format("{0:N}", Convert.ToDouble(root.ChildNodes(i).InnerText))
                                tCell.Text = Session("ML") + " " + String.Format("{0:N}", totalSuma)
                                tCell.HorizontalAlign = HorizontalAlign.Right
                                tRow.Cells.Add(tCell)

                                Table1.Rows.Add(tRow)
                            Case "SaldoFC"
                                'Session("currency") = root.ChildNodes(i).InnerText 
                                tCell = New TableCell()
                                ' tCell.Text = Session("currency") + " " + String.Format("{0:N}", Convert.ToDouble(root.ChildNodes(i).InnerText))
                                tCell.Text = cosa + " " + String.Format("{0:N}", totalSumaFC)
                                tCell.HorizontalAlign = HorizontalAlign.Right
                                tRow.Cells.Add(tCell)

                                Table1.Rows.Add(tRow)

                        End Select
                    Next i
                End If


                If Table1.Rows.Count = 1 Then
                    Table1.Visible = False
                End If


            Catch ex As Exception

            End Try
        End If

    End Sub
    Dim totalSuma As Double = 0
    Dim totalSumaFC As Double = 0
    Protected Sub buscaredo_ServerClick(sender As Object, e As EventArgs) Handles buscaredo.ServerClick
        Dim tRow As New TableRow()
        Dim tCell As New TableCell()






        ws = New DIS.DIServer



        Try

            For i = 1 To Table1.Rows.Count - 1
                Table1.Rows.RemoveAt(i)
            Next

            Dim Respuesta As XmlNode



            Dim sqldato As String = "SELECT   T0.TransType , T0.[RefDate] as Fechaconta,T0.[BaseRef] as numori,T0.[ContraAct] as cuentacontra ,T0.[LineMemo] as infodeta ,  T0.[Debit]-T0.[Credit] as saldo  " &
    "FROM  [dbo].[JDT1] T0   LEFT OUTER  JOIN [dbo].[OUSR] T1  ON  T0.[UserSign] = T1.[USERID]   LEFT OUTER  JOIN (SELECT T0.[TransId] AS 'TransId', " &
    "T0.[TransRowId] AS 'TransRowId', MAX(T0.[ReconNum]) AS 'MaxReconNum' FROM  [dbo].[ITR1] T0  GROUP BY T0.[TransId], T0.[TransRowId]) T2  ON  T0.[TransId] = T2.[TransId]  " &
    "AND  T0.[Line_ID] = T2.[TransRowId]   WHERE T0.[ShortName] = '" & Session("RazCode") & "'   " &
     "AND  (T0.[BalDueDeb] &lt; &gt; 0  " &
    "OR  T0.[BalDueCred]  &lt; &gt; 0 " &
    "OR  T0.[BalFcDeb]  &lt; &gt; 0 " &
    "OR  T0.[BalFcCred]  &lt; &gt; 0 )" &
    "AND  T0.[RefDate] &gt;= '" & fecha1.Value & "'  " &
   " AND  T0.[RefDate] &lt;= '" & fecha2.Value & "'   " &
    "ORDER BY T0.RefDate asc"





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
                        For x = 0 To rootin.ChildNodes.Count - 1


                            Select Case rootin.ChildNodes(x).Name
                                Case "TransType"
                                    transtipo = rootin.ChildNodes(x).InnerText
                                Case "Fechaconta"
                                    tCell = New TableCell()
                                    tCell.Text = DateTime.ParseExact((rootin.ChildNodes(x).InnerText), "yyyyMMdd", CultureInfo.InvariantCulture)
                                    tRow.Cells.Add(tCell)
                                Case "numori"
                                    tCell = New TableCell()
                                    tCell.Text = rootin.ChildNodes(x).InnerText
                                    numori = rootin.ChildNodes(x).InnerText
                                    tRow.Cells.Add(tCell)
                                Case "cuentacontra"
                                    tCell = New TableCell()
                                    tCell.Text = rootin.ChildNodes(x).InnerText
                                    tRow.Cells.Add(tCell)
                                Case "infodeta"
                                    tCell = New TableCell()
                                    tCell.Text = rootin.ChildNodes(x).InnerText
                                    tRow.Cells.Add(tCell)
                                Case "saldo"
                                    totalSuma = totalSuma + Convert.ToDouble(rootin.ChildNodes(x).InnerText)
                                    tCell = New TableCell()



                                    If transtipo = "13" Then
                                        transtipo = "oinv"
                                    ElseIf transtipo = "14" Then
                                        transtipo = "orin"
                                    ElseIf transtipo = "24" Then
                                        transtipo = "orct"
                                    End If
                                    Dim cosa As String = ""
                                    If transtipo = "orct" Then
                                        Respuesta = ws.ExecuteSQL(Session("Token"), " select DocCurr from " & transtipo & " where DocNum  =" & numori & "")
                                        cosa = ReadXML(Respuesta.InnerXml, "DocCurr")
                                    Else
                                        Respuesta = ws.ExecuteSQL(Session("Token"), " select DocCur from " & transtipo & " where DocNum  =" & numori & "")
                                        cosa = ReadXML(Respuesta.InnerXml, "DocCur")
                                    End If
                                    tCell.Text = cosa + " " + String.Format("{0:N}", Convert.ToDouble(rootin.ChildNodes(x).InnerText))


                                    'tCell.Text = Session("currency") + " " + String.Format("{0:N}", Convert.ToDouble(rootin.ChildNodes(x).InnerText))
                                    tCell.HorizontalAlign = HorizontalAlign.Right
                                    tRow.Cells.Add(tCell)
                            End Select
                        Next x
                        Table1.Rows.Add(tRow)

                    End If
                Next s
            End If


            If Table1.Rows.Count = 1 Then
                Table1.Visible = False
            End If





            sqldato = "select SUM(T0.[Debit])-sum(T0.[Credit]) as [Saldo]  " &
     "FROM  [dbo].[JDT1] T0   LEFT OUTER  JOIN [dbo].[OUSR] T1  ON  T0.[UserSign] = T1.[USERID]   LEFT OUTER  JOIN (SELECT T0.[TransId] AS 'TransId', " &
    "T0.[TransRowId] AS 'TransRowId', MAX(T0.[ReconNum]) AS 'MaxReconNum' FROM  [dbo].[ITR1] T0  GROUP BY T0.[TransId], T0.[TransRowId]) T2  ON  T0.[TransId] = T2.[TransId]  " &
    "AND  T0.[Line_ID] = T2.[TransRowId]   WHERE T0.[ShortName] ='" & Session("RazCode") & "'  " &
     "AND  (T0.[BalDueDeb] &lt; &gt; 0  " &
    "OR  T0.[BalDueCred]  &lt; &gt; 0 " &
    "OR  T0.[BalFcDeb]  &lt; &gt; 0 " &
    "OR  T0.[BalFcCred]  &lt; &gt; 0 ) "



            Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)
            Dim doc As New XmlDocument()
            doc.LoadXml(Respuesta.InnerXml)
            System.Diagnostics.Debug.Write(Respuesta.InnerXml & vbCrLf)
            Dim root As XmlNode = doc.FirstChild
            root = root.LastChild
            root = root.FirstChild
            If root.HasChildNodes Then


                Dim i As Integer
                For i = 0 To root.ChildNodes.Count - 1
                    Select Case root.ChildNodes(i).Name
                        Case "Saldo"
                            ' Session("currency") = root.ChildNodes(i).InnerText
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

                            tCell = New TableHeaderCell()
                            tCell.Text = "Total:"
                            tRow.Cells.Add(tCell)

                            tCell = New TableCell()
                            'tCell.Text = Session("currency") + " " + String.Format("{0:N}", Convert.ToDouble(root.ChildNodes(i).InnerText))
                            tCell.Text = Session("MS") + " " + String.Format("{0:N}", totalSuma)
                            tCell.HorizontalAlign = HorizontalAlign.Right


                            tRow.Cells.Add(tCell)

                            Table1.Rows.Add(tRow)

                    End Select
                Next i
            End If


            If Table1.Rows.Count = 1 Then
                Table1.Visible = False
            End If



        Catch ex As Exception
            System.Diagnostics.Debug.Write(ex.Message & vbCrLf)
            Table1.Visible = False
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
End Class
