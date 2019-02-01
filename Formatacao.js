$(document).ready(function () {
    $('.datetime').mask("00/00/0000 00:00");
    FormataCNPJ();
    formatarCep()
});


function formataDataHora(data) {
    if (data != null && data != "") {
        var date = new Date(data);
        var options = {
            year: 'numeric', month: '2-digit', day: '2-digit',
            hour: '2-digit', minute: '2-digit', hour12: false
        };
        return new Intl.DateTimeFormat('pt-BR', options).format(date).replace(/\//g, '/').replace(',', '');
    }
}

function FormataCNPJ() {
    $('.Cnpj').mask("99.999.999/9999-99")
}

function formataTelefone(telefone) {
    $(".telefone").mask("(00) 0000-00009");
    if ($(telefone).val().length == 15) {
        $('.telefone').mask('(00) 00000-0000');
    } else {
        $('.telefone').mask('(99) 0000-00009');
    }   
}

function formatarCep() {
    $('.cep').mask("99.999-999");
}

function formatarPeloTipo(tipo, campo) {
    if (tipo == "")
        $(campo).unmask();
    else if (tipo == 0)
        $(campo).mask("99.999.999/9999-99")
    else if (tipo == 1)
        $(campo).mask("000.000.000-00")
    else
        $(campo).unmask();
}

$(document).on('change', '.tipoDocumento', function () {
    formatarPeloTipo($('.tipoDocumento').val(), '.NumeroDocumento')
})