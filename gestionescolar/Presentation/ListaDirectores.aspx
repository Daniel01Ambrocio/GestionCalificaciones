<%@ Page Title="" Language="C#" MasterPageFile="~/Presentation/menu.Master" AutoEventWireup="true" CodeBehind="ListaDirectores.aspx.cs" Inherits="gestionescolar.Presentation.ListaDirectores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <!-- Encabezado -->
        <div class="section-header">
            <h3>Lista de Directores</h3>
        </div>

        <!-- Contenedor principal del formulario y lista -->
        <div id="contform">
            <!-- Filtros -->
            <div class="row mb-3">
                <div class="col-md-4">
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtFiltro" placeholder="Filtrar..." />
                </div>
                <div class="col-12 mt-2">
                    <asp:Button runat="server" Text="Filtrar" CssClass="btn btn-primary" ID="btnFiltrar" />
                </div>
            </div>

            <div class="row">
                <!-- Lista de grupos -->
                <div class="col-md-11">
                    <div class="card">
                        <div class="card-header bg-primary text-white">
                            Lista de Directores
                        </div>
                        <div class="card-body p-0">
                            <asp:GridView runat="server" ID="gvDirectores" CssClass="table table-striped table-bordered mb-0"
                                AutoGenerateColumns="False" GridLines="None">
                                <Columns>
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                    <asp:BoundField DataField="ApellidoPaterno" HeaderText="Apellido paterno" />
                                    <asp:BoundField DataField="ApellidoMaterno" HeaderText="Apellido materno" />
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" />
                                    <asp:BoundField DataField="PeriodoIngreso" HeaderText="Periodo ingreso" />
                                    <asp:BoundField DataField="PeriodoFin" HeaderText="Periodo fin" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <!-- fin row -->
        </div>
        <!-- fin contform -->
    </div>
</asp:Content>
