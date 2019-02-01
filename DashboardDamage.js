
$(window).resize(function () {
    google.charts.load('current', { 'packages': ['corechart'], 'language': 'pt' });
    google.charts.setOnLoadCallback(function () {
        PopularComboDenunciantesAvulso();
        CarregarDenunciados();
        PopularEmpresaAvulsa();
        PopularTipoCategoriaAvulso();
        popularProduto();
        CarregarClassesProdutos();
        gerarGrafico()
    });
});

function abrirImpressao() {
    bootbox.dialog({
        size: 'large',
        message: observacao
    });
}

function imprimirDamage() {
    var observacao = $('#observacao').val()
    imprimir('imprimir', observacao)
}

var DadosFiltro = null;
function gerarGrafico(b,form, event) {
    if (event != null) {
        event.preventDefault();
    }
    if (form != null) {
        DadosFiltro = form;
    }
    var objeto = montarObjeto(DadosFiltro);
    var url = servidor.url + "Dashboards/DashboardDamage";
    var promisse = requisicao(url, 'POST', objeto);
    promisse.done(function (dados) {
        bootbox.hideAll();
        conteudoDamage(dados);

    });
}
