<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmEmbarquesDiscrep.aspx.vb" Inherits="frmEmbarquesDiscrep" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



    <script src="Scripts/bootstrap-datepicker.min.js"></script>

    <link href="Content/bootstrap-datepicker.min.css" rel="stylesheet" />


    <link href="Content/tablanocolor.css" rel="stylesheet" />
    <style runat="server" id="estilo"></style>


       <style> 
        .fileUpload {
            position: relative;
            overflow: hidden;
            margin: 10px;
        }

            .fileUpload input.upload {
                position: absolute;
                top: 0;
                right: 0;
                margin: 0;
                padding: 0;
                font-size: 20px;
                cursor: pointer;
                opacity: 0;
                filter: alpha(opacity=0);
            }
    </style>


    <div id="page-wrapper">
        <div class="row">



            <div class="titulolink">
                <br />
                <a href="frmInicio.aspx"><i class="fa fa-home fa-fw"></i></a>>
            <a href="frmEmbarques">Confirmar pedido</a>>
            <a href="#">Discrepancia</a>
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
                            <asp:TableHeaderCell>Eliminar</asp:TableHeaderCell>

                        </asp:TableHeaderRow>
                    </asp:Table>
                </div>
            </div>





            <div class="row">
                <div class="col-xs-12 col-md-6    ">

                    <div>
                        Código:<br />

                        <asp:DropDownList ID="DropDownList1" runat="server" Style="width: 300px;">
                        </asp:DropDownList>


                    </div>
                    <div>
                        Discrepancia:<br />

                        <asp:DropDownList ID="DropDownList2" runat="server" Style="width: 300px;">
                            <asp:ListItem>Dañado</asp:ListItem>
                            <asp:ListItem>Faltante</asp:ListItem>
                            <asp:ListItem>Otro</asp:ListItem>
                        </asp:DropDownList>

                    </div>
                    <div>
                        Cantidad:<br />
                        <input type="number" runat="server" id="Cantidad" clientidmode="Static" style="width: 300px;" />
                    </div>
                    Imagen:<br />
                    <input type="hidden" name="MAX_FILE_SIZE" value="4194304" />
                    <div class="fileUpload btn btn-primary">
                        <span>Examinar</span>
                        <input id="File1" runat="server" type="file" class="upload"  accept="image/*" /> 
                        <input type="file" class="upload" />
                    </div>
                </div>



                <div class="col-xs-12 col-md-6   ">

                    <div style="">
                        Observación:
                 <textarea id="TextArea1" cols="20" rows="2" runat="server" class="form-control"></textarea>
                    </div>
                    <br />
                    <div >
                        <button id="enviardisc" runat="server" class="btn btn-default" type="button">Enviar Discrepancia</button>
                    </div>
                </div>

            </div>


            <br />
            <br />



            <input type="text" runat="server" id="actuaid" clientidmode="Static" style="display: none" />
            <button id="secretbutton" runat="server" class="btn btn-default" type="button" style="display: none">dois</button>
        </div>


        <script>
            function basura(id) {

                document.getElementById("actuaid").value = id.substring(3, id.length);
                document.getElementById('<%= secretbutton.ClientID%>').click();
                    //reload();
                }


        </script>



        </div>

    </div>





</asp:Content>

