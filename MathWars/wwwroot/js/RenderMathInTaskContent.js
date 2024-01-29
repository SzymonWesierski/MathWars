document.addEventListener("DOMContentLoaded", function () {

    const contentElement = document.getElementById("latex-preview-input");

    contentElement.addEventListener("input", function () {
        renderContent();
    });

    renderContent();
});
function addEquations() {
    const equations = "\\( \\begin{cases} { x+y=2 } \\\\ { x-y=0 } \\end{cases} \\)";

    const contentElement = document.getElementById("latex-preview-input");
    const currentContent = contentElement.value;

    const newContent = currentContent + "\n" + equations;

    contentElement.value = newContent;

    renderContent();
}

function renderContent() {
    const contentElement = document.getElementById("latex-preview-input");
    const previewElement = document.getElementById("latex-preview");
    const contentText = contentElement.value;

    previewElement.textContent = contentText;

    renderMathInElement(previewElement, {
        delimiters: [
            { left: "$$", right: "$$", display: true },
            { left: "$", right: "$", display: false },
            { left: "\\(", right: "\\)", display: false },
            { left: "\\[", right: "\\]", display: true },
        ],
        throwOnError: false,
    });
}

