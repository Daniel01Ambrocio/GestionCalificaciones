<%@ Page Title="" Language="C#" MasterPageFile="~/Presentation/menu.Master" AutoEventWireup="true" CodeBehind="MisCalificaciones.aspx.cs" Inherits="gestionescolar.Presentation.MisCalificaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <!-- Encabezado -->
        <div class="section-header">
            <h3>Historial academico</h3>
        </div>

        <!-- Contenedor principal del formulario y lista -->
        <div id="contform">

            <!-- Lista de grupos -->
            <div class="col-md-11">
                <div class="card">
                    <div class="card-header bg-primary text-white">
                        Historial academico
                    </div>
                    <div class="card-body p-0">
                        <asp:GridView runat="server" ID="gvCalificaciones" CssClass="table table-striped table-bordered mb-0"
                            AutoGenerateColumns="False" GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="Parcial1" HeaderText="Parcial 1" />
                                <asp:BoundField DataField="Parcial2" HeaderText="Parcial 2" />
                                <asp:BoundField DataField="Parcial3" HeaderText="Parcial 3" />
                                <asp:BoundField DataField="Parcial4" HeaderText="Parcial 4" />
                                <asp:BoundField DataField="Promedio" HeaderText="Promedio" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <!-- fin contform -->
    </div>
</asp:Content>
