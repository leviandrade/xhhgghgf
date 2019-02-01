var listaAnexos = [];

function abrirAnexo() {
    bootbox.dialog({
        size: 'large',
        message: $('.form-Anexo').html()
    });
    atualizarGridAnexos();
}

function adicionarAnexo(input) {
    var file = input.files[0];
    if (file) {

        if (validaExtensao(file.type)) {
            bootbox.alert("Só é possivel realizar uploads de imagens do formato JPEG e PNG ou arquivos PDF.");
            return false;
        }

        if (validaArquivoDuplicado(file.name)) {
            bootbox.alert("Este arquivo já está anexado.");
            return false;
        }

        var reader = new FileReader();
        reader.onload = function (e) {
            var data = formataDataHora(file.lastModifiedDate);
            var objeto = dadosFormulario();
            if (objeto.id == "" || objeto.id == null)
                objeto.id = 0;
            var arquivo = {
                idOcorrenciaItem: objeto.id,
                nome: file.name,
                tipo: file.type,
                data: data,
                arquivoBase64: e.target.result
            };
            listaAnexos.push(arquivo);
            atualizarGridAnexos();
        };
        reader.readAsDataURL(file);
    }
}


function validaExtensao(type) {
    var listaExtensao = type.split("/");

    var extensao = listaExtensao[1];
    if (extensao) {
        extensao = extensao.toLowerCase();
        if (extensao != "jpeg" && extensao != "jpg" && extensao != "png" && extensao != "pdf")
            return true;
    }
    return false;
}
function LimparAnexos() {
    listaAnexos = [];
}

function validaArquivoDuplicado(name) {
    var objeto = dadosFormulario();
    var arquivoDuplicado = listaAnexos.find(x => x.idOcorrenciaItem == objeto.id && x.nome == name);
    return arquivoDuplicado != undefined;
}

function removerAnexo(index) {
    excluirAnexo(index);
    atualizarGridAnexos();
    $('#anexo').val('');
    return false;
}

function baixarAnexo(index) {
    var anexo = listaAnexos[index];
    var element = document.createElement('a');
    element.setAttribute('href', anexo.arquivoBase64);
    element.setAttribute('download', anexo.nome);
    element.style.display = 'none';
    document.body.appendChild(element);
    element.click();
    document.body.removeChild(element);
}


function excluirAnexo(index) {
    listaAnexos.splice(index, 1);
}

//function limparGridAnexos() {
//    $("#gridAnexos tbody tr").remove();

//    var objeto = dadosFormulario();

//    var listaAnexosOcorrencias = objeto.id==""? null : listarAnexosID(objeto.id);

//    if (listaAnexosOcorrencias && listaAnexosOcorrencias.length > 0) {
//        for (var i = 0; i < listaAnexosOcorrencias.length; i++) {
//            var info = listaAnexosOcorrencias[i];
//            var linha = '<tr><td><a href="#" title="Baixar Arquivo" class="fa fa-download" onclick="return baixarAnexo(' + i + ');"></a> &nbsp; &nbsp;<a href="#" title="Excluir" class="fa fa-remove" onclick="return removerAnexo(' + i + ');"></a></td><td>' + info.nome + '</td><td>' + info.tipo + '</td><td>' + info.data + '</td></tr>';
//            $('#gridAnexos tbody').append(linha);

//        }
//    } else {
//        var linha = '<tr id="linhaSemRegistro"><td></td><td>Nenhum Arquivo Anexado</td><td></td><td></td></tr >';
//        $('#gridAnexos tbody').append(linha);
//    }
//}

function atualizarGridAnexos() {
    $("#gridAnexos tbody tr").remove();

    var objeto = dadosFormulario();

    var listaAnexosOcorrencias = listarAnexosID(objeto.id);

    if (listaAnexosOcorrencias && listaAnexosOcorrencias.length > 0) {
        for (var i = 0; i < listaAnexosOcorrencias.length; i++) {
            var info = listaAnexosOcorrencias[i];
            var linha = '<tr><td><a href="#" title="Baixar Arquivo" class="fa fa-download" onclick="return baixarAnexo(' + i + ');"></a> &nbsp; &nbsp;<a href="#" title="Excluir" class="fa fa-remove" onclick="return removerAnexo(' + i + ');"></a></td><td>' + info.nome + '</td><td>' + info.tipo + '</td><td>' + info.data + '</td></tr>';
            $('#gridAnexos tbody').append(linha);
            
        }
    } else {
        var linha = '<tr id="linhaSemRegistro"><td></td><td>Nenhum Arquivo Anexado</td><td></td><td></td></tr >';
        $('#gridAnexos tbody').append(linha);
    }
}

function listarAnexosID(id) {
    return listaAnexos.filter(x => x.idOcorrenciaItem == id);
}

function dadosFormulario() {
    var form = $("#fOcorrenciasItens");
    return montarObjeto(form);
}

function listarAnexosIdBase64(id) {

    var listaArquivosBase64 = [];

    var lista = listarAnexosID(id);
    if (lista) {
        for (var i = 0; i < lista.length; i++) {
            listaArquivosBase64.push(lista[i].arquivoBase64);
        }
    }

    return listaArquivosBase64;
}