Imports Modulo

Imports System.Xml
Imports System.Globalization
Imports System.Drawing

Partial Class frmPedidosView
    Inherits System.Web.UI.Page
    Public ws As DIS.DIServer
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            Dim myvar As String = Session("pedidoview")

            'Busqueda de facturas venciadas Balance
            Dim tRow As New TableRow()
            Dim tCell As New TableCell()

            ws = New DIS.DIServer

            Try
                Dim fechastring = Today.ToString("yyyy-MM-dd")
                Dim Respuesta As XmlNode
                Dim Respuesta2 As XmlNode
                Dim strQryDestino As String = ""

                Respuesta = ws.ReadObjects(Session("Token"), "oOrders", "DocEntry", myvar)
                viewCliente.Text = ReadXML(Respuesta.InnerXml, "CardCode")
                viewCardName.Text = ReadXML(Respuesta.InnerXml, "CardName")
                viewDocdate.Text = DateTime.ParseExact(ReadXML(Respuesta.InnerXml, "Docdate"), "yyyyMMdd", CultureInfo.InvariantCulture)

                viewDocDuedate.Text = DateTime.ParseExact(ReadXML(Respuesta.InnerXml, "DocDuedate"), "yyyyMMdd", CultureInfo.InvariantCulture)

                viewNum.Text = ReadXML(Respuesta.InnerXml, "DocNum")

                viewmoneda.Text = ReadXML(Respuesta.InnerXml, "DocCurrency")

                'Campos Definidos 
                Try
                    txtCC_U_TM2.Value = ReadXML(Respuesta.InnerXml, "U_TM2")
                    txtCC_U_TKG.Value = ReadXML(Respuesta.InnerXml, "U_TKG")
                    txtCC_U_COMXMILLAR.Value = ReadXML(Respuesta.InnerXml, "U_COMXMILLAR")
                    txtCC_U_DESCUENTO.Value = ReadXML(Respuesta.InnerXml, "U_DESCUENTO")
                    txtCC_U_DREAL.Value = ReadXML(Respuesta.InnerXml, "U_DREAL")

                    Respuesta2 = ws.ExecuteSQL(Session("Token"), "select U_COMISION from ORDR where docentry = " & myvar)
                    txtCC_U_COMISION.Value = ReadXML(Respuesta2.InnerXml, "U_COMISION")

                    txtCC_U_PORCOMISION.Value = ReadXML(Respuesta.InnerXml, "U_PORCOMISION")
                    txtCC_U_PK.Value = ReadXML(Respuesta.InnerXml, "U_PK")
                    txtCC_U_PKR.Value = ReadXML(Respuesta.InnerXml, "U_PKR")
                    txtCC_U_COSTESP.Value = ReadXML(Respuesta.InnerXml, "U_COSTESP")

                    '11-04-2019 HRS - Agregar Orden de Compra (U_PEDIDONO)
                    txtCC_U_pedidono.Value = ReadXML(Respuesta.InnerXml, "U_PEDIDONO")
                Catch ex As Exception
                End Try

                'Destino
                Try
                    strQryDestino = "SELECT top 1 isnull(T0.ShipToCode,'') + ' - ' + isnull(T1.[StreetS],'') + ' ' + isnull(T1.BlockS,'') + ' ' + isnull(T1.CityS,'') AS 'Destino'"
                    strQryDestino = strQryDestino & " FROM ORDR T0 INNER JOIN RDR12 T1 ON T1.DocEntry = T0.DocEntry"
                    strQryDestino = strQryDestino & " WHERE T0.DocEntry = " & myvar
                    Respuesta2 = ws.ExecuteSQL(Session("Token"), strQryDestino)
                    txtCC_Destino.Value = ReadXML(Respuesta2.InnerXml, "Destino")
                Catch ex As Exception
                End Try

                Dim doc2 As New XmlDocument()
                doc2.LoadXml(Respuesta.InnerXml)
                System.Diagnostics.Debug.Write(Respuesta.InnerXml & vbCrLf)
                Dim root2 As XmlNode = doc2.FirstChild
                Dim rootin As XmlNode

                'root2 = root2.LastChild
                'root2 = root2.FirstChild

                ' root2 = root2.SelectNodes("/Document_Lines")
                root2 = root2.ChildNodes(2)

                If root2.HasChildNodes Then
                    Dim s As Integer
                    For s = 0 To root2.ChildNodes.Count - 1

                        rootin = root2.ChildNodes(s)
                        Dim ItemCode, ItemDescription, Quantity, Price, LineTotal, Currency, Pendientes As String

                        If rootin.HasChildNodes Then
                            Dim x As Integer
                            tRow = New TableRow()
                            For x = 0 To rootin.ChildNodes.Count - 1 
                                If Session("RazMON") = "USD" Then
                                    Select Case rootin.ChildNodes(x).Name

                                        Case "ItemCode" 
                                            ItemCode = rootin.ChildNodes(x).InnerText 
                                        Case "ItemDescription"
                                            ItemDescription = rootin.ChildNodes(x).InnerText
                                        Case "Quantity"
                                            Quantity = rootin.ChildNodes(x).InnerText
                                        Case "Price"
                                            Price = rootin.ChildNodes(x).InnerText
                                        Case "RowTotalFC"
                                            LineTotal = rootin.ChildNodes(x).InnerText
                                        Case "Currency"
                                            Currency = rootin.ChildNodes(x).InnerText
                                        Case "RemainingOpenQuantity"
                                            Pendientes = rootin.ChildNodes(x).InnerText

                                    End Select
                                Else
                                    Select Case rootin.ChildNodes(x).Name

                                        Case "ItemCode"

                                            ItemCode = rootin.ChildNodes(x).InnerText

                                        Case "ItemDescription"
                                            ItemDescription = rootin.ChildNodes(x).InnerText
                                        Case "Quantity"
                                            Quantity = rootin.ChildNodes(x).InnerText
                                        Case "Price"
                                            Price = rootin.ChildNodes(x).InnerText
                                        Case "LineTotal"
                                            LineTotal = rootin.ChildNodes(x).InnerText
                                        Case "Currency"
                                            Currency = rootin.ChildNodes(x).InnerText
                                        Case "RemainingOpenQuantity"
                                            Pendientes = rootin.ChildNodes(x).InnerText

                                    End Select
                                End If 
                            Next x
                             

                            tCell = New TableCell()
                            tCell.Text = ItemCode
                            tRow.Cells.Add(tCell)

                            tCell = New TableCell()
                            tCell.Text = ItemDescription
                            tRow.Cells.Add(tCell)

                            tCell = New TableCell()
                            tCell.Text = CDbl(Quantity)
                            tRow.Cells.Add(tCell)

                            tCell = New TableCell()
                            tCell.Text = CDbl(Quantity - Pendientes)
                            tRow.Cells.Add(tCell)

                            tCell = New TableCell()
                            tCell.Text = Currency + " " + String.Format("{0:N}", Convert.ToDouble(Price))
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            tRow.Cells.Add(tCell)

                            tCell = New TableCell()
                            tCell.Text = Session("RazMON") + " " + String.Format("{0:N}", Convert.ToDouble(LineTotal))
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            tRow.Cells.Add(tCell)


                            If CInt(Quantity - Pendientes) > 0 Then
                                If CInt(Quantity - Pendientes) = Quantity Then
                                    tRow.BackColor = ColorTranslator.FromHtml("#2cae19")
                                Else
                                    tRow.BackColor = ColorTranslator.FromHtml("#e3db20")
                                End If 
                            Else
                                tRow.BackColor = ColorTranslator.FromHtml("#e2776b")
                            End If

                            Table1.Rows.Add(tRow)

                        End If
                    Next s
                End If


                tRow = New TableRow()
                tCell = New TableCell()
                tCell.Text = " "

                tRow.Cells.Add(tCell)
                tCell = New TableCell()
                tCell.Text = " "

                tRow.Cells.Add(tCell)
                tCell = New TableCell()
                tCell.Text = " "
                tRow.Cells.Add(tCell)
                tCell = New TableCell()
                tCell.Text = " "
                tRow.Cells.Add(tCell)
                tCell = New TableHeaderCell()
                tCell.Text = "Subtotal"
                tRow.Cells.Add(tCell)
                tCell = New TableCell()

                If Session("RazMON") = "USD" Then 
                    tCell.Text = Session("RazMON") + " " + String.Format("{0:N}", Convert.ToDouble(ReadXML(Respuesta.InnerXml, "DocTotalFc") - Convert.ToDouble(ReadXML(Respuesta.InnerXml, "VatSumFc"))))
                Else
                    tCell.Text = Session("RazMON") + " " + String.Format("{0:N}", Convert.ToDouble(ReadXML(Respuesta.InnerXml, "DocTotal") - Convert.ToDouble(ReadXML(Respuesta.InnerXml, "VatSum"))))
                End If

                tCell.HorizontalAlign = HorizontalAlign.Right
                tRow.Cells.Add(tCell)
                Table1.Rows.Add(tRow) 

                tRow = New TableRow()
                tCell = New TableCell()
                tCell.Text = " "
                tRow.Cells.Add(tCell)
                tCell = New TableCell()
                tCell.Text = " "
                tRow.Cells.Add(tCell)
                tCell = New TableCell()
                tCell.Text = " "
                tRow.Cells.Add(tCell)

                tCell = New TableCell()
                tCell.Text = " "
                tRow.Cells.Add(tCell)
                tCell = New TableHeaderCell()
                tCell.Text = "Impuesto"
                tRow.Cells.Add(tCell)
                tCell = New TableCell()



                If Session("RazMON") = "USD" Then
                    tCell.Text = Session("RazMON") + " " + String.Format("{0:N}", Convert.ToDouble(ReadXML(Respuesta.InnerXml, "VatSumFc")))
                Else
                    tCell.Text = Session("RazMON") + " " + String.Format("{0:N}", Convert.ToDouble(ReadXML(Respuesta.InnerXml, "VatSum")))
                End If

                tCell.HorizontalAlign = HorizontalAlign.Right


                tRow.Cells.Add(tCell)
                Table1.Rows.Add(tRow)

                tRow = New TableRow()
                tCell = New TableCell()
                tCell.Text = " "

                tRow.Cells.Add(tCell)
                tCell = New TableCell()
                tCell.Text = " "

                tRow.Cells.Add(tCell)
                tCell = New TableCell()
                tCell.Text = " "
                tRow.Cells.Add(tCell)
                tCell = New TableCell()
                tCell.Text = " "
                tRow.Cells.Add(tCell)
                tCell = New TableHeaderCell()
                tCell.Text = "Total"
                tRow.Cells.Add(tCell)
                tCell = New TableCell()

                If Session("RazMON") = "USD" Then
                    tCell.Text = Session("RazMON") + " " + String.Format("{0:N}", Convert.ToDouble(ReadXML(Respuesta.InnerXml, "DocTotalFc")))
                Else
                    tCell.Text = Session("RazMON") + " " + String.Format("{0:N}", Convert.ToDouble(ReadXML(Respuesta.InnerXml, "DocTotal")))
                End If

                tCell.HorizontalAlign = HorizontalAlign.Right
                tRow.Cells.Add(tCell)
                Table1.Rows.Add(tRow)




            Catch ex As Exception
                System.Diagnostics.Debug.Write(ex.Message + " errorrr" & vbCrLf)
            End Try
        End If

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
