var urlPrincipal = servidor.url + "Login/";
function Entrar(form, event) {
    event.preventDefault();

    var loading = new loadingGif();
    loading.abrir();

    form = $(form);
    var valores = form.serializeArray();

    var objeto = {};
    $.each(valores, function (i, item) {
        objeto[item.name] = item.value;
    });

    objeto.senha = btoa(objeto.senha);

    $.ajax({
        url: urlPrincipal,
        type: "POST",
        data: JSON.stringify(objeto),
        contentType: 'application/json'
    }).done(function (retorno) {
        sessionStorage.setItem("token", retorno.accessToken);
        window.location.href = location.origin + "/Home";
    }).fail(function () {
        $('#LoginInvalido').show();
    }).always(loading.fechar);

}
