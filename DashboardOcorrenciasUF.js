
$(window).resize(function () {
    var b = false;
    google.charts.load('current', { 'packages': ['corechart'], 'language': 'pt' });
    google.charts.setOnLoadCallback(function () {
        PopularComboDenunciantesAvulso();
        CarregarDenunciados();
        PopularEmpresaAvulsa();
        PopularTipoCategoriaAvulso();
        popularProduto();
        CarregarClassesProdutos();
        gerarGrafico(b)
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

var indice = 0;
var a = 0;
var DadosFiltro = null;
function gerarGrafico(b,form, event) {
    if (event != null) {
        event.preventDefault();
    }
    if (form != null) {
        DadosFiltro = form;
    }
    bootbox.hideAll();
    var objeto = montarObjeto(DadosFiltro);
    var url = servidor.url + "Dashboards/DashboardOcorrenciasUF";
    var promisse = requisicao(url, 'POST', objeto);
    promisse.done(function (dados) {
        conteudoOcorreciaPorUF(dados, b)
    });
}
