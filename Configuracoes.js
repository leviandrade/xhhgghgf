var urlPrincipal = servidor.url + "Configuracoes/";

$(document).ready(function () {
    permissaoUsuario(window.location.href, sessionStorage.getItem('menus'));
    populaConfiguracoes();
});


function limparDados() {
    $("#fConfiguracoes")[0].reset();
    $("#txtId").val("");
}


function salvarRegistro(form, event) {

    var objeto = montarObjeto(form);

    if (!validarEmail(objeto.email)) {
        bootbox.alert("E-mail inválido.");
        return false;
    }

    objeto.senha = btoa(objeto.senha);

    var metodo = "POST";
    var endereco = urlPrincipal;

    if (objeto.id == "") {
        objeto.id = 0;
    }

    if (objeto.id > 0) {
        metodo = "PUT";
        endereco = endereco + objeto.id
    }

    var promisse = requisicao(endereco, metodo, objeto);

    promisse.done(function (data) {
        $("#txtId").val(data.id);
        successAlert("A configuração foi salva com sucesso.");
    });

    return false;
}


function populaConfiguracoes() {
    var promise = requisicao(urlPrincipal, 'GET');
    promise.done(function (data) {
        if (data.length > 0) {
            var formulario = $('#fConfiguracoes');
            var dados = data[0];
            formulario.data("item", dados);
            $.each(dados, function (prop, valor) {
                if (prop == "ssl")
                    $("#ddlSsl").find("[value=" + valor + "]").attr('selected', 'selected');
                else if (prop == "senha")
                    formulario.find("[name=" + prop + "]").val(atob(valor));
                else
                    formulario.find("[name=" + prop + "]").val(valor);
            });
        }
    });
}