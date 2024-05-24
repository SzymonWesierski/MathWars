const canvas = document.querySelector("#mathCanvas"),
    toolBtns = document.querySelectorAll(".tool"),
    fillColor = document.querySelector("#fill-color"),
    sizeSlider = document.querySelector("#size-slider"),
    colorBtns = document.querySelectorAll(".colors .option"),
    colorPicker = document.querySelector("#color-picker"),
    clearCanvas = document.querySelector(".clear-canvas"),
    saveImg = document.querySelector(".save-img"),
    ctx = canvas.getContext("2d");

let prevMouseX, prevMouseY, snapshot,
    isDrawing = false,
    selectedTool = "brush",
    brushWidth = 5,
    selectedColor = "#000";

const setCanvasBackground = () => {
    ctx.fillStyle = selectedColor;
};

window.addEventListener("load", () => {
    canvas.width = canvas.offsetWidth;
    canvas.height = canvas.offsetHeight;
    setCanvasBackground();
});

const resizeDrawingCanvas = () => {
    const tempCanvas = document.createElement("canvas");
    tempCanvas.width = canvas.width;
    tempCanvas.height = canvas.height;
    const tempCtx = tempCanvas.getContext("2d");
    tempCtx.drawImage(canvas, 0, 0);

    canvas.width = canvas.offsetWidth;
    canvas.height = canvas.offsetHeight;
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.drawImage(tempCanvas, 0, 0);
};

const drawRect = (e) => {
    if (!fillColor.checked) {
        return ctx.strokeRect(prevMouseX, prevMouseY, e.offsetX - prevMouseX, e.offsetY - prevMouseY);
    }
    ctx.fillRect(prevMouseX, prevMouseY, e.offsetX - prevMouseX, e.offsetY - prevMouseY);
};

const drawCircle = (e) => {
    ctx.beginPath();
    let radius = Math.sqrt(Math.pow((e.offsetX - prevMouseX), 2) + Math.pow((e.offsetY - prevMouseY), 2));
    ctx.arc(prevMouseX, prevMouseY, radius, 0, 2 * Math.PI);
    fillColor.checked ? ctx.fill() : ctx.stroke();
};

const drawTriangle = (e) => {
    ctx.beginPath();
    ctx.moveTo(prevMouseX, prevMouseY);
    ctx.lineTo(e.offsetX, e.offsetY);
    ctx.lineTo(prevMouseX * 2 - e.offsetX, e.offsetY);
    ctx.closePath();
    fillColor.checked ? ctx.fill() : ctx.stroke();
};

const startDraw = (e) => {
    isDrawing = true;
    prevMouseX = e.offsetX;
    prevMouseY = e.offsetY;
    ctx.beginPath();
    ctx.lineWidth = brushWidth;
    ctx.strokeStyle = selectedColor;
    ctx.fillStyle = selectedColor;
    ctx.lineCap = 'round';
    snapshot = ctx.getImageData(0, 0, canvas.width, canvas.height);
};

const drawing = (e) => {
    if (!isDrawing) return;

    if (selectedTool === "eraser") {
        const eraseSize = brushWidth;
        const eraseStartX = e.offsetX - eraseSize / 2;
        const eraseStartY = e.offsetY - eraseSize / 2;
        ctx.clearRect(eraseStartX, eraseStartY, eraseSize, eraseSize);
    } else if (selectedTool === "brush") {
        ctx.putImageData(snapshot, 0, 0);
        ctx.lineTo(e.offsetX, e.offsetY);
        ctx.stroke();
    } else {
        ctx.putImageData(snapshot, 0, 0);
        if (selectedTool === "rectangle") {
            drawRect(e);
        } else if (selectedTool === "circle") {
            drawCircle(e);
        } else if (selectedTool === "triangle") {
            drawTriangle(e);
        }
    }
};

toolBtns.forEach(btn => {
    btn.addEventListener("click", () => {
        document.querySelector(".options .active").classList.remove("active");
        btn.classList.add("active");
        selectedTool = btn.id;
    });
});

sizeSlider.addEventListener("change", () => {
    brushWidth = parseInt(sizeSlider.value, 10);
});

colorBtns.forEach(btn => {
    btn.addEventListener("click", () => {
        document.querySelector(".options .selected").classList.remove("selected");
        btn.classList.add("selected");
        selectedColor = window.getComputedStyle(btn).getPropertyValue("background-color");
    });
});

colorPicker.addEventListener("change", () => {
    colorPicker.parentElement.style.background = colorPicker.value;
    colorPicker.parentElement.click();
});

clearCanvas.addEventListener("click", () => {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    setCanvasBackground();
});

saveImg.addEventListener("click", () => {
    const link = document.createElement("a");
    link.download = `${Date.now()}.jpg`;
    link.href = canvas.toDataURL();
    link.click();
});

canvas.addEventListener("mousedown", startDraw);
canvas.addEventListener("mousemove", drawing);
canvas.addEventListener("mouseup", () => isDrawing = false);