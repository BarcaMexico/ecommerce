<%@ Page Title="" Language="VB" MasterPageFile="~/master_blanco.master" AutoEventWireup="false" CodeFile="frmrazon.aspx.vb" Inherits="frmrazon" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="blanco" runat="Server">
    <link href="Content/loginestilo.css" rel="stylesheet" />

    <link href="Content/metisMenu.min.css" rel="stylesheet" />
    <link href="Content/sb-admin-2.css" rel="stylesheet" />
    <link href="Content/morris.css" rel="stylesheet" />
    <link href="Content/contentfix.css" rel="stylesheet" />
    <link href="Content/tablanocolor.css" rel="stylesheet" />
    <%--   <div class="row">
    <div class="Absolute-Center is-Responsive">

        <div class="text-center">
            hola mundo  <i class="fa fa-envelope fa-fw"></i> 
          </div>  

    </div>
    </div>--%>
   
    <div id="wrapper">

        <!-- Navigation -->
        <nav class="navbar navbar-default navbar-static-top" role="navigation" style="margin-bottom: 0; background: #fff">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class=" " href="">
                <img style="margin: auto; margin-left: 10px; max-width: 200px; max-height: 50px;" runat="server"  id="logo" ClientIDMode="Static"
                                src="http://www.siliconweek.com/wp-content/uploads/2012/08/microsoft-nuevo-logo-alta.png" /></a>
                
                    
                     </div>
            <!-- /.navbar-header -->

            <ul class="nav navbar-top-links navbar-right">

                 
                <!-- /.dropdown -->
                <li class="dropdown">
                    <a class="dropdown-toggle" data-toggle="dropdown" href="#">
                        <i class="fa fa-user fa-fw"></i><i class="fa fa-caret-down"></i>
                    </a>
                    <ul class="dropdown-menu dropdown-user">
                         
                        <li><a href="" runat="server" id="CerrarSesion"><i class="fa fa-sign-out fa-fw"></i>Cerrar Sesion </a>
                        </li>
                    </ul>
                    <!-- /.dropdown-user -->
                </li>
                <!-- /.dropdown -->
            </ul>
            <!-- /.navbar-top-links -->
            <link href="Content/menucolor.css" rel="stylesheet" />







            <div class="navbar-default sidebar" role="navigation">

                <div class="sidebar-nav navbar-collapse">
                    <ul class="nav" id="side-menu">



                        



                            <li class="infousu">
                                   



                                    <div class="row">
                                        <div class="col-xs-3 " style =" text-align :center ;">
                                            <a href="frmImagen.aspx">
                                                <img runat="server" id="divUserImg" class="imguser" src="" /></a>
                                        </div>
                                        <div class="col-xs-9  " runat="server" id="divUserName" style =" margin-top :18px; ">
                                             
                                        </div>
                                    </div>

                                </li>
                         
                       <%-- <li >
                            <br />
                            <a href="index.html" class="not-active" style="color: transparent; text-shadow: 0 0 3px #aaa;" ><i class="fa fa-home fa-fw"></i>Inicio</a>
                        </li>
                        <li>
                            <a href="index.html" class="not-active" style="color: transparent; text-shadow: 0 0 3px #aaa;"><i class="fa fa-check-square-o fa-fw"></i>Confirmar embarque</a>
                        </li>
                        <li>
                            <a href="index.html" class="not-active" style="color: transparent; text-shadow: 0 0 3px #aaa;"><i class="fa fa-list-ul fa-fw"></i>Estado de cuenta</a>
                        </li>
                        <li>
                            <a href="index.html" class="not-active" style="color: transparent; text-shadow: 0 0 3px #aaa;"><i class="fa fa-truck  fa-fw"></i>Pedidos</a>
                        </li>
                        <li>
                            <a href="index.html" class="not-active" style="color: transparent; text-shadow: 0 0 3px #aaa;"><i class="fa fa-file-text-o  fa-fw"></i>Historial de facturacion</a>
                        </li>
                        <li>
                            <a href="index.html" class="not-active" style="color: transparent; text-shadow: 0 0 3px #aaa;"><i class="fa fa-usd fa-fw"></i>Pagos</a>
                        </li>
                        <li>
                            <a href="index.html" class="not-active" style="color: transparent; text-shadow: 0 0 3px #aaa;"><i class="fa fa-shopping-cart  fa-fw"></i>Orden de compra</a>
                        </li>
                        <li>
                            <a href="index.html" class="not-active" style="color: transparent; text-shadow: 0 0 3px #aaa;"><i class="fa fa-envelope-o fa-fw"></i>Mensajes</a>
                        </li>--%>



                    </ul>
                </div>
                <!-- /.sidebar-collapse -->
            </div>
            <!-- /.navbar-static-side -->
        </nav>


        <%--Contenido de pagina --%>

        <div id="page-wrapper">
            <div class="row">
                <div class="Absolute-Center is-Responsive " id="centrado" style="">
                    <div class=" text-center">
                        <span style="font-size: 23px">Socio de negocio</span>
                        <hr    style="color: black;height: 1px; background-color: black; width:75%;" />
                    </div>

                    <div class="input-group">
                        <input type="text" runat="server" id="barrabusqueda" class="form-control" placeholder="Buscar..." />
                        <span class="input-group-btn">
                            <button id="btnbuscar" runat="server" class="btn btn-default" type="button"><i class="fa fa-search"></i></button>
                        </span>
                    </div>
                    <br />

                    <label>Busqueda por:</label>
                    <br />

                    <asp:RadioButtonList runat="server" ID="radios" RepeatLayout="Flow" CssClass="labels">
                        <asp:ListItem Text="Nombre" Value="Nombre" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Codigo" Value="Codigo"></asp:ListItem>
                    </asp:RadioButtonList>

                    <br />
                    <br />



                    <asp:ListBox ID="ListBox1" runat="server" Rows="5" Style="height: 200px !important; width: 600px !important; max-width: 100% !important;"></asp:ListBox>

                    <br /><br /> 
                    <div style="width: 115px; margin: 0 auto;">

                        <button type="button" runat="server" id="aceptarrazon" class="btn btn-default btn-lg">
                            Aceptar <i class="fa fa-sign-in"></i>

                        </button>
                    </div>


                </div>
            </div>
        </div>





    </div>
    <!-- /#page-wrapper -->
      <style runat="server" id="estilo"></style>
    </div>
    <!-- /#wrapper -->


    <script src="Scripts/metisMenu.min.js"></script> 
    <script src="Scripts/sb-admin-2.js"></script>

     
     
</asp:Content>

