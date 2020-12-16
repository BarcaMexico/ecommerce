<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmEmbarquesDiscrepAdmin.aspx.vb" Inherits="frmEmbarquesDiscrepAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    
    <script src="Scripts/bootstrap-datepicker.min.js"></script>
    <link href="Content/bootstrap-datepicker.min.css" rel="stylesheet" />
    <link href="Content/tablanocolor.css" rel="stylesheet" />
    <style runat="server" id="estilo"></style>
    <div id="page-wrapper">
        <div class="row">

            <div class="titulolink">
                <br />
                <a href="frmdiscreadmin.aspx"><i class="fa fa-home fa-fw"></i></a>>
            <a href="frmdiscreadmin.aspx">Discrepancias</a>>
            <a href="#">Detalle</a>
                <br />
                <br />
            </div> 
            <div style="width: auto; overflow-x: auto;">

                <div style="min-width: 600px;">

                    <asp:Table ID="Table1" class="table abc" runat="server" Style="width: 100%; min-width: 600px">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell>Código</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Producto</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Cantidad</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Unidad</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </div>
            </div>
            <div style="width: auto; overflow-x: auto;">
                <div style="min-width: 600px;">
                    <div class="titulolink">
                        <br />
                        <a href="#">Discrepancia</a>
                    </div>
                    <asp:Table ID="Table2" class="table abc" runat="server" Style="width: 100%; min-width: 600px">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell>Articulo</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Cantidad</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Observacion</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Imagen</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </div>
            </div>
            <br />
            <br />
        </div>
    </div>
     
</asp:Content>

