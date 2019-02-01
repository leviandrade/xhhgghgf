var urlPrincipal = servidor.url + "Encaminhamento/";
function salvarRegistro(form, event) {
    event.preventDefault();
    var objeto = montarObjeto(form);

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
    promisse.done(function () {
        //alert("sucesso");
    });
    promisse.fail(function (data) {
        console.log(data.responseJSON[0]);
        bootbox.alert(data.responseJSON[0]);
    });
}