<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DriversEdit.aspx.cs" Inherits="DriversEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="chpHeader" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpContent" runat="Server">
    <div class="container-fluid">
        <asp:UpdateProgress runat="server" ID="updProgWrap" DisplayAfter="500">
            <ProgressTemplate>
                <div class="text-center" style="width: 100%; height: 100%; position: absolute; background-color: #343a406b; z-index: 99999; top: 0px; left: 0px">
                    <div class="p-5 text-center" style="background: #fff; width: 50%; left: 25%; margin-top: 20%; border-radius: 10px; position: absolute">
                        <h3>Attendere...</h3>
                        <span class="fa fa-4x fa-cog fa-spin"></span>
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <div class="page-breadcrumb p-0 mb-3">
            <div class="row">

                <div class="col-10 align-self-center">
                    <h3 class="page-title text-truncate text-dark font-weight-medium mb-1">Autisti - <span class="custom-red-color font-weight-bolder">
                        <asp:Literal runat="server" ID="litPageTitle" Text="Nuovo"></asp:Literal></span></h3>
                </div>
                <div class="col-2 text-right">
                    <a href="Drivers.aspx" class="btn btn-light btn-sm mr-2"><i class="fa fa-backward"></i>&nbsp;Torna alla lista</a>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-8">
                <div class="card">
                    <div class="card-body">
                        <asp:UpdatePanel runat="server" ID="updForm" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row" id="formdata" data-parsley-validate>
                                    <div class="col-12 mb-2">
                                        <h5 class="font-weight-medium">Dati generali</h5>
                                    </div>
                                    <div class="col-4 mb-2">
                                        <label class="text-muted">Nome <span class="custom-red-color">*</span></label>
                                        <input type="text" runat="server" id="txtName" class="form-control" required data-parsley-group="main" />
                                    </div>
                                    <div class="col-4 mb-2">
                                        <label class="text-muted">Cognome <span class="custom-red-color">*</span></label>
                                        <input type="text" runat="server" id="txtSurname" class="form-control" required data-parsley-group="main" />
                                    </div>
                                    <div class="col-4 mb-2">
                                        <label class="text-muted">Telefono</label>
                                        <input type="text" runat="server" id="txtPhone" class="form-control" data-parsley-group="main" />
                                    </div>
                                    <div class="col-4 mb-2">
                                        <label class="text-muted">Username <span class="custom-red-color">*</span></label>
                                        <input type="text" runat="server" id="txtUsername" class="form-control" required data-parsley-group="main" />
                                    </div>
                                    <div class="col-4 mb-2">
                                        <label class="text-muted">Password <span class="custom-red-color">*</span></label>
                                        <input type="text" runat="server" id="txtPassword" readonly class="form-control" required data-parsley-group="main" />
                                    </div>
                                    <div class="col-4 mb-2 pt-4 bg-light-100 text-center" style="border-radius: 5px">
                                        <input type="checkbox" runat="server" id="chkEnabled" checked />
                                        <label class="text-muted ml-2 font-weight-medium" for="<%=chkEnabled.ClientID %>">Login App abilitato <i class="fa fa-info-circle" data-toggle="tooltip" data-placement="top" title="Disattivando questo flag non sarà possibile accedere all'app mobile"></i></label>
                                    </div>
                                    <div class="col-12 mb-2">
                                        <hr />
                                        <h5 class="font-weight-medium mb-3">Tariffe <i class="fa fa-info-circle" data-toggle="tooltip" data-placement="top" title="Inserire la tariffa in Euro per ogni tipologia di attività"></i></h5>
                                        <div class="row">
                                            <asp:Repeater runat="server" ID="rpFee">
                                                <ItemTemplate>
                                                    <div class="col-4 mb-2 d-flex">
                                                        <label class="text-muted mr-2 flex-grow-1 text-right"><%# ((Tariffa)Container.DataItem).Tipo %></label>
                                                        <input type="text" runat="server" id="txtValue" style="width: 100px" tp="<%# ((Tariffa)Container.DataItem).Tipo %>" value="<%# ((Tariffa)Container.DataItem).Valore.ToString() %>" class="form-control input-price" required data-parsley-group="main" />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                    <div class="col-12 text-center">
                                        <asp:LinkButton runat="server" ID="lnkSave" CssClass="btn btn-primary" OnClick="lnkSave_Click" OnClientClick=<%# string.Format("return multiformValidateConfirmDialog('{0}','{1}','{2}','{3}','{4}','{5}', '{6}')","Attenzione", "Procedere al salvataggio dei dati inseriti?", "Conferma", "Annulla", lnkSave.UniqueID, "#formdata", "main") %>><i class="fa icon-check"></i> Salva</asp:LinkButton>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>
            <div class="col-4">
                <div class="card">
                    <div class="card-body">
                        <asp:UpdatePanel runat="server" ID="updNotes" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="d-flex mb-2">
                                    <h5 class="font-weight-medium flex-grow-1 mr-2">Note</h5>
                                    <asp:LinkButton runat="server" ID="lnkAddNote" CssClass="custom-red-color" OnClick="lnkAddNote_Click" Visible="false"><i class="fa fa-1-5x fa-plus-circle"></i></asp:LinkButton>
                                </div>
                                <asp:Repeater runat="server" ID="rpNotes">
                                    <ItemTemplate>
                                        <div class="border-bottom mb-3 pb-1">
                                            <div class="d-flex">
                                                <small class="custom-red-color flex-grow-1"><%# ((Nota)Container.DataItem).Data.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss") %></small>
                                                <small runat="server" visible="<%# ((Nota)Container.DataItem).Pubblica %>"><i class="fa fa-check"></i> Pubblica <i class="fa fa-info-circle" data-toggle="tooltip" data-placement="top" title="Questa nota è visibile anche all'autista nell'app"></i></small>
                                            </div>
                                            <p class="mt-1 mb-1"><%# ((Nota)Container.DataItem).Testo %></p>
                                            <div class="w-100 text-right">

                                                <asp:LinkButton runat="server" ID="lnkEditNote" idx="<%# Container.ItemIndex %>" OnClick="lnkEditNote_Click" CssClass="fa icon-pencil custom-red-color ml-2"></asp:LinkButton>
                                                <asp:LinkButton runat="server" ID="lnkDeleteNote" idx="<%# Container.ItemIndex %>" OnClick="lnkDeleteNote_Click" CssClass="fa icon-trash custom-red-color ml-2"></asp:LinkButton>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>


                                <div id="modalEditNote" class="modal" tabindex="-1" role="dialog" aria-hidden="true" style="display: none;">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h4 class="modal-title font-weight-bolder">Nota autista</h4>
                                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                            </div>
                                            <div class="modal-body p-2">
                                                <div class="mb-2">
                                                    <div class="card-body p-2">
                                                        <div class="row" id="formNote" data-parsley-validate>
                                                            <div class="col-12 mb-2">
                                                                <textarea runat="server" id="txtNoteText" class="form-control" rows="6" placeholder="Inserisci qui la nota..." required data-parsley-group="note" />
                                                            </div>

                                                            <div class="col-12 mb-2 bg-light-100 text-center p-2">
                                                                <input type="checkbox" runat="server" id="chkNotePublic" />
                                                                <label class="text-muted" for="<%= chkNotePublic.ClientID %>">La nota sarà visibile anche nell'app dell'autista</label>

                                                            </div>
                                                            <div class="col-12 mb-2 text-center">
                                                                <small class="font-weight-light">
                                                                    <asp:Literal runat="server" ID="litNoteDate"></asp:Literal></small>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer p-0">
                                                    <asp:LinkButton runat="server" ID="lnkSaveNote" CssClass="btn btn-primary waves-effect" OnClick="lnkSaveNote_Click" OnClientClick=<%# string.Format("return multiformValidateConfirmDialog('{0}','{1}','{2}','{3}','{4}','{5}', '{6}')","Attenzione", "Procedere al salvataggio dei dati inseriti?", "Conferma", "Annulla", lnkSaveNote.UniqueID, "#formNote", "note") %>><span class="fa fa-save"></span> Salva</asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpScripts" runat="Server">
    <script src="<%= ResolveUrl("~/assets/libs/parsley/parsley.min.js") %>"></script>
    <script src="<%= ResolveUrl(string.Format("~/assets/libs/parsley/i18n/it.js")) %>"></script>
    <script src="<%= ResolveUrl(string.Format("~/assets/libs/parsley/i18n/it.extra.js")) %>"></script>
    <script src="<%= ResolveUrl("~/assets/libs/inputmask/new/inputmask.js") %>"></script>
    <script src="<%= ResolveUrl("~/assets/libs/inputmask/new/inputmask.numeric.extensions.js") %>"></script>
    <script src="<%= ResolveUrl("~/assets/libs/inputmask/new/jquery.inputmask.js") %>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".input-price").inputmask({
                alias: "decimal",
                radixPoint: ",",
                digits: 2,
                allowPlus: false,
                allowMinus: false
            });
        });
    </script>
</asp:Content>

