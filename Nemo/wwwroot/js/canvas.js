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
    const svgElement = document.createElementNS("http://www.w3.org/2000/svg", 'image')

    svgElement.setAttribute('href', url);
    svgElement.setAttribute('width', 'calc(80vw - 100px)');
    svgElement.setAttribute('height', '657px');
    svgElement.setAttribute('x', '0');
    svgElement.setAttribute('y', '0');
    svgElement.onload = () => {
        URL.revokeObjectURL(url);
    }

    await window.clearSvgElements();
    await window.removeSvgElement(elementId);


    svgElement.id = elementId;
    canvas.prepend(svgElement);
};

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


window.getSvg = async () => {
    const input = document.querySelector('#canvas')
    const svgData = new XMLSerializer().serializeToString(input)
    return svgData;
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