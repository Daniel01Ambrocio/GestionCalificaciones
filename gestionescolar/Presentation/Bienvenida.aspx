<%@ Page Title="" Language="C#" MasterPageFile="~/Presentation/Principal.Master" AutoEventWireup="true" CodeBehind="Bienvenida.aspx.cs" Inherits="gestionescolar.Presentation.Bienvenida" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="container mt-5">
        <h1 class="text-center mb-4">Noticias de Programación, NASA y Educación</h1>

        <!-- Botones para mostrar categorías -->
        <div class="row mb-4">
            <div class="col-md-4">
                <asp:Button ID="btnMostrarProgramacion" runat="server" Text="Programación" class="btn btn-primary w-100" OnClick="btnMostrarProgramacion_Click" />
            </div>
            <div class="col-md-4">
                <asp:Button ID="btnMostrarNASA" runat="server" Text="NASA" class="btn btn-success w-100" OnClick="btnMostrarNASA_Click" />
            </div>
            <div class="col-md-4">
                <asp:Button ID="btnMostrarEducacion" runat="server" Text="Educación" class="btn btn-warning w-100" OnClick="btnMostrarEducacion_Click" />
            </div>
        </div>

        <!-- Fila para el calendario y GridView -->
        <div class="row mb-4">
            <div class="col-md-3">
                <h5>Filtrar búsqueda por fecha</h5>
                <asp:Calendar ID="calFecha" runat="server" CssClass="form-control" />
            </div>

            <div class="col-md-9">
                <!-- Contenedor con scroll para el GridView -->
                <div style="max-height: 250px; overflow-y: auto;">
                    <asp:GridView ID="gdvNoticias" runat="server" AutoGenerateColumns="True" CssClass="table table-striped mt-4">
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- Contenedor para mostrar mensajes -->
        <div id="noticias-container">
            <div class="alert alert-info" role="alert">
                <h4 class="alert-heading">Seleccione una categoría de noticias</h4>
                <p>Haz clic en los botones para ver las noticias más recientes sobre programación, NASA o educación.</p>

                <!-- Label para mostrar mensajes -->
                <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
