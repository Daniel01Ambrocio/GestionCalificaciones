<%@ Page Title="" Language="C#" MasterPageFile="~/Presentation/menu.Master" AutoEventWireup="true" CodeBehind="AprobarSolicitudes.aspx.cs" Inherits="gestionescolar.Presentation.AprobarSolicitudes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="col-12 col-md-11 col-lg-11">
            <!-- Encabezado -->
            <div class="section-header">
                <h3>Historial de solicitud de bajas de usuario</h3>
            </div>
            <div id="contform">
                <!-- Filtros -->
                <div class="row mb-3">
                    <div class="input-group">
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtFiltro" placeholder="Filtrar..." />
                        <asp:Button runat="server" Text="Filtrar" CssClass="btn btn-primary" ID="btnFiltrarPendientes" OnClick="btnFiltrarPendientes_Click" />
                    </div>
                </div>

                <div class="row">
                    <!-- Aprobaciones pendientes -->
                    <div class="card-header bg-primary text-white">
                        Solicitudes pendientes por aprobar
                    </div>
                    <div class="card-body p-0">
                        <asp:GridView
                            runat="server"
                            ID="gvSolicitudesPendientes"
                            CssClass="table table-striped table-bordered mb-0"
                            AutoGenerateColumns="False"
                            GridLines="None"
                            DataKeyNames="IDSolicitudBajas"
                            OnRowCommand="gvSolicitudesPendientes_RowCommand">

                            <Columns>
                                <asp:BoundField DataField="NombAdministrativo" HeaderText="Administrativo" />
                                <asp:BoundField DataField="NombUsuarioBaja" HeaderText="Usuario a suspender" />
                                <asp:BoundField DataField="NombreRol" HeaderText="Rol" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                <asp:BoundField DataField="NombDirectivo" HeaderText="Directivo" />
                                <asp:BoundField DataField="FechaSolicitud" HeaderText="Fecha de solicitud" />
                                <asp:BoundField DataField="FechaAprobacion" HeaderText="Fecha de aprobacion" />
                                <asp:BoundField DataField="Estado" HeaderText="Estado" />

                                <asp:TemplateField HeaderText="Acción">
                                    <ItemTemplate>
                                        <asp:Button
                                            ID="btnAprobar"
                                            runat="server"
                                            Text="Aprobar"
                                            CssClass="btn btn-success btn-sm"
                                            CommandName="Aprobar"
                                            CommandArgument='<%# Eval("IDSolicitudBajas") %>'
                                            OnClientClick="return confirm('¿Seguro que deseas aprobar esta solicitud?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>


                    </div>
                </div>


            </div>
        </div>
    </div>
    <div class="container mt-5">
        <div class="col-12 col-md-11 col-lg-11">
            <!-- Filtros -->
            <div class="row mb-3">
                <div class="input-group">
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtFiltrarAprobados" placeholder="Filtrar..." />
                    <asp:Button runat="server" Text="Filtrar" CssClass="btn btn-primary" ID="btnFiltrarHistorial" OnClick="btnFiltrarHistorial_Click" />
                </div>
            </div>

            <div class="row">
                <!-- Historial de solicitudes Aprobadas -->
                <div class="card-header bg-primary text-white">
                    Historial de aprobaciones
                </div>
                <div class="card-body p-0">
                    <asp:GridView
                        runat="server"
                        ID="gvHistorialAprovaciones"
                        CssClass="table table-striped table-bordered mb-0"
                        AutoGenerateColumns="False"
                        GridLines="None"
                        DataKeyNames="IDSolicitudBajas">

                        <Columns>
                            <asp:BoundField DataField="NombAdministrativo" HeaderText="Administrativo" />
                            <asp:BoundField DataField="NombUsuarioBaja" HeaderText="Usuario a suspender" />
                            <asp:BoundField DataField="NombreRol" HeaderText="Rol" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                            <asp:BoundField DataField="NombDirectivo" HeaderText="Directivo" />
                            <asp:BoundField DataField="FechaSolicitud" HeaderText="Fecha de solicitud" />
                            <asp:BoundField DataField="FechaAprobacion" HeaderText="Fecha de aprobacion" />
                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                        </Columns>
                    </asp:GridView>


                </div>
            </div>
        </div>
    </div>

</asp:Content>
