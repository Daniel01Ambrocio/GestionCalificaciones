<%@ Page Title="" Language="C#" MasterPageFile="~/Presentation/menu.Master" AutoEventWireup="true" CodeBehind="RegistrarVerGrupos.aspx.cs" Inherits="gestionescolar.Presentation.RegistrarVerGrupos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <!-- Encabezado -->
        <div class="section-header">
            <h3>📋 Gestión de Grupos</h3>
        </div>

        <!-- Contenedor principal del formulario y lista -->
        <div id="contform">
            <!-- Filtros -->
            <div class="row mb-3">
                <div class="col-md-4">
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtFiltro" placeholder="Filtrar..." />
                </div>
                <div class="col-12 mt-2">
                    <asp:Button runat="server" Text="Filtrar" CssClass="btn btn-primary" ID="btnFiltrar"/>
                </div>
            </div>

            <div class="row">
                <!-- Lista de grupos -->
                <div class="col-md-8">
                    <div class="card">
                        <div class="card-header bg-primary text-white">
                            Lista de Grupos
                        </div>
                        <div class="card-body p-0">
                            <asp:GridView runat="server" ID="gvGrupos" CssClass="table table-striped table-bordered mb-0"
                                AutoGenerateColumns="False" GridLines="None">
                                <Columns>
                                    <asp:BoundField DataField="grado" HeaderText="Grado" />
                                    <asp:BoundField DataField="Grupo" HeaderText="Grupo" />
                                    <asp:BoundField DataField="anio" HeaderText="Año" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>

                <!-- Formulario de registro -->
                <div class="col-md-4">
                    <div class="card">
                        <div class="card-header bg-primary text-white">
                            Registrar nuevo grupo
                        </div>
                        <div class="card-body">
                            <div class="form-group">
                                <label>Grado</label>
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtGrado" />
                            </div>
                            <div class="form-group">
                                <label>Grupo</label>
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtGrupo" />
                            </div>
                            <div class="form-group">
                                <label>Año</label>
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtAnio" />
                            </div>
                            <asp:Button runat="server" Text="Registrar" CssClass="btn btn-primary btn-block" ID="btnRegistrar" OnClick="btnRegistrar_Click"/>
                        </div>
                    </div>
                </div>
            </div>
            <!-- fin row -->
        </div>
        <!-- fin contform -->
    </div>
</asp:Content>
