<%@ Page Title="" Language="C#" MasterPageFile="~/Presentation/menu.Master" AutoEventWireup="true" CodeBehind="HistorialMisSolicitudes.aspx.cs" Inherits="gestionescolar.Presentation.HistorialSolicitudes" %>

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
                        <asp:Button runat="server" Text="Filtrar" CssClass="btn btn-primary" ID="btnFiltrar" OnClick="btnFiltrar_Click" />
                    </div>
                </div>

                <div class="row">
                    <!-- Lista de grupos -->
                    <div class="card-header bg-primary text-white">
                        Mis solicitudes
                    </div>
                    <div class="card-body p-0">
                        <asp:GridView
                            runat="server"
                            ID="gvSolicitudes"
                            CssClass="table table-striped table-bordered mb-0"
                            AutoGenerateColumns="False"
                            GridLines="None"
                            DataKeyNames="IDSolicitudBajas">

                            <Columns>
                                <asp:BoundField DataField="NombAdministrativo" HeaderText="Administrativo" />
                                <asp:BoundField DataField="NombUsuarioBaja" HeaderText="Usuario a suspender" />
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
    </div>
</asp:Content>
