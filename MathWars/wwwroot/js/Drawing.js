document.addEventListener('DOMContentLoaded', function () {
    var canvas = document.getElementById('drawingCanvas');
    var ctx = canvas.getContext('2d');
    var isDrawing = false;
    var currentTool = 'brush';
    var gridImageData;

    function drawGrid(gridSpacing) {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.beginPath();

        // Rysowanie pionowych linii
        for (let x = 0; x <= canvas.width; x += gridSpacing) {
            ctx.moveTo(x, 0);
            ctx.lineTo(x, canvas.height);
        }

        // Rysowanie poziomych linii
        for (let y = 0; y <= canvas.height; y += gridSpacing) {
            ctx.moveTo(0, y);
            ctx.lineTo(canvas.width, y);
        }

        ctx.strokeStyle = '#ddd'; // Kolor linii
        ctx.stroke();

        // Zapisz stan kratki
        gridImageData = ctx.getImageData(0, 0, canvas.width, canvas.height);
    }

    function restoreGrid() {
        if (gridImageData) {
            ctx.putImageData(gridImageData, 0, 0);
        }
    }

    drawGrid(20); // Rysowanie kratki na pocz¹tku

    canvas.addEventListener('mousedown', function (e) {
        isDrawing = true;
        ctx.beginPath();
        ctx.moveTo(e.clientX - canvas.offsetLeft, e.clientY - canvas.offsetTop);
    });

    canvas.addEventListener('mousemove', function (e) {
        if (isDrawing) {
            draw(e);
        }
    });

    canvas.addEventListener('mouseup', function () {
        isDrawing = false;
    });

    var brushBtn = document.getElementById('brushBtn');
    brushBtn.addEventListener('click', function () {
        currentTool = 'brush';
        brushBtn.classList.add('active');
        eraserBtn.classList.remove('active');
    });

    var eraserBtn = document.getElementById('eraserBtn');
    eraserBtn.addEventListener('click', function () {
        currentTool = 'eraser';
        eraserBtn.classList.add('active');
        brushBtn.classList.remove('active');
    });

    function clearCanvasAndRedrawGrid() {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        restoreGrid(); // Przywróæ kratkê
    }

    var clearCanvasBtn = document.getElementById('clearCanvasBtn');
    clearCanvasBtn.addEventListener('click', clearCanvasAndRedrawGrid);

    function draw(e) {
        if (!isDrawing) return;

        var x = e.clientX - canvas.offsetLeft;
        var y = e.clientY - canvas.offsetTop;

        ctx.lineCap = 'round';
        ctx.lineJoin = 'round';

        if (currentTool === 'brush') {
            ctx.globalCompositeOperation = 'source-over';
            ctx.strokeStyle = '#000';
            ctx.lineWidth = 2;
        } else if (currentTool === 'eraser') {
            ctx.globalCompositeOperation = 'destination-out';
            ctx.lineWidth = 50;
        }

        ctx.lineTo(x, y);
        ctx.stroke();
        ctx.beginPath();
        ctx.moveTo(x, y);
    }
});
