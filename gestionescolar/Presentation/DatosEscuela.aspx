<%@ Page Title="" Language="C#" MasterPageFile="~/Presentation/menu.Master" AutoEventWireup="true" CodeBehind="DatosEscuela.aspx.cs" Inherits="gestionescolar.Presentation.DatosEscuela" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card-body p-0">
        <asp:GridView runat="server" ID="gdvEscuela"
            CssClass="table table-striped table-bordered mb-0"
            AutoGenerateColumns="False"
            DataKeyNames="IDEscuela"
            AutoGenerateEditButton="False"
            OnRowEditing="gdvEscuela_RowEditing"
            OnRowCancelingEdit="gdvEscuela_RowCancelingEdit"
            OnRowUpdating="gdvEscuela_RowUpdating">

            <Columns>
                <asp:TemplateField HeaderText="Escuela">
                    <ItemTemplate>
                        <table class="table table-borderless mb-0">
                            <tr>
                                <th>ID Escuela</th>
                                <td><%# Eval("IDEscuela") %></td>
                            </tr>
                            <tr>
                                <th>Nombre Escuela</th>
                                <td><%# Eval("NombreEscuela") %></td>
                            </tr>
                            <tr>
                                <th>Clave Institución</th>
                                <td><%# Eval("ClaveInstitucion") %></td>
                            </tr>
                            <tr>
                                <th>Dirección</th>
                                <td><%# Eval("Direccion") %></td>
                            </tr>
                            <tr>
                                <th>Teléfono</th>
                                <td><%# Eval("Telefono") %></td>
                            </tr>
                            <tr>
                                <th>Logotipo</th>
                                <td>
                                    <asp:Image runat="server" ID="imgLogotipo" ImageUrl='<%# Eval("Logotipo") %>' Width="100px" />
                                </td>
                            </tr>
                            <tr>
                                <th>Ciclo Escolar</th>
                                <td><%# Eval("CicloEscolar") %></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:LinkButton ID="btnEditar" runat="server" 
                                        CommandName="Edit" CssClass="btn btn-primary btn-sm" Text="Editar" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <table class="table table-borderless mb-0">
                            <tr>
                                <th>Nombre Escuela</th>
                                <td><asp:TextBox ID="txtNombreEscuela" runat="server" Text='<%# Bind("NombreEscuela") %>' CssClass="form-control" /></td>
                            </tr>
                            <tr>
                                <th>Clave Institución</th>
                                <td><asp:TextBox ID="txtClaveInstitucion" runat="server" Text='<%# Bind("ClaveInstitucion") %>' CssClass="form-control" /></td>
                            </tr>
                            <tr>
                                <th>Dirección</th>
                                <td><asp:TextBox ID="txtDireccion" runat="server" Text='<%# Bind("Direccion") %>' CssClass="form-control" /></td>
                            </tr>
                            <tr>
                                <th>Teléfono</th>
                                <td><asp:TextBox ID="txtTelefono" runat="server" Text='<%# Bind("Telefono") %>' CssClass="form-control" /></td>
                            </tr>
                            <tr>
                                <th>Logotipo</th>
                                <td>
                                    <asp:Image ID="imgEditLogotipo" runat="server" Width="100px" />
                                    <br />
                                    <asp:FileUpload ID="fuLogotipo" runat="server" CssClass="form-control-file" />
                                </td>
                            </tr>
                            <tr>
                                <th>Ciclo Escolar</th>
                                <td><asp:TextBox ID="txtCicloEscolar" runat="server" Text='<%# Bind("CicloEscolar") %>' CssClass="form-control" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:LinkButton ID="btnGuardar" runat="server" Text="Guardar" CommandName="Update" CssClass="btn btn-success btn-sm" />
                                    <asp:LinkButton ID="btnCancelar" runat="server" Text="Cancelar" CommandName="Cancel" CssClass="btn btn-secondary btn-sm" />
                                </td>
                            </tr>
                        </table>
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
