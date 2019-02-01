function requisicao(endereco, metodo, obj) {

    var loading = new loadingGif();
    loading.abrir();

    var promise = $.ajax({
        url: endereco,
        type: metodo,
        data: obj ? JSON.stringify(obj) : null,
        contentType: 'application/json',
        headers: {
            'Authorization': 'bearer ' + sessionStorage.token
        }
    });

    promise.fail(function (data) {
        if (data.status == 401)
            logoff();
    });

    promise.always(loading.fechar);

    return promise;
}