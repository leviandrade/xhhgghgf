function successAlert(msg) {
    var dialog = bootbox.dialog({
        message: "<p class='h5 text-center'><i class='fa fa-check'></i> " + msg + "</p>",
        closeButton: false
    });

    dialog.find('.modal-content').addClass("alert alert-success").css({ 'background-color': '#ebfaeb' });

    setTimeout(function () {
        dialog.modal('hide');
    }, 1300);
}


function dialogLoad() {
    var dialogLoad = bootbox.dialog({
        message: "<p class='h5 text-center'><i class='fa fa-spin fa-spinner'></i>&nbsp;&nbsp;&nbsp;<strong> Por favor, aguarde...</strong></p><p class='h6'></p>",
        closeButton: false,
        size: 'large',
        onEscape: true,
        className: "hey-modal"
    });

    dialogLoad.find('.modal-dialog').addClass("modal-dialog-centered");
    dialogLoad.find('.modal-content').addClass("alert alert-secundary").css({ 'background-color': '#d9d9d9' });

    time = setTimeout(dialogLoadAction, 1000);

    clearTimeout(time);

}

function dialogLoadAction() {
    jQuery(".hey-modal").modal("hide");

}

function dialogLoadClose(tempo) {

    time = setTimeout(dialogLoadAction, tempo);
}