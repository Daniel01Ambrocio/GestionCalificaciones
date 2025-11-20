<%@ Page Title="" Language="C#" MasterPageFile="~/Presentation/menu.Master" AutoEventWireup="true" CodeBehind="ImprimirBoleta.aspx.cs" Inherits="gestionescolar.Presentation.ImprimirBoleta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="w-100 Content-general">
            <h2>Imprimir Boletas</h2>

            <!-- Selección: Grupo -->
            <div class="mb-3">
                <asp:Label ID="lbGrupo" runat="server" class="form-label" Text="Grupo: "></asp:Label>
                <asp:DropDownList ID="ddlGrupo" runat="server" CssClass="form-select textbox-delgado" AutoPostBack="true" OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged">
                </asp:DropDownList>
            </div>

            <!-- Tipo de impresión -->
            <div class="mb-3">
                <asp:Label ID="Label1" runat="server" Text="Tipo de impresión:" CssClass="form-label"></asp:Label>
                <asp:RadioButtonList ID="rblTipoImpresion" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblTipoImpresion_SelectedIndexChanged">
                    <asp:ListItem Text="Por grupo (todas las boletas)" Value="grupo" Selected="True" />
                    <asp:ListItem Text="Por alumno" Value="alumno" />
                </asp:RadioButtonList>
            </div>

            <!-- Selección de alumno (solo si el usuario elige 'por alumno') -->
            <div class="mb-3" id="divAlumno" runat="server" visible="false">
                <asp:Label ID="lbAlumno" runat="server" class="form-label" Text="Alumno:" />
                <asp:DropDownList ID="ddlAlumno" runat="server" CssClass="form-select textbox-delgado"></asp:DropDownList>
            </div>

            <!-- Botones -->
            <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" CssClass="btn btn-primary" OnClick="btnImprimir_Click" />
            <asp:Button ID="btnAtras" runat="server" Text="Atrás" CssClass="btn btn-danger" />
        </div>
    </div>
</asp:Content>

