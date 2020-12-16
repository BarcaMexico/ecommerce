<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmPedidos.aspx.vb" Inherits="frmPedidos" %>

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
            <a  href ="frmInicio.aspx"><i class="fa fa-home fa-fw"></i></a>>
            <a    href ="#">Pedidos</a>
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

                        <div class="input-group" style="max-width: 300px">
                            <%--barra de busqueda--%>
                            <input type="text" runat="server" id="txtBusquedaPed" class="form-control" onkeypress="runScript(event)" placeholder="Pedido..." />
                            <span class="input-group-btn">
                                <button id="btnBuscarOrden" runat="server" class="btn btn-default" type="button"><i class="fa fa-search"></i></button>
                            </span>
                        </div>
                        <br />

                        <asp:Table ID="Table1" class="table abc" runat="server" Style="width: 100%; min-width: 600px">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell>Fecha</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Numero de pedido</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Comentarios</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Importe</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Ver documento</asp:TableHeaderCell>
                        </asp:TableHeaderRow> 
                    </asp:Table> 
                </div> 

                 <div style="text-align: center;" runat="server" id="PaginationDiv"> 
            </div> 
                 
            <input type="text" runat="server" id="actuacan" clientidmode="Static" style="display: none" />
            <input type="text" runat="server" id="actuaid" clientidmode="Static" style="display: none" />
            <button id="secretbutton" runat="server" class="btn btn-default" type="button" style="display: none">dois</button>
             <button id="botonpagina" runat="server" class="btn btn-default" type="button" style="display: none">dois</button>
               <input type="text" runat="server" id="idpagina" clientidmode="Static" style="display: none" />
              
            <script>
                
                function ver(id) { 
                    document.getElementById("actuacan").value = 'dis';
                    document.getElementById("actuaid").value = id.substring(3, id.length);
                    document.getElementById('<%= secretbutton.ClientID%>').click();
                    //reload(); 
                }
                function discre(id) {
                    document.getElementById("idpagina").value = id.substring(3, id.length);
                    document.getElementById('<%= botonpagina.ClientID%>').click();
                      //reload(); 
                    }
            </script>












        </div>

    </div>





</asp:Content>

