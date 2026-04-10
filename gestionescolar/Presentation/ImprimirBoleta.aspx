<%@ Page Title="" Language="C#" MasterPageFile="~/Presentation/menu.Master" AutoEventWireup="true" CodeBehind="ImprimirBoleta.aspx.cs" Inherits="gestionescolar.Presentation.ImprimirBoleta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="col-12 col-md-8 col-lg-7">
            <h2>Imprimir Boletas</h2>

            <!-- Selección: Grupo -->
            <div class="mb-3">
                <asp:Label ID="lbGrupo" runat="server" class="form-label" Text="Grupo: "></asp:Label>
                <asp:DropDownList ID="ddlGrupo" runat="server" CssClass="form-select textbox-delgado">
                </asp:DropDownList>
            </div>

            <!-- Tipo de impresión -->
            <div class="mb-3">
                <asp:Label ID="lbtipoImpresion" runat="server" Text="Tipo de impresión:" CssClass="form-label"></asp:Label>
                <asp:RadioButtonList ID="rblTipoImpresion" runat="server">
                    <asp:ListItem Text="Por grupo (todas las boletas)" Value="grupo" Selected="True" />
                    <asp:ListItem Text="Por alumno" Value="alumno" />
                </asp:RadioButtonList>
                <br />
                <asp:Panel DefaultButton="btnOpcion" runat="server">
                    <asp:Button ID="btnOpcion" runat="server" Text="Seleccionar" CssClass="btn btn-primary" OnClick="btnOpcion_Click" />
                </asp:Panel>
                

            </div>

            <!-- Selección de alumno (solo si el usuario elige 'por alumno') -->
            <div class="mb-3" id="divAlumno" runat="server" visible="false">
                <asp:Label ID="lbAlumno" runat="server" class="form-label" Text="Alumno:" />
                <asp:DropDownList ID="ddlAlumno" runat="server" CssClass="form-select textbox-delgado"></asp:DropDownList>
            </div>

            <!-- Botones -->
            <asp:Button ID="btnSleccionarAlumno" runat="server" Text="Seleccionar Alumno" CssClass="btn btn-primary" OnClick="btnSleccionarAlumno_Click" />
            <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" CssClass="btn btn-success" OnClick="btnImprimir_Click" />
            <asp:Button ID="btnAtras" runat="server" Text="Atrás" CssClass="btn btn-danger" OnClick="btnAtras_Click" />
        </div>
    </div>
</asp:Content>

