Imports Modulo

Imports System.Xml
Partial Class frmInicio
    Inherits System.Web.UI.Page
    Public ws As DIS.DIServer
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load 
        'Busqueda de facturas venciadas Balance
        Dim tRow As New TableRow() 
        Dim tCell As New TableCell() 
        Try
            ws = New DIS.DIServer

            Dim Respuesta As XmlNode
            Respuesta = ws.ExecuteSQL(Session("Token"), "Select top 1 Currency,Balance,BalanceFC ,OrdersBal ,OrderBalFC from OCRD where CardCode='" & Session("RazCode") & "'")
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
                        Case "Currency"
                            Session("currency") = root.ChildNodes(i).InnerText
                            If Session("currency") = "##" Or Session("currency") = Session("ML") Then
                                Session("currency") = Session("MS") 
                            End If
                             
                            tCell = New TableHeaderCell()
                            tCell.Text = "Documentos"
                            tRow.Cells.Add(tCell)
                            tCell = New TableHeaderCell()
                            tCell.Text = "Saldo" + " " + Session("ML")
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            tRow.Cells.Add(tCell)
                            tCell = New TableHeaderCell()
                            tCell.Text = "Saldo" + " " + Session("currency")
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            tRow.Cells.Add(tCell)
                            Table1.Rows.Add(tRow)
                            tRow = New TableRow()
                            tCell = New TableCell()
                            tCell.Text = ("Facturas vencidas")
                            tRow.Cells.Add(tCell)
                        Case "Balance" 
                            tCell = New TableCell()
                            tCell.Text = String.Format("{0:N}", Convert.ToDouble(root.ChildNodes(i).InnerText)) + " " + Session("ML")
                            tRow.Cells.Add(tCell)
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            Table1.Rows.Add(tRow) 
                        Case "BalanceFC"
                            tCell = New TableCell()
                            tCell.Text = String.Format("{0:N}", Convert.ToDouble(root.ChildNodes(i).InnerText)) 
                            tCell.Text = tCell.Text + " " + Session("currency")
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            tRow.Cells.Add(tCell)
                            Table1.Rows.Add(tRow)
                        Case "OrdersBal"
                            Session("OrdersBal") = root.ChildNodes(i).InnerText
                        Case "OrderBalFC"
                            Session("OrdersBalFC") = root.ChildNodes(i).InnerText

                    End Select
                Next i
            End If

            'Busqueda de facturas por vencer fecha 0 a 15.
            Dim today As Date = DateTime.Now
            Dim answer As Date = today.AddDays(15)
            Dim fechastring = answer.ToString("yyyyMMdd")
            Dim ahora = DateTime.Now.ToString("yyyyMMdd")
            Dim sqlquery As String = " select sum(DocTotal) as suma,sum(DocTotalFC) as sumafc  from OINV where DocStatus='O' AND CardCode ='" & Session("RazCode") & "' and DocDueDate &lt;='" & fechastring & "' and DocDueDate &gt;='" & ahora & "'"
            Respuesta = ws.ExecuteSQL(Session("Token"), sqlquery)
            doc = New XmlDocument()
            doc.LoadXml(Respuesta.InnerXml)
            System.Diagnostics.Debug.Write(Respuesta.InnerXml & vbCrLf)
            root = doc.FirstChild
            root = root.LastChild
            root = root.FirstChild
            If root.HasChildNodes Then
                tRow = New TableRow()
                tCell = New TableCell()
                tCell.Text = ("Facturas próximas a vencer 0-15 días")
                tRow.Cells.Add(tCell)
                Dim i As Integer
                For i = 0 To root.ChildNodes.Count - 1
                    Select Case root.ChildNodes(i).Name
                        Case "suma"

                            tCell = New TableCell()
                            tCell.Text = String.Format("{0:N}", Convert.ToDouble(root.ChildNodes(i).InnerText)) + " " + Session("ML")
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            tRow.Cells.Add(tCell)
                            Table1.Rows.Add(tRow)
                        Case "sumafc"

                            tCell = New TableCell()
                            tCell.Text = String.Format("{0:N}", Convert.ToDouble(root.ChildNodes(i).InnerText)) + " " + Session("currency")
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            tRow.Cells.Add(tCell)
                            Table1.Rows.Add(tRow)
                    End Select
                Next i
            End If

            'Busqueda de facturas por vencer fecha 15  a 30.
            today = DateTime.Now
            answer = today.AddDays(15)
            fechastring = answer.ToString("yyyyMMdd")

            answer = today.AddDays(30)
            Dim fechastring2 As String = answer.ToString("yyyyMMdd")

            sqlquery = " select   sum(DocTotal) as suma,sum(DocTotalFC) as sumafc   from OINV where DocStatus='O' AND CardCode ='" & Session("RazCode") & "' and DocDueDate &gt;='" & fechastring & "' and   DocDueDate &lt;='" & fechastring2 & "'"
            Respuesta = ws.ExecuteSQL(Session("Token"), sqlquery)
            doc = New XmlDocument()
            doc.LoadXml(Respuesta.InnerXml)
            System.Diagnostics.Debug.Write(Respuesta.InnerXml & vbCrLf)
            root = doc.FirstChild
            root = root.LastChild
            root = root.FirstChild
            If root.HasChildNodes Then
                tRow = New TableRow()
                tCell = New TableCell()
                tCell.Text = ("Facturas próximas  a vencer 15-30 días")
                tRow.Cells.Add(tCell)
                Dim i As Integer
                For i = 0 To root.ChildNodes.Count - 1
                    Select Case root.ChildNodes(i).Name

                        Case "suma"

                            tCell = New TableCell()
                            tCell.Text = String.Format("{0:N}", Convert.ToDouble(root.ChildNodes(i).InnerText)) + " " + Session("ML")
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            tRow.Cells.Add(tCell)
                            Table1.Rows.Add(tRow)
                        Case "sumafc"

                            tCell = New TableCell()
                            tCell.Text = String.Format("{0:N}", Convert.ToDouble(root.ChildNodes(i).InnerText)) + " " + Session("currency")
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            tRow.Cells.Add(tCell)
                            Table1.Rows.Add(tRow)

                    End Select
                Next i
            End If

            'Busqueda de facturas por vencer fecha 15  a 30.
            today = DateTime.Now
            answer = today.AddDays(30)
            fechastring = answer.ToString("yyyyMMdd")

            sqlquery = " select sum(DocTotal) as suma,sum(DocTotalFC) as sumafc  from OINV where DocStatus='O' AND CardCode ='" & Session("RazCode") & "' and DocDueDate &gt;='" & fechastring & "'"
            Respuesta = ws.ExecuteSQL(Session("Token"), sqlquery)
            doc = New XmlDocument()
            doc.LoadXml(Respuesta.InnerXml)
            System.Diagnostics.Debug.Write(Respuesta.InnerXml & vbCrLf)
            root = doc.FirstChild
            root = root.LastChild
            root = root.FirstChild
            If root.HasChildNodes Then
                tRow = New TableRow()
                tCell = New TableCell()
                tCell.Text = ("Facturas próximas a vencer 30 días en adelante")
                tRow.Cells.Add(tCell)
                Dim i As Integer
                For i = 0 To root.ChildNodes.Count - 1
                    Select Case root.ChildNodes(i).Name
                        Case "suma"

                            tCell = New TableCell()
                            tCell.Text = String.Format("{0:N}", Convert.ToDouble(root.ChildNodes(i).InnerText)) + " " + Session("ML")
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            tRow.Cells.Add(tCell)
                            Table1.Rows.Add(tRow)
                        Case "sumafc"

                            tCell = New TableCell()
                            tCell.Text = String.Format("{0:N}", Convert.ToDouble(root.ChildNodes(i).InnerText)) + " " + Session("currency")
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            tRow.Cells.Add(tCell)
                            Table1.Rows.Add(tRow)
                    End Select
                Next i
            End If

            'agrega el saldo de los pedidos
            tRow = New TableRow()
            tCell = New TableCell()
            tCell.Text = ("Pedidos")
            tRow.Cells.Add(tCell)

            tCell = New TableCell()
            tCell.HorizontalAlign = HorizontalAlign.Right
            tCell.Text = String.Format("{0:N}", Convert.ToDouble(Session("OrdersBal"))) + " " + Session("ML")
            tRow.Cells.Add(tCell)
            Table1.Rows.Add(tRow)

             
            tCell = New TableCell()
            tCell.HorizontalAlign = HorizontalAlign.Right
            tCell.Text = String.Format("{0:N}", Convert.ToDouble(Session("OrdersBalFC"))) + " " + Session("currency")
            tRow.Cells.Add(tCell)

            Table1.Rows.Add(tRow)



            'anticio
            Respuesta = ws.ExecuteSQL(Session("Token"), "Select sum(DocTotal) as suma,sum(DocTotalFC) as sumafc from ODPI where CardCode='" & Session("RazCode") & "' and DocStatus='O'")

            doc = New XmlDocument()
            doc.LoadXml(Respuesta.InnerXml)
            System.Diagnostics.Debug.Write(Respuesta.InnerXml & vbCrLf)
            root = doc.FirstChild
            root = root.LastChild
            root = root.FirstChild
            If root.HasChildNodes Then
                tRow = New TableRow()
                tCell = New TableCell()
                tCell.Text = ("Anticipos")
                tRow.Cells.Add(tCell)
                Dim i As Integer
                For i = 0 To root.ChildNodes.Count - 1

                    Select Case root.ChildNodes(i).Name
                        Case "suma"

                            tCell = New TableCell()
                            tCell.Text = String.Format("{0:N}", Convert.ToDouble(root.ChildNodes(i).InnerText)) + " " + Session("ML")
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            tRow.Cells.Add(tCell)
                            Table1.Rows.Add(tRow)
                        Case "sumafc"
                            tCell = New TableCell()
                            tCell.Text = String.Format("{0:N}", Convert.ToDouble(root.ChildNodes(i).InnerText)) + " " + Session("currency")
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            tRow.Cells.Add(tCell)
                            Table1.Rows.Add(tRow)

                    End Select





                Next i
            End If


        Catch ex As Exception

        End Try
    End Sub
End Class
