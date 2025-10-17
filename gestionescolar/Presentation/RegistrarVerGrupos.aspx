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
                <div class="col-md-6">
                    <div class="input-group">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtFiltro" placeholder="Filtrar..." />
                        <asp:Button runat="server" Text="Filtrar" CssClass="btn btn-primary" ID="btnFiltrar" OnClick="btnFiltrar_Click" />
                    </div>
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
                                AutoGenerateColumns="False" GridLines="None" DataKeyNames="IDGrupo"
                                OnRowDeleting="gvGrupos_RowDeleting">
                                <Columns>
                                    <asp:BoundField DataField="grado" HeaderText="Grado" />
                                    <asp:BoundField DataField="Grupo" HeaderText="Grupo" />
                                    <asp:BoundField DataField="anio" HeaderText="Año" />

                                    <asp:TemplateField HeaderText="Acciones">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEliminar" runat="server" CommandName="Delete"
                                                Text="Eliminar"
                                                CssClass="btn btn-danger btn-sm"
                                                OnClientClick="return confirm('¿Está seguro que desea eliminar este grupo?');">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
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
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtAnio" TextMode="Number"/>
                            </div>
                            <asp:Button runat="server" Text="Registrar" CssClass="btn btn-primary btn-block" ID="btnRegistrar" OnClick="btnRegistrar_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <!-- fin row -->
        </div>
        <!-- fin contform -->
    </div>
</asp:Content>
