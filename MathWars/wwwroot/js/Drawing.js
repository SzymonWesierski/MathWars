document.addEventListener('DOMContentLoaded', function () {
    var canvas = document.getElementById('drawingCanvas');
    var ctx = canvas.getContext('2d');
    var isDrawing = false;
    var currentTool = 'brush';

    canvas.addEventListener('mousedown', function (e) {
        isDrawing = true;
        ctx.beginPath();
        ctx.moveTo(e.clientX - canvas.offsetLeft, e.clientY - canvas.offsetTop);
        draw(e);
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

    var clearCanvasBtn = document.getElementById('clearCanvasBtn');
    clearCanvasBtn.addEventListener('click', function () {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
    });

    // brush and eraser function
    function draw(e) {
        var x = e.clientX - canvas.offsetLeft;
        var y = e.clientY - canvas.offsetTop;

        if (isDrawing) {
            ctx.lineCap = 'round';
            ctx.lineJoin = 'round';

            if (currentTool === 'brush') {
                ctx.strokeStyle = '#000';
                ctx.lineWidth = 2;
            } else if (currentTool === 'eraser') {
                ctx.strokeStyle = '#fff';
                ctx.lineWidth = 50;
            }

            ctx.lineTo(x, y);
            ctx.stroke();
            ctx.beginPath();
            ctx.moveTo(x, y);
        }
    }
});
