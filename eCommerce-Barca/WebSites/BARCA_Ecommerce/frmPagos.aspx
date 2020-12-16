<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmPagos.aspx.vb" Inherits="frmPagos" %>

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
            <a    href ="#">Pagos</a>
            <br />
            <br />
            </div>

            <div>
                <div class="row row-centered">
                    <div class="col-xs-6 col-centered col-fixed">
                        <div style="width: 20px; margin-left: auto; margin-right: auto">De:</div>
                        <div class="input-group date" style="width: 200px; margin-left: auto; margin-right: auto">
                            <input type="text" class="form-control datepicker" runat="server" id="fecha1"><span class="input-group-addon"><i class="glyphicon glyphicon-th"></i></span>
                        </div>
                    </div>
                    <div class="col-xs-6 col-centered col-fixed">
                        <div style="width: 20px; margin-left: auto; margin-right: auto">A:</div>
                        <div class="input-group date" style="width: 200px; margin-left: auto; margin-right: auto">
                            <input type="text" class="form-control datepicker" runat="server" id="fecha2"><span class="input-group-addon"><i class="glyphicon glyphicon-th"></i></span>
                        </div>
                    </div>
                </div>



                <div class="row row-centered">
                    <div class="col-xs-12 col-centered col-fixed">
                        <div style="width: 68px; margin-left: auto; margin-right: auto">


                            <button runat="server" id="buscaredo" type="button" class="btn btn-default" aria-label="Left Align">
                                Buscar
                            </button>
                        </div>


                    </div>
                </div>






                <script>

                    function discre(id) {
                        document.getElementById("idpagina").value = id.substring(3, id.length);
                        document.getElementById('<%= botonpagina.ClientID%>').click();
                        //reload(); 
                    }

                    $('.input-group.date').datepicker({

                        autoclose: true, format: 'yyyy-mm-dd'

                    });
                </script>



 


             <div style="width: auto; overflow-x: auto;"> 
                    <div style="min-width: 600px;"> 

                        <asp:Table ID="Table1" class="table abc" runat="server" Style="width: 100%; min-width: 600px">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell>Fecha de pago</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Pago</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Factura</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Moneda</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Total</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Importe</asp:TableHeaderCell> 
                            </asp:TableHeaderRow>

                        </asp:Table>

                    </div>



                </div>

                             <input type="text" runat="server" id="idpagina" clientidmode="Static" style="display: none" />
                 <button id="botonpagina" runat="server" class="btn btn-default" type="button"   style="display: none">dois</button>

                  <div style="text-align: center;" runat="server" id="PaginationDiv">

            </div>

        </div>
</asp:Content>

