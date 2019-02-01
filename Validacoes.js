$(document).on('blur', ':input', function () {
    var elem = $(this);
    if (elem[0].value.length > 0 && elem[0].className.match(/obrigatorios/))
        elem.css("border-color", "green");

    if (elem[0].value.length == 0 && elem[0].className.match(/obrigatorios/))
        elem.css("border-color", "#e60000")
});

function LimparDados() {
    $('#formulario').find(":input").val("");
    $('#formulario').find(".obrigatorios").css("border-color", "#e60000");
}

function removerBorda(formulario) {
    $(formulario).find(".obrigatorios").css("border-color", "green");
}

function adicionarBorda(formulario) {
    $(formulario).find(".obrigatorios").css("border-color", "#e60000");
}
