document.addEventListener("DOMContentLoaded", () => {
    const gridCanvas = document.querySelector("#gridCanvas");
    if (!gridCanvas) {
        console.error("gridCanvas not found!");
        return;
    }
    const gridCtx = gridCanvas.getContext("2d");
    const gridSize = 20;
    const gridColor = '#eaeaea';

    function drawGrid() {
        gridCanvas.width = gridCanvas.offsetWidth;
        gridCanvas.height = gridCanvas.offsetHeight;
        gridCtx.clearRect(0, 0, gridCanvas.width, gridCanvas.height); // Czyści poprzednią kratkę
        gridCtx.beginPath();
        gridCtx.strokeStyle = gridColor;

        for (let x = 0.5; x < gridCanvas.width; x += gridSize) {
            gridCtx.moveTo(x, 0);
            gridCtx.lineTo(x, gridCanvas.height);
        }

        for (let y = 0.5; y < gridCanvas.height; y += gridSize) {
            gridCtx.moveTo(0, y);
            gridCtx.lineTo(gridCanvas.width, y);
        }

        gridCtx.stroke();
    }

    window.addEventListener("resize", () => {
        drawGrid();
        resizeDrawingCanvas();
    });
    drawGrid();
});