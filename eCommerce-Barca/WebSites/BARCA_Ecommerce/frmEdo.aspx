<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmEdo.aspx.vb" Inherits="frmEdo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



    <script src="Scripts/bootstrap-datepicker.min.js"></script>

    <link href="Content/bootstrap-datepicker.min.css" rel="stylesheet" />
     <style runat="server" id="estilo"></style>

    <div id="page-wrapper">
        <div class="row">

            <link href="Content/tablanocolor.css" rel="stylesheet" />

            

            <div class="titulolink">
                <br />
                <a href="frmInicio.aspx"><i class="fa fa-home fa-fw"></i></a>>
            <a href="#">Estado de cuenta</a>
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
                    $('.input-group.date').datepicker({

                        autoclose: true, format: 'yyyy-mm-dd'

                    });
                </script>






                <div style="width: auto; overflow-x: auto;">
                    <div style="min-width: 600px;">

                        <asp:Table ID="Table1" class="table abc" runat="server" Style="width: 100%; min-width: 600px">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell>Fecha Contabilización</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Numero de origen</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Cuenta de contrapartida</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Información  Detallada</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Saldo Vencido</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Saldo Vencido FC</asp:TableHeaderCell>
                            </asp:TableHeaderRow>

                        </asp:Table>

                    </div>



                </div>



            </div>

        </div>
</asp:Content>

