//sessionStorage.clear();
var menus = sessionStorage.getItem('menus');
if (menus == null) {
    $.ajax({
        url: servidor.url + "UsuariosPermissoes/",
        type: "GET",
        data: null,
        contentType: 'application/json',
        headers: {
            'Authorization': 'bearer ' + sessionStorage.token
        }
    }).done(function (dados) {
        sessionStorage.setItem('menus', dados);
        verificarPermissoes(sessionStorage.getItem('menus'));
    });
}
else {
    verificarPermissoes(menus)
}
function verificarPermissoes(menusPermitidos) {
    $(".itemMenu").css('display', 'none');
    var permissoes = menusPermitidos.split(",");
    for (var i = 0; i < permissoes.length; i++) {
        $("#" + permissoes[i]).css('display', 'block');
    }
}

function permissaoUsuario(endereco, permissoes) {
    var permissoesUsuarios = permissoes.split(",");
    var cont = 0;
    for (var i = 0; i < permissoesUsuarios.length; i++) {
        if (endereco.match(permissoesUsuarios[i]))
            cont++;
    }
    if (cont < 1)
        logoff();
}