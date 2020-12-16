<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Pruebamaster.aspx.vb" Inherits="Pruebamaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


      <div class=" text-center">
                        <h1 class="page-header">Razon social</h1>
                    </div>

                    <div class="input-group">
                        <input type="text" runat="server" id="barrabusqueda" class="form-control" placeholder="Search for..." />
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



                    <asp:ListBox ID="ListBox1" runat="server" Rows="10" Style="height: 300px !important; width: 600px !important; max-width: 100% !important;"></asp:ListBox>


                    <div style="width: 115px; margin: 0 auto;">
                        <button type="button" runat ="server" id="aceptarrazon" class="btn btn-default btn-lg">
                            Aceptar <i class="fa fa-sign-in"></i>

                        </button>
                    </div>
</asp:Content>

