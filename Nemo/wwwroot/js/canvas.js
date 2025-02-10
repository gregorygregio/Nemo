var GLOBAL = {};
GLOBAL.DotNetReference = null;
GLOBAL.SetDotnetReference = function (pDotNetReference) {
    GLOBAL.DotNetReference = pDotNetReference;
};

const CANVAS_ID = '#canvas';


window.setSource = async (stream, contentType) => {
    console.log("setSource", stream, contentType);
    const arrayBuffer = await stream.arrayBuffer();
    console.log("arrayBuffer", arrayBuffer);
    let blobOptions = {};
    if (contentType) {
        blobOptions['type'] = contentType;
    }
    const blob = new Blob([arrayBuffer], blobOptions);
    const url = URL.createObjectURL(blob);
    console.log("url", url);

    const canvas = document.querySelector(CANVAS_ID);
    const ctx = canvas.getContext('2d');

    const img = new Image();
    img.src = url;

    img.onload = async () => {
        console.log("Image loaded");
        await GLOBAL.DotNetReference.invokeMethodAsync('SetImageSize', { width: img.width, height: img.height });
        canvas.setAttribute('width', img.width);
        canvas.setAttribute('height', img.height);
        const svg = document.querySelector("#svg");
        svg.setAttribute('width', img.width);
        svg.setAttribute('height', img.height);
        console.log("drawing image", img);
        ctx.drawImage(img, 0, 0);
    }

};


window.getImageData = async () => {
    const canvas = document.querySelector(CANVAS_ID)
    const ctx = canvas.getContext('2d');
    const imageData = ctx.getImageData(0, 0, canvas.width, canvas.height);
    contentType = 'image/jpeg';
    let blobOptions = {};
    if (contentType) {
        blobOptions['type'] = contentType;
    }
    const blob = new Blob([imageData.data], blobOptions);
    return blob;
}

window.drawRect = async (x, y, width, height, color) => {
    const canvas = document.querySelector(CANVAS_ID);
    const ctx = canvas.getContext('2d');

    ctx.strokeStyle = color;
    
    ctx.strokeRect(x, y, width, height);
}

window.drawDot = async (x, y, color) => {
    const canvas = document.querySelector(CANVAS_ID);
    const ctx = canvas.getContext('2d');

    ctx.fillStyle = color;

    ctx.beginPath();
    ctx.fillRect(x, y, 1, 1);
    
}

window.drawLine = async (x1, y1, x2, y2, color) => {
    const canvas = document.querySelector(CANVAS_ID);
    const ctx = canvas.getContext('2d');

    ctx.strokeStyle = color;
    
    ctx.beginPath();
    ctx.moveTo(x1, y1);
    ctx.lineTo(x2, y2);
    ctx.stroke();
}


window.drawCircle = async (x, y, radius, color) => {
    const canvas = document.querySelector(CANVAS_ID);
    const ctx = canvas.getContext('2d');

    ctx.strokeStyle = color;

    ctx.beginPath();
    ctx.arc(x, y, radius, 0, 2 * Math.PI, true);
    ctx.stroke();
}


window.clearCanvas = async (width, height) => {
    const canvas = document.querySelector(CANVAS_ID);
    const ctx = canvas.getContext('2d');
    ctx.clearRect(0, 0, width, height);
}

window.clearRect = async (x, y, width, height) => {
    const canvas = document.querySelector(CANVAS_ID);
    const ctx = canvas.getContext('2d');
    ctx.clearRect(x, y, width, height);
}


window.addSvgElement = async (elementName, elementId, attrs) => {
    const canvas = document.querySelector("#svg");
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
    const elements = document.querySelectorAll("#svg #" + elementId);
    for (var i = 0; i < elements.length; i++) {
        elements[i].remove();
    }
};


window.clearSvgElements = () => {
    const svg = document.getElementById('svg');
    for (var i = svg.children.length - 1; i >= 0; i--) {
        var el = svg.children[i];
        if (el.tagName == 'image') { console.log(el.tagName); continue; }
        console.log(el.tagName + ' removed');
        svg.removeChild(el);
    }
}


window.downloadImage = async (fileName, contentType) => {
    const canvas = document.querySelector(CANVAS_ID)
    const imgUrl = canvas.toDataURL(contentType);
    const downloadLink = document.createElement("a");
    downloadLink.href = imgUrl;
    downloadLink.download = fileName;
    downloadLink.click();
    URL.revokeObjectURL(imgUrl);
}

window.displayErrorMessage = async (message) => {
    alert(message);
}

window.executeBatchActions = async (actions) => {
    for (let action of actions) {
        console.log("executing action", action);
        if(typeof(window[action.action]) == 'function') {
            window[action.action](...action.args);
        }
    }
}