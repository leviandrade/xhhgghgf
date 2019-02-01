function montarObjeto(form) {
    form = $(form);
    var valores = form.serializeArray();
    var objeto = {};
    $.each(valores, function (i, item) {
        objeto[item.name] = item.value;
    });

    return objeto;
}


function repopularComboBox(dados, comboBox) {

    for (var i = 0; i < comboBox.length; i++) {
        comboBox[i].selected = false;
    }

    comboBox.find("option").remove();

    for (var i = 0; i < dados.length; i++) {
        comboBox.append("<option value=" + dados[i].id + ">" + dados[i].Nome + "</option>");
    }

    comboBox.multipleSelect("refresh");
}

function imprimir(div, mensagem) {
    var logo = $(".logoSindiveg");
    logo.css("display", "block");
    var conteudo = document.getElementById(div).innerHTML;
    var tela_impressao = window.open();
    tela_impressao.document.write(conteudo);
    if (mensagem)
        tela_impressao.document.write("<h4><strong>Observação:</strong> " + mensagem + "</h4>");
    logo.css("display", "none");
    tela_impressao.window.print();
    tela_impressao.window.close();
}

function logoff() {
    sessionStorage.clear();
    window.location = location.origin + '/Login';
}

function validarEmail(email) {
    var regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return regex.test(String(email).toLowerCase());
}