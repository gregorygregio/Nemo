var GLOBAL = {};
GLOBAL.DotNetReference = null;
GLOBAL.SetDotnetReference = function (pDotNetReference) {
    GLOBAL.DotNetReference = pDotNetReference;
};


window.setSource = async (elementId, stream, contentType, title) => {
    const arrayBuffer = await stream.arrayBuffer();
    let blobOptions = {};
    if (contentType) {
        blobOptions['type'] = contentType;
    }
    const blob = new Blob([arrayBuffer], blobOptions);
    const url = URL.createObjectURL(blob);

    const canvas = document.querySelector("#canvas");
    const ctx = canvas.getContext('2d');

    const img = new Image();
    img.src = url;

    img.onload = async () => {
        await GLOBAL.DotNetReference.invokeMethodAsync('SetImageSize', { width: img.width, height: img.height });
        canvas.setAttribute('width', img.width);
        canvas.setAttribute('height', img.height);
        const svg = document.querySelector("#svg");
        svg.setAttribute('width', img.width);
        svg.setAttribute('height', img.height);

        ctx.drawImage(img, 0, 0);
    }

};

window.drawRect = async (x, y, width, height, color) => {
    const canvas = document.querySelector("#canvas");
    const ctx = canvas.getContext('2d');

    ctx.strokeStyle = color;
    
    ctx.strokeRect(x, y, width, height);
}

window.drawDot = async (x, y, color) => {
    const canvas = document.querySelector("#canvas");
    const ctx = canvas.getContext('2d');

    ctx.fillStyle = color;

    ctx.beginPath();
    ctx.fillRect(x, y, 1, 1);
    
}

window.drawLine = async (x1, y1, x2, y2, color) => {
    const canvas = document.querySelector("#canvas");
    const ctx = canvas.getContext('2d');

    ctx.strokeStyle = color;
    
    ctx.beginPath();
    ctx.moveTo(x1, y1);
    ctx.lineTo(x2, y2);
    ctx.stroke();
}


window.drawCircle = async (x, y, radius, color) => {
    const canvas = document.querySelector("#canvas");
    const ctx = canvas.getContext('2d');

    ctx.strokeStyle = color;

    ctx.beginPath();
    ctx.arc(x, y, radius, 0, 2 * Math.PI, true);
    ctx.stroke();
}


window.clearCanvas = async () => {
    const canvas = document.querySelector("#canvas");
    const ctx = canvas.getContext('2d');
    ctx.clearRect(0, 0, 800, 600);
}




window.addSvgElement = async (elementName, elementId, attrs) => {
    const canvas = document.querySelector("#canvas");
    const svgElement = document.createElementNS("http://www.w3.org/2000/svg", elementName)
    for (let key in attrs) {
        svgElement.setAttribute(key, attrs[key]);
    }
    svgElement.id = elementId;

    svgElement.onclick = async () => {
        console.log("click ", elementId)
        await GLOBAL.DotNetReference.invokeMethodAsync('ElementClicked', elementId);
    }

    canvas.appendChild(svgElement);
};

window.removeSvgElement = async (elementId) => {
    console.log("removeSvgElement ", elementId)
    const elements = document.querySelectorAll("#canvas #" + elementId);
    for (var i = 0; i < elements.length; i++) {
        elements[i].remove();
    }
};


window.clearSvgElements = () => {
    const svg = document.getElementById('canvas');
    for (var i = svg.children.length - 1; i >= 0; i--) {
        var el = svg.children[i];
        if (el.tagName == 'image') { console.log(el.tagName); continue; }
        console.log(el.tagName + ' removed');
        svg.removeChild(el);
    }
}


window.downloadImage = async (fileName, contentType) => {
    const canvas = document.querySelector('#canvas')
    const imgUrl = canvas.toDataURL(contentType);
    const downloadLink = document.createElement("a");
    downloadLink.href = imgUrl;
    downloadLink.download = fileName;
    downloadLink.click();
    URL.revokeObjectURL(imgUrl);
}

window.downloadSvg = async (fileName, svgData) => {
    const svgBlob = new Blob([svgData], { type: "image/svg+xml;charset=utf-8" });
    const svgUrl = URL.createObjectURL(svgBlob);
    const downloadLink = document.createElement("a");
    downloadLink.href = svgUrl;
    downloadLink.download = fileName + ".svg";
    document.body.appendChild(downloadLink);
    downloadLink.click();
    document.body.removeChild(downloadLink);
    URL.revokeObjectURL(svgUrl);
}


window.displayErrorMessage = async (message) => {
    alert(message);
}