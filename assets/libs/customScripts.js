function enableExitConfirmation() {
    $('input').on('input', function () {
        window.onbeforeunload = function () {
            return "Bye now!";
        };
    });
}

function setupImageDropzone(el, uploadUrl, fileSize, maxFiles, maxWidth, acceptedFiles, elPreview, mediaURL) {
    el.dropzone({
        url: uploadUrl,
        addRemoveLinks: true,
        uploadMultiple: false,
        maxFilesize: fileSize,
        maxFiles: maxFiles,
        resizeWidth: maxWidth,
        acceptedFiles: acceptedFiles,
        success: function (file, response) {
            file.previewElement.classList.add("dz-success");
            if (response.indexOf("ERROR|") !== -1) {
                showNotification("Attenzione", response.replace("ERROR|", ""), "error", "toast-top-center");
            } else {
                if (acceptedFiles === "image/*") {
                    elPreview.attr("src", mediaURL + "/" + response);
                } else {
                    elPreview.attr("href", mediaURL + "/" + response);
                }
            }
            
        },
        error: function (file, response) {
            file.previewElement.classList.add("dz-error");
        }
    });
}

function showNotification(title, message, type, position) {
    toastr.options.closeButton = true;
    toastr.options.preventDuplicates = true;
    toastr.options.positionClass = position;
    if (type === "error") {
        toastr.error(message, title);
    } else if (type === "warning") {
        toastr.warning(message, title);
    } else if (type === "success") {
        toastr.success(message, title);
    }

}


function enableAllTabs() {
    $(".nav-tabs").find("a.nav-link.disabled").removeClass("disabled");
}
function triggerModal(modal, action) {
    if ($(".modal-backdrop")[0]) {
        $('.modal-backdrop').remove();
    }
    $("#" + modal).modal(action);
    if (action == "hide") {
        $("body").removeAttr("class");
        $("body").removeAttr("style");
    }
}

function showConfirmDialog(title, message, confirmtext, canceltext, callbackID, type = 'warning', clientID = '') {
    if (clientID != "" && $("#" + clientID).attr("class") == "aspNetDisabled") {
        return false;
    }
    Swal.fire({
        title: title,
        html: message,
        type: type,
        showCancelButton: true,
        confirmButtonText: confirmtext,
        cancelButtonText: canceltext,
        reverseButtons: true
    }).then((result) => {
        if (result.value) {
            window.onbeforeunload = null;
            __doPostBack(callbackID, '');
        }
    })
    return false;
}
function showAutodismissDialog(title, message) {
    Swal.fire({
        title: title,
        html: message,
        timer: 5000
    }).then((result) => {

    })
}
function validateAndConfirmDialog(title, message, confirmtext, canceltext, callbackID, formID, formGroup) {
    var result = $(formID).parsley().validate(formGroup);
    if (true === result) {
        $('.bs-callout-info').removeClass('hidden');
        $('.bs-callout-warning').addClass('hidden');
        Swal.fire({
            title: title,
            text: message,
            type: 'warning',
            showCancelButton: true,
            confirmButtonText: confirmtext,
            cancelButtonText: canceltext,
            reverseButtons: true
        }).then((result) => {
            if (result.value) {
                window.onbeforeunload = null;
                __doPostBack(callbackID, '');
            }
        })
    } else {
        $('.bs-callout-info').addClass('hidden');
        $('.bs-callout-warning').removeClass('hidden');
    }
    return false;
}
function multiformValidateConfirmDialog(title, message, confirmtext, canceltext, callbackID, formIDs, formGroup) {
    var i;
    var result = true;
    var ids = formIDs.split(',');
    for (i = 0; i < ids.length; i++) {
        var formResult = $(ids[i]).parsley().validate(formGroup);
        if (false === formResult) {
            $('.bs-callout-info').addClass('hidden');
            $('.bs-callout-warning').removeClass('hidden');
            result = false;
        }
    }
    if (true === result) {
        $('.bs-callout-info').removeClass('hidden');
        $('.bs-callout-warning').addClass('hidden');
        Swal.fire({
            title: title,
            text: message,
            type: 'warning',
            showCancelButton: true,
            confirmButtonText: confirmtext,
            cancelButtonText: canceltext,
            reverseButtons: true
        }).then((result) => {
            if (result.value) {
                window.onbeforeunload = null;
                __doPostBack(callbackID, '');
            }
        })
    } else {
        $('.bs-callout-info').addClass('hidden');
        $('.bs-callout-warning').removeClass('hidden');
    }
    return false;
}
function validateForm(formID, formGroup) {
    var result = $(formID).parsley().validate(formGroup);
    if (true === result) {
        $('.bs-callout-info').removeClass('hidden');
        $('.bs-callout-warning').addClass('hidden');
        return true;
    } else {
        $('.bs-callout-info').addClass('hidden');
        $('.bs-callout-warning').removeClass('hidden');
        return false;
    }
}

function initTabListner() {
    $("ul.nav-tabs > li > a").on("shown.bs.tab", function (e) {
        var id = $(e.target).attr("id");
        currentTab = id;
    });
}

function initUserSearchAutocomplete(input, sourceURL, storage, trigger) {
    $("#" + input).autocomplete({
        minLength: 3,
        select: function (e, ui) {
            $("#" + storage).val(ui.item.value);
            document.getElementById(trigger).click();
        },
        source: function (request, response) {
            $.ajax({
                url: sourceURL,
                data: "{ 'q': '" + request.term + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.Addresses === null ? item.LastName + " " + item.FirstName + " (" + item.Username + ")" : item.LastName + " " + item.FirstName + " (" + item.Username + ") - " + item.Addresses[0]._Address + " " + item.Addresses[0].Note,
                            value: item.Id
                        }
                    }))
                }
            });
        }
    });
}


