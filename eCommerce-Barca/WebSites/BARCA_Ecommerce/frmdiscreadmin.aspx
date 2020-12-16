<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmdiscreadmin.aspx.vb" Inherits="frmdiscreadmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

        <script src="Scripts/bootstrap-datepicker.min.js"></script>

    <link href="Content/bootstrap-datepicker.min.css" rel="stylesheet" />


    <style runat="server" id="estilo"></style>



    <div id="page-wrapper">
        <div class="row">

            <link href="Content/tablanocolor.css" rel="stylesheet" />

           <div class="titulolink" >
             <br />
            <a  href ="frmdiscreadmin.aspx"><i class="fa fa-home fa-fw"></i></a>>
            <a    href ="#">Discrepancias</a>
            <br />
            <br />
            </div>



            <script>
                $('.input-group.date').datepicker({

                    autoclose: true, format: 'yyyy-mm-dd'

                });
            </script>

             

            <div style="width: auto; overflow-x: auto;"> 
                    <div style="min-width: 600px;"> 

                        <asp:Table ID="Table1" class="table abc" runat="server" Style="width: 100%; min-width: 600px">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell>Fecha</asp:TableHeaderCell>
                              <asp:TableHeaderCell>Entrega</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Pedido</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Socio</asp:TableHeaderCell> 
                            <asp:TableHeaderCell>Detalle</asp:TableHeaderCell>
                         
                        </asp:TableHeaderRow> 
                    </asp:Table> 
                </div> 
            </div> 








             <input type="text" runat="server" id="actuacan" clientidmode="Static" style="display: none"  />
            <input type="text" runat="server" id="actuaid" clientidmode="Static" style="display: none" />
            <button id="secretbutton" runat="server" class="btn btn-default" type="button" style="display: none">dois</button>


            
            <script>
               

                function discre(id) {

                    document.getElementById("actuacan").value = 'dis';
                    document.getElementById("actuaid").value = id.substring(3, id.length);
                    document.getElementById('<%= secretbutton.ClientID%>').click();
                    //reload(); 
                }
            </script>



        </div>

    </div>







</asp:Content>

