<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PlatesEdit.aspx.cs" Inherits="PlatesEdit" ClientIDMode="AutoID" %>

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
                    <h3 class="page-title text-truncate text-dark font-weight-medium mb-1">Targhe - <span class="custom-red-color font-weight-bolder">
                        <asp:Literal runat="server" ID="litPageTitle" Text="Nuovo"></asp:Literal></span></h3>
                </div>
                <div class="col-2 text-right">
                    <a href="Plates.aspx" class="btn btn-light btn-sm mr-2"><i class="fa fa-backward"></i>&nbsp;Torna alla lista</a>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-7">
                <div class="card">
                    <div class="card-body">
                        <asp:UpdatePanel runat="server" ID="updForm" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row" id="formdata" data-parsley-validate>
                                    <div class="col-12 mb-2">
                                        <h5 class="font-weight-medium">Dati generali</h5>
                                    </div>
                                    <div class="col-6 mb-2">
                                        <label class="text-muted">Targa <span class="custom-red-color">*</span></label>
                                        <input type="text" runat="server" id="txtCode" class="form-control" required data-parsley-group="main" />
                                    </div>
                                    <div class="col-6 mb-2">
                                        <label class="text-muted">Tipologia <span class="custom-red-color">*</span></label>
                                        <asp:DropDownList runat="server" ID="dropType" CssClass="form-control"></asp:DropDownList>
                                    </div>

                                    <div class="col-3 mb-2">
                                        <label class="text-muted">Km <span class="custom-red-color">*</span> <i class="fa fa-info-circle" data-toggle="tooltip" title="KM totali segnati nel tachimetro del mezzo"></i></label>
                                        <input type="text" runat="server" id="txtKm" class="form-control input-numeric" required data-parsley-group="main" />
                                    </div>
                                    <div class="col-9 mb-2 p-2 bg-light-100" style="border-radius: 5px">
                                        <div class="d-flex">
                                            <div class="text-center">
                                                <input type="checkbox" runat="server" id="chkEnabled" checked />
                                                <label class="text-muted ml-2 font-weight-medium" for="<%=chkEnabled.ClientID %>">Attiva</label>
                                            </div>
                                            <div class="ml-2 pl-2 border-left">
                                                <label class="txt-alert">Disattivando questo flag non sarà possibile selezionare questa targa nell'app mobile</label>
                                            </div>
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
            <div class="col-5">
                <div class="card">
                    <div class="card-body">
                        <asp:UpdatePanel runat="server" ID="updNotes" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="d-flex mb-2">
                                    <h5 class="font-weight-medium flex-grow-1 mr-2">Assi</h5>
                                    <asp:LinkButton runat="server" ID="lnkAddAxis" CssClass="custom-red-color" OnClick="lnkAddAxis_Click" Visible="false"><i class="fa fa-1-5x fa-plus-circle"></i></asp:LinkButton>
                                </div>
                                <div class="mb-2 font-weight-medium">
                                    E' possibile spostare uno pneumatico da un asse ad un altro, o all'interno dello stesso asse, trascinandolo nella posizione desiderata
                                </div>
                                <asp:Repeater runat="server" ID="rpAxis">
                                    <ItemTemplate>
                                        <div class="border-bottom mb-3 pb-1">
                                            <div class="d-flex axis-bg">
                                                <asp:Repeater runat="server" DataSource="<%# ((Asse)Container.DataItem).Pneumatici %>">
                                                    <ItemTemplate>
                                                        <div class="flex-grow-1 dragdrop" idaxis="<%# ((RepeaterItem)(Container.Parent.Parent)).ItemIndex %>" idx="<%# Container.ItemIndex %>">
                                                            <asp:LinkButton runat="server" ID="lnkTyreDetail" OnClick="lnkTyreDetail_Click" idaxis="<%# ((RepeaterItem)(Container.Parent.Parent)).ItemIndex %>" idx="<%# Container.ItemIndex %>" class='<%# Container.ItemIndex >=  ((List<Pneumatico>)((Repeater)(Container.Parent)).DataSource).Count / 2 ? "axis-tyre float-right" : "axis-tyre" %>'>
                                                                <div style=" height:100%; width:100%; background-color:#fbfafab8; display:flex" runat="server" visible="<%# !((Pneumatico)Container.DataItem).InFunzione %>">
                                                                <i class="fa fa-ban"></i>
                                                            </div>
                                                            </asp:LinkButton>
                                                            <span class="<%# Container.ItemIndex >=  ((List<Pneumatico>)((Repeater)(Container.Parent)).DataSource).Count / 2 ? "d-block text-right" : "d-block" %>" style="clear: both">
                                                                <span class="custom-red-color"><%# ((Pneumatico)Container.DataItem).Marca %></span><br />
                                                                <span class="font-weight-bolder"><%# ((Pneumatico)Container.DataItem).KmTotali %></span> km

                                                            </span>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <asp:LinkButton runat="server" ID="lnkDeleteAxis" idx="<%# Container.ItemIndex %>" OnClick="lnkDeleteAxis_Click" CssClass="fa fa-1-5x icon-trash custom-red-color ml-2" Style="margin: auto 0"></asp:LinkButton>
                                            </div>

                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>


                                <div id="modalNewAxis" class="modal" tabindex="-1" role="dialog" aria-hidden="true" style="display: none;">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h4 class="modal-title font-weight-bolder">Aggiungi asse</h4>
                                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                            </div>
                                            <div class="modal-body p-2">
                                                <div class="mb-2">
                                                    <div class="card-body p-2">
                                                        <div class="row">
                                                            <div class="col-12 mb-2">
                                                                <label class="text-muted">Quanti pneumatici sono presenti per lato?</label>
                                                                <asp:DropDownList runat="server" ID="dropAxisTyre" CssClass="form-control">
                                                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer p-0">

                                                    <asp:LinkButton runat="server" ID="lnkConfirmNewAxis" CssClass="btn btn-primary waves-effect" OnClick="lnkConfirmNewAxis_Click"><span class="fa fa-check"></span> Conferma</asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div id="modalEditTyre" class="modal" tabindex="-1" role="dialog" aria-hidden="true" style="display: none;">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h4 class="modal-title font-weight-bolder">Dati pneumatico</h4>
                                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                            </div>
                                            <div class="modal-body p-2">
                                                <div class="mb-2">
                                                    <div class="card-body p-2">
                                                        <div class="row" id="formtyre" data-parsley-validate>
                                                            <div class="col-6 mb-2">
                                                                <label class="text-muted">Marca</label>
                                                                <input type="text" runat="server" id="txtTyreBrand" class="form-control" />
                                                            </div>
                                                            <div class="col-col-3 mb-2">
                                                                <label class="text-muted">Km totali</label>
                                                                <input type="text" runat="server" id="txtTyreKm" class="form-control input-numeric" required data-parsley-group="tyre" />
                                                            </div>
                                                            <div class="col-12 mb-2 p-2 bg-light-100" style="border-radius: 5px">
                                                                <div class="d-flex">
                                                                    <div class="text-center">
                                                                        <input type="checkbox" runat="server" id="chkTyreActive" checked />
                                                                        <label class="text-muted ml-2 font-weight-medium" for="<%=chkTyreActive.ClientID %>">In funzione</label>
                                                                    </div>
                                                                    <div class="ml-2 pl-2 border-left">
                                                                        <label class="text-muted">Disattivando questo flag il pneumatico verrà dichiarato NON in funzione. I km generati da viaggi registrati con la targa associata non verranno conteggiati in automatico</label>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </div>
                                                        <input type="hidden" runat="server" id="hdnAxisIdx" />
                                                        <input type="hidden" runat="server" id="hdnTyreIdx" />
                                                    </div>
                                                </div>
                                                <div class="modal-footer p-0">

                                                    <asp:LinkButton runat="server" ID="lnkSaveTyre" CssClass="btn btn-primary waves-effect" OnClick="lnkSaveTyre_Click" OnClientClick=<%# string.Format("return multiformValidateConfirmDialog('{0}','{1}','{2}','{3}','{4}','{5}', '{6}')","Attenzione", "Procedere al salvataggio dei dati inseriti?", "Conferma", "Annulla", lnkSaveTyre.UniqueID, "#formtyre", "tyre") %>><span class="fa fa-save"></span> Salva</asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <input type="hidden" runat="server" id="hdnDragTyre" />
                                <input type="hidden" runat="server" id="hdnDropTyre" />
                                <asp:LinkButton runat="server" ID="lnkUpdateTyrePosition" OnClick="lnkUpdateTyrePosition_Click" style="display:none"></asp:LinkButton>
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
    <script src="https://code.jquery.com/ui/1.13.1/jquery-ui.min.js" integrity="sha256-eTyxS0rkjpLEo16uXTS0uVCS4815lc40K2iVpWDvdSY=" crossorigin="anonymous"></script>
    <script type="text/javascript">
        jQuery.fn.swap = function (b) {
            // method from: http://blog.pengoworks.com/index.cfm/2008/9/24/A-quick-and-dirty-swap-method-for-jQuery
            b = jQuery(b)[0];
            var a = this[0];
            var t = a.parentNode.insertBefore(document.createTextNode(''), a);
            b.parentNode.insertBefore(a, b);
            t.parentNode.insertBefore(b, t);
            t.parentNode.removeChild(t);
            return this;
        };

        function pageLoad() {
            $(".input-numeric").inputmask({
                alias: "decimal",
                digits: 0,
                allowPlus: false,
                allowMinus: false
            });

            $(".dragdrop").draggable({ revert: true, helper: "clone" });

            $(".dragdrop").droppable({
                accept: ".dragdrop",
                activeClass: "ui-state-hover",
                hoverClass: "ui-state-active",
                drop: function (event, ui) {

                    var draggable = ui.draggable, droppable = $(this),
                        dragPos = draggable.position(), dropPos = droppable.position();

                    draggable.css({
                        left: dropPos.left + 'px',
                        top: dropPos.top + 'px'
                    });

                    droppable.css({
                        left: dragPos.left + 'px',
                        top: dragPos.top + 'px'
                    });
                    $("#<%= hdnDragTyre.ClientID%>").val(draggable.attr("idaxis") + "|" + draggable.attr("idx"));
                    $("#<%= hdnDropTyre.ClientID%>").val(droppable.attr("idaxis") + "|" + droppable.attr("idx"));

                    draggable.swap(droppable);

                    document.getElementById('<%= lnkUpdateTyrePosition.ClientID %>').click();
                }
            });
        }

    </script>
</asp:Content>

