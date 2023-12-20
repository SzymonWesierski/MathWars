document.addEventListener('mousemove', (e) => {
    const { clientX: mouseX, clientY: mouseY } = e;
    const elements = document.querySelectorAll('.symbols');

    elements.forEach(element => {
        const speedFactor = 100; // Możesz dostosować ten współczynnik
        const x = -(window.innerWidth / 2 - mouseX) / window.innerWidth * speedFactor;
        const y = -(window.innerHeight / 2 - mouseY) / window.innerHeight * speedFactor;

        element.style.transform = `translate(${x}px, ${y}px)`;
    });
});