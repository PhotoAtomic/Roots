marvin.onReady(function () {
    $("#imageContainer").empty();
    $.each(molecules, function (index, value) {
        var imgData = marvin.ImageExporter.molToDataUrl(value);
        if (imgData != null) {
            var molCell = $('<div class="mol-cell">');
            $("#imageContainer").append(molCell);
            molCell.append($('<span>ID: ' + (index + 1) + '</span>'));
            molCell.append($('<img src="' + imgData + '" data-mol="' + escape(value) + '"/>'));
        }
    });
    $("#imageContainer").append($('<div class="table-bottom"></div>'));
});

var molecules = new Array(
        "Ac\n  Mrv0541 02121315482D          \n\n  3  2  0  0  0  0            999 V2000\n    1.2375   -0.7145    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n    1.9520   -1.1270    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n    2.6664   -0.7145    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0\n  1  2  1  0  0  0  0\n  2  3  2  0  0  0  0\nM  STY  1   1 SUP\nM  SAL   1  3   1   2   3\nM  SMT   1 Ac\nM  SAP   1  1   2   0  1\nM  SCL   1 CXN\nM  SDS EXP  1   1\nM  END\n",
        "AcAc\n  Mrv0541 02121315492D          \n\n  6  5  0  0  0  0            999 V2000\n    2.4750   -1.4289    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n    2.0625   -0.7145    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n    2.4750    0.0000    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0\n    1.2375   -0.7145    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n    0.8250    0.0000    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n    0.0000    0.0000    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0\n  1  2  1  0  0  0  0\n  2  3  2  0  0  0  0\n  2  4  1  0  0  0  0\n  4  5  1  0  0  0  0\n  5  6  2  0  0  0  0\nM  STY  1   1 SUP\nM  SAL   1  6   1   2   3   4   5   6\nM  SMT   1 AcAc\nM  SAP   1  1   5   0  1\nM  SCL   1 CXN\nM  SDS EXP  1   1\nM  END\n",
        "Acet\n  Mrv0541 02121315502D          \n\n  3  2  0  0  0  0            999 V2000\n    1.2375   -0.7145    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n    1.9520   -1.1270    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n    2.6664   -0.7145    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0\n  1  2  1  0  0  0  0\n  2  3  2  0  0  0  0\nM  STY  1   1 SUP\nM  SAL   1  3   1   2   3\nM  SMT   1 Acet\nM  SAP   1  1   2   0  1\nM  SCL   1 CXN\nM  SDS EXP  1   1\nM  END\n",
        "Acm\n  Mrv0541 02121315512D          \n\n  5  4  0  0  0  0            999 V2000\n    0.8250    1.4289    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n    1.2375    0.7145    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0\n    0.8250    0.0000    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n    1.2375   -0.7145    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n    0.0000    0.0000    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0\n  1  2  1  0  0  0  0\n  2  3  1  0  0  0  0\n  3  4  1  0  0  0  0\n  3  5  2  0  0  0  0\nM  STY  1   1 SUP\nM  SAL   1  5   1   2   3   4   5\nM  SMT   1 Acm\nM  SAP   1  1   1   0  1\nM  SCL   1 CXN\nM  SDS EXP  1   1\nM  END\n"
);