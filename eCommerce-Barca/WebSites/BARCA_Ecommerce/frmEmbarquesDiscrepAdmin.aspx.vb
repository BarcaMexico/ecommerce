Imports Modulo

Imports System.Xml
Imports System.Globalization 
Partial Class frmEmbarquesDiscrepAdmin
    Inherits System.Web.UI.Page
    Public ws As DIS.DIServer
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load



        If Not Page.IsPostBack Then

            Dim twoDarray(20, 10) As String



            'Busqueda de facturas venciadas Balance
            Dim tRow As New TableRow()
            Dim tCell As New TableCell()

            Dim factura As ArrayList = New ArrayList






            ws = New DIS.DIServer



            Try

                Dim Respuesta As XmlNode

                Dim sqldato As String = "   select DocEntry,DocNum ,DocDate  from ODLN where DocNum  ='" & Session("DocNumDis") & "' "
                Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)


                sqldato = "   select ItemCode ,Dscription ,Quantity ,unitMsr from DLN1 where DocEntry  ='" & ReadXML(Respuesta.InnerXml, "DocEntry") & "' "
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

                                    Case "ItemCode"
                                        tCell = New TableCell()
                                        tCell.Text = rootin.ChildNodes(x).InnerText
                                        tRow.Cells.Add(tCell)

                                    Case "Dscription"
                                        tCell = New TableCell()
                                        tCell.Text = rootin.ChildNodes(x).InnerText
                                        tRow.Cells.Add(tCell)

                                    Case "Quantity"
                                        tCell = New TableCell()
                                        tCell.Text = CInt(rootin.ChildNodes(x).InnerText)
                                        tRow.Cells.Add(tCell)

                                    Case "unitMsr"
                                        tCell = New TableCell()
                                        tCell.Text = rootin.ChildNodes(x).InnerText
                                        tRow.Cells.Add(tCell)



                                End Select
                            Next x


                            Table1.Rows.Add(tRow)

                        End If
                    Next s
                End If

                sqldato = "  select art,can,obs,img,id from  [Ecom].[dbo].[discrepancias] where fac='" & Session("DocNumDis") & "' and edo='1'  "
                Respuesta = ws.ExecuteSQL(Session("Token"), sqldato)

                If ReadXML(Respuesta.InnerXml, "art") <> "" Then

                    doc2 = New XmlDocument()
                    doc2.LoadXml(Respuesta.InnerXml)
                    System.Diagnostics.Debug.Write(Respuesta.InnerXml & vbCrLf)
                    root2 = doc2.FirstChild
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

                                        Case "art"
                                            tCell = New TableCell()
                                            tCell.Text = rootin.ChildNodes(x).InnerText
                                            tRow.Cells.Add(tCell)
                                            factura.Add(rootin.ChildNodes(x).InnerText)
                                        Case "can"
                                            tCell = New TableCell()
                                            tCell.Text = rootin.ChildNodes(x).InnerText
                                            tRow.Cells.Add(tCell)

                                        Case "obs"
                                            tCell = New TableCell()
                                            tCell.Text = rootin.ChildNodes(x).InnerText
                                            tRow.Cells.Add(tCell)

                                        Case "img"
                                            tCell = New TableCell()
                                            tCell.Text = "<a href='data:image/png;base64," & rootin.ChildNodes(x).InnerText & "' target='_blank'><img alt='' src='data:image/png;base64," & rootin.ChildNodes(x).InnerText & "' width='50' height='50'   /></a>"
                                            'tCell.Text = rootin.ChildNodes(x).InnerText.Length
                                            tCell.HorizontalAlign = HorizontalAlign.Center
                                            tRow.Cells.Add(tCell)
                                      
                                    End Select
                                Next x

                                Table2.Rows.Add(tRow)

                            End If
                        Next s
                    End If
                Else
                    Table2.Style("display") = "none"

                End If

                ' factura = CType(Session("factura"), ArrayList)

                Session("factura") = factura

            Catch ex As Exception
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
     

    Public Overloads Shared Function ResizeImage(bmSource As Drawing.Bitmap, TargetWidth As Int32, TargetHeight As Int32) As Drawing.Bitmap
        Dim bmDest As New Drawing.Bitmap(TargetWidth, TargetHeight, Drawing.Imaging.PixelFormat.Format32bppArgb)

        Dim nSourceAspectRatio = bmSource.Width / bmSource.Height
        Dim nDestAspectRatio = bmDest.Width / bmDest.Height

        Dim NewX = 0
        Dim NewY = 0
        Dim NewWidth = bmDest.Width
        Dim NewHeight = bmDest.Height

        If nDestAspectRatio = nSourceAspectRatio Then
            'same ratio
        ElseIf nDestAspectRatio > nSourceAspectRatio Then
            'Source is taller
            NewWidth = Convert.ToInt32(Math.Floor(nSourceAspectRatio * NewHeight))
            NewX = Convert.ToInt32(Math.Floor((bmDest.Width - NewWidth) / 2))
        Else
            'Source is wider
            NewHeight = Convert.ToInt32(Math.Floor((1 / nSourceAspectRatio) * NewWidth))
            NewY = Convert.ToInt32(Math.Floor((bmDest.Height - NewHeight) / 2))
        End If

        Using grDest = Drawing.Graphics.FromImage(bmDest)
            With grDest
                .CompositingQuality = Drawing.Drawing2D.CompositingQuality.HighQuality
                .InterpolationMode = Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
                .PixelOffsetMode = Drawing.Drawing2D.PixelOffsetMode.HighQuality
                .SmoothingMode = Drawing.Drawing2D.SmoothingMode.AntiAlias
                .CompositingMode = Drawing.Drawing2D.CompositingMode.SourceOver

                .DrawImage(bmSource, NewX, NewY, NewWidth, NewHeight)
            End With
        End Using

        Return bmDest
    End Function


End Class
