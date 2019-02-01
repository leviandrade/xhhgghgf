$(window).resize(function () {
    google.charts.load('current', { 'packages': ['corechart'], 'language': 'pt' });
    google.charts.setOnLoadCallback(function () {
        PopularComboDenunciantesAvulso();
        CarregarDenunciados();
        PopularEmpresaAvulsa();
        PopularTipoCategoriaAvulso();
        popularProduto();
        CarregarClassesProdutos();
        gerarGrafico();
    });
});
//$(document).on('change', '#Intervalo',function () {
//    if ($('#Intervalo').val() == 1) {
//        $('#IntervaloDias').removeClass("Desabilitado")
//        alert("ok")
//    }
//})

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
    var url = servidor.url + "Dashboards/DashboardOcorrencias";
    var promisse = requisicao(url, 'POST', objeto);
    promisse.done(function (dados) {
        bootbox.hideAll();
        conteudoTotalOcorrencias(dados);
    });
}

