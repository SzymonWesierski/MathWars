var MQ = MathQuill.getInterface(2);
var mathField = MQ.MathField(document.getElementById('mathquill-editor'), {
    spaceBehavesLikeTab: true,
    handlers: {
        edit: function () {
            var latex = "\\(" + mathField.latex() + "\\)";
            document.getElementById('latex-input').value = latex;
        }
    }
});

function insertSymbol(symbol) {
    mathField.write(symbol);
    mathField.focus();
}

document.getElementById('copy-button').addEventListener('click', function () {
    var textarea = document.getElementById('latex-input');
    textarea.disabled = false;
    textarea.select();
    document.execCommand('copy');
    textarea.disabled = true;
});